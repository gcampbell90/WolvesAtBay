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
        if (GameObject.FindGameObjectWithTag("Player") == null) { return; }

        target = GameObject.FindGameObjectWithTag("Player").transform;

        transform.LookAt(target);
    }
}
