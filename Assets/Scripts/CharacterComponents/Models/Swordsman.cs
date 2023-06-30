using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Swordsman : Enemy
{
    [SerializeField] HitBox hitbox;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        ////Testing other behaviours
        //if(Health <= 80)
        //{
        //    DropWeapon();
        //}
    }


    private void OnEnable()
    {
        IHitable _HitBox = hitbox;
        _HitBox.OnColliderHit += enemy_OnHit;
    }

    private void OnDisable()
    {
        IHitable _HitBox = hitbox;
        _HitBox.OnColliderHit -= enemy_OnHit;
    }

    private void enemy_OnHit(object sender, EventArgs e)
    {
        Debug.Log($"Subscriber receives the Ihitable event.{sender} {e}");
        TakeDamage(20);
    }

    //removed phyics(for now)
    //public override void OnCollisionStay(Collision collision)
    //{

    //    if (collision.collider.gameObject.tag != "Weapon") return;
    //    Debug.Log($"SWORDMAN - {gameObject.name} hit by a " + collision.collider.gameObject);

    //    //replace damage with weapon/player strength/damage
    //    TakeDamage(10);
    //}

    //public override void OnTriggerEnter(Collider collider)
    //{
    //    //Debug.Log($"SWORDMAN - {gameObject.name} hit by a " + collider.gameObject.tag);

    //    if (collider.gameObject.tag != "Weapon") return;
    //    GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    //replace damage with weapon/player strength/damage
    //    TakeDamage(0);
    //}

}
