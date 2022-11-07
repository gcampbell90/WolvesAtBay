using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterBase
{
    public delegate void DeathEvent();
    public static event DeathEvent event_death;

    //[SerializeField]
    //GMLevelAbstract gmLevelAbstract;
    private void OnEnable()
    {
        //GMLevelAbstract.event_death += EnemyDeath;
    }

    private void OnDisable()
    {
        //GMLevelAbstract.event_death -= EnemyDeath
        event_death?.Invoke();
    }

    private void Awake()
    {
        Speed = 5;
        Health = 20;

        //gmLevelAbstract = FindObjectOfType<GMLevelAbstract>();
    }

    private void Start()
    {
        //place into Enemy layer for physics etc
        gameObject.layer = 7;
        var rb = GetComponent<Rigidbody>();
        rb.mass = 50f;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;

        var col = GetComponent<Collider>();
        col.isTrigger = false;

        transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
        //rb.useGravity = false;
        //VirtualMethodTest(1, "Enemy is created");


        //Move component and Attack component both getting ref to player pos
        //TODO: fix inefficient method(s)
        AddLookAtTarget();
        DEBUG_AddMoveComponent();
        gameObject.AddComponent<AttackBehaviour>();

        // also has ref to player pos



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

    public override void AddLookAtTarget()
    {
        base.AddLookAtTarget();
    }

    public override void ITakeDamage(int damage)
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
        //gmLevelAbstract.EnemyKilled();
        //gmLevelAbstract.CheckIfAllEnemiesKilled();

        Debug.Log($"{name} Killed");
        GetComponent<IKillable>().Destroy();

        //var gm = GameObject.FindObjectOfType<GMLevelAbstract>();
        //gm.GetComponent<GMLevelAbstract>().EnemyKilled();

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
        if (collision.gameObject.name != "Sword") return;
        if (collision.gameObject.tag != "Player") return;
        //Debug.Log("Player Hit! " + collision.gameObject.tag);

        //replace damage with weapon/player strength/damage
        ITakeDamage(10);
    }

    #endregion
}

