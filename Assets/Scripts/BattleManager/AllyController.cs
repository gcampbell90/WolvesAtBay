using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    private GameObject _player;
    private GameObject _playerGuide;

    private List<Ally> _allies = new List<Ally>();
    private List<Ally> Allies { get => _allies; set => _allies = value; }
    private List<Follower> _followers = new List<Follower>();
    //[SerializeField] public float shieldArea;//Needs implemented with Editior script for debugging visuals
    CancellationTokenSource _cancellationSourceFollowPlayer;
    CancellationTokenSource _cancellationSourceFollowLeader;

    Task _followPlayerTask;
    Task _followLeaderTask;
    Task[] AllyControllerTasks = new Task[2];

    [SerializeField] private float _smoothSpeed;
    [Range(0.1f, 1f)]
    [SerializeField] private float _shieldWallXSpacing;
    [SerializeField] private float _shieldWallZSpacing;

    private bool isDefending = false;

    //Debug
    private void OnDrawGizmos()
    {
        if (_playerGuide == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_playerGuide.transform.position + _playerGuide.transform.forward, Vector3.one);

        foreach (var follower in _followers)
        {
            Gizmos.DrawCube(follower.GameObject.transform.position + follower.GameObject.transform.forward, Vector3.one);
        }
    }
    private void OnEnable()
    {
        PlayerController.OnDefend += DefendCommand;
    }
    private void OnDisable()
    {
        PlayerController.OnDefend -= DefendCommand;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _playerGuide =
        new GameObject("LeaderGuide");

        _playerGuide.transform.position = _player.transform.position;
        _playerGuide.transform.rotation = _player.transform.rotation;

        SetAllyList();

        try
        {
            _followPlayerTask = FollowPlayer();

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Debug.Log("Ally Controller- Follower player task set");
        }
        try
        {
            _followPlayerTask = FollowTheLeader();

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            Debug.Log("Ally Controller- Follower leader task set");
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
            Debug.Log("AllyController-TryCatch Finally- Follow Leader Async Task finished.");
            _cancellationSourceFollowLeader.Dispose();
        }
    }
    private async Task FollowPlayerAsync(CancellationToken token)
    {
            while (!_player.Equals(null) ||
                   !_playerGuide.Equals(null) ||
                   _player.transform.hasChanged &&
                   _playerGuide.transform.position != _player.transform.position ||
                   _playerGuide.transform.rotation != _player.transform.rotation)
            {

                if (token.IsCancellationRequested)
                {
                    Debug.Log("Ally Controller- Follow Async Task cancel token");
                    return;
                }

                //var pos = Vector3.Lerp(_playerGuide.transform.position, _player.transform.position, Time.deltaTime * _smoothSpeed);

                _playerGuide.transform.position = _player.transform.position;
                _playerGuide.transform.rotation = _player.transform.rotation;

                //_playerGuide.transform.SetPositionAndRotation(
                //      Vector3.Lerp(_playerGuide.transform.position,
                //      _player.transform.position, Time.deltaTime),
                //      Quaternion.Slerp(_playerGuide.transform.rotation,
                //      _player.transform.rotation, Time.deltaTime)
                //      );

                await Task.Yield();
                _player.transform.hasChanged = false;
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
            await Task.Yield();
    }
    private async Task FollowTheLeaderAsync(CancellationToken token)
    {
        while (!ApplicationStateManager.playMode)
        {
            while (!_player.Equals(null) ||
                !_playerGuide.Equals(null) ||
                _playerGuide.transform.hasChanged &&
                _playerGuide.transform.position != _player.transform.position ||
                _playerGuide.transform.rotation != _player.transform.rotation)
            {
                if (token.IsCancellationRequested)
                {
                    //Debug.Log("FollowTheLeader Async Task has been cancelled");
                    return;
                }
                foreach (var follower in _followers)
                {
                    var offset = isDefending ? follower.DefensivePosition : follower.FormationPosition;
                    //Debug.Log("Is Defending?" + isDefending + offset);
                    var pos = _playerGuide.transform.TransformPoint(-offset);

                    follower.GameObject.transform.SetPositionAndRotation(pos, _playerGuide.transform.rotation);
                }
                await Task.Yield();
                _playerGuide.transform.hasChanged = false;
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
        }
        await Task.Yield();
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
            var m_allyComponent = _tmpAllyArr[i].GetComponent<Ally>();
            CreateFollower(m_allyComponent);
            Allies.Add(_tmpAllyArr[i].GetComponent<Ally>());
        }
        //Debug.Log($"List of allies set by finding - check groupController\nTotal Ally Count = {_allies.Count}");
    }
    public void CreateFollower(Ally m_allyComponent)
    {
        //create follower
        var m_follower = new GameObject("FollowerGuide");
        //assign position and rotation to curr position of placed allies and rotation of leader
        var m_followerpos = m_allyComponent.transform.position;
        m_follower.transform.SetPositionAndRotation(m_followerpos, _playerGuide.transform.rotation);

        //Assign follower to ally script for reference - TODO: Update this to have a global ref here?
        m_allyComponent.SetFollower(m_follower);
        var formationOffset = _playerGuide.transform.position - m_followerpos;
        var defensiveOffset = formationOffset;

        defensiveOffset.x *= _shieldWallXSpacing;
        defensiveOffset.z += _shieldWallZSpacing;
        //Mathf.Abs(Leader.transform.position.z - _follower.transform.position.z);

        _followers.Add(
            new Follower(m_follower, formationOffset, defensiveOffset)
            );

        //Debug.Log($"Leader Pos:{_playerGuide.transform.position}, Follower Pos : {m_follower.transform.position}" +
        //    $"Vector Difference: {Offset}");
    }
    private void DefendCommand()
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
    private async Task Defend(Follower follower, CancellationToken token)
    {
        var formationPos = follower.FormationPosition;

        while (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Defending");
            isDefending = true;

            if (token.IsCancellationRequested)
            {
                //Debug.Log("Task Cancelled");
                return;
                //token.ThrowIfCancellationRequested();
            }
            await Task.Yield();
            isDefending = false;
        }
        Debug.Log("Break Defense");
    }

    private async void OnDestroy()
    {
        if (_followPlayerTask == null) return;
        if (!_followPlayerTask.IsCompleted || !_followPlayerTask.IsCanceled)
        {
            _cancellationSourceFollowPlayer.Cancel();
        }
        if (!_followLeaderTask.IsCompleted || !_followLeaderTask.IsCanceled)
        {
            _cancellationSourceFollowLeader.Cancel();
        }
        await Task.Yield();
    }

}
