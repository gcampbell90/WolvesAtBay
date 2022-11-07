using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GMLevel2 : GMLevelAbstract
{
    [SerializeField]
    CharacterFactory charFactory;

    [SerializeField] private int SwordsmenCount;
    [SerializeField] private int SpearmenCount;

    private void Awake()
    {
        TotalSwordsmen = SwordsmenCount;
        TotalSpearmen = SpearmenCount;

        base.Awake();

        charFactory.swordsmanCount = TotalSwordsmen;
        charFactory.spearmanCount = TotalSpearmen;

        charFactory.SpawnCharacters();

    }

    public override void OnCompletion()
    {
        Debug.Log("Level Completed");

        GM.Level2TotalEnemiesKilled = TotalEnemiesKilled;
    }
}