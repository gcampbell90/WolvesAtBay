using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public int npcCount;
    public int enemyCount;
    public Material npc_Material;
    public Material enemy_Material;

    /*
     * Should go into a start method rather? Or is awake fine for initialisation?
     */
    private void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            CharacterBase<SmartNPC_Character> testSmartNPC_character = new CharacterBase<SmartNPC_Character>($"TestNPC_{i}");
            testSmartNPC_character.ScriptComponent.Initialise(
                health: 100,
                speed: 5,
                position: new Vector3(-npcCount + (i * 2), 0.5f, 20),
                mat: npc_Material
            );
        }
        for (int i = 0; i < enemyCount; i++)
        {
            CharacterBase<Enemy> testEnemy_character = new CharacterBase<Enemy>($"TestEnemy_{i}");
            testEnemy_character.ScriptComponent.Initialise(
                health: 100,
                speed: 2,
                position: new Vector3(-enemyCount + (i * 2), 0.5f, 15),
                mat: enemy_Material
            );
        }

        //TODO: Update test Methods to accept newly created character to initialise instead of in awake method
        //CreateNPCs();
        //CreateEnemies();
    }

    #region TestMethods
    private void CreateNPCs()
    {
        int posX = -10;
        foreach (var character in MockCharacterDB.NPC_CharacterList)
        {
            character.ScriptComponent.Initialise(
                health: 100,
                speed: 5,
                position: new Vector3(posX, 0.5f, 70),
                mat: npc_Material);
            posX += 2;
        }

    }

    private void CreateEnemies()
    {
        int posX = -5;
        foreach (var character in MockCharacterDB.Enemy_CharacterList)
        {
            character.ScriptComponent
                .Initialise(
                    health: 100,
                    speed: 5,
                    position: new Vector3(posX, 0.5f, 50),
                    mat: enemy_Material
                );
            posX += 5;
        }
    }
    #endregion
}
