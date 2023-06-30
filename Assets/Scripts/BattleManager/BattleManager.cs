using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();
    private List<Ally> _allies = new List<Ally>();

    [SerializeField] bool DebugTargetLinesEnabledAllies = false;
    [SerializeField] bool DebugTargetLinesEnabledEnemies = false;

    private EnemyManager[] _enemyManager;
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
        _enemyManager = FindObjectsOfType<EnemyManager>();

        if (_allyManager != null)
        {
            FindAndSetAllies();
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


    List<Enemy> tmpEnemies = new List<Enemy>();
    private IEnumerator CreateEnemies()
    {
        float duration = 10f;
        for (int i = 0; i < EnemyWaves; i++)
        {
            foreach (var manager in _enemyManager)
            {
                tmpEnemies = manager.SpawnEnemies(swordsmanCount, spearmanCount);

                if (_allies.Count > 0) { SetEnemyTargets(tmpEnemies); }
                Enemies.AddRange(tmpEnemies);
                tmpEnemies.Clear();
                yield return null;
            }
            yield return new WaitForSeconds(duration);
        }
    }

    List<Ally> _freeTargets;

    private void SetEnemyTargets(List<Enemy> tmpEnemies)
    {
        Debug.Log("BATTLE MANAGER - Setting targets for enemies");
        foreach (var enemy in tmpEnemies)
        {
            enemy.gameObject.GetComponent<ICanTarget>().SetTarget(GetNearestTarget(enemy.gameObject));
            enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabledEnemies;  
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

    public Transform GetNearestTarget(GameObject _source)
    {
        //if (_allies.Count == 0 | _enemies.Count == 0) return null;

        var nearestDist = float.MaxValue;
        Transform nearestObject = null;
        Ally tempAlly = null;

        if(_freeTargets == null || _freeTargets.Count == 0)
        {
            _freeTargets = new List<Ally>(Allies);
        }

        foreach (var entity in _freeTargets)
        {
            //Vector3 offset = entity.position - transform.position;
            //float sqrLen = entity.sqrMagnitude;

            //calculates closest object
            var distance = Vector3.SqrMagnitude(_source.transform.position - entity.transform.position);
            //Debug.Log($"Source- {_source.name} Target- {entity.name} distance: {distance}");

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
        if (nearestObject != null)
        {
            //Debug.Log($"BATTLE MANAGER - Enemy {_source.name} Target- {nearestObject}");
            tempAlly = nearestObject.GetComponent<Ally>();
            _freeTargets.Remove(tempAlly);
            //if (_freeTargets.Contains(tempAlly)) Debug.Log($"Removing from free list."); _freeTargets.Remove(tempAlly); Debug.Log($"Remaining {_freeTargets.Count}");
            return nearestObject;
        }
        else
        {
            //Debug.Log($"BATTLE MANAGER - Enemy {_source.name} -No available targets");

            return null;
        }
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
