using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevel3 : GMLevelAbstract
{
    [SerializeField]
    CharacterFactory charFactory;

    private void Awake()
    {
        base.Awake();

        charFactory.spearmanCount += TotalEnemies;
        charFactory.swordsmanCount += TotalEnemies;
    }

    public override void OnCompletion()
    {
        //GM.Level3TotalEnemiesKilled = TotalEnemiesKilled;
    }
}
