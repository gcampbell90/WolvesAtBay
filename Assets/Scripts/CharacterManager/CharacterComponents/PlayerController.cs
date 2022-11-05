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
        Pivot.rotation = Quaternion.Euler(0, -45, 0);
    }
    private void Start()
    {
        speed = gameObject.GetComponent<CharacterBase>().Speed;
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.position += move * Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(move);
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(AttackMove());
            //print("space key was pressed");
        }

    }

    //TODO: Turn into animation call/method...abstract away from player controller.
    private IEnumerator AttackMove()
    {
        // Just make the animation interval configurable for easier modification later
        float duration = 0.1f;
        float rot = 25;
        float progress = 0f;
        // Loop until instructed otherwise
        while (progress <= 1f)
        {

            // Do some nice animation
            Pivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -45, 0), Quaternion.Euler(0, rot, 0), progress);
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
