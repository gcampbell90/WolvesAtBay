using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    //public static List<TargetingSystem> Enemies = new List<TargetingSystem>();
    //public static List<TargetingSystem> Allies = new List<TargetingSystem>();
    private List<Enemy> _enemies = new List<Enemy>();
    private List<AllyScript> _allies = new List<AllyScript>();

    [SerializeField]bool DebugTargetLinesEnabled = false;
    public List<Enemy> Enemies { get
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
        _enemies = await FindEnemies();
 
        var alliesInScene = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in alliesInScene)
        {
            _allies.Add(ally.GetComponent<AllyScript>());
        }
    }

    async Task<List<Enemy>> FindEnemies()
    {
        var enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        List<Enemy> m_tmpList = new List<Enemy>();
        while(enemiesInScene.Length == 0 || !ApplicationStateManager.playMode)
        {
            enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("Looking for enemies");
            await Task.Yield();
        }
        Debug.Log("Setting Enemies");
        foreach (var enemy in enemiesInScene)
        {
            var enemyComponent = enemy.GetComponent<Enemy>();

            enemy.GetComponent<TargetingSystem>().EnableDebugLines = DebugTargetLinesEnabled;

            m_tmpList.Add(enemyComponent);
        }
        if(m_tmpList.Count > 0)
        {
            return m_tmpList;
        }
        else
        {
            return null;
        }

    }

    //TODO: Move to its own task based approach
    private void FixedUpdate()
    {
        //SetAllyTargets();
        SetEnemyTargets();
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
                    enemy.SetTarget(nearestObject.transform);
                }

            }
        }
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

}
