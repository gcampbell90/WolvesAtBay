using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    Transform target;
    private int _speed;
    //TODO: Fix this inefficient method(s) 

    private void Awake()
    {
        _speed = gameObject.GetComponent<CharacterBase>().Speed;
    }
    void FixedUpdate()
    {
        MoveTo();
    }
    public void MoveTo()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Move our position a step closer to the target.
        var step = gameObject.GetComponent<CharacterBase>().Speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            target.position *= -1.0f;
        }

        //Debug.Log("Moving to Target" + step + " " + transform.position + " " + target.position);

    }
}
