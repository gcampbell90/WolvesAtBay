using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour
{
    Transform _commander;
    GameObject _leader;
    GameObject _follower;

    Vector3 formationPos;

    bool isDefending = false;
    Coroutine myRoutine = null;
    private void Start()
    {
        _leader = AllyController.Leader;

        _follower =
            //GameObject.CreatePrimitive(PrimitiveType.Cube);
            new GameObject("FollowerGuide");

        var followerpos = transform.position;
        followerpos.y = 0f;

        _follower.transform.SetParent(_leader.transform);
        _follower.transform.SetLocalPositionAndRotation(followerpos, _leader.transform.rotation);
        StartCoroutine(StayInFormation());
    }

    public void DefensiveFormationCommand()
    {
        if (isDefending)
        {
            Debug.Log("Already running" + myRoutine + "..stopping");
            //StopCoroutine(myRoutine);
            isDefending = false;
        }
        else
        {
            myRoutine = StartCoroutine(Defend());
        }
    }
    public void ChargeCommand()
    {
        myRoutine = StartCoroutine(Charge());
    }

    public IEnumerator StayInFormation()
    {
        _commander = AllyController.GetNewTransform();
        float _t = 0f;
        while (true)
        {
            //if (transform.position == formationPos | transform.rotation == leaderRot) yield break;
            formationPos = _follower.transform.position;
            var leaderRot = _commander.transform.rotation;

            //Debug.Log("Move to pos" + formationPos);

            while (_t < 0.25f)
            {
                transform.position = Vector3.Slerp(transform.position, formationPos, _t);
                transform.rotation = Quaternion.Slerp(transform.rotation, leaderRot, _t);
                _t += Time.deltaTime;
                yield return null;
            }
            _t = 0f;
        }
    }

    public IEnumerator Defend()
    {
        isDefending = true;

        Vector3 _originPos = new Vector3();
        _originPos = _follower.transform.localPosition;

        //move followers to correct pos
        var defensiveLine = _follower.transform.localPosition;
        defensiveLine.x *= 0.85f;
        defensiveLine.y = 0f;
        defensiveLine.z /= 2f;

        _follower.transform.localPosition = defensiveLine;

        while (isDefending)
        {
            Debug.Log("Defending");

            yield return null;
        }
        Debug.Log("Break Defense");


        _follower.transform.localPosition = _originPos;
        isDefending = false;

    }

    bool isCharging = false;
    public IEnumerator Charge()
    {
        if (isCharging) yield break;
        isCharging = true;

        Vector3 _originPos = new Vector3();
        _originPos = _follower.transform.localPosition;

        //move followers to correct pos
        var chargeLine = _follower.transform.localPosition;
        chargeLine.y = 0f;

        chargeLine.z = UnityEngine.Random.Range(-0.5f,1f);

        _follower.transform.localPosition = chargeLine;

        float _t = 0f;
        while (_t < 1f)
        {
            Debug.Log("Charging");
            _t += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Reforming the line");

        _follower.transform.localPosition = _originPos;
        isCharging = false;

    }
}
