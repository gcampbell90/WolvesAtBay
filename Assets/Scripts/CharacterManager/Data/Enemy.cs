using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterBase
{
    private void Awake()
    {
        Speed = 5;
        Health = 20;
    }

    private void Start()
    {

        var rb = GetComponent<Rigidbody>();
        rb.mass = 0f;

        var col = GetComponent<Collider>();
        col.isTrigger = false;

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
            Debug.Log($"{name} Killed");
            Destroy(gameObject);
        }
    }

    private void DEBUG_AddMoveComponent()
    {
        gameObject.AddComponent<MoveToTarget>();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{gameObject.name} has been hit by {collision.gameObject.name}");

        ITakeDamage(5);
    }

    #endregion
}

