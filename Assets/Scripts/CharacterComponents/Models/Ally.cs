using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : CharacterBase
{
    private GameObject _follower;
    private Transform _weaponPivot;

    [SerializeField] float _smoothSpeed;

    Animator _animator;
    private enum AnimationState { Idle, Walk, Turn, Block, Attack, Death };
    private AnimationState _state;

    private bool isDefending = false;
    private bool isAttacking = false;

    public delegate void DefendCommand();
    public static DefendCommand OnDefendCommand;

    public delegate void DefendAttackCommand();
    public static DefendAttackCommand OnDefendAttackCommand;

    public delegate void AttackCommand();
    public static AttackCommand OnAttackCommand;

    public delegate void DeathRemoveEvent(Ally ally);
    public static event DeathRemoveEvent OnDeathRemoveEvent;

    private void OnEnable()
    {
        OnDefendCommand += DefendMove;
        OnAttackCommand += AttackMove;
        OnDefendAttackCommand += DefendAttackMove;
    }
    private void OnDisable()
    {
        OnDefendCommand -= DefendMove;
        OnAttackCommand -= AttackMove;
        OnDefendAttackCommand -= DefendAttackMove;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponPivot = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Transform>();
        Health = 100;
    }
    private void Start()
    {
        _animator.Play("Idle", 0);
    }

    private void AttackMove()
    {
        StartCoroutine(Attack());
    }
    private void DefendMove()
    {
        StartCoroutine(Defend());
    }
    private void DefendAttackMove()
    {
        StartCoroutine(DefendAttack());
    }
    public IEnumerator StayInFormation()
    {
        while (true)
        {
            //Debug.Log("FormationCall");
            var pos = transform.position;
            var dist = Vector3.Distance(pos, _follower.transform.position);
            if (dist > 0.1f)
            {
                transform.position = Vector3.Lerp(pos, _follower.transform.position, _smoothSpeed);
            }
            else if (!isAttacking & !isDefending)
            {
                AnimationController(AnimationState.Idle);
            }
            transform.rotation = _follower.transform.rotation;
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("AllyController - Attacking");
        AnimationController(AnimationState.Attack);

        // Just make the animation interval configurable for easier modification later
        float duration = 0.5f;
        float progress = 0f;

        //Player.OnActionCompleted?.Invoke(false);
        isAttacking = true;
        // Loop until instructed otherwise
        while (progress <= duration)
        {
            //_swordPivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime;

            yield return null;
        }
        isAttacking = false;
    }
    private IEnumerator Defend()
    {
        if (isDefending) yield break;
        isDefending = true;
        AnimationController(AnimationState.Block);
        var originRot = transform.GetChild(0).transform.localRotation;
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(originRot.eulerAngles + new Vector3(0, 25, 0));

        while (Input.GetKey(KeyCode.Mouse1))
        {
            //m_shield.localRotation = Quaternion.Euler(0, 340, 0);
            yield return null;
        }
        isDefending = false;
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator DefendAttack()
    {
        //Debug.Log("Ally - AttackingFromPhalanx");
        // Just make the animation interval configurable for easier modification later
        float duration = 0.5f;
        float progress = 0f;

        Player.OnActionCompleted?.Invoke(false);
        var newPos = new Vector3(0, 0.1f, 0);

        //Debug.Log(_weaponPivot.transform.localPosition);
        // Loop until instructed otherwise
        while (progress <= duration)
        {
            _weaponPivot.Translate(newPos, Space.Self);
            _weaponPivot.transform.localPosition = Vector3.Lerp(_weaponPivot.localPosition, Vector3.zero, progress + Time.deltaTime);
            progress += Time.deltaTime;

            yield return null;
        }

        Player.OnActionCompleted?.Invoke(true);

        //_swordPivot.localRotation = Quaternion.Euler(0, -70, 0);
    }

    void AnimationController(AnimationState newstate)
    {
        string motionTitle;
        motionTitle = "";
        var currState = _state;

        switch (newstate)
        {
            case AnimationState.Idle:
                {
                    motionTitle = "Idle";

                    break;
                }
            case AnimationState.Walk:
                {
                    motionTitle = "Walk";
                    break;
                }
            case AnimationState.Turn:
                {

                    break;
                }
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
            case AnimationState.Death:
                {
                    motionTitle = "Death";
                    break;
                }
            default:
                break;
        }

        _state = newstate;
        PlayAnimation(motionTitle);
    }
    void PlayAnimation(string motion)
    {
        _animator.Play(motion, 0);
    }

    internal void SetFollower(GameObject m_follower)
    {
        _follower = m_follower;
        StartCoroutine(StayInFormation());
    }

    //private void SetTarget(Transform target)
    //{
    //    TargetingSystem.Target = target;
    //}

    //Character base interface methods

    public override void OnCollisionStay(Collision collision)
    {

        if (collision.collider.gameObject.tag != "Weapon") return;

        Debug.Log($"Collision on {gameObject.name} from {collision.collider.gameObject.name}");

        TakeDamage(10);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;


    }

    public override void TakeDamage(int damage)
    {
        //Debug.Log($"{gameObject.name} have taken damage of {damage}");
        Health -= damage;
        if (Health <= 0)
        {
            OnDeathRemoveEvent?.Invoke(this);
            //EffectController.Instance.PlayDeathSound();
            //Destroy(gameObject);
            AnimationController(AnimationState.Death);
        }
    }
}

