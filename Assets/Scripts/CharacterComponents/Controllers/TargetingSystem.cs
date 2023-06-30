using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class TargetingSystem : MonoBehaviour, ICanTarget
{
    public Transform Target { get; set; }
    public Transform Closest { get; set; }
    public bool CanAttack { get; set; }

    [SerializeField] bool randomMovement;
    [SerializeField] bool _enableDebugLines;

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
        //Debug.Log("Checking for target" + Target);
        if (Target != null)
        {
            //transform.GetComponent<MoveToTarget>().Target = Target;
            if (!_enableDebugLines) return;

            //Draws line to closest target regardless of whats in the way
            Debug.DrawLine(transform.position + new Vector3(0.1f, 0, 0), Target.position + new Vector3(0.1f, 0, 0), Color.blue);

            //Shows if unit can attack by showing red or green line
            var lineCol = CanAttack ? Color.green : Color.red;
            Debug.DrawLine(transform.position, Target.position, lineCol);
        }
    }

    public void CheckNewTarget()
    {
        if (BattleManager.Instance == null) return;

        //if (BattleManager.Instance.GetNearestTargetGeneric(gameObject) == null) return;

        SetTarget(BattleManager.Instance.GetNearestTarget(gameObject));

        //if (m_target == null) return;
        //SetTarget(m_target);
    }

    public void SetTarget(Transform _target)
    {
        //if (Target == null) return;
        Target = _target;
    }
    private void OnDestroy()
    {
        //_cts.Cancel();
    }
}
