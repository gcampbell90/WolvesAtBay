using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : CharacterBase , IMoveable
{
    private void Start()
    {
        //VirtualMethodTest(1, "Enemy is created");
        AddLookAtTarget();
        DEBUG_AddMoveComponent();

        gameObject.GetComponent<IKillable>().ITakeDamage(5);
    }

    #region OOP test methods
    //Public method to call classes protected method for layer of control/security.
    public void CallMustBeOverridden()
    {
        MustBeOverridden();
    }

    protected override void MustBeOverridden()
    {
        Debug.Log($"This is a mandatory override method for {name}- this method should have no implementation in the base class as its inheriting members will need to provide this functionality.");
        //throw new System.NotImplementedException();
    }

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
        Debug.Log($"{gameObject.name} have taken damage of {damage}");
        Health -= damage;
    }

    private void DEBUG_AddMoveComponent()
    {
        gameObject.AddComponent<MoveToTarget>();
    }

    //Attaches additional component to Enemy type GO, is this the right place for it?

    #endregion
}

