using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    Transform _target;

    //TODO: Fix this inefficient method(s) 

    private void Start()
    {
        
    }

    void Update()
    {
        if (_target == null)
        {
            _target = GetComponent<TargetingSystem>().Target;
        }
        else
        {
            LookAt();
        }
    }
    public void LookAt()
    {
        transform.LookAt(_target);
    }
}
