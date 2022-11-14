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
            Debug.Log("Already running" + myRoutine);
            return;
            StopCoroutine(myRoutine);
        }
        myRoutine = StartCoroutine(Defend());
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
        Debug.Log("Defending");
        isDefending = true;

        float _t = 0f;
        float defendLine = 0f;

        Vector3 _originPos = new Vector3();
        _originPos = _follower.transform.localPosition;

        //move followers to correct pos
        var defensiveLine = _follower.transform.localPosition;
        defensiveLine.z = 0f;
        defensiveLine.y = 1f;

        _follower.transform.localPosition = defensiveLine;
        while (_t < 2f)
        {
            _t += Time.deltaTime;
            yield return null;
        }
        _t = 0f;
        _follower.transform.localPosition = _originPos;
        isDefending = false;

    }
}
