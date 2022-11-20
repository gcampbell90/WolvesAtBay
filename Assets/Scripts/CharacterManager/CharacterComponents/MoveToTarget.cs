using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class MoveToTarget : MonoBehaviour
{

    private Transform target;
    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
    private int _speed;

    Task MoveToTask;
    CancellationTokenSource _cts = null;
    CancellationTokenSource _childcts = null;

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
        _cts = new CancellationTokenSource();
        var token = _cts.Token; 
        CancellationTokenSource.CreateLinkedTokenSource(token);
        
        try
        {
            await (MoveToTask = MoveToTargetOverTime(token));
        }
        catch (OperationCanceledException e)
        {
            //Debug.Log($"Movement Cancelled {e.Message}");
        }
        finally
        {
            //Debug.Log("Destination Reached");
            _cts.Dispose();
        }
    }

    public async Task MoveToTargetOverTime(CancellationToken token)
    {
        while (Target == null)
        {
            Debug.Log("Move to Target - Looking for target");

            if (token.IsCancellationRequested)
            {
                //Debug.Log("Task Cancelled");
                return;
                //throw exception?
                //token.ThrowIfCancellationRequested();
            }

            //Add linked cancellation token here for propgating cancel req
            try
            {
                Target = await GetTargetFromTargetingScript(token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                Debug.Log("Got Target " + Target);
            }

            while (Target != null)
            {
                if (token.IsCancellationRequested)
                {
                    //Debug.Log("Task Cancelled");
                    return;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                while (distance > 3)
                {
                    if (token.IsCancellationRequested)
                    {
                        //Debug.Log("Task Cancelled");
                        return;
                    }

                    distance = Vector3.Distance(transform.position, Target.position);

                    // Move our position a step closer to the target.
                    var step = _speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, Target.position, step);

                    await Task.Yield();
                }
                Debug.Log("Move to Target - Destination Arrived");
                await Task.Yield();
            }
            await Task.Yield();
        }


        //while (Target == null || Target.Equals(null) || !ApplicationStateManager.playMode)
        //{
        //    if (token.IsCancellationRequested)
        //    {
        //        //Debug.Log("Task Cancelled");
        //        return;
        //    }
        //    if (!TryGetComponent(out TargetingSystem targetingSystem)) await Task.Yield();

        //    while (targetingSystem.Target != null || !Target.Equals(null))
        //    {
        //        Target = targetingSystem.Target;
        //        float distance = Vector3.Distance(transform.position, Target.position);
        //        while (distance > 3)
        //        {
        //            distance = Vector3.Distance(transform.position, Target.position);

        //            // Move our position a step closer to the target.
        //            var step = _speed * Time.deltaTime; // calculate distance to move
        //            transform.position = Vector3.MoveTowards(transform.position, Target.position, step);

        //            if (token.IsCancellationRequested)
        //            {
        //                //Debug.Log("Task Cancelled");
        //                return;
        //                //throw exception?
        //                //token.ThrowIfCancellationRequested();
        //            }
        //            await Task.Yield();
        //        }
        //        Debug.Log("Destination Arrived");
        //    }
        //    await Task.Yield();
        //    Debug.Log("Waiting for another target");
    }
    private async Task<Transform> GetTargetFromTargetingScript(CancellationToken token)
    {
        var m_target = Target;
        while (m_target == null)
        {
            if (token.IsCancellationRequested)
            {
                return null;
            }
            m_target = GetComponent<TargetingSystem>().Target;
            await Task.Yield();
        }

        return m_target;
    }

    public async void OnDestroy()
    {
        //Debug.Log("MoveToTarget cleanup");
        if (MoveToTask.IsCompleted) return;
        _cts.Cancel();
        while (!MoveToTask.IsCanceled)
        {
            await Task.Yield();
        }
    }
}
