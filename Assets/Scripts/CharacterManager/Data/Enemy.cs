using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterBase
{
    public delegate void DeathEvent();
    public static event DeathEvent deathEvent;

    public delegate void DeathRemoveEvent(Enemy enemy);
    public static event DeathRemoveEvent deathRemoveEvent;

    //[SerializeField]
    //GMLevelAbstract gmLevelAbstract;

    public void Awake()
    {

        //place into Enemy layer for physics etc
        gameObject.layer = 7;
        //give enemy tag for in game behaviours and finding
        gameObject.tag = "Enemy";

        //gmLevelAbstract = FindObjectOfType<GMLevelAbstract>();

        var rb = GetComponent<Rigidbody>();
        rb.mass = 30f;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX|
            RigidbodyConstraints.FreezeRotationZ;


        var col = GetComponent<Collider>();
        col.isTrigger = false;

        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        gameObject.AddComponent<TargetingSystem>();
        gameObject.AddComponent<LookAtTarget>();
        gameObject.AddComponent<MoveToTarget>();
        gameObject.AddComponent<AttackBehaviour>();

    }

    private void Start()
    {
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
        //Debug.Log($"{gameObject.name} Damage: {damage}");
        Health -= damage;
        if(Health <= 0)
        {
            EnemyDeath();
        }
    }

    private async void EnemyDeath()
    {
        //Debug.Log("Enemy Death async task cleanup");

        //EnemyDeath Cleanup
        var moveComponent = GetComponent<MoveToTarget>();
        //var task = moveComponent.OnKilled();
        GetComponent<IKillable>().Destroy();

    }

    public void SetTarget(Transform target)
    {
        //Debug.Log("Setting target" + target);
        var targetSys = GetComponent<TargetingSystem>();
        //if (targetSys != null || targetSys.Target != null) return;
        targetSys.Target = target;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        Debug.Log($"{gameObject.name} hit by a " + collision.gameObject.tag);

        //replace damage with weapon/player strength/damage
        ITakeDamage(20);
    }

    private void OnDestroy()
    {
        deathEvent?.Invoke();
        deathRemoveEvent?.Invoke(this);
    }
    //GMLevelAbstract gmLevelAbstract;

    #endregion
}

