using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform m_FollowTarget;

    int offsetY;
    int offsetZ = -10;
    // Start is called before the first frame update

    private void Awake()
    {
        offsetY = (int)transform.position.y;
        m_FollowTarget = GameObject.FindGameObjectWithTag("Player").transform;

    }
    // Update is called once per frame
    void Update()
    {
        var targetPos = m_FollowTarget.position;
        Vector3 newTargetPos = Vector3.zero;
        if (targetPos == newTargetPos) return;
            
            newTargetPos = targetPos;

        transform.position = newTargetPos + new Vector3(0, offsetY, offsetZ);
    }
}
