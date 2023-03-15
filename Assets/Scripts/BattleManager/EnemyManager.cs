using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : GenericFactory<Enemy>
{
    [SerializeField]
    private int debug_swordsmanCount;
    [SerializeField]
    private int debug_spearmanCount;

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

    public List<Enemy> SpawnEnemies(int swordsmanCount, int spearmanCount)
    {
        _instance = swordsManGO.GetComponent<Enemy>();
        for (int i = 0; i < swordsmanCount; i++)
        {
            var _swordsManGO = GetNewInstance();
            _swordsManGO.Initialise(100, 2, transform.position + new Vector3(-swordsmanCount + (i * 2), 1, 15));
            pool.Add(_swordsManGO);
        }
        _instance = spearManGO.GetComponent<Enemy>();
        for (int i = 0; i < spearmanCount; i++)
        {
            var _spearManGo = GetNewInstance();
            _spearManGo.Initialise(100, 2, transform.position + new Vector3(-spearmanCount + (i * 2), 1, 10));
            pool.Add(_spearManGo);
        }
        return pool;
    }
}
