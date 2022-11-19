using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public partial class MoveToTarget : MonoBehaviour
{

    private Transform target;
    public Transform Target { get
        {
            return target; 
        }
        set {
            target = value;
        }
    }
    private int _speed;

    Task MoveToTask;
    CancellationTokenSource _cancellationTokenSource = null;

    //private void OnEnable()
    //{
    //    Enemy.deathEvent += OnKilled;
    //}
    //private void OnDisable()
    //{
    //    Enemy.deathEvent -= OnKilled;
    //}

    private void Awake()
    {
        if (gameObject.GetComponent<CharacterBase>() == null)
        {
            _speed = 2;
        }
        else
        {
            _speed = gameObject.GetComponent<CharacterBase>().Speed;
        }
        //Debug.Log($"Move Speed = {_speed}. if zero character base not set.");

        if (_speed <= 0)
            _speed = 5;

    }

    private async void Start()
    {
        //set up cancellation token for move task
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        MoveToTask = MoveToTargetOverTime(token);

        try
        {
            await MoveToTask;
        }
        catch (OperationCanceledException e)
        {
            //Debug.Log($"Movement Cancelled {e.Message}");
        }
        finally
        {
            //Debug.Log("Destination Reached");
            _cancellationTokenSource.Dispose();
        }

    }

    public async Task MoveToTargetOverTime(CancellationToken token)
    {
        while (Target == null)
        {  
            Target = GetComponent<TargetingSystem>().Target;
            await Task.Yield();
        } 
        float distance = Vector3.Distance(transform.position, Target.position);
        while (distance > 3)
        {
            distance = Vector3.Distance(transform.position, Target.position);

            // Move our position a step closer to the target.
            var step = _speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Target.position, step);

            if (token.IsCancellationRequested)
            {
                //Debug.Log("Task Cancelled");

                return;

                //throw exception?
                //token.ThrowIfCancellationRequested();
            }
            await Task.Yield();
        }
        //Debug.Log("Destination Arrived");
    }
    public async Task OnKilled()
    {
        //Debug.Log("MoveToTarget cleanup");
        if (MoveToTask.IsCompleted) return;
        _cancellationTokenSource.Cancel();
        while (!MoveToTask.IsCanceled)
        {
            await Task.Yield();
        }
    }



}
