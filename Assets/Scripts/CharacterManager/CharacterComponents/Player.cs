using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    //public int speed = 10;
    //// Start is called before the first frame update
    private void Awake()
    {
        Speed = 10;
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
    }


    public override void OnTriggerEnter(Collider collider)
    {
        ITakeDamage(10);
        //throw new System.NotImplementedException();
    }
}
