using System.Threading;
using UnityEngine;

internal interface ICanMove
{
    void MoveToTarget(Transform _target, CancellationToken token);
}