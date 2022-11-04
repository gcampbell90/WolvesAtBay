using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterBase
{
    private void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.mass = 0f;

        var col = GetComponent<Collider>();
        col.isTrigger = false;

        //rb.useGravity = false;
        //VirtualMethodTest(1, "Enemy is created");
        //AddLookAtTarget();
        //DEBUG_AddMoveComponent();

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
        Debug.Log($"{gameObject.name} Damage: {damage}");
        Health -= damage;
    }

    private void DEBUG_AddMoveComponent()
    {
        gameObject.AddComponent<MoveToTarget>();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{gameObject.name} have been hit from {collision}");

        ITakeDamage(5);
    }

    //Attaches additional component to Enemy type GO, is this the right place for it?

    #endregion
}

