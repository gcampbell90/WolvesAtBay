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
        PlayerController.onDefend += DefensiveFormation;
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

    private async Task FollowPlayer(CancellationToken token)
    {
        while (true || !ApplicationStateManager.playMode)
        {
            while (Commander.transform.hasChanged && Leader.transform.position != Commander.transform.position)
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
            while (Leader.transform.hasChanged && Leader.transform.position != Commander.transform.position)
            {

                //Leader.transform.SetPositionAndRotation(
                //    Vector3.Lerp(Leader.transform.position,
                //    Commander.transform.position, Time.deltaTime * 2),
                //    Quaternion.Slerp(Leader.transform.rotation,
                //    Commander.transform.rotation, Time.deltaTime * 2)
                //    );
                foreach(var follower in _followers)
                {
                    var _originPos = Leader.transform.position - follower.Offset;
                    var _targetPos = isDefending ? follower.GameObject.transform.position : _originPos;
                    //follower.GameObject.transform.position = Leader.transform.position - followerPos;
                    follower.LerpToVector(_targetPos, Leader.transform.rotation);
                }
                if (token.IsCancellationRequested)
                {
                    Debug.Log("FollowTheLeader Async Task has been cancelled");
                    return;
                }
                await Task.Yield();
                Leader.transform.hasChanged = false;
                //Debug.Log($"Commander(Player){Commander.transform.position} Leader(To follow): {Leader.transform.position}");
            }
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


    private void Update()
    {
        //if (Leader.transform.position != Commander.transform.position || Leader.transform.rotation != Commander.transform.rotation)
        //{
        //    Leader.transform.SetPositionAndRotation(Commander.transform.position, Commander.transform.rotation);
        
        //if (_followers.Count <= 0) return;

        //    //if (isDefending) return;
        //foreach (var follower in _followers)
        //{
        //    var followerPos = follower.Offset;

        //    follower.GameObject.transform.position = Leader.transform.position - followerPos;
        //}
        //}
    }

    bool isDefending;
    public void DefensiveFormation()
    {
        foreach (var follower in _followers)
        {

            //ally.DefensiveFormationCommand();
            StartCoroutine(Defend(follower));

        }
    }

    private IEnumerator Defend(Follower follower)
    {
        isDefending = true;

        Vector3 _originPos = Leader.transform.position - follower.Offset;
        Debug.Log(follower.Offset);
        //move followers to correct pos
        var defensiveLine = _originPos;
        defensiveLine.x /= 1.75f;
        defensiveLine.y = 0f;
        defensiveLine.z = _originPos.z + 1f;

        follower.GameObject.transform.position = defensiveLine;

        //Debug.Log($"Should be moving {follower} from {_originPos} to {defensiveLine}");
        while (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Defending");

            yield return null;
        }
        Debug.Log("Break Defense");

        follower.GameObject.transform.position = _originPos;
        isDefending = false;
    }

    //public IEnumerator Defend(GameObject follower)
    //{
    //    isDefending = true;

    //    Vector3 _originPos = follower.transform.position;

    //    //move followers to correct pos
    //    var defensiveLine = _originPos;
    //    defensiveLine.x *= 0.75f;
    //    defensiveLine.y = 0f;
    //    defensiveLine.z = _originPos.z + 1f;

    //    follower.transform.position = defensiveLine;

    //    //Debug.Log($"Should be moving {follower} from {_originPos} to {defensiveLine}");
    //    while (Input.GetKey(KeyCode.Mouse1))
    //    {
    //        Debug.Log("Defending");

    //        yield return null;
    //    }
    //    Debug.Log("Break Defense");

    //    follower.transform.position = _originPos;
    //    isDefending = false;
    //}

    //public static Transform GetNewTransform()
    //{
    //    return Leader.transform;
    //}

    //public void ChargeCommand()
    //{
    //    //foreach (var ally in MyAllies)
    //    //{
    //    //    ally.ChargeCommand();
    //    //}
    //    foreach (var ally in _allies)
    //    {
    //        ally.ChargeCommand();
    //    }
    //}
    //public void AttackCommand()
    //{
    //    Debug.Log("AllyController - attack command");
    //    foreach (var ally in _allies)
    //    {
    //        ally.AttackCommand();
    //    }
    //}
    //public void Reform()
    //{
    //    foreach (var ally in Allies)
    //    {
    //        ally.StayInFormationCall();
    //    }
    //}

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
