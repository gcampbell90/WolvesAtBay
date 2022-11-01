using System;
using UnityEngine;

public class SmartNPC_Character : CharacterBase
{

    private void Start()
    {
        Debug.Log("I'm a Smart Npc Created called " + this.name + ". This component was added to the new GO at runtime and I was called!");
    }

    protected override void MustBeOverridden()
    {
        throw new System.NotImplementedException();
    }

}
