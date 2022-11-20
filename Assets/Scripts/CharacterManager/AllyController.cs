using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class AllyController : MonoBehaviour
{

    [SerializeField] List<Follower> _followers = new List<Follower>();

    [SerializeField] private GameObject Commander;
    public GameObject Leader { get; private set; }

    private List<AllyScript> _allies = new List<AllyScript>();
    public List<AllyScript> Allies { get => _allies; set => _allies = value; }

    CancellationTokenSource _cancellationSourceFollowPlayer;
    CancellationTokenSource _cancellationSourceFollowLeader;
    Task _followPlayerTask;
    Task _followLeaderTask;
    Task[] AllyControllerTasks = new Task[2];
    //Debug
    private void OnDrawGizmos()
    {
        if (Leader == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Leader.transform.position + Vector3.forward, Vector3.one);

        foreach (var ally in _allies)
        {
            Gizmos.DrawCube(ally.transform.position + Vector3.forward, Vector3.one);

            if (_allies == null)
            {
                continue;
            }
        }
    }

    private void Awake()
    {
        PlayerController.onDefend += DefendCommand;
        PlayerController.onAttack += AttackCommand;
    }

    private async void Start()
    {

        Leader =
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
        new GameObject("LeaderGuide");

        AllyControllerTasks[0] = _followPlayerTask;
        AllyControllerTasks[1] = _followLeaderTask;

        SetAllyList();
        try
        {
            FollowPlayer();

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Debug.Log("Ally Controller- Follower player task ended");
        }
        try
        {
            await FollowTheLeader();

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Debug.Log("Ally Controller- Follower player task ended");
        }
    }

    private async Task FollowPlayer()
    {
        _cancellationSourceFollowPlayer = new CancellationTokenSource();
        var token = _cancellationSourceFollowPlayer.Token;

        try
        {
            await (_followPlayerTask = FollowPlayerAsync(token));

        }
        catch (OperationCanceledException e)
        {
            Debug.LogError("Error in setting leader to player " + e.Message);
        }
        finally
        {
            Debug.Log("AllyController-TryCatch Finally- Follow Player Async Task finished.");
            _cancellationSourceFollowPlayer.Dispose();
        }

    }

    //TODO fix to allow this to run indefinitely
    private async Task FollowPlayerAsync(CancellationToken token)
    {
        //while (!ApplicationStateManager.playMode)
        //{
        //}
        while (!isDestroyed)
        {
            while (!Commander.Equals(null) ||
       !Leader.Equals(null) ||
       Commander.transform.hasChanged &&
       Leader.transform.position != Commander.transform.position ||
       Leader.transform.rotation != Commander.transform.rotation)
            {
                if (token.IsCancellationRequested)
                {
                    Debug.Log("Ally Controller- Follow Async Task cancel token");
                    return;
                }

                Leader.transform.SetPositionAndRotation(
                    Vector3.Lerp(Leader.transform.position,
                    Commander.transform.position, Time.deltaTime * 2),
                    Quaternion.Slerp(Leader.transform.rotation,
                    Commander.transform.rotation, Time.deltaTime * 2)
                    );

                await Task.Yield();
                Commander.transform.hasChanged = false;
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
            await Task.Yield();
        }

    }
    private async Task FollowTheLeader()
    {
        _cancellationSourceFollowLeader = new CancellationTokenSource();

        var token = _cancellationSourceFollowLeader.Token;
        try
        {
            await (_followLeaderTask = FollowTheLeaderAsync(token));

        }
        catch (OperationCanceledException e)
        {
            Debug.LogError("Error in setting leader to player " + e.Message);
        }
        finally
        {
            Debug.Log("AllyController-TryCatch Finally- Follow Player Async Task finished.");
            _cancellationSourceFollowLeader.Dispose();
        }
    }
    private async Task FollowTheLeaderAsync(CancellationToken token)
    {
        while (!isDestroyed 
            //|| !ApplicationStateManager.playMode
            )
        {
            while (!Commander.Equals(null) ||
                    !Leader.Equals(null) ||
                    Leader.transform.position != Commander.transform.position || 
                    Leader.transform.rotation != Commander.transform.rotation)
            {
                if (token.IsCancellationRequested)
                {
                    Debug.Log("FollowTheLeader Async Task has been cancelled");
                    return;
                }
                foreach (var follower in _followers)
                {
                    var _targetPos = Leader.transform.position - follower.Offset;
                    //var _targetPos = Leader.transform.position - follower.GameObject.transform.position;

                    follower.LerpToVector(_targetPos, Leader.transform.rotation);
                }

                await Task.Yield();
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
            Leader.transform.hasChanged = false;
            await Task.Yield();
        }
    }
    public void SetAllyList()
    {
        //if (GroupController._allies != null)
        //{
        //    _allies = GroupController._allies;
        //    Debug.Log("Setting allies from GroupController");
        //}
        //else
        //{
        //    Debug.Log("No allies in GroupController");
        //}

        var _tmpAllyArr = GameObject.FindGameObjectsWithTag("Ally");
        //Debug.Log(_tmpAllyArr.Length);

        for (int i = 0; i < _tmpAllyArr.Length; i++)
        {
            var m_allyComponent = _tmpAllyArr[i].GetComponent<AllyScript>();
            CreateFollower(m_allyComponent);
            Allies.Add(_tmpAllyArr[i].GetComponent<AllyScript>());

        }

        //Debug.Log($"List of allies set by finding - check groupController\nTotal Ally Count = {_allies.Count}");
    }
    public void CreateFollower(AllyScript m_allyComponent)
    {
        //create follower
        var _follower = new GameObject("FollowerGuide");

        //assign position and rotation to curr position of placed allies and rotation of leader
        var _followerpos = m_allyComponent.transform.position;
        _follower.transform.SetPositionAndRotation(_followerpos, Leader.transform.rotation);

        //Assign follower to ally script for reference - TODO: Update this to have a global ref here?
        m_allyComponent.Follower = _follower;
        m_allyComponent.Leader = Leader.transform;
        var Offset = Leader.transform.position - _followerpos;
        //Mathf.Abs(Leader.transform.position.z - _follower.transform.position.z);

        _followers.Add(
            new Follower(_follower, Offset)
            );

        //Debug.Log($"Leader Pos:{Leader.transform.position}, Follower Pos : {_follower.transform.position}" +
        //    $"Vector Difference: {Offset}");

    }
    public void DefendCommand()
    {
        foreach (var follower in _followers)
        {
            CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            //ally.DefensiveFormationCommand();
            try
            {
                _followLeaderTask = Defend(follower, token);

            }
            catch (OperationCanceledException e)
            {
                Debug.Log("Defence Task Cancelled " + e.Message);

            }
            finally
            {
                Debug.Log("Defence Task Completed");
                _cancellationTokenSource.Dispose();
            }

        }
    }
    private void AttackCommand()
    {
        foreach (var ally in Allies)
        {
            ally.AttackBehaviour.Attack();
        }
    }
    private async Task Defend(Follower follower, CancellationToken token)
    {
        while (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Defending");
            var commanderPos = Commander.transform.position;
            var _relOrigin = commanderPos - follower.Offset;
            var defensiveLine = _relOrigin;

            defensiveLine.x /= 1.25f;
            defensiveLine.y = 0f;
            //should be following leader not commander pos

            follower.GameObject.transform.position = defensiveLine;

            //Debug.Log($"Should be moving {follower} from {_originPos} to {defensiveLine}");
            if (token.IsCancellationRequested)
            {
                //Debug.Log("Task Cancelled");
                return;
                //token.ThrowIfCancellationRequested();
            }
            await Task.Yield();
            follower.GameObject.transform.position = Commander.transform.position - follower.Offset;
        }
        Debug.Log("Break Defense");

    }

    bool isDestroyed;
    private async void OnDestroy()
    {
        isDestroyed = true;
        if (!_followPlayerTask.IsCompleted || !_followPlayerTask.IsCanceled)
        {
            _cancellationSourceFollowPlayer.Cancel();
        }
        if (!_followLeaderTask.IsCompleted || !_followLeaderTask.IsCanceled)
        {
            _cancellationSourceFollowLeader.Cancel();
        }
        await Task.Yield();
        //await Task.WhenAll(AllyControllerTasks);
        ////Debug.Log("MoveToTarget cleanup");
        //if (_followPlayerTask.IsCompleted) return;
        //if (_followPlayerTask.IsCompleted) return;
        ////_followLeaderTask.IsCompleted
        ////_cancellationTokenSource.Cancel();
        //while (!_followPlayerTask.IsCanceled)
        //{
        //    await Task.Yield();
        //}
    }

}
