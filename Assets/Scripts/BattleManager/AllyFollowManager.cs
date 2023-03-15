using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AllyFollowManager : MonoBehaviour
{
    private GameObject _player;
    private GameObject _playerGuide;

    private List<Follower> _followers = new List<Follower>();

    CancellationTokenSource _cancellationSourceFollowPlayer;
    CancellationTokenSource _cancellationSourceFollowLeader;

    private Task _followPlayerTask;
    private Task _followLeaderTask;

    private bool isDefending = false;


    private void Start()
    {
        _cancellationSourceFollowPlayer = new CancellationTokenSource();
        _cancellationSourceFollowLeader = new CancellationTokenSource();

        try
        {
            _followPlayerTask = FollowPlayerAsync(_cancellationSourceFollowPlayer.Token);

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            //Debug.Log("Ally Controller- Follower player task set");
        }

        try
        {
            _followLeaderTask = FollowTheLeaderAsync(_cancellationSourceFollowLeader.Token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            //Debug.Log("Ally Controller- Follower leader task set");
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
                //Debug.Log("Ally Controller- Follow Async Task cancel token");
                return;
            }

            //var pos = Vector3.Lerp(_playerGuide.transform.position, _player.transform.position, Time.deltaTime * _smoothSpeed);

            _playerGuide.transform.position = _player.transform.position;
            _playerGuide.transform.rotation = _player.transform.rotation;

            await Task.Yield();
            _player.transform.hasChanged = false;
            //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
        }
        await Task.Yield();
    }
    private async Task FollowTheLeaderAsync(CancellationToken token)
    {
        while (!_player.Equals(null) || !_playerGuide.Equals(null) ||
            _player.transform.hasChanged &&
                 _playerGuide.transform.position != _player.transform.position ||
                 _playerGuide.transform.rotation != _player.transform.rotation)
        {
            if (token.IsCancellationRequested)
            {
                Debug.Log("Ally Controller- Follow Leader Async Task cancel token");

                return;
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                //Debug.Log("Defending");
                if (!isDefending)
                {
                    isDefending = true;
                }
            }
            else if (!Input.GetKey(KeyCode.Mouse1))
            {
                isDefending = false;
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
        await Task.Yield();
    }

    public void SetPlayer(GameObject _player, GameObject _playerGuide)
    {
        this._player = _player;
        this._playerGuide = _playerGuide;
    }
    public void SetFollowers(List<Follower> _followers)
    {
        this._followers = _followers;
    }

    private async void OnDestroy()
    {
        if (_followPlayerTask == null) return;
        if (!_followPlayerTask.IsCompleted || !_followPlayerTask.IsCanceled)
        {
            _cancellationSourceFollowPlayer.Cancel();
            _cancellationSourceFollowPlayer.Dispose();
            //Debug.Log("Cancelling FP " + _followPlayerTask);


        }
        if (_followLeaderTask == null) return;
        if (!_followLeaderTask.IsCompleted || !_followLeaderTask.IsCanceled)
        {
            _cancellationSourceFollowLeader.Cancel();
            _cancellationSourceFollowLeader.Dispose();

            //Debug.Log("Cancelling FL " + _followLeaderTask);

        }
        await Task.Yield();
    }

}