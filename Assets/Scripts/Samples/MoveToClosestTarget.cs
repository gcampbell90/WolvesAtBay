using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClosestTarget : MonoBehaviour
{
    [SerializeField] int CountWhite = 0;
    [SerializeField] int CountBlack = 0;
    protected List<TargetingSystem> WhiteBalls = new List<TargetingSystem>();
    protected List<TargetingSystem> BlackBalls = new List<TargetingSystem>();
    [SerializeField] GameObject BlackSphere;
    [SerializeField] GameObject WhiteSphere;

    private void Start()
    {
        for (int i = 0; i < CountWhite; i++)
        {
            WhiteBalls.Add(Instantiate(WhiteSphere).GetComponent<TargetingSystem>());
            WhiteBalls[i].tag = "WhiteBalls";
        }
        for (int i = 0; i < CountBlack; i++)
        {
            BlackBalls.Add(Instantiate(BlackSphere).GetComponent<TargetingSystem>());
            BlackBalls[i].transform.position = new Vector3(i, 0, -5);
            BlackBalls[i].tag = "BlackBalls";
        }
    }

    private void Update()
    {
        foreach (var whiteball in WhiteBalls)
        {
            var nearestDist = float.MaxValue;
            Transform nearestObject = null;
            Transform nearestVisibleObject = null;

            var canAttack = false;
            foreach (var blackball in BlackBalls)
            {
                //calculates closest object
                var distance = Vector3.Distance(whiteball.transform.position, blackball.transform.position);
                if (distance < nearestDist)
                {
                    nearestDist = distance;
                    nearestObject = blackball.transform;
                }

                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(whiteball.transform.position, blackball.transform.position, out hit))
                {
                    //checks for any collider that is not a black ball
                    if (!hit.collider.CompareTag("BlackBalls"))
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
                    nearestObject = FindNextNearestTarget(whiteball.gameObject);
                }

                whiteball.Target = nearestObject;
                whiteball.CanAttack = canAttack;
            }
        }
    }

    private Transform FindNextNearestTarget(GameObject whiteball)
    {
        var nearestDist = float.MaxValue;
        Transform nearestObject = null;
        bool canAttack = false;
        foreach (var blackball in BlackBalls)
        {
            //calculates closest object
            var distance = Vector3.Distance(whiteball.transform.position, blackball.transform.position);

            //if when finds an object closest will check the raycast
            if (distance < nearestDist)
            {

                //then checks if there is an object in the way
                RaycastHit hit;
                if (Physics.Linecast(whiteball.transform.position, blackball.transform.position, out hit))
                {
                    //checks for any collider that is not a black ball
                    if (!hit.collider.CompareTag("BlackBalls"))
                    {
                        // Stop chasing
                        canAttack = false;
                        //Debug.Log("Waiting");
                    }
                    else
                    {
                        canAttack = true;
                        nearestDist = distance;
                        nearestObject = blackball.transform;
                        //Debug.Log("Attacking");
                    }
                }
            }
        }
        if (nearestObject == null)
        {
            Debug.Log("No available targets");
        }


        return nearestObject;
    }

    public void Battle()
    {
        StartCoroutine(BattleSpheres());
    }

    public void StopBattle()
    {
        StopAllCoroutines();
    }

    private IEnumerator BattleSpheres()
    {
        var _t = 0f;

        while (_t < 2f)
        {
            foreach (var whiteBall in WhiteBalls)
            {

            }
            _t += Time.deltaTime;
            yield return null;
        }
    }
}

