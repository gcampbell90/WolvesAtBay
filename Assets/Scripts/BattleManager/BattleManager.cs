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

    [SerializeField]
    private EnemyManager _enemyManager;
    [SerializeField]
    private AllyManager _allyManager;

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
        Player.OnDefend += DefendEvent;
        Player.OnAttack += AttackEvent;
    }


    private void OnDisable()
    {
        Enemy.deathRemoveEvent -= RemoveEntity;
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
    async void Start()
    {
        _allyManager = FindObjectOfType<AllyManager>();
        _enemyManager = FindObjectOfType<EnemyManager>();
        if (_enemyManager == null | _allyManager == null) return;
        Enemies = _enemyManager.SpawnEnemies(swordsmanCount, spearmanCount);

        FindAndSetAllies();

        if (Enemies.Count > 0)
        {
            //Debug.Log("Group Controller - Will arrive here after enemies found");
            foreach (var enemy in Enemies)
            {
                Debug.Log("BATTLE MANAGER - Setting Enemy in list");
                enemy.GetComponent<ICanTarget>().SetTarget(GetNearestTargetGeneric(enemy.gameObject));
                enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabledEnemies;
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

        if (_source.tag == "Ally")
        {
            _targetGroup = _enemies.Select(f => f.transform).ToArray();
        }
        else if (_source.tag == "Enemy")
        {
            _targetGroup = _allies.Select(f => f.transform).ToArray();
        }

        var nearestDist = float.MaxValue;
        Transform nearestObject = null;
        bool canAttack = false;

        foreach (var entity in _targetGroup)
        {
            //calculates closest object
            var distance = Vector3.Distance(_source.transform.position, entity.transform.position);

            //if when finds an object closest will check the raycast
            if (distance < nearestDist)
            {
                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(_source.transform.position, entity.transform.position, out hit))
                {
                    //checks for any collider that is not a black ball
                    if (!hit.collider.CompareTag("Enemy"))
                    {
                        // Stop chasing
                        canAttack = false;
                        //Debug.Log("Waiting");
                    }
                    else
                    {
                        canAttack = true;
                        nearestDist = distance;
                        nearestObject = entity.transform;
                    }
                }
            }
        }
        if (nearestObject == null)
        {
            //Debug.Log($"BATTLE MANAGER - Ally {_source.name} -No available targets");
            return null;
        }
        else
        {
            //Debug.Log($"BATTLE MANAGER - {gameObject.name} - Setting Target {nearestObject.name}");
            return nearestObject;
        }
    }

    private void RemoveEntity(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }

    //private void FindAndSetEnemyTargets()
    //{
    //    foreach (var enemy in Enemies)
    //    {
    //        //Debug.Log("Set enemy target");

    //        var nearestDist = float.MaxValue;
    //        Transform nearestObject = null;
    //        Transform nearestVisibleObject = null;

    //        var canAttack = false;
    //        foreach (var ally in _allies)
    //        {
    //            //calculates closest object
    //            var distance = Vector3.Distance(enemy.transform.position, ally.transform.position);
    //            if (distance < nearestDist)
    //            {
    //                nearestDist = distance;
    //                nearestObject = ally.transform;
    //            }
    //            //Debug.Log("Setting enemy target to" + nearestObject);

    //            //then checks if there is an object in the way
    //            RaycastHit hit;
    //            if (Physics.Linecast(enemy.transform.position, ally.transform.position, out hit))
    //            {
    //                //checks for any collider that is not a black ball
    //                if (!hit.collider.CompareTag("Ally"))
    //                {
    //                    // Stop chasing
    //                    //Debug.Log("Obstacle in the way of target");
    //                    canAttack = false;
    //                    //Debug.Log("Waiting");
    //                }
    //                else
    //                {
    //                    //Debug.Log("TargetFound");
    //                    canAttack = true;
    //                    //Debug.Log("Attacking");
    //                }
    //            }
    //        }

    //        if (!canAttack)
    //        {
    //            nearestObject = GetNearestTargetGeneric(enemy.gameObject);
    //        }
    //        else if (canAttack && nearestObject != null)
    //        {
    //            Debug.Log("BATTLE MANAGER - Enemy - Setting Target");
    //            enemy.SetTarget(nearestObject.transform);
    //            //ally.SetTarget(enemy.transform);
    //        }
    //    }
    //    return;
    //}
    //private void FindAndSetAllyTargets()
    //{
    //    foreach (var ally in _allies)
    //    {
    //        Transform target = null;
    //        float closestDistance = Mathf.Infinity;

    //        foreach (var enemy in _enemies)
    //        {
    //            var distance = Vector3.Distance(ally.transform.position, enemy.transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                target = enemy.transform;
    //            }
    //        }
    //        ally.SetTarget(target);
    //        //Debug.Log($"{ally} target set to {m_tmpNearestObject}");
    //    }
    //}
    //private void FindAndSetTargets()
    //{
    //    foreach (var ally in _allies)
    //    {
    //        Transform target = null;
    //        float closestDistance = Mathf.Infinity;

    //        foreach (var enemy in _enemies)
    //        {
    //            var distance = Vector3.Distance(ally.transform.position, enemy.transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                target = enemy.transform;
    //            }
    //        }
    //        ally.SetTarget(target);
    //        //Debug.Log($"{ally} target set to {m_tmpNearestObject}");
    //    }
    //}

    //private Transform GetNearestTarget(GameObject _source)
    //{
    //    var nearestDist = float.MaxValue;
    //    Transform nearestObject = null;
    //    bool canAttack = false;
    //    foreach (var ally in _allies)
    //    {
    //        //calculates closest object
    //        var distance = Vector3.Distance(_source.transform.position, ally.transform.position);

    //        //if when finds an object closest will check the raycast
    //        if (distance < nearestDist)
    //        {

    //            //then checks if there is an object in the way
    //            RaycastHit hit;
    //            if (Physics.Linecast(_source.transform.position, ally.transform.position, out hit))
    //            {
    //                //checks for any collider that is not a black ball
    //                if (!hit.collider.CompareTag("Enemy"))
    //                {
    //                    // Stop chasing
    //                    canAttack = false;
    //                    //Debug.Log("Waiting");
    //                }
    //                else
    //                {
    //                    canAttack = true;
    //                    nearestDist = distance;
    //                    nearestObject = ally.transform;
    //                    //Debug.Log("Attacking");
    //                }
    //            }
    //        }
    //    }
    //    if (nearestObject == null)
    //    {
    //        //Debug.Log("No available targets");
    //    }


    //    return nearestObject;
    //}

}
