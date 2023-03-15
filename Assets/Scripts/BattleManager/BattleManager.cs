using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    private List<Ally> _allies = new List<Ally>();

    [SerializeField] bool DebugTargetLinesEnabledAllies = false;
    [SerializeField] bool DebugTargetLinesEnabledEnemies = false;

    private EnemyManager _enemyManager;
    private AllyManager _allyManager;

    [Header("Enemy Setup")]
    [SerializeField]
    private int EnemyWaves;
    [SerializeField]
    private int swordsmanCount;
    public int SwordsmanCount
    {
        get { return swordsmanCount; }
        set { swordsmanCount = value; }
    }

    [SerializeField]
    private int spearmanCount;
    public int SpearmanCount
    {
        get { return spearmanCount; }
        set { spearmanCount = value; }
    }

    public List<Enemy> Enemies
    {
        get
        {
            return _enemies;
        }
        set
        {
            _enemies = value;
        }
    }
    private List<Ally> Allies
    {
        get
        {
            return _allies;
        }
        set
        {
            _allies = value;
        }
    }

    CancellationTokenSource _cts;

    public static BattleManager Instance;

    private void OnEnable()
    {
        Enemy.deathRemoveEvent += RemoveEntity;
        Ally.OnDeathRemoveEvent += RemoveAlly;
        Player.OnDefend += DefendEvent;
        Player.OnAttack += AttackEvent;
    }


    private void OnDisable()
    {
        Enemy.deathRemoveEvent -= RemoveEntity;
        Ally.OnDeathRemoveEvent -= RemoveAlly;
        Player.OnDefend -= DefendEvent;
        Player.OnAttack -= AttackEvent;
    }

    private void AttackEvent()
    {
        AllyManager.OnAttackCommand?.Invoke();
    }

    private void DefendEvent()
    {
        AllyManager.OnDefendCommand?.Invoke();
    }


    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        _allyManager = FindObjectOfType<AllyManager>();
        _enemyManager = FindObjectOfType<EnemyManager>();

        if (_allyManager != null)
        {
            FindAndSetAllies();
        }
        else
        {
            Debug.Log("AllyManager null");
        }


        if (_enemyManager != null)
        {
            if (EnemyWaves > 0)
            {
                StartCoroutine(CreateEnemies());
            }
        }

        //_cts = new CancellationTokenSource();
        //var token = _cts.Token;
        //try
        //{
        //    _enemies = await FindAndSetEnemies(token);
        //}
        //catch (OperationCanceledException e)
        //{
        //    //Debug.Log("Group Controller - Operation Cancelled" + e.Message);
        //}
        //finally
        //{
        //    var enemyCount = Enemies == null ? 0 : Enemies.Count;
        //    Debug.Log("BATTLE MANAGER  - Find Enemies Task Finished - Total Enemies: " + enemyCount);
        //    _cts.Dispose();
        //}

        //if (_enemies.Count > 0 && _allies.Count > 0)
        //{
        //    FindAndSetEnemyTargets();
        //    FindAndSetAllyTargets();
        //}
    }



    private IEnumerator CreateEnemies()
    {
        float duration = 3f;
        for (int i = 0; i < EnemyWaves; i++)
        {
            Enemies = _enemyManager.SpawnEnemies(swordsmanCount, spearmanCount);
            if (_allies.Count > 0) { SetEnemyTarget(); }
            yield return new WaitForSeconds(duration);
        }
    }
    private void SetEnemyTarget()
    {
        if (Enemies.Count > 0)
        {
            //Debug.Log("Group Controller - Will arrive here after enemies found");
            foreach (var enemy in Enemies)
            {
                Debug.Log("BATTLE MANAGER - Setting targets for enemies");
                enemy.GetComponent<ICanTarget>().SetTarget(GetNearestTargetGeneric(enemy.gameObject));
                enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabledEnemies;
            }
        }
    }

    private void FindAndSetAllies()
    {
        var alliesInScene = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in alliesInScene)
        {
            _allies.Add(ally.GetComponent<Ally>());
            //ally.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabledAllies;
        }
    }

    //async Task<List<Enemy>> FindAndSetEnemies(CancellationToken token)
    //{
    //    List<Enemy> m_tmpList = new List<Enemy>();

    //    var enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    while (enemies == null || m_tmpList.Count == 0 || !ApplicationStateManager.playMode)
    //    {
    //        Debug.Log("BATTLE MANAGER - Looking for enemies");

    //        if (token.IsCancellationRequested)
    //        {
    //            //Debug.Log("Group Controller - FollowTheLeader Async Task has been cancelled");
    //            return null;
    //        }
    //        enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //        if (enemies.Length > 0)
    //        {
    //            //Debug.Log("Group Controller - Will arrive here after enemies found");
    //            foreach (var enemy in enemies)
    //            {
    //                Debug.Log("BATTLE MANAGER - Setting Enemy in list");

    //                var enemyComponent = enemy.GetComponent<Enemy>();

    //                enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabledEnemies;
    //                m_tmpList.Add(enemyComponent);
    //            }

    //            continue;
    //        }
    //        await Task.Yield();
    //    }

    //    //Debug.Log("Group Controller - Returning Found Array/List" + m_tmpList);

    //    if (m_tmpList.Count > 0)
    //    {
    //        return m_tmpList;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    public Transform GetNearestTargetGeneric(GameObject _source)
    {
        Transform[] _targetGroup = null;

        if (_source.tag == "Ally" && _enemies.Count > 0)
        {
            _targetGroup = _enemies.Select(f => f.transform).ToArray();
        }
        else if (_source.tag == "Enemy" && _allies.Count > 0)
        {
            _targetGroup = _allies.Select(f => f.transform).ToArray();
        }

        var nearestDist = float.MaxValue;
        Transform nearestObject = null;

        foreach (var entity in _targetGroup)
        {
            //Vector3 offset = entity.position - transform.position;
            //float sqrLen = entity.sqrMagnitude;

            //calculates closest object
            var distance = Vector3.SqrMagnitude(_source.transform.position - entity.transform.position);
            //Debug.Log($"{_source.name} {entity.name} distance: {distance}");

            if (distance < nearestDist)
            {
                nearestDist = distance;
                nearestObject = entity.transform;
            }
            //    Debug.Log($"Distance from {_source.name} to {entity.name} is {distance}");

            //    //if when finds an object closest will check the raycast
            //    if (distance < nearestDist)
            //    {
            //        //then checks if there is an object in the way
            //        RaycastHit hit;
            //        if (Physics.Linecast(_source.transform.position, entity.transform.position, out hit))
            //        {
            //            //checks for any collider that is not a black ball
            //            //if (!hit.collider.CompareTag("Enemy"))
            //            //{
            //            //    // Stop chasing
            //            //    //Debug.Log("Waiting");
            //            //}
            //            //else
            //            //{
            //            //    nearestDist = distance;
            //            //    nearestObject = entity.transform;
            //            //}
            //        }
            //    }
            //    Debug.Log($"BATTLE MANAGER - Target for {_source.name} set to {entity.name}. Allies size = {_targetGroup.Length}");
            //}
            //if (nearestObject == null)
            //{
            //    //Debug.Log($"BATTLE MANAGER - Ally {_source.name} -No available targets");
            //    return null;
            //}
            //else
            //{
            //    //Debug.Log($"BATTLE MANAGER - {gameObject.name} - Setting Target {nearestObject.name}");
            //}
        }
        return nearestObject;
    }

    private void RemoveEntity(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
    private void RemoveAlly(Ally ally)
    {
        Allies.Remove(ally);
    }
}
