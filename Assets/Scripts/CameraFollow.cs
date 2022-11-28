using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{

    //[SerializeField] bool UseCameraStartPosition;
    //Vector3 m_cameraStartPos;

    [SerializeField] Vector2 sensitivity = new Vector2(1, 1);
    [SerializeField] Vector2 acceleration;
    Vector2 velocity;
    Vector2 rotation;

    [SerializeField] float maxVerticalAngleHorizon;
    [SerializeField] float maxHorizontalAngleHorizon;
    float _timer = 0f;

    public static Vector3 CameraForward;
    Transform _targetToFollow;

    Vector3 _targetOffset;

    [SerializeField] float smoothTime;
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null) return;
        _targetToFollow = GameObject.FindGameObjectWithTag("Player").transform;

            _targetOffset = transform.position - _targetToFollow.position;
        //m_cameraStartPos = transform.position;

        //CameraForward = Vector3.zero;

        //CheckIfPlayerExists();
        StartCoroutine(FollowMouse());
       

    }
    void LateUpdate()
    {
        //if (!CheckIfPlayerExists()) return;

        //FollowMouse();
        // Define a target position above and behind the target transform

        // Smoothly move the camera towards that target position
        transform.position = Vector3.Lerp(transform.position, _targetToFollow.position + _targetOffset, Time.deltaTime * smoothTime);
    }


    //private bool CheckIfPlayerExists()
    //{
    //    if (GameObject.FindGameObjectWithTag("Player") != null)
    //    {
    //        SetPlayerTarget();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    ////This is a methods copied a lot throughout - fix with utility method?
    //private void SetPlayerTarget()
    //{
    //    _targetToFollow = GameObject.FindGameObjectWithTag("Player").transform;
    //    StartCoroutine(FollowPlayer());
    //}

    //// Update is called once per frame

    ////should only be called if player exists and is then set
    //private IEnumerator FollowPlayer()
    //{

    //    while (_targetToFollow != null)
    //    {
    //        var targetPos = _targetToFollow.position;

    //        transform.position = Vector3.Slerp(transform.position, targetPos + GetVector(), _timer * 10);

    //        yield return null;
    //    }
    //}
    Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        return input;
    }

    private IEnumerator FollowMouse()
    {
        Vector2 targetVelocity = GetInput() * sensitivity;

        while (true)
        {
            while (targetVelocity != GetInput() * sensitivity)
            {

                //targetVelocity = GetInput() * sensitivity;

                //velocity = new Vector2(
                //    Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration.x * _timer * 5),
                //    Mathf.MoveTowards(velocity.y, targetVelocity.y, acceleration.y * _timer * 5));

                //rotation += velocity * Time.deltaTime;

                //rotation.y = ClampVerticalAngle(rotation.y);
                //rotation.x = ClampHorizontalAngle(rotation.x);


                //CameraForward = new Vector3(-rotation.y, rotation.x, 0);

                ////transform.eulerAngles = CameraForward;

                //transform.Rotate(_targetToFollow.up * velocity.x);
                transform.LookAt(_targetToFollow);

            _timer += Time.unscaledDeltaTime;
            yield return null;

            }
            yield return null;
        }

    }

    float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle, -35, -maxVerticalAngleHorizon);
    }
    float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    }

}
