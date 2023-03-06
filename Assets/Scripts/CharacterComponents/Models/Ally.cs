using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : CharacterBase
{
    public CombatController AttackBehaviour { get; set; }
    public TargetingSystem TargetingSystem { get; set; }
    public GameObject Follower { get; set; }

    [SerializeField] float _smoothSpeed;

    Animator _animator;
    private enum AnimationState { Idle, Walk, Turn, Block, Attack };
    private AnimationState _state;

    float velocityX = 0f;
    float velocityZ = 0f;

    public delegate void DefendCommand();
    public static DefendCommand OnDefendCommand;

    public delegate void AttackCommand();
    public static AttackCommand OnAttackCommand;

    private void OnEnable()
    {
        OnDefendCommand += Defend;
        OnAttackCommand += Attack;
    }
    private void OnDisable()
    {
        OnDefendCommand -= Defend;
        OnAttackCommand -= Attack;
    }

    private void Awake()
    {
        AttackBehaviour = GetComponent<CombatController>();
        TargetingSystem = GetComponent<TargetingSystem>();

        _animator = GetComponent<Animator>();
        //_animator.Play("Block Idle");
        //Physics.IgnoreCollision(col, _colliderBlocker, true);

        //StartCoroutine(CheckLeader());
    }
    private void Start()
    {
        //if (Follower == null) yield return null;

        //_follower.transform.SetLocalPositionAndRotation(_followerpos, _leader.transform.rotation);
        _animator.Play("Idle", 0);

    }
    private void FixedUpdate()
    {
        if (Follower == null) return;

        transform.position = Vector3.Lerp(transform.position, Follower.transform.position, _smoothSpeed);
    }
    private void Attack()
    {
        //AttackBehaviour.Attack();
        isAttacking = true;
        AnimationController(AnimationState.Attack);
        isAttacking = false;
    }
    private void Defend()
    {
        StartCoroutine(RaiseShield());
    }
    public void SetTarget(Transform target)
    {
        TargetingSystem.Target = target;
    }
    public IEnumerator StayInFormation()
    {
        float _t = 0f;
        var speed = 2f;

        while (true)
        {

            //var pos = Vector3.Lerp(transform.position, Follower.transform.position, Time.deltaTime * _smoothSpeed);
            //var rot = Quaternion.Slerp(transform.rotation, Follower.transform.rotation, Time.deltaTime * _smoothSpeed);
            var pos = transform.position;
            if (Vector3.Distance(pos, Follower.transform.position) < 0.15f && !isDefending && !isAttacking)
            {
                //_animator.SetBool("IsIdle", true);
                //_animator.SetBool("IsWalking", false);
                AnimationController(AnimationState.Idle);
                //velocityX = 0f;
                //velocityZ = 0f;
            }
            else
            {
                if (!isDefending && !isAttacking)
                {
                    AnimationController(AnimationState.Walk);
                    //velocityX = 0f;
                    //velocityZ = 1f;
                }
                //transform.position = Follower.transform.position;

            }
            transform.rotation = Follower.transform.rotation;
            //_animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
            //_animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            yield return null;
        }
    }

    bool isDefending = false;
    private bool isAttacking;

    private IEnumerator RaiseShield()
    {
        isDefending = true;
        AnimationController(AnimationState.Block);

        var originRot = transform.GetChild(0).transform.localRotation;
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(originRot.eulerAngles + new Vector3(0, 45, 0));

        while (Input.GetKey(KeyCode.Mouse1))
        {
            //m_shield.localRotation = Quaternion.Euler(0, 340, 0);
            yield return null;
        }
        isDefending = false;
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
        //_animator.Play("Movement");
        //m_shield.localRotation = m_originRot;
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
            //case AnimationState.Attack:
            //    {
            //        //motionTitle = "Slash";
            //        break;
            //    }
            case AnimationState.Block:
                {
                    motionTitle = "Block Idle";
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
        Follower = m_follower;
        StartCoroutine(StayInFormation());
    }

    public override void OnCollisionEnter(Collision collision)
    {
        throw new NotImplementedException();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        throw new NotImplementedException();
    }

    public override void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }
}

