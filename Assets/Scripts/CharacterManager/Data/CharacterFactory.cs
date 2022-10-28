using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public Material npc_Material;
    public Material enemy_Material;

    private void Awake()
    {
        int posX = -5;
        foreach (var character in MockCharacterDB.NPC_CharacterList)
        {
            character.ScriptComponent.Initialise(
                speed: 5,
                position: new Vector3(posX, 0.5f, 50),
                mat: npc_Material
            );
            posX += 5;
        }
        //CharacterBase<SmartNPC_Character> testSmartNPC_character1 = new CharacterBase<SmartNPC_Character>("Gary");
        //testSmartNPC_character1.ScriptComponent.Initialise(
        //    speed: 5,
        //    position: new Vector3(-5, 0.5f, 50),
        //    mat: npc_Material
        //    );
        //CharacterBase<SmartNPC_Character> testSmartNPC_character2 = new CharacterBase<SmartNPC_Character>("Cameron");
        //testSmartNPC_character2.ScriptComponent.Initialise(
        //    speed: 5,
        //    position: new Vector3(0, 0.5f, 50),
        //    mat: npc_Material
        //    );
        //CharacterBase<SmartNPC_Character> testSmartNPC_character3 = new CharacterBase<SmartNPC_Character>("Johnny");
        //testSmartNPC_character3.ScriptComponent.Initialise(
        //    speed: 5,
        //    position: new Vector3(5, 0.5f, 50),
        //    mat: npc_Material
        //    );

    }
}
