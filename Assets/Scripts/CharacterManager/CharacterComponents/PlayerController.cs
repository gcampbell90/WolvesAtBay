using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : Player
{
    public static PlayerMovementController.PlayerMoveCommand OnMove;
    public static PlayerCombatController.AttackCommand OnAttack;
    public static PlayerCombatController.DefendCommand OnDefend;

    private void OnEnable()
    {
        OnMove += PlayerMovementController.OnMovePlayer;
        OnAttack += PlayerCombatController.OnAttackCommand;
        OnDefend += PlayerCombatController.OnDefendCommand;
    }
    private void OnDisable()
    {
        OnMove -= PlayerMovementController.OnMovePlayer;
        OnAttack -= PlayerCombatController.OnAttackCommand;
        OnDefend -= PlayerCombatController.OnDefendCommand;
    }

    private void Awake()
    {
        this.Name = "Player";
        this.Health = 100;
        this.Speed = 1;

        //_state = AnimationState.Idle;
    }
    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

}
