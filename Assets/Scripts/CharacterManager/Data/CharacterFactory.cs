using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    private void Awake()
    {
        CharacterBase<SmartNPC_Character> testSmartNPC_character = new CharacterBase<SmartNPC_Character>("Johnny");
        testSmartNPC_character.ScriptComponent.Initialise(
            speed: 5,
            position: new Vector3(1, 0, 0)
            );
    }
}
