using UnityEngine;

public class SmartNPC_Character : CharacterBase
{
    private void Start()
    {
        Debug.Log("Smart Npc Created");
    }
    protected override void MustBeOverridden()
    {
        throw new System.NotImplementedException();
    }
}
