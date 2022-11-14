using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform m_FollowTarget;

    float offsetY;
    float offsetZ = 0;
    // Start is called before the first frame update

    public static Vector3 CameraForward;
    private void Awake()
    {
        offsetY = transform.position.y;
        offsetZ = transform.position.z;
        CameraForward = Vector3.zero;
    }

    private void Start()
    {
        CheckIfPlayerExists();
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
        Vector3 newTargetPos = Vector3.zero;
        if (targetPos == newTargetPos) return;

        newTargetPos = targetPos;

        transform.position = newTargetPos + new Vector3(0, offsetY, offsetZ);
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
