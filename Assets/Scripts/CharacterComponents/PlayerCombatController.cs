using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public delegate void AttackCommand();
    public static AttackCommand OnAttackCommand;

    public delegate void DefendCommand();
    public static DefendCommand OnDefendCommand;

    public delegate void DefendAttackCommand();
    public static DefendAttackCommand OnDefendAttackCommand;

    public delegate void DefendMoveCommand();
    public static DefendMoveCommand OnDefendMarch;

    private Coroutine m_Coroutine;

    [SerializeField]
    private Transform _weaponPivot;

    private void OnEnable()
    {
        OnAttackCommand += AttackCommandAction;
        OnDefendCommand += DefendCommandAction;
        OnDefendAttackCommand += DefendAttackCommandAction;
        OnDefendMarch += DefendMoveCommandAction;
    }

    private void OnDisable()
    {
        OnAttackCommand -= AttackCommandAction;
        OnDefendCommand -= DefendCommandAction;
        OnDefendAttackCommand -= DefendAttackCommandAction;
        OnDefendMarch -= DefendMoveCommandAction;
    }

    private void Awake()
    {

    }

    private void AttackCommandAction()
    {
        StartCoroutine(Attack());
    }
    private void DefendCommandAction()
    {
        StartCoroutine(Defend());
    }
    private void DefendAttackCommandAction()
    {
        StartCoroutine(DefendAttack());
    }
    private void DefendMoveCommandAction()
    {
        StartCoroutine(DefendMarch());
    }

    private IEnumerator Attack()
    {
        Debug.Log("PlayerCombatController - Attacking");
        // Just make the animation interval configurable for easier modification later
        float duration = 0.5f;
        float progress = 0f;

        //Play the sword swing audio from the effect controller
        //EffectController.Instance.PlaySwordSound();

        Player.OnActionCompleted?.Invoke(false);

        // Loop until instructed otherwise
        while (progress <= duration)
        {
            //_swordPivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime;

            yield return null;
        }

        Player.OnActionCompleted?.Invoke(true);

        //_swordPivot.localRotation = Quaternion.Euler(0, -70, 0);
    }
    private IEnumerator DefendAttack()
    {
        //Debug.Log("PlayerCombatController - AttackingFromPhalanx");
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
    private IEnumerator Defend()
    {
        //Debug.Log("PlayerCombatController - Defending");
        float duration = 1f;
        float progress = 0f;

        //PlayerRot for defense
        Quaternion originRot = Quaternion.identity;
        var newRot = Quaternion.Euler(originRot.eulerAngles + new Vector3(0, 25, 0));
        transform.GetChild(0).transform.localRotation = newRot;
        //weaponrot for defense
        var originWeaponRot = _weaponPivot.transform.localRotation;
        var newWeaponRot = Quaternion.Euler(0, 0, 130f);
        _weaponPivot.transform.localRotation = newWeaponRot;

        //Debug.Log($"PlayerCombatController - Defending   {originRot}->{newRot}");
        Player.OnActionCompleted?.Invoke(false);

        while (Input.GetKey(KeyCode.Mouse1))
        {
            yield return null;
        }
        transform.GetChild(0).transform.localRotation = originRot;

        Player.OnActionCompleted?.Invoke(true);
    }

    private IEnumerator DefendMarch()
    {
        //Debug.Log("PlayerCombatController - Defending");
        float duration = 1f;
        float progress = 0f;

        //PlayerRot for defense
        Quaternion originRot = Quaternion.identity;
        var newRot = Quaternion.Euler(originRot.eulerAngles + new Vector3(0, 25, 0));
        transform.GetChild(0).transform.localRotation = newRot;
        //weaponrot for defense
        var originWeaponRot = _weaponPivot.transform.localRotation;
        var newWeaponRot = Quaternion.Euler(0, 0, 130f);
        _weaponPivot.transform.localRotation = newWeaponRot;

        //Debug.Log($"PlayerCombatController - Defending   {originRot}->{newRot}");
        Player.OnActionCompleted?.Invoke(false);

        while (Input.GetKey(KeyCode.Mouse1))
        {
            yield return null;
        }
        transform.GetChild(0).transform.localRotation = originRot;

        Player.OnActionCompleted?.Invoke(true);
    }

}
