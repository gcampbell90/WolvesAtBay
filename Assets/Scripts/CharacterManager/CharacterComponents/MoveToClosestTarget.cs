using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClosestTarget : MonoBehaviour
{
    Transform target;
    private int _speed;

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

        //int counter = 0;
        //foreach (var whiteball in WhiteBalls)
        //{
        //    whiteball.transform.position = new Vector3(counter++, 0, 5);
        //}

        //counter = 0;
        //foreach (var blackball in BlackBalls)
        //{
        //    blackball.transform.position = new Vector3(counter++, 0, -5);
        //}
    }

    //public IEnumerator MoveAround(GameObject _go)
    //{
    //    while (true)
    //    {
    //        float _t = 0f;
    //        var randomVector = GetRandomVector();

    //        while (_t < 3)
    //        {
    //            _go.transform.position = Vector3.Lerp(_go.transform.position, randomVector, _t);

    //            _t += Time.deltaTime;
    //            yield return null;
    //        }
    //        yield return null;
    //    }
    //}

    //Vector3 GetRandomVector()
    //{
    //    float xClamped;
    //    float yClamped;
    //    float zClamped;

    //    xClamped = Random.Range(-15, 15);
    //    yClamped = Random.Range(0, 0);
    //    zClamped = Random.Range(-15, 15);

    //    return new Vector3(xClamped, yClamped, zClamped);
    //}

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

    //void Awake()
    //{
    //    if (gameObject.GetComponent<CharacterBase>() == null)
    //    {
    //        _speed = 2;
    //    }
    //    else
    //    {
    //        _speed = gameObject.GetComponent<CharacterBase>().Speed;
    //    }
    //    //Debug.Log($"Move Speed = {_speed}. if zero character base not set.");

    //    if (_speed <= 0)
    //        _speed = 5;
    //}

    //public void MoveToForwardTarget()
    //{
    //    target = GameObject.FindGameObjectWithTag("Player").transform;

    //    if (Vector3.Distance(transform.position, target.position) < 3f)
    //    {
    //        return;
    //    }


    //    // Move our position a step closer to the target.
    //    var step = _speed * Time.deltaTime; // calculate distance to move
    //    transform.position = Vector3.MoveTowards(transform.position, target.position, step);


    //    //Debug.Log("Moving to Target" + step + " " + transform.position + " " + target.position);

    //}
}

