using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    public GameObject PlayerGameObject { get; set; }
    public GameObject PlayerGuide { get; set; }

    private List<Ally> _allies = new List<Ally>();
    private List<Ally> Allies { get => _allies; set => _allies = value; }

    //TODO: these need to be referenced in ally follow manager
    private List<Follower> _followers = new List<Follower>();
    //[SerializeField] public float shieldArea;//Needs implemented with Editior script for debugging visuals

    [SerializeField] private float _smoothSpeed;
    [Range(0.1f, 1f)]
    [SerializeField] private float _shieldWallXSpacing;
    [SerializeField] private float _shieldWallZSpacing;

    public delegate void DefendCommand();
    public static DefendCommand OnDefendCommand;

    public delegate void DefendAttackCommand();
    public static DefendAttackCommand OnDefendAttackCommand;

    public delegate void AttackCommand();
    public static AttackCommand OnAttackCommand;

    private void OnDrawGizmos()
    {
        if (PlayerGuide == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(PlayerGuide.transform.position + PlayerGuide.transform.forward, Vector3.one);

        foreach (var follower in _followers)
        {
            Gizmos.DrawCube(follower.GameObject.transform.position + follower.GameObject.transform.forward, Vector3.one);
        }
    }
    private void OnEnable()
    {
        Player.OnDefend += () => OnDefendCommand?.Invoke();
        Player.OnAttack += () => OnAttackCommand?.Invoke();
        Player.OnDefendAttack += () => OnDefendAttackCommand?.Invoke();
    }
    private void OnDisable()
    {
        Player.OnDefend -= () => OnDefendCommand?.Invoke();
        Player.OnAttack -= () => OnAttackCommand?.Invoke();
        Player.OnDefendAttack -= () => OnDefendAttackCommand?.Invoke();

    }

    private void Awake()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        PlayerGuide = new GameObject("LeaderGuide");
        PlayerGuide.transform.position = PlayerGameObject.transform.position;
        PlayerGuide.transform.rotation = PlayerGameObject.transform.rotation;

        GetComponent<AllyFollowManager>().SetPlayer(PlayerGameObject, PlayerGuide);
    }
    private void Start()
    {
        SetAllyList();
        SetFollowers();
    }

    private void SetFollowers()
    {
        GetComponent<AllyFollowManager>().SetFollowers(_followers);
    }
    public void SetAllyList()
    {
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
        m_follower.transform.SetPositionAndRotation(m_followerpos, PlayerGuide.transform.rotation);

        //Assign follower to ally script for reference - TODO: Update this to have a global ref here?
        m_allyComponent.SetFollower(m_follower);
        var formationOffset = PlayerGuide.transform.position - m_followerpos;
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



}
