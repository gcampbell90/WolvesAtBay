using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private float playerSpeed;
    CharacterController characterController;

    public delegate void PlayerMoveCommand(Vector3 movement, bool isBoosting);
    public static PlayerMoveCommand OnMovePlayer;

    //public delegate void PlayerDefendMoveCommand(Vector3 movement, bool isBoosting);
    //public static PlayerDefendMoveCommand OnDefendMove;

    private void OnEnable()
    {
        OnMovePlayer += MovePlayer;
        //OnDefendMove += MovePlayer;
    }
    private void OnDisable()
    {
        OnMovePlayer -= MovePlayer;
        //OnDefendMove -= MovePlayer;

    }

    private void Start()
    {
        playerSpeed = GetComponent<Player>().Speed;
        characterController = GetComponent<CharacterController>();
    }

    private float GetBoostMultipledSpeed(bool isBoosting)
    {
        var boostVal = isBoosting ? 2 : 1;
        playerSpeed = boostVal;
        GetComponent<Player>().Speed = (int)playerSpeed;

        return playerSpeed; 
    }

    void MovePlayer(Vector3 movement, bool isBoosting)
    {
        Player.OnActionCompleted?.Invoke(false);

        transform.Rotate(transform.up * movement.x);

        movement.Normalize();
        movement *= GetBoostMultipledSpeed(isBoosting) * Time.deltaTime;

        characterController.Move(transform.forward * movement.z);

        Player.OnActionCompleted?.Invoke(true);

        //Debug.Log("Moving Player " + movement + " " + playerSpeed);
    }

    void IdlePlayer()
    {

        //Debug.Log("Moving Player " + movement + " " + playerSpeed);
    }

}
