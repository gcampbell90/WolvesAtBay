using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class AttackBehaviour : MonoBehaviour
{
    //WeaponInfo 
    public Transform Pivot { get; set; }
    float range;

    //TODO implement into attack speed?
    int _speed;

    public Transform target;
    public Rigidbody WeaponRB { get; set; }
    private void Awake()
    {

        if (gameObject.GetComponentInChildren<Transform>().childCount <= 0)
        {
            Pivot = new GameObject("WeaponPivot").transform;
            GameObject sword = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var rb = sword.AddComponent<Rigidbody>();
            rb.useGravity = false;

            Pivot.transform.SetParent(transform, false);
            sword.transform.SetParent(Pivot, false);

            sword.transform.localPosition = new Vector3(0, 0, 2);
            sword.transform.localScale = new Vector3(0.3f, 0.3f, 2.5f);
        }
        else
        {
            Pivot = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
        }

        range = Pivot.GetComponentInChildren<Transform>().GetChild(0).localScale.z;
        WeaponRB = Pivot.GetComponentInChildren<Rigidbody>();
        //Debug.Log($"Attack Behaviour Component - Range: {range}");
    }
    void Start()
    {
        _speed = gameObject.GetComponent<CharacterBase>().Speed;

    }

    //Check every frame if player is in range of weapon, if so, attack.
    private void Update()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        var _distance = Vector3.Distance(transform.position, target.position);

        if (_distance < range + 5f)
        {
            StartCoroutine(AttackMove());
        }
        //Debug.Log($"Attack Behaviour Component"
        //    + $" - Range: {_distance}"
        //    );

    }

    bool isRunning;
    private IEnumerator AttackMove()
    {
        if (isRunning)
            yield break;

        isRunning = true;
        // Just make the animation interval configurable for easier modification later
        float duration = 1f;
        float rot = Pivot.localRotation.y > 0 ? -45 : 45;
        float progress = 0f;
        // Loop until instructed otherwise
        while (progress <= 1f)
        {

            // Do some nice animation
            Pivot.localRotation = Quaternion.Slerp(Pivot.localRotation, Quaternion.Euler(new Vector3(0, rot, 0)), progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }

        isRunning = false;
    }
}