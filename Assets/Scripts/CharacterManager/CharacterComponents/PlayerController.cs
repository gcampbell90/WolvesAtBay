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
    private void Start()
    {
        speed = gameObject.GetComponent<CharacterBase>().Speed;
    }

    void FixedUpdate()
    { 
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        gameObject.transform.position += move * Time.deltaTime * speed;

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
