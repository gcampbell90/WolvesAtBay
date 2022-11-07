using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevel1 : GMLevelAbstract
{
    [SerializeField]
    CharacterFactory charFactory;

    private void Awake()
    {
        charFactory.spearmanCount += TotalEnemies;
        charFactory.swordsmanCount += TotalEnemies;
    }
    public override void OnCompletion()
    {
        //GM.Level1TotalEnemiesKilled = TotalEnemiesKilled;
    }

}