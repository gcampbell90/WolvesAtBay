using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swordsman : Enemy
{

    public override void ITakeDamage(int damage)
    {
        base.ITakeDamage(damage);

        ////Testing other behaviours
        //if(Health <= 80)
        //{
        //    DropWeapon();
        //}
    }

    //removed phyics(for now)
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        Debug.Log($"SWORDMAN - {gameObject.name} hit by a " + collision.gameObject.tag);
        
        //replace damage with weapon/player strength/damage
        ITakeDamage(10);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag != "Weapon") return;
        //Debug.Log($"SWORDMAN - {gameObject.name} hit by a " + collider.gameObject.tag);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        //replace damage with weapon/player strength/damage
        ITakeDamage(30);
    }

}
