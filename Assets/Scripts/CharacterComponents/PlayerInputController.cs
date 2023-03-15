using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public delegate void ActionCompleted(bool animState);
    public static ActionCompleted OnActionCompleted;

    private bool _isActionCompleted;

    private void OnEnable()
    {
        OnActionCompleted += SetActionCompleted;
    }
    private void OnDisable()
    {
        OnActionCompleted -= SetActionCompleted;
    }

    private void SetActionCompleted(bool animState)
    {
        _isActionCompleted = animState;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        //Debug.Log($"{movement.magnitude == 0}, {Input.GetMouseButton(0)}, {Input.GetMouseButton(1)} {_isActionCompleted}");
        if (movement.magnitude > 0 && !Input.GetMouseButton(1))
        {
            //Debug.Log("PlayerInputController - Move Event Fired");
            Player.OnMove?.Invoke(movement, IsBoosting());
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetMouseButton(1))
        {
            //Attack
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
            {
                //Debug.Log("PlayerInputController -  Attack Event Fired");
                Player.OnAttack?.Invoke();
            }

            //Defend
            if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.Log("PlayerInputController -  Defend Event Fired");
                Player.OnDefend?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetMouseButton(1))
            {
                //DefendAttack
                //Debug.Log("PlayerInputController -  DefendAttack Event Fired");
                Player.OnDefendAttack?.Invoke();
            }
            if (movement.magnitude > 0 && Input.GetMouseButton(1))
            {
                //DefendAttack
                //Debug.Log("PlayerInputController -  Phalanx March Event Fired");
                Player.OnMove?.Invoke(movement, false);
                //Player.OnDefendMarch?.Invoke();

            }
        }
        else if (_isActionCompleted && movement.magnitude == 0)
        {
            //Debug.Log("PlayerInputController -  Idle Event Fired");
            Player.OnIdle?.Invoke();
        }

    }

    private bool IsBoosting()
    {

        //Sprint/Charge
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
