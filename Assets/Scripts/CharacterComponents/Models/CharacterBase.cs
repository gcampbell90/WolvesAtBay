using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Base Character Class - Root of all characters
public abstract class CharacterBase : MonoBehaviour, IKillable
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Speed { get; set; }
    public GameObject Model { get; set; }

    /*
     * NOTE: You should use Awake to set up references between scripts, and use Start, which is called after all Awake calls are finished, to pass any information back and forth.
     */
    void Awake()
    {
        Debug.Log("CharacterBase awake method - the monobehaviour is in the base class so all inherited members should also have inherit monobehaviour class");
    }

    //Initialisation
    public virtual void Initialise(int health, int speed, Vector3 position)
    {
        Name = gameObject.name;
        Speed = speed;
        Health = health;
        gameObject.transform.position = position;
    }

    #region Behaviours
    //Adding Behaviour Components and abstract behaviours - how to decide to limit monobehaviour count?
    //IKillable interface methods
    public abstract void OnCollisionStay(Collision collision);
    //public abstract void OnTriggerEnter(Collider collision);
    public abstract void TakeDamage(int damage);
    public void Destroy()
    {
        Destroy(gameObject);
    }
    //This is an example of an method that can be optionally overriden by inherited members.
    //public virtual void VirtualMethodTest(int testInt, string testString)
    //{
    //    Debug.Log("This is an overriden(virtual) method call from the base.");
    //}

    //Mandatory Override example
    //protected abstract void MustBeOverridden();

    #endregion
}

