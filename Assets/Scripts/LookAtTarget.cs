using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    Transform target;

    //TODO: Fix this inefficient method(s) 

    void Update()
    {
        LookAt();
    }
    public void LookAt()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target);
    }
}
