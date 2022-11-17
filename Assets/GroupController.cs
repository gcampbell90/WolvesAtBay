using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
    public static List<TargetingSystem> Enemies = new List<TargetingSystem> ();
    public static List<TargetingSystem> Allies = new List<TargetingSystem>();

    // Start is called before the first frame update
    void Start()
    {
        var enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemiesInScene)
        {
            Enemies.Add(enemy.GetComponent<TargetingSystem>());
        }

        var alliesInScene = GameObject.FindGameObjectsWithTag("Ally");
        foreach (var ally in alliesInScene)
        {
            Allies.Add(ally.GetComponent<TargetingSystem>());
        }
    }


    private void FixedUpdate()
    {
        //SetAllyTargets();
        SetEnemyTargets();
    }

    private void SetEnemyTargets()
    {
        foreach (var enemy in Enemies)
        {
            var nearestDist = float.MaxValue;
            Transform nearestObject = null;
            Transform nearestVisibleObject = null;

            var canAttack = false;
            foreach (var ally in Allies)
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

                enemy.Target = nearestObject;
                enemy.CanAttack = canAttack;
            }
        }
    }

    private void SetAllyTargets()
    {
        foreach (var ally in Allies)
        {
            var nearestDist = float.MaxValue;
            Transform nearestObject = null;
            Transform nearestVisibleObject = null;

            var canAttack = false;
            foreach (var enemy in Enemies)
            {
                //calculates closest object
                var distance = Vector3.Distance(ally.transform.position, enemy.transform.position);
                if (distance < nearestDist)
                {
                    nearestDist = distance;
                    nearestObject = enemy.transform;
                }

                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(ally.transform.position, enemy.transform.position, out hit))
                {
                    //checks for any collider that is not a black ball
                    if (!hit.collider.CompareTag("Enemy"))
                    {
                        // Stop chasing
                        Debug.Log("Obstacle in the way of target");
                        canAttack = false;
                        //Debug.Log("Waiting");
                    }
                    else
                    {
                        Debug.Log("TargetFound");

                        canAttack = true;
                        //Debug.Log("Attacking");
                    }
                }
                if (!canAttack)
                {
                    nearestObject = FindNextNearestTarget(ally.gameObject);
                }

                ally.Target = nearestObject;
                ally.CanAttack = canAttack;
            }
        }
    }

    private Transform FindNextNearestTarget(GameObject _source)
    {
        var nearestDist = float.MaxValue;
        Transform nearestObject = null;
        bool canAttack = false;
        foreach (var ally in Allies)
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
