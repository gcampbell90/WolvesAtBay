using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MoveToTarget : MonoBehaviour
{
    public Transform Target { get; set; }
    private int _speed;
    //TODO: Fix this inefficient method(s) 

    private void Awake()
    {
        if (gameObject.GetComponent<CharacterBase>() == null)
        {
            _speed = 2;
        }
        else
        {
            _speed = gameObject.GetComponent<CharacterBase>().Speed;
        }
        //Debug.Log($"Move Speed = {_speed}. if zero character base not set.");

        if (_speed <= 0)
            _speed = 5;

    }

    void FixedUpdate()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null) { return; }

        MoveTo();
    }
    public void MoveTo()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        if (Vector3.Distance(transform.position, Target.position) < 3f)
        {
            return;
        }


        // Move our position a step closer to the target.
        var step = _speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);


        //Debug.Log("Moving to Target" + step + " " + transform.position + " " + target.position);

    }
  
}
