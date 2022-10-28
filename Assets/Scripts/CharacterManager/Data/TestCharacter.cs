using UnityEngine;

public class TestCharacter : CharacterBase
{
    //public override void Initialise(int speed)
    //{
    //    speed += 10;
    //}
    protected override void MustBeOverridden()
    {
        Debug.Log("TestCharacter Class overridden");
    }
}