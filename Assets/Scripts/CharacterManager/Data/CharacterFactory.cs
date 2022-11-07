using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : MonoBehaviour
{
    public int npcCount;
    public int swordsmanCount;
    public int spearmanCount;
    public Material npc_Material;
    public Material enemy_Material;

    public GameObject swordsManGO;
    public GameObject spearManGO;

    /*
     * Should go into a start method rather? Or is awake fine for initialisation?
     */
    public void SpawnCharacters()
    {
        for (int i = 0; i < swordsmanCount; i++)
        {
            var _swordsManGO = Instantiate(swordsManGO);
            CharacterBase<Swordsman> testSwordsman_character = new CharacterBase<Swordsman>($"TestSwordsman_{i}", _swordsManGO);
            testSwordsman_character.ScriptComponent.Initialise(
                health: 100,
                speed: 2,
                position: transform.position + new Vector3(-swordsmanCount + (i * 2), 0.5f, 15));
        }

        for (int i = 0; i < spearmanCount; i++)
        {
            var _spearManGO = Instantiate(spearManGO);
            CharacterBase<Spearman> testSpearman_character = new CharacterBase<Spearman>($"TestSpearman_{i}", _spearManGO);
            testSpearman_character.ScriptComponent.Initialise(
                health: 100,
                speed: 2,
                position: transform.position + new Vector3(-spearmanCount + (i * 2), 0.5f, 10)                
            );
        }

        //TODO: Update test Methods to accept newly created character to initialise instead of in awake method
        //CreateNPCs();
        //CreateEnemies();
    }

    #region TestMethods
    //private void CreateNPCs()
    //{
    //    int posX = -10;
    //    foreach (var character in MockCharacterDB.NPC_CharacterList)
    //    {
    //        character.ScriptComponent.Initialise(
    //            health: 100,
    //            speed: 5,
    //            position: new Vector3(posX, 0.5f, 70));
    //        posX += 2;
    //    }

    //}

    //private void CreateEnemies()
    //{
    //    int posX = -5;
    //    foreach (var character in MockCharacterDB.Enemy_CharacterList)
    //    {
    //        character.ScriptComponent
    //            .Initialise(
    //                health: 100,
    //                speed: 5,
    //                position: new Vector3(posX, 0.5f, 50));
    //        posX += 5;
    //    }
    //}
    #endregion
}
