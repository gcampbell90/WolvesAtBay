using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    [SerializeField] HitBox hitbox;
    [SerializeField] HitBox hitboxShield;

    public static PlayerMovementController.PlayerMoveCommand OnMove;
    public static PlayerCombatController.AttackCommand OnAttack;
    public static PlayerCombatController.DefendCommand OnDefend;
    public static PlayerCombatController.DefendAttackCommand OnDefendAttack;
    public static PlayerCombatController.DefendMoveCommand OnDefendMarch;
    public static PlayerInputController.ActionCompleted OnActionCompleted;

    public delegate void IdleCommand();
    public static IdleCommand OnIdle;

    public enum AnimationState { Idle, Attack, Block, BlockAttack, BlockMove, Walk, Death }
    private AnimationState _animationState;

    public AnimationState CurrentAnimationState
    {
        get { return _animationState; }
        set { _animationState = value; }
    }

    private Animator _animator;

    private void shield_OnHit(object sender, EventArgs e)
    {
        Debug.Log($"Subscriber receives the Ihitable event.{sender} {e}");
    }
    private void player_OnHit(object sender, EventArgs e)
    {
        Debug.Log($"Subscriber receives the Ihitable event.{sender} {e}");
    }
    private void OnEnable()
    {
        OnMove += PlayerMovementController.OnMovePlayer;
        OnAttack += PlayerCombatController.OnAttackCommand;
        OnDefend += PlayerCombatController.OnDefendCommand;
        OnDefendAttack += PlayerCombatController.OnDefendAttackCommand;
        OnDefendMarch += PlayerCombatController.OnDefendMarch;
        OnActionCompleted += PlayerInputController.OnActionCompleted;

        OnMove += delegate { GetAnimation(); };
        OnAttack += delegate { UpdateAnimator(AnimationState.Attack); };
        OnDefend += delegate { UpdateAnimator(AnimationState.Block); }; ;
        OnDefendMarch += delegate { UpdateAnimator(AnimationState.BlockMove); }; ;
        OnIdle += delegate { UpdateAnimator(AnimationState.Idle); };
        OnIdle += delegate { TogglePlayerHitbox(true); };

        //test for blocking attacks
        OnDefend += delegate { TogglePlayerHitbox(false); };

        IHitable _playerHitBox = hitbox;
        _playerHitBox.OnColliderHit += player_OnHit;
        IHitable _shieldHitBox = hitboxShield;
        _shieldHitBox.OnColliderHit += shield_OnHit;
    }

    private void TogglePlayerHitbox(bool v)
    {
        //Debug.Log($"Toggle player call");

        if (hitbox.Collider.enabled == v) return;
        //Debug.Log($"Toggling player hitbox {v}");

        hitbox.Collider.enabled = v; 
    }

    private void OnDisable()
    {
        OnMove -= PlayerMovementController.OnMovePlayer;
        OnAttack -= PlayerCombatController.OnAttackCommand;
        OnDefend -= PlayerCombatController.OnDefendCommand;
        OnDefendAttack -= PlayerCombatController.OnDefendAttackCommand;
        OnDefendMarch -= PlayerCombatController.OnDefendMarch;

        OnActionCompleted -= PlayerInputController.OnActionCompleted;

        OnMove -= delegate { GetAnimation(); };
        OnAttack -= delegate { UpdateAnimator(AnimationState.Attack); };
        OnDefend -= delegate { UpdateAnimator(AnimationState.Block); }; ;
        OnDefendMarch -= delegate { UpdateAnimator(AnimationState.BlockMove); }; ;
        OnIdle -= delegate { UpdateAnimator(AnimationState.Idle); };
        OnIdle -= delegate { TogglePlayerHitbox(true); };

        //test for blocking attacks
        OnDefend -= delegate { TogglePlayerHitbox(false); };
        IHitable _playerHitBox = hitbox;
        _playerHitBox.OnColliderHit -= player_OnHit;
        IHitable _shieldHitBox = hitboxShield;
        _shieldHitBox.OnColliderHit -= shield_OnHit;

    }

    private void Awake()
    {
        Initialise(100, 1, Vector3.zero);
        Name = "Player";
        Model = this.gameObject;

        _animator = GetComponent<Animator>();

        UpdateAnimator(AnimationState.Idle);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void GetAnimation()
    {
        if (_animationState == AnimationState.Block || _animationState == AnimationState.BlockMove)
        {
            UpdateAnimator(AnimationState.BlockMove);
        }
        else
        {
            UpdateAnimator(AnimationState.Walk);
        }
    }
    void UpdateAnimator(AnimationState newstate)
    {
        string motionTitle;
        motionTitle = "";

        //Debug.Log("Animator Call");
        if (newstate == _animationState) return;
        //Debug.Log($"{_animationState} -> {newstate}");
        switch (newstate)
        {
            case AnimationState.Attack:
                {
                    motionTitle = "Slash";
                    break;
                }
            case AnimationState.Block:
                {
                    motionTitle = "Block Idle";
                    break;
                }
            case AnimationState.BlockAttack:
                {
                    //motionTitle = "Block_Attack";
                    break;
                }
            case AnimationState.BlockMove:
                {
                    motionTitle = "Block Idle";
                    break;
                }
            case AnimationState.Walk:
                {
                    motionTitle = "Walk";
                    break;
                }
            case AnimationState.Idle:
                {
                    motionTitle = "Idle";
                    break;
                }
            case AnimationState.Death:
                {
                    motionTitle = "Death";
                    break;
                }
            default:
                break;
        }
        _animationState = newstate;
        PlayAnimation(motionTitle);
    }


    void PlayAnimation(string motion)
    {
        //Debug.Log($"Playing animation @{Speed} speed");
        _animator.speed = Speed;
        _animator.Play(motion, 0);
    }

    public override void TakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name} have taken damage of {damage}");
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("GameOver");
            //EffectController.Instance.PlayDeathSound();
            //Destroy(gameObject);
            UpdateAnimator(AnimationState.Death);
        }
    }
    public override void OnCollisionStay(Collision collision)
    {
        //if (collision.collider.gameObject.tag != "Weapon") return;
        //Debug.Log($"Collision on {gameObject.name} from {collision.gameObject.name}'s {collision.collider.gameObject.name}");

        //TakeDamage(5);
        //gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
