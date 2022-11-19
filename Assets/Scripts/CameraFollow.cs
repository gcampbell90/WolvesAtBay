using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform m_FollowTarget;

    [SerializeField] bool UseCameraStartPosition;
    Vector3 m_cameraStartPos;

    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] float offsetZ;

    public static Vector3 CameraForward;

    private void Awake()
    {
        CheckIfPlayerExists();
        m_cameraStartPos = transform.position;

        CameraForward = Vector3.zero;
    }

    Vector3 GetVector()
    {
        var targetVector = Vector3.zero;

        if (UseCameraStartPosition)
        {
            targetVector = m_cameraStartPos - m_FollowTarget.position;
        }
        else
        {
            targetVector = new Vector3(offsetX,offsetY,offsetZ);
        }
 
        return targetVector;
    }

    private bool CheckIfPlayerExists()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            SetPlayerTarget();
            return true;
        }
        else
        {
            return false;
        }
    }

    //This is a methods copied a lot throughout - fix with utility method?
    private void SetPlayerTarget()
    {
        m_FollowTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckIfPlayerExists()) return;
        FollowPlayer();
        FollowMouse();
    }

    //should only be called if player exists and is then set
    private void FollowPlayer()
    {
        var targetPos = m_FollowTarget.position;

        transform.position = targetPos + GetVector();
    }

    [SerializeField] Vector2 sensitivity = new Vector2(1, 1);
    [SerializeField] Vector2 acceleration;
    Vector2 velocity;
    Vector2 rotation;

    [SerializeField] float maxVerticalAngleHorizon;
    [SerializeField] float maxHorizontalAngleHorizon;
    float _timer = 0f;

    private void FollowMouse()
    {
        Vector2 targetVelocity = GetInput() * sensitivity;

        velocity = new Vector2(
            Mathf.MoveTowards(velocity.x, targetVelocity.x, acceleration.x * _timer),
            Mathf.MoveTowards(velocity.y, targetVelocity.y, acceleration.y * _timer));

        rotation += velocity * Time.deltaTime;

        rotation.y = ClampVerticalAngle(rotation.y);
        rotation.x = ClampHorizontalAngle(rotation.x);

            
        CameraForward = new Vector3(-rotation.y, rotation.x, 0);

        transform.eulerAngles = CameraForward;

        _timer += Time.unscaledDeltaTime;
    }

    Vector2 GetInput()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
            );

        return input;
    }

    float ClampVerticalAngle(float angle)
    {
        return Mathf.Clamp(angle,-35, -maxVerticalAngleHorizon);
    }
    float ClampHorizontalAngle(float angle)
    {
        return Mathf.Clamp(angle, -maxHorizontalAngleHorizon, maxHorizontalAngleHorizon);
    }

}
