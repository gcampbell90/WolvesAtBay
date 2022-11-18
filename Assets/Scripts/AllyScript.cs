using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour
{
    private Transform _leader;
    public GameObject Follower { get; set; }
    Vector3 formationPos = Vector3.zero;
    bool isDefending = false;
    Coroutine myRoutine = null;


    private void Start()
    {
        //_follower.transform.SetLocalPositionAndRotation(_followerpos, _leader.transform.rotation);
        StartCoroutine(StayInFormation());
    }

    public void DefensiveFormationCommand()
    {
        myRoutine = StartCoroutine(Defend());
    }
    public void ChargeCommand()
    {
        myRoutine = StartCoroutine(Charge());
    }

    public IEnumerator StayInFormation()
    {
        _leader = AllyController.GetNewTransform();

        float _t = 0f;
        while (true)
        {

            yield return StartCoroutine(CheckFollowerPosition());

            formationPos = Follower.transform.position;

            //Debug.Log("Formation Moving from " + transform.position + " " + " " + formationPos);

            var leaderRot = _leader.transform.rotation;

            while (_t < 0.15f)
            {
                var pos = Vector3.Lerp(transform.position, formationPos, _t);
                var rot = Quaternion.Slerp(transform.rotation, leaderRot, _t);

                transform.SetPositionAndRotation(pos, rot);
                //transform.position = Vector3.Slerp(transform.position, formationPos, _t);
                //transform.rotation = Quaternion.Slerp(transform.rotation, leaderRot, _t);
                _t += Time.deltaTime;
                yield return null;
            }
            _t = 0f;
        }
    }

    IEnumerator CheckFollowerPosition()
    {
        Vector3 m_followerPosTmp = Follower.transform.position;
        //Debug.Log("Formation Check");
        while (formationPos == m_followerPosTmp)
        {
            m_followerPosTmp = Follower.transform.position;
            //Debug.Log("Waiting for formation change = " + formationPos +" "+ m_followerPosTmp);

            yield return null;
        }
        //Debug.Log("Formation change = " + formationPos + " " + m_followerPosTmp);

    }

    public IEnumerator Defend()
    {
        isDefending = true;

        Vector3 _originPos = new Vector3();
        _originPos = Follower.transform.position;

        //move followers to correct pos
        var defensiveLine = _originPos;
        defensiveLine.x *= 0.75f;
        defensiveLine.y = 0f;
        defensiveLine.z = _originPos.z + 1f;

        Follower.transform.localPosition = defensiveLine;

        while (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Defending");

            yield return null;
        }
        Debug.Log("Break Defense");

        Follower.transform.position = _originPos;
        isDefending = false;
    }

    bool isCharging = false;
    public IEnumerator Charge()
    {
        if (isCharging) yield break;
        isCharging = true;

        Vector3 m_originPos = Follower.transform.position;

        //move followers to correct pos
        var m_chargeLine = m_originPos;
        m_chargeLine.y = 0f;
        m_chargeLine.z = _leader.transform.position.z + UnityEngine.Random.Range(-0.5f, 1f);

        float _t = Time.deltaTime;
        while (_t < 0.5f)
        {
            Follower.transform.position = Vector3.Lerp(m_originPos, m_chargeLine, _t);
            while (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Charging");
                yield return null;
            }
            _t += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Reforming the line");

        Follower.transform.position = m_originPos;
        isCharging = false;
    }

    internal void AttackCommand()
    {
        //Debug.Log("AllyScript - attack command");
        var attackBehaviour = GetComponent<AttackBehaviour>();
        attackBehaviour.Attack();
    }
}
