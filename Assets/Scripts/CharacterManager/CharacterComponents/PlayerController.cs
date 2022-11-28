using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private int playerSpeed; //Speed of the player cube
    private int maxVerticalAngleHorizon = 50;
    private int maxHorizontalAngleHorizon = 20;
    [SerializeField] float _smoothSpeed;
    public Transform _swordPivot;
    public Transform _shieldPivot;

    private bool Boost;
    private bool isAttacking = false;

    public delegate void DefendCommand();
    public static event DefendCommand onDefend;

    public delegate void AttackCommand();
    public static event AttackCommand onAttack;

    CharacterController _characterController;
    Animator _animator;

    private enum AnimationState { Idle, Walk, Block, Attack };
    private AnimationState _state;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _state = AnimationState.Idle;
        AnimationController(_state);
    }
    private void Start()
    {
        //playerSpeed = gameObject.GetComponent<CharacterBase>().Speed;
        playerSpeed = 1;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
        _animator.Play("Movement");
    }

    private void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical =  Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if(movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= GetSpeed() * Time.deltaTime;
            _characterController.Move(transform.forward * movement.z);
            transform.Rotate(transform.up * horizontal * GetSpeed());
        }

        //animating
        float velocityX = Vector3.Dot(movement.normalized, transform.right);
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);

        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(BoostSpeed());
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(AttackMove());
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(DefensiveMove());
            AnimationController(AnimationState.Block);
        }
    }

    void AnimationController(AnimationState newstate)
    {
        string motionTitle;
        motionTitle = "";
        var currState = _state;

        switch (newstate)
        {
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
    private int GetSpeed()
    {
        var boostVal = Boost ? 2 : 1;
        return playerSpeed * boostVal;
    }
    private IEnumerator BoostSpeed()
    {
        Boost = true;
        while (Input.GetKey(KeyCode.LeftShift))
        {
            yield return null;
        }
        Boost = false;
    }
    private IEnumerator AttackMove()
    {
        // Just make the animation interval configurable for easier modification later
        float duration = 0.1f;
        float rot = 40;
        float progress = 0f;

        //Play the sword swing audio from the effect controller
        //EffectController.Instance.PlaySwordSound();
        //Debug.Log("PlayerController - attacking");

        onAttack?.Invoke();

        //Get Mouse Position
        //var cam = Camera.main;
        //Vector3 point = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        //// Using some math to calculate the point of intersection between the line going through the camera and the mouse position with the XZ-Plane
        //float t = cam.transform.position.y / (cam.transform.position.y - point.y);
        //Vector3 finalPoint = new Vector3(t * (point.x - cam.transform.position.x) + cam.transform.position.x, 1, t * (point.z - cam.transform.position.z) + cam.transform.position.z);
        //transform.LookAt(finalPoint);

        // Loop until instructed otherwise
        while (progress <= 1f)
        {
            _swordPivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime / duration;

            yield return null;
        }

        _swordPivot.localRotation = Quaternion.Euler(0, -70, 0);
    }
    private IEnumerator DefensiveMove()
    {
        onDefend?.Invoke();

        //float rot = 40;
        float timer = 0f;
        float duration = 2f;
        Quaternion originRot = Quaternion.identity;
        var newRot = Quaternion.Euler(originRot.eulerAngles + new Vector3(0, 45, 0));
        transform.GetChild(0).transform.localRotation = newRot;

        while (!Input.GetKeyUp(KeyCode.Mouse1))
        {
            //_swordPivot.gameObject.SetActive(false);
            //_shieldPivot.gameObject.SetActive(true);
            

            yield return null;
        }
        //_animator.SetBool("IsDefending", false);
        transform.GetChild(0).transform.localRotation = originRot;
        _animator.Play("Movement",0);
        yield return null;
    }

}
