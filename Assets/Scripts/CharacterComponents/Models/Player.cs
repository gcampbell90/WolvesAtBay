using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public static PlayerMovementController.PlayerMoveCommand OnMove;
    public static PlayerCombatController.AttackCommand OnAttack;
    public static PlayerCombatController.DefendCommand OnDefend;

    private void OnEnable()
    {
        OnMove += PlayerMovementController.OnMovePlayer;
        OnAttack += PlayerCombatController.OnAttackCommand;
        OnDefend += PlayerCombatController.OnDefendCommand;
        //OnDefendCommand += delegate { 
        //    BattleManager.OnDefendCommand?.Invoke();
        //    PlayerCombatController.OnDefendCommand?.Invoke();
        //};
    }

    private void OnDisable()
    {
        OnMove -= PlayerMovementController.OnMovePlayer;
        OnAttack -= PlayerCombatController.OnAttackCommand;
        OnDefend -= PlayerCombatController.OnDefendCommand;
        //OnDefendCommand -= delegate {
        //    BattleManager.OnDefendCommand?.Invoke();
        //    PlayerCombatController.OnDefendCommand?.Invoke();
        //};
    }

    private void Awake()
    {
        Initialise(100, 1, Vector3.zero);
        this.Name = "Player";
        this.Model = this.gameObject;
        //this.Health = 1000;
        //this.Speed = 1;

        //_state = AnimationState.Idle;
    }
    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public override void TakeDamage(int damage)
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
            TakeDamage(5);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
    public override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Weapon") return;
        TakeDamage(5);
    }
}
