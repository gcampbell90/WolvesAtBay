using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    [SerializeField] List<AllyScript> allies = new List<AllyScript>();
    public List<AllyScript> Allies
    {
        get
        {
            return allies;
        }
        set
        {
            allies = value;
        }
    }

    public static Transform Commander { get; set; }
    public static GameObject Leader { get; set; }

    public List<Follower> _followers = new List<Follower>();

    public sealed class Follower
    {
        public GameObject GameObject { get; set; }
        public Vector3 Offset { get; set; }
        public Follower(GameObject follower, Vector3 offset)
        {
            GameObject = follower;
            Offset = offset;
        }
    }

    private void OnDrawGizmos()
    {
        if (Leader == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Leader.transform.position + Vector3.forward, Vector3.one);

        foreach (var ally in allies)
        {
            Gizmos.DrawCube(ally.transform.position + Vector3.forward, Vector3.one);

            if (allies == null)
            {
                continue;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Commander = GameObject.FindGameObjectWithTag("Player").transform;

        Leader =
        //GameObject.CreatePrimitive(PrimitiveType.Cube);
        new GameObject("LeaderGuide");

        Leader.transform.SetPositionAndRotation(Commander.position, Commander.rotation);
        foreach (var ally in allies)
        {
            //create follower
            var _follower = new GameObject("FollowerGuide");

            //assign position and rotation to curr position of placed allies and rotation of leader
            var _followerpos = ally.transform.position;
            _followerpos.y = 1f;
            _follower.transform.SetPositionAndRotation(_followerpos, Leader.transform.rotation);
            //_follower.transform.SetParent(Leader.transform, false);

            //Assign follower to ally script for reference - TODO: Update this to have a global ref here?
            ally.Follower = _follower;
            var Offset = Leader.transform.position - _followerpos;
                //Mathf.Abs(Leader.transform.position.z - _follower.transform.position.z);

            _followers.Add(
                new Follower(_follower, Offset)
                );

            //Debug.Log($"Leader Pos:{Leader.transform.position}, Follower Pos : {_follower.transform.position}" +
            //    $"Vector Difference: {Offset}");
        }
    }

    private void Update()
    {
        if (Leader.transform.position != Commander.position || Leader.transform.rotation != Commander.rotation)
        {
            Leader.transform.SetPositionAndRotation(Commander.position, Commander.rotation);
            foreach (var follower in _followers)
            {
                var followerPos = follower.Offset;

                follower.GameObject.transform.position = Leader.transform.position - followerPos;
            }
        } 
    }

    public static Transform GetNewTransform()
    {
        return Leader.transform;
    }
    public void SetDefensiveFormation()
    {
        //foreach (var ally in MyAllies)
        //{
        //    ally.DefensiveFormationCommand();
        //}
        foreach (var ally in allies)
        {
            ally.DefensiveFormationCommand();
        }
    }
    public void ChargeCommand()
    {
        //foreach (var ally in MyAllies)
        //{
        //    ally.ChargeCommand();
        //}
        foreach (var ally in allies)
        {
            ally.ChargeCommand();
        }
    }
    public void AttackCommand()
    {
        Debug.Log("AllyController - attack command");
        foreach (var ally in allies)
        {
            ally.AttackCommand();
        }
    }


}
