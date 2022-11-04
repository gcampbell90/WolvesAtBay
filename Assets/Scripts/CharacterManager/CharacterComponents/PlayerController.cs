using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int speed; //Speed of the player cube

    public float mapHeight = 5f; //width of the movable area
    public float mapWidth = 5f; //width of the movable area
    //public event System.Action OnCubeDeath; //cube death event

    //private Rigidbody rb;

    //float screenHalfWidthInWorldUnits
    public Transform Pivot { get; set; }
    private void Awake()
    {
        Pivot = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
    }
    private void Start()
    {
        speed = gameObject.GetComponent<CharacterBase>().Speed;
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.position += move * Time.deltaTime * speed;

        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(AttackMove());
            //Attack();
            //print("space key was pressed");
        }

    }

    //TODO: Turn into animation call/method...abstract away from player controller.
    private void Attack()
    {
        float rot = Pivot.rotation.y > 0 ? -45 : 45;
        Pivot.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
        //Debug.Log(rot);
        //yield return null;
    }

    bool isRunning;
    private IEnumerator AttackMove()
    {
        // Just make the animation interval configurable for easier modification later
        float duration = 0.2f;
        float rot = Pivot.localRotation.y > 0 ? -45 : 45;
        float progress = 0f;
        // Loop until instructed otherwise
        while (progress <= 1f)
        {

            // Do some nice animation
            Pivot.localRotation = Quaternion.Slerp(Pivot.localRotation, Quaternion.Euler(new Vector3(0, rot, 0)), progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }
    }

    //void OnTriggerEnter(Collider triggerCollider)
    //{
    //    if (triggerCollider.tag == "Enemy") 
    //    {
    //        if (OnCubeDeath != null)
    //        {
    //            OnCubeDeath();
    //        }
    //        Destroy(gameObject);
    //    }
    //}
}
