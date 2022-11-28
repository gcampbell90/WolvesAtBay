using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(BoxCollider))]
public class Player : CharacterBase
{
    private void Awake()
    {
        Speed = 2;
        Health = 1000;
    }
    public override void ITakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name} have taken damage of {damage}");
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("GameOver");
            //EffectController.Instance.PlayDeathSound();
            Destroy(gameObject);
        }
    }
    public override void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision on {gameObject.name} from {collision.gameObject.name}");
        if (collision.gameObject.name != "Sword" && collision.gameObject.name != "Spear") return;
        { 
            ITakeDamage(5);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
    public override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        ITakeDamage(5);
    }
}
