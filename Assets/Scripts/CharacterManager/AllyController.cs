using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public partial class AllyController : MonoBehaviour
{

    [SerializeField] List<Follower> _followers = new List<Follower>();

    [SerializeField] private GameObject Commander;
    public GameObject Leader { get; private set; }

    private List<AllyScript> _allies = new List<AllyScript>();
    public List<AllyScript> Allies { get => _allies; set => _allies = value; }

    Task _followPlayerTask;
    Task _followLeaderTask;
    CancellationTokenSource _cancellationTokenSource = null;

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

        SetAllyList();

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        _followPlayerTask = FollowPlayer(token);

        _cancellationTokenSource = new CancellationTokenSource();
        var token1 = _cancellationTokenSource.Token;
        _followLeaderTask = FollowTheLeader(token1);

        //try
        //{
        // //await _followPlayerTask;
        //}
        //catch(Exception e)
        //{
        //   Debug.LogError("Error in setting leader to player " + e.Message);
        //}
        //finally
        //{
        //    Debug.Log("AllyController-TryCatch Finally- Follow Player Async Task finished.");
        //}
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

    private async Task FollowPlayer(CancellationToken token)
    {
        while (true || !ApplicationStateManager.playMode)
        {
            while (Commander.transform.hasChanged && Leader.transform.position != Commander.transform.position || Leader.transform.rotation != Commander.transform.rotation)
            {
                Leader.transform.SetPositionAndRotation(
                    Vector3.Lerp(Leader.transform.position,
                    Commander.transform.position, Time.deltaTime * 2),
                    Quaternion.Slerp(Leader.transform.rotation,
                    Commander.transform.rotation, Time.deltaTime * 2)
                    );

                if (token.IsCancellationRequested)
                {
                    Debug.Log("Follow Async Task has been cancelled");
                    return;
                }
                await Task.Yield();
                Commander.transform.hasChanged = false;
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
            await Task.Yield();
        }
    }

    private async Task FollowTheLeader(CancellationToken token)
    {
        while (true || !ApplicationStateManager.playMode)
        {
            while (Leader.transform.hasChanged && Leader.transform.position != Commander.transform.position || Leader.transform.rotation != Commander.transform.rotation)
            {
                foreach (var follower in _followers)
                {
                    var _targetPos = Leader.transform.position - follower.Offset;
                    //var _targetPos = Leader.transform.position - follower.GameObject.transform.position;

                    follower.LerpToVector(_targetPos, Leader.transform.rotation);
                }
                if (token.IsCancellationRequested)
                {
                    Debug.Log("FollowTheLeader Async Task has been cancelled");
                    return;
                }
                await Task.Yield();
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
            Leader.transform.hasChanged = false;
            await Task.Yield();
        }
    }

    public void DefendCommand()
    {
        foreach (var follower in _followers)
        {
            _cancellationTokenSource = new CancellationTokenSource();
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
    private async void OnDestroy()
    {
        //Debug.Log("MoveToTarget cleanup");
        if (_followPlayerTask.IsCompleted) return;
        _cancellationTokenSource.Cancel();
        while (!_followPlayerTask.IsCanceled)
        {
            await Task.Yield();
        }
    }

}
