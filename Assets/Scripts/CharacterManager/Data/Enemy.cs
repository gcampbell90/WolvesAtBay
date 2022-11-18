using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : CharacterBase
{
    public delegate void DeathEvent();
    public static event DeathEvent deathEvent;

    //[SerializeField]
    GMLevelAbstract gmLevelAbstract;

    protected void Awake()
    {

        //place into Enemy layer for physics etc
        gameObject.layer = 7;
        //give enemy tag for in game behaviours and finding
        gameObject.tag = "Enemy";

        //gmLevelAbstract = FindObjectOfType<GMLevelAbstract>();
    }

    private void Start()
    {

        var rb = GetComponent<Rigidbody>();
        rb.mass = 50f;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;

        var col = GetComponent<Collider>();
        col.isTrigger = false;

        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        base.AddLookAtTarget();

        //gameObject.AddComponent<MoveToTarget>();
        gameObject.AddComponent<AttackBehaviour>();
        gameObject.AddComponent<TargetingSystem>();

        //gameObject.GetComponent<IKillable>().ITakeDamage(5);
    }

    #region OOP test methods
    //Public method to call classes protected method for layer of control/security.
    //public void CallMustBeOverridden()
    //{
    //    MustBeOverridden();
    //}

    //Example of overridding a virtual method from the base when needed. Fetching/sending data to base methods?  
    //TODO: Find examples of actual useful implementations
    //public override void VirtualMethodTest(int testInt, string testString)
    //{
    //    base.VirtualMethodTest(testInt, testString);
    //    Debug.Log($"Output: {testInt},{testString}. Overridden Virtual method called directly from class than base. I can also add to this implementation with or without calling the base method.\n Whenever that behaviour is required/useful is still to be figured out. ");
    //}

    public override void ITakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name} Damage: {damage}");
        Health -= damage;
        if(Health <= 0)
        {
            //Debug.Log("Death Event");
            GroupController.Enemies.Remove(gameObject.GetComponent<TargetingSystem>());
            //deathEvent?.Invoke();
            EnemyDeath();
        }
    }

    private async void EnemyDeath()
    {
        //Debug.Log("Enemy Death async task cleanup");

        //EnemyDeath Cleanup
        var moveComponent = GetComponent<MoveToTarget>();
        var task = moveComponent.OnKilled();

        try
        {
            await task;
        }
        catch (ObjectDisposedException e)
        {
            Debug.Log($"Kill cleanup error {e.Message}");
        }
        finally
        {
            //Debug.Log("Finished Cleanup");
            GetComponent<IKillable>().Destroy();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Sword") return;
        if (collision.gameObject.tag != "Player") return;
        if (collision.gameObject.tag != "Ally") return;
        //Debug.Log("Player Hit! " + collision.gameObject.tag);

        //replace damage with weapon/player strength/damage
        ITakeDamage(10);
    }

    private void OnDestroy()
    {
        deathEvent?.Invoke();
    }
    #endregion
}

