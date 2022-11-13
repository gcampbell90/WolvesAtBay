using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int speed; //Speed of the player cube

    //public float mapHeight = 5f; //width of the movable area
    //public float mapWidth = 5f; //width of the movable area
    //public event System.Action OnCubeDeath; //cube death event

    //private Rigidbody rb;

    //float screenHalfWidthInWorldUnits
    public Transform Pivot { get; set; }
    private void Awake()
    {
        Pivot = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
        Pivot.rotation = Quaternion.Euler(0, -70, 0);
    }
    private void Start()
    {
        //set particle system
        EffectController.swordTrail = Pivot.GetComponentInChildren<ParticleSystem>();
        speed = gameObject.GetComponent<CharacterBase>().Speed;
    }

    void Update()
    {
        //Keyboard input and player movement via transform
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.position += move * Time.deltaTime * speed;

        //Mouse Input
        var cam = Camera.main;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        // Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 1, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
        
        //Rotating the object to that point
        transform.LookAt(finalPoint, Vector3.up);

        if (Input.GetKeyDown(KeyCode.Mouse0))
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
        float rot = 40;
        float progress = 0f;

        //Play the sword swing audio from the effect controller
        EffectController.Instance.PlaySwordSound();
        EffectController.Instance.EnableSwordTrail();

        // Loop until instructed otherwise
        while (progress <= 1f)
        {
            // Do some nice animation
            Pivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }
        EffectController.Instance.DisableSwordTrail();

        Pivot.localRotation = Quaternion.Euler(0, -70, 0);
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
