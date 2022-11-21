using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public Transform Leader { get; set; }
    public GameObject Follower { get; set; }
    Transform formationTransform;
    public AttackBehaviour AttackBehaviour { get; set; }
    public TargetingSystem TargetingSystem { get; set; }
    public int Speed { get; set; } = 5;

    private void OnEnable()
    {
        PlayerController.onDefend += Defend;
        PlayerController.onAttack += Attack;
    }

    private void OnDisable()
    {
        PlayerController.onDefend -= Defend;
        PlayerController.onAttack -= Attack;

    }
    private void Awake()
    {
        AttackBehaviour = GetComponent<AttackBehaviour>();
        TargetingSystem = GetComponent<TargetingSystem>();
        //StartCoroutine(CheckLeader());
    }

    private void Start()
    {
        //_follower.transform.SetLocalPositionAndRotation(_followerpos, _leader.transform.rotation);
        StartCoroutine(StayInFormation());
    }
    private void Attack()
    {
        AttackBehaviour.Attack();
    }
    private void Defend()
    {
        StartCoroutine(RaiseShield());
    }
    public void SetTarget(Transform target)
    {
        TargetingSystem.Target = target;
    }
    private IEnumerator RaiseShield()
    {
        var m_shield = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
        var m_originRot = m_shield.localRotation;
        while (Input.GetKey(KeyCode.Mouse1))
        {
            m_shield.localRotation = Quaternion.Euler(0, 0, 0);
            yield return null;
        }
        m_shield.localRotation = m_originRot;
    }
    public IEnumerator StayInFormation()
    {
        float _t = 0f;
        while (true)
        {
            yield return StartCoroutine(CheckFollowerPosition());

            //Debug.Log("Formation Moving from " + transform.position + "to " + formationTransform.position);

            while (_t < 0.15f)
            {
                var pos = Vector3.Lerp(transform.position, formationTransform.position, _t);
                var rot = Quaternion.Slerp(transform.rotation, formationTransform.rotation, _t);
                transform.SetPositionAndRotation(pos, rot);
                _t += Time.deltaTime;
                yield return null;
            }
            _t = 0f;
        }
    }
    IEnumerator CheckFollowerPosition()
    {
        while (Follower == null) yield return null;
        Transform m_followerTmp = Follower.transform;
        //Debug.Log("Formation Check");
        while (transform.position == m_followerTmp.position)
        {
            //Debug.Log("Waiting for formation change = " + formationTransform.position + " " + m_followerTmp.position);
            yield return null;
        }
        formationTransform = m_followerTmp;
        //Debug.Log("Formation change = " + formationPos + " " + m_followerPosTmp);
    }

}

//    bool isCharging = false;
//    public IEnumerator Charge()
//    {
//        if (isCharging) yield break;
//        isCharging = true;

//        Vector3 m_originPos = Follower.transform.position;

//        //move followers to correct pos
//        var m_chargeLine = m_originPos;
//        m_chargeLine.y = 0f;
//        m_chargeLine.z = _leader.transform.position.z + UnityEngine.Random.Range(-0.5f, 1f);

//        float _t = Time.deltaTime;
//        while (_t < 0.5f)
//        {
//            Follower.transform.position = Vector3.Lerp(m_originPos, m_chargeLine, _t);
//            while (Input.GetKey(KeyCode.LeftShift))
//            {
//                Debug.Log("Charging");
//                yield return null;
//            }
//            _t += Time.deltaTime;
//            yield return null;
//        }
//        Debug.Log("Reforming the line");

//        Follower.transform.position = m_originPos;
//        isCharging = false;
//    }

//    internal void AttackCommand()
//    {
//        //Debug.Log("AllyScript - attack command");
//        var attackBehaviour = GetComponent<AttackBehaviour>();
//        attackBehaviour.Attack();
//    }
//}
