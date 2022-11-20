using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    //public static List<TargetingSystem> Enemies = new List<TargetingSystem>();
    //public static List<TargetingSystem> Allies = new List<TargetingSystem>();
    private List<Enemy> _enemies = new List<Enemy>();
    private List<AllyScript> _allies = new List<AllyScript>();

    [SerializeField] bool DebugTargetLinesEnabled = false;

    CancellationTokenSource _cts;

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
    private List<AllyScript> Allies
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

    private AllyController allyController;

    private void OnEnable()
    {
        Enemy.deathRemoveEvent += RemoveEntity;
    }
    private void OnDisable()
    {
        Enemy.deathRemoveEvent -= RemoveEntity;
    }

    private void Awake()
    {
        allyController = GetComponent<AllyController>();
    }
    // Start is called before the first frame update
    async void Start()
    {
        var alliesInScene = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in alliesInScene)
        {
            _allies.Add(ally.GetComponent<AllyScript>());
        }

        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        try
        {
            _enemies = await FindEnemies(token);
        }
        catch (OperationCanceledException e)
        {
            //Debug.Log("Group Controller - Operation Cancelled" + e.Message);
        }
        finally
        {
            var enemyCount = Enemies == null ? 0 : Enemies.Count;
            //Debug.Log("GroupController - Find Enemies Task Finished - Total Enemies: " + enemyCount);
            _cts.Dispose();
        }

        if (_enemies.Count > 0 && _allies.Count > 0)
        {
            SetEnemyTargets();
        }
    }

    async Task<List<Enemy>> FindEnemies(CancellationToken token)
    {

        List<Enemy> m_tmpList = new List<Enemy>();

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        while (enemies.Length <= 0)
        {
            //Debug.Log("Group Controller - Looking for enemies");

            if (token.IsCancellationRequested)
            {
                //Debug.Log("Group Controller - FollowTheLeader Async Task has been cancelled");
                return null;
            }
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
            {
                //Debug.Log("Group Controller - Will arrive here after enemies found");
                foreach (var enemy in enemies)
                {
                    //Debug.Log("Group Controller - Setting Enemy");

                    var enemyComponent = enemy.GetComponent<Enemy>();

                    enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabled;
                    m_tmpList.Add(enemyComponent);
                }
                continue;
            }
            await Task.Yield();
        }

        //Debug.Log("Group Controller - Returning Found Array/List" + m_tmpList);

        if (m_tmpList.Count > 0)
        {
            return m_tmpList;
        }
        else
        {
            return null;
        }
    }

    private void RemoveEntity(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }

    //Targeting system - send targets to entity's targeting system
    private void SetEnemyTargets()
    {
        foreach (var enemy in Enemies)
        {
            //Debug.Log("Set enemy target");

            var nearestDist = float.MaxValue;
            Transform nearestObject = null;
            Transform nearestVisibleObject = null;

            var canAttack = false;
            foreach (var ally in _allies)
            {
                //calculates closest object
                var distance = Vector3.Distance(enemy.transform.position, ally.transform.position);
                if (distance < nearestDist)
                {
                    nearestDist = distance;
                    nearestObject = ally.transform;
                }
                //Debug.Log("Setting enemy target to" + nearestObject);

                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(enemy.transform.position, ally.transform.position, out hit))
                {
                    //checks for any collider that is not a black ball
                    if (!hit.collider.CompareTag("Ally"))
                    {
                        // Stop chasing
                        //Debug.Log("Obstacle in the way of target");
                        canAttack = false;
                        //Debug.Log("Waiting");
                    }
                    else
                    {
                        //Debug.Log("TargetFound");
                        canAttack = true;
                        //Debug.Log("Attacking");
                    }
                }
                if (!canAttack)
                {
                    nearestObject = FindNextNearestTarget(enemy.gameObject);
                }
                else if (nearestObject != null)
                {
                    Debug.Log("Group Controller -Setting Target");
                    enemy.SetTarget(nearestObject.transform);
                }
            }
        }
        return;
    }
    //private void SetAllyTargets()
    //{
    //    foreach (var ally in Allies)
    //    {
    //        var nearestDist = float.MaxValue;
    //        Transform nearestObject = null;
    //        Transform nearestVisibleObject = null;

    //        var canAttack = false;
    //        foreach (var enemy in Enemies)
    //        {
    //            //calculates closest object
    //            var distance = Vector3.Distance(ally.transform.position, enemy.transform.position);
    //            if (distance < nearestDist)
    //            {
    //                nearestDist = distance;
    //                nearestObject = enemy.transform;
    //            }

    //            //then checks if there is an object in the way
    //            RaycastHit hit;
    //            if (Physics.Linecast(ally.transform.position, enemy.transform.position, out hit))
    //            {
    //                //checks for any collider that is not a black ball
    //                if (!hit.collider.CompareTag("Enemy"))
    //                {
    //                    // Stop chasing
    //                    Debug.Log("Obstacle in the way of target");
    //                    canAttack = false;
    //                    //Debug.Log("Waiting");
    //                }
    //                else
    //                {
    //                    Debug.Log("TargetFound");

    //                    canAttack = true;
    //                    //Debug.Log("Attacking");
    //                }
    //            }
    //            if (!canAttack)
    //            {
    //                nearestObject = FindNextNearestTarget(ally.gameObject);
    //            }

    //            ally.Target = nearestObject;
    //            ally.CanAttack = canAttack;
    //        }
    //    }
    //}
    private Transform FindNextNearestTarget(GameObject _source)
    {
        var nearestDist = float.MaxValue;
        Transform nearestObject = null;
        bool canAttack = false;
        foreach (var ally in _allies)
        {
            //calculates closest object
            var distance = Vector3.Distance(_source.transform.position, ally.transform.position);

            //if when finds an object closest will check the raycast
            if (distance < nearestDist)
            {

                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(_source.transform.position, ally.transform.position, out hit))
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
                        nearestObject = ally.transform;
                        //Debug.Log("Attacking");
                    }
                }
            }
        }
        if (nearestObject == null)
        {
            //Debug.Log("No available targets");
        }


        return nearestObject;
    }

    private void OnDestroy()
    {
    }
}
