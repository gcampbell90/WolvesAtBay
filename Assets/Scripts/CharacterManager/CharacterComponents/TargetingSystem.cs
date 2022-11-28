using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class TargetingSystem : MonoBehaviour, ITarget
{
    public Transform Target { get; set; }
    public Transform Closest { get; set; }
    public bool CanAttack { get; set; }

    [SerializeField] bool randomMovement;
    [SerializeField] bool _enableDebugLines;

    //CancellationTokenSource _cts = null;
    //Task _findNewTarget;
    public bool EnableDebugLines
    {
        get
        {
            return _enableDebugLines;
        }
        set
        {
            _enableDebugLines = value;
        }
    }

    private void Update()
    {
        if (Target == null) {
            GetNewTarget();
            return;
        }

        //transform.GetComponent<MoveToTarget>().Target = Target;
        if (!_enableDebugLines) return;

        //Draws line to closest target regardless of whats in the way
        Debug.DrawLine(transform.position + new Vector3(0.1f, 0, 0), Target.position + new Vector3(0.1f, 0, 0), Color.blue);

        //Shows if unit can attack by showing red or green line
        var lineCol = CanAttack ? Color.green : Color.red;
        Debug.DrawLine(transform.position, Target.position, lineCol);
    }
    public void GetNewTarget()
    {
        Target = BattleManager.Instance.GetNearestTargetGeneric(gameObject);
    }
    public void SetTarget(Transform _target)
    {
        Target = _target;
    }
    
    private void OnDestroy()
    {
        //_cts.Cancel();
    }

    //public async Task GetAndSetTargetAsync()
    //{
    //    Debug.Log("TARGETING SYSTEM - Setting token and running task");
    //    var token = _cts.Token;
    //    _cts = new CancellationTokenSource();

    //        try
    //        {
    //            await GetTargetFromBattleManager(token);
    //        }
    //        catch (OperationCanceledException e)
    //        {
    //            Debug.LogError(e.Message);
    //        }
    //        finally
    //        {
    //            _cts.Dispose();
    //            Debug.Log("TARGETING SYSTEM - Target get task ended");
    //        }

    //}
    ////should run indefinitely if no target
    //private async Task<Transform> GetTargetFromBattleManager(CancellationToken token)
    //{
    //    var m_target = Target;

    //    while (m_target == null || !ApplicationStateManager.playMode)
    //    {
    //        Debug.Log("TARGETING SYSTEM - Looking for target from BattleManager");
    //        if (token.IsCancellationRequested)
    //        {
    //            return null;
    //        }
    //        await Task.Yield();
    //    }

    //    return m_target;
    //}

    //debug random movement for testing finding method.
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

    //    xClamped = Random.Range(-10, 10);
    //    yClamped = Random.Range(0, 0);
    //    zClamped = Random.Range(-10, 10);

    //    return new Vector3(xClamped, yClamped, zClamped);
    //}
}
