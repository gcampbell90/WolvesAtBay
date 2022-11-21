using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Swordsman : Enemy
{
    private void Start()
    {

        //var rb = GetComponent<Rigidbody>();
        //rb.mass = 0f;

        //var col = GetComponent<Collider>();
        //col.isTrigger = false;

        //transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //rb.useGravity = false;
        //VirtualMethodTest(1, "Enemy is created");


        //Move component and Attack component both getting ref to player pos
        //TODO: fix inefficient method(s)
        //AddLookAtTarget();
        //DEBUG_AddMoveComponent();
        //gameObject.AddComponent<AttackBehaviour>();

        //gameObject.GetComponent<IKillable>().ITakeDamage(5);
    }

    #region OOP test methods
    //Public method to call classes protected method for layer of control/security.
    //public void CallMustBeOverridden()
    //{
    //    MustBeOverridden();
    //}

    //Example of overridding a virtual method from the base when needed. Fetching/sending data to base methods?  
    //TODO: Find examples of actual useful implementations
    //public override void VirtualMethodTest(int testInt, string testString)
    //{
    //    base.VirtualMethodTest(testInt, testString);
    //    Debug.Log($"Output: {testInt},{testString}. Overridden Virtual method called directly from class than base. I can also add to this implementation with or without calling the base method.\n Whenever that behaviour is required/useful is still to be figured out. ");
    //}

    public override void ITakeDamage(int damage)
    {
        base.ITakeDamage(damage);

        ////Testing other behaviours
        //if(Health <= 80)
        //{
        //    DropWeapon();
        //}
    }

    //private void DropWeapon()
    //{
    //    var weapon = transform.GetChild(0);
    //    var rb = weapon.GetComponentInChildren<Rigidbody>();
    //    rb.transform.SetParent(null);
    //    rb.useGravity = true;
    //}

    private void DEBUG_AddMoveComponent()
    {
        gameObject.AddComponent<MoveToTarget>();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        Debug.Log($"{gameObject.name} hit by a " + collision.gameObject.tag);

        //replace damage with weapon/player strength/damage
        ITakeDamage(10);
    }

    #endregion
}
