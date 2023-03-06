using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public delegate void AttackCommand();
    public static AttackCommand OnAttackCommand;

    public delegate void DefendCommand();
    public static DefendCommand OnDefendCommand;

    private void OnEnable()
    {
        OnAttackCommand += AttackCommandAction;
        OnDefendCommand += DefendCommandAction;
    }
    private void OnDisable()
    {
        OnAttackCommand -= AttackCommandAction;
        OnDefendCommand -= DefendCommandAction;
    }
    private void AttackCommandAction()
    {
        StartCoroutine(AttackMove());
    }
    private void DefendCommandAction()
    {
        StartCoroutine(DefensiveMove());
    }

    private IEnumerator AttackMove()
    {
        Debug.Log("PlayerCombatController - Attacking");
        // Just make the animation interval configurable for easier modification later
        float duration = 0.1f;
        float rot = 40;
        float progress = 0f;

        //Play the sword swing audio from the effect controller
        //EffectController.Instance.PlaySwordSound();


        //PlayerController?.On onAttack?.Invoke();

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
            //_swordPivot.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -70, 0), Quaternion.Euler(0, rot, 0), progress);
            progress += Time.deltaTime / duration;

            yield return null;
        }

        //_swordPivot.localRotation = Quaternion.Euler(0, -70, 0);
    }
    private IEnumerator DefensiveMove()
    {
        Debug.Log("PlayerCombatController - Defending");

        
        //onDefend?.Invoke();

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
        //_animator.Play("Movement", 0);
        yield return null;
    }
}
