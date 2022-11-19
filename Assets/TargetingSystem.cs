using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
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
    private void Start()
    {
        if (randomMovement)
        {
            StartCoroutine(MoveAround(gameObject));
        }
    }

    private void Update()
    {
        if (Target == null) return;

        //transform.GetComponent<MoveToTarget>().Target = Target;
        if (!_enableDebugLines) return;
        //Draws line to closest target regardless of whats in the way
        Debug.DrawLine(transform.position + new Vector3(0.1f, 0, 0), Target.position + new Vector3(0.1f, 0, 0), Color.blue);

        //Shows if unit can attack by showing red or green line
        var lineCol = CanAttack ? Color.green : Color.red;
        Debug.DrawLine(transform.position, Target.position, lineCol);
    }

    public IEnumerator MoveAround(GameObject _go)
    {
        while (true)
        {
            float _t = 0f;
            var randomVector = GetRandomVector();

            while (_t < 3)
            {
                _go.transform.position = Vector3.Lerp(_go.transform.position, randomVector, _t);

                _t += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }

    Vector3 GetRandomVector()
    {
        float xClamped;
        float yClamped;
        float zClamped;

        xClamped = Random.Range(-10, 10);
        yClamped = Random.Range(0, 0);
        zClamped = Random.Range(-10, 10);

        return new Vector3(xClamped, yClamped, zClamped);
    }
}
