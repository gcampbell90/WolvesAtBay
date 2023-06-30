using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : GenericFactory<Enemy>
{
    [SerializeField]
    [Header("BattleManager scene will overwrite these values if present")]
    private int debug_swordsmanCount;
    [SerializeField]
    private int debug_spearmanCount;
    [Header("------------")]
    public GameObject swordsManGO;
    public GameObject spearManGO;

    private List<Enemy> pool = new List<Enemy>();

    private void Start()
    {
        var _battleManager = FindObjectOfType<BattleManager>();
        if (_battleManager != null) return;
        else
        {
            SpawnEnemies(debug_swordsmanCount, debug_spearmanCount);
        }
    }

    int rows;
    [SerializeField] int cols;

    public List<Enemy> SpawnEnemies(int swordsmanCount, int spearmanCount)
    {
        CalculateSquadSize(swordsmanCount);
        //_instance = swordsManGO.GetComponent<Enemy>();
        //for (int i = 0; i < swordsmanCount; i++)
        //{
        //    var _swordsManGO = GetNewInstance();
        //    _swordsManGO.Initialise(100, 2, transform.position + new Vector3(-swordsmanCount + (i * 2), 0, 15));
        //    pool.Add(_swordsManGO);
        //}olive
        //_instance = spearManGO.GetComponent<Enemy>();
        //for (int i = 0; i < spearmanCount; i++)
        //{
        //    var _spearManGo = GetNewInstance();
        //    _spearManGo.Initialise(100, 2, transform.position + new Vector3(-spearmanCount + (i * 2), 1, 10));
        //    pool.Add(_spearManGo);
        //}
        return pool;
    }

    void CalculateSquadSize(int soldierCount)
    {
        int rowcount = soldierCount / cols;
        int rowcountmodulo = soldierCount % cols;
        int totalCount = 0;
            //(rowcount * cols) + rowcountmodulo;

        if (rowcountmodulo > 0) { rowcount++; }

        //Debug.Log($"Total Soldier squad size ={totalCount}" +
            //$" rows = {soldierCount} / {cols} = {rowcount} ");
        //if (rowcountmodulo > 0)
        //{
        //    Debug.Log($"The last row will fit only Modulus = {rowcountmodulo}");
        //}

        _instance = swordsManGO.GetComponent<Enemy>();

        for (int i = 0; i < rowcount; i++)
        {
            //Debug.Log($"row {i}");
            for (int j = 0; j < cols; j++)
            {
                if (totalCount == soldierCount) break;

                var _swordsManGO = GetNewInstance();
                _swordsManGO.Initialise(100, 1, transform.position + new Vector3(-cols + (j * 2), 0, i));
                pool.Add(_swordsManGO);
                totalCount++;
                //Debug.Log($"col {j} row {i} - Total Spawned {totalCount}");
            }
        }
    }
}
