using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class MovementController : MonoBehaviour, ICanMove
{

    [SerializeField]private Transform target;
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

    Task MoveToTargetAsync;
    CancellationTokenSource _cts = null;
    CancellationTokenSource _childcts = null;

    bool destinationArrived = false;
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
        _childcts = CancellationTokenSource.CreateLinkedTokenSource(token);
        MoveToTarget(Target, token);

    }

    public async void MoveToTarget(Transform _target, CancellationToken token)
    {
        try
        {
            await(MoveToTargetAsync = MoveToTargetOverTime(token));
        }
        catch (OperationCanceledException e)
        {
            //Debug.Log($"Movement Cancelled {e.Message}");
        }
        finally
        {
            //Debug.Log("MOVE TO TARGET - Move to target task set");
            _cts.Dispose();
        }
    }
    public async Task MoveToTargetOverTime(CancellationToken token)
    {
        while (Target == null || !ApplicationStateManager.playMode)
        {
            //Debug.Log("MOVE TO TARGET - Looking for target");
            destinationArrived = false;

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
                //Debug.Log($"MOVE TO TARGET -{gameObject.name} received Target " + Target);
            }
            await Task.Yield();
        }

        while (Target != null)
        {
            if (token.IsCancellationRequested)
            {
                //Debug.Log("Task Cancelled");
                return;
            }
            float distance = Vector3.Distance(transform.position, Target.position);
            var targetPos = Target.position + new Vector3(0, transform.position.y, 0);
            while (distance > 5 && Target != null)
            {
                if (token.IsCancellationRequested)
                {
                    //Debug.Log("Task Cancelled");
                    return;
                }

                //Debug.Log($"MOVE TO TARGET - {gameObject} Moving to Target {distance}");

                //distance = Vector3.Distance(transform.position, Target.position);

                // Move our position a step closer to the target.
                var step = _speed * Time.deltaTime; // calculate distance to move
                
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                transform.LookAt(targetPos);
                await Task.Yield();
            }
            destinationArrived = true;
            //Debug.Log("MOVE TO TARGET - Destination Arrived");
            await Task.Yield();
        }
    }

    private async Task<Transform> GetTargetFromTargetingScript(CancellationToken token)
    {
        var m_target = Target;
        while (m_target == null)
        {
            //Debug.Log("MOVE TO TARGET - Looking for target from targeting script");
            if (token.IsCancellationRequested)
            {
                return null;
            }
            m_target = GetComponent<TargetingSystem>().Target;
            await Task.Yield();
        }

        return m_target;
    }

    public void OnDestroy()
    {
        if (MoveToTargetAsync == null || MoveToTargetAsync.IsCompleted) return;
        _cts.Cancel();
    }
}
