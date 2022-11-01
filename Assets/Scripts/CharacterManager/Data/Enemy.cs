using UnityEngine;

public class Enemy : CharacterBase
{

    private void Start()
    {
        VirtualMethod(1, "Enemy is created");
    }
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
    public override void VirtualMethod(int testInt, string testString)
    {
        base.VirtualMethod(testInt, testString);
        Debug.Log($"Output: {testInt},{testString}. Overridden Virtual method called directly from class than base. I can also add to this implementation with or without calling the base method.\n Whenever that behaviour is required/useful is still to be figured out. ");
    }

}
