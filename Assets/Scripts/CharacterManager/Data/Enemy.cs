using UnityEngine;

public class Enemy : CharacterBase
{
    public void Initialise(int speed, Vector3 position)
    {
        speed -= 5;
        position = new Vector3(0, 0, 10);
    }

    protected override void MustBeOverridden()
    {
        Debug.Log("Enemy Class overridden");
    }
}
