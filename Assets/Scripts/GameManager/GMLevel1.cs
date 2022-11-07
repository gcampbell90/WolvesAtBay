using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLevel1 : GMLevelAbstract
{
    [SerializeField]
    CharacterFactory charFactory;

    private void Awake()
    {
        //GM = GameManager.Instance;
        base.Awake();

        charFactory.spearmanCount += totalSpearmen;
        charFactory.swordsmanCount += totalSwordsmen;
    }
    public override void OnCompletion()
    {
        //GM.Level1TotalEnemiesKilled = TotalEnemiesKilled;
    }

}