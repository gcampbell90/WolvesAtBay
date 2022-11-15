using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class AttackBehaviour : MonoBehaviour
{
    //WeaponInfo 
    public Transform Pivot { get; set; }
    float range;
    //TODO implement into attack speed?
    int _speed;
    public Rigidbody WeaponRB { get; set; }
    //coroutine check
    bool isRunning;

    public Transform target;

    //Delegate to hold attack animation type - should eventually be managed by weapon system
    public delegate IEnumerator AttackBehaviourDelegate();
    public AttackBehaviourDelegate mydelegate;

    private void Awake()
    {
        AttachWeaponObjects();

        range = Pivot.GetComponentInChildren<Transform>().GetChild(0).localScale.z;
        WeaponRB = Pivot.GetComponentInChildren<Rigidbody>();

        //Debug.Log($"Attack Behaviour Component - Range: {range}");
    }
    void Start()
    {

        transform.TryGetComponent(out CharacterBase component);
        if (!component) _speed = 2; return;
        _speed = component.Speed;
        target = GetComponent<TargetingSystem>().Target;
        Debug.Log("Start target: " + target);

    }

    //Check every frame if player is in range of weapon, if so, attack.
    private void Update()
    {
        //target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if (target == null)
        {
            target = GetComponent<TargetingSystem>().Target;
            return;
        }
        Debug.Log("Update target: " + target);

        var _distance = Vector3.Distance(transform.position, target.position);

        if (_distance < range + 5f)
        {
            Debug.Log("Attacking" + target);

            StartCoroutine(mydelegate());
        }
        //Debug.Log($"Attack Behaviour Component"
        //    + $" - Range: {_distance}"
        //    );

    }

    void AttachWeaponObjects()
    {
        if (gameObject.GetComponentInChildren<Transform>().childCount <= 0)
        {
            Pivot = new GameObject("WeaponPivot").transform;

            if(gameObject.GetComponent<Swordsman>() != null)
            {
                AttachSword();
                mydelegate += SwordAttackMove;

            }else if(gameObject.GetComponent<Spearman>() != null)
            {
                AttachSpear();
                mydelegate += SpearAttackMove;
            }
            else
            {
                AttachSword();
                mydelegate += SwordAttackMove;
            }
        }
        else
        {
            Pivot = gameObject.GetComponentInChildren<Transform>().GetChild(0).transform;
        }
        Pivot.transform.SetParent(transform, false);

    }

    private void AttachSword()
    {
        Pivot.transform.localPosition = new Vector3(0.7f, 0, 0.9f);

        GameObject sword = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sword.name = "Sword";
        var rb = sword.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        sword.transform.SetParent(Pivot, false);

        sword.transform.localPosition = new Vector3(0, 0, 1);
        sword.transform.localScale = new Vector3(0.3f, 0.3f, 2f);

        var tag = gameObject.tag;
        //Pivot.tag = tag;
        //sword.tag = tag;
        sword.layer = tag == "Enemy" ? 10 : 11;
    }
    private void AttachSpear()
    {
        //Pivot.transform.localPosition = new Vector3(0, 0, 0.9f);

        GameObject spear = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spear.name = "Spear";
        var rb = spear.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        spear.transform.SetParent(Pivot, false);

        spear.transform.localPosition = new Vector3(0.55f, -0.3f, 1);
        spear.transform.localScale = new Vector3(0.1f, 0.1f,5);

        spear.layer = 10;
    }

    private IEnumerator SwordAttackMove()
    {
        if (isRunning)
            yield break;

        isRunning = true;
        // Just make the animation interval configurable for easier modification later
        float duration = 1f;
        float rot = Pivot.localRotation.y > 0 ? -45 : 45;
        float progress = 0f;
        // Loop until instructed otherwise
        while (progress <= duration)
        {

            // Do some nice animation
            Pivot.localRotation = Quaternion.Slerp(Pivot.localRotation, Quaternion.Euler(new Vector3(0, rot, 0)), progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }

        isRunning = false;
    }
    private IEnumerator SpearAttackMove()
    {
        if (isRunning)
            yield break;

        isRunning = true;
        // Just make the animation interval configurable for easier modification later
        float duration = 1f;
        //float rot = Pivot.localRotation.y > 0 ? -45 : 45;
        float progress = 0f;
        // Loop until instructed otherwise

        while (progress <= duration/2)
        {

            // Do some nice animation
            Pivot.localPosition = Vector3.Lerp(Vector3.zero, Vector3.forward, progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }
        transform.GetChild(0).GetComponentInChildren<BoxCollider>().enabled = true;
        yield return new WaitForFixedUpdate();

        while (progress <= duration/2)
        {

            // Do some nice animation
            Pivot.localPosition = Vector3.Lerp(Vector3.forward, Vector3.zero, progress);
            progress += Time.deltaTime / duration;

            // Make the coroutine wait for a moment
            yield return null;
        }
        transform.GetChild(0).GetComponentInChildren<BoxCollider>().enabled = false;

        isRunning = false;
    }

}