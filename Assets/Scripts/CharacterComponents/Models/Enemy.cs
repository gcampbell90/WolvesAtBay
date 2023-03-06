using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class Enemy : CharacterBase
{
    [SerializeField]private BoxCollider _colliderBlocker;

    public delegate void DeathEvent();
    public static event DeathEvent deathEvent;

    public delegate void DeathRemoveEvent(Enemy enemy);
    public static event DeathRemoveEvent deathRemoveEvent;


    public void Awake()
    {

        //place into Enemy layer for physics etc
        gameObject.layer = 7;
        //give enemy tag for in game behaviours and finding
        gameObject.tag = "Enemy";

        //gmLevelAbstract = FindObjectOfType<GMLevelAbstract>();

        var rb = GetComponent<Rigidbody>();
        rb.mass = 500f;
        rb.drag = 5f;

        rb.constraints = RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        rb.isKinematic = false;

        var col = GetComponent<Collider>();
        col.isTrigger = false;

        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        gameObject.AddComponent<TargetingSystem>();
        //gameObject.AddComponent<TargetController>();
        gameObject.AddComponent<MovementController>();
        gameObject.AddComponent<CombatController>();


        //Physics.IgnoreCollision(col, _colliderBlocker, true);

    }
    public override void TakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name} Damage: {damage}");
        Health -= damage;
        if(Health <= 0)
        {
            EnemyDeath();
        }
    }
    private void EnemyDeath()
    {
        deathEvent?.Invoke();
        deathRemoveEvent?.Invoke(this);
        GetComponent<IKillable>().Destroy();
    }
    //public void SetTarget(Transform target)
    //{
    //    Debug.Log("Setting target" + target);
    //    GetComponent<ICanTarget>().SetTarget(target);
    //}
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        Debug.Log($"{gameObject.name} hit by a " + collision.gameObject.tag);

        //replace damage with weapon/player strength/damage
        TakeDamage(20);
    }
    public override void OnTriggerEnter(Collider collision)
    {
        throw new NotImplementedException();
    }

}

