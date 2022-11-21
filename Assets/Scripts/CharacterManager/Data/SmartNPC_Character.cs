using System;
using UnityEngine;

public class SmartNPC_Character : CharacterBase
{

    private void Start()
    {
        Debug.Log("I'm a Smart Npc Created called " + this.name + ". This component was added to the new GO at runtime and I was called!");
    }

    public override void ITakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
    }
}
