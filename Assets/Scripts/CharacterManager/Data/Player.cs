using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Collider))]
public class Player : CharacterBase
{
    //public int speed = 10;
    //// Start is called before the first frame update
    private void Awake()
    {
        Speed = 10;
        Health = 1000;
    }

    //void Start()
    //{
    //    //gameObject.GetComponent<IKillable>().ITakeDamage(5);
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void ITakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} have taken damage of {damage}");
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("GameOver");
        }
    }


    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Sword") return;
        ITakeDamage(2);
        //throw new System.NotImplementedException();
    }
}
