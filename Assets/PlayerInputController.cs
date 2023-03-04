using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerInputController : MonoBehaviour
{
    private void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.magnitude > 0)
        {
            PlayerController.OnMove?.Invoke(movement, IsBoosting());
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerController.OnAttack?.Invoke();

        }

        //Defend
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerController.OnDefend?.Invoke();
        }
    }

    private bool IsBoosting()
    {

        //Sprint/Charge
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
