using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MockCharacterDB 
{

    static CharacterBase<SmartNPC_Character> smartNPC_Character1 = new CharacterBase<SmartNPC_Character>("Gary");
    static CharacterBase<SmartNPC_Character> smartNPC_Character2 = new CharacterBase<SmartNPC_Character>("Cameron");
    static CharacterBase<SmartNPC_Character> smartNPC_Character3 = new CharacterBase<SmartNPC_Character>("John");

    public static List<CharacterBase<SmartNPC_Character>> NPC_CharacterList = new List<CharacterBase<SmartNPC_Character>>() { smartNPC_Character1, smartNPC_Character2, smartNPC_Character3 };

}
