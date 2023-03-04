using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovementController : MonoBehaviour
{
    private float playerSpeed;
    CharacterController characterController;

    public delegate void PlayerMoveCommand(Vector3 movement, bool isBoosting);
    public static PlayerMoveCommand OnMovePlayer;

    private void OnEnable()
    {
        OnMovePlayer += MovePlayer;
    }
    private void OnDisable()
    {
        OnMovePlayer -= MovePlayer;
    }

    private void Start()
    {
        playerSpeed = GetComponent<PlayerController>().Speed;
        characterController = GetComponent<CharacterController>();
    }

    private float GetBoostMultipledSpeed(bool isBoosting)
    {
        var boostVal = isBoosting ? 5 : 1;
        return playerSpeed * boostVal; 
    }

    void MovePlayer(Vector3 movement, bool isBoosting)
    {
        transform.Rotate(transform.up * movement.x);

        movement.Normalize();
        movement *= GetBoostMultipledSpeed(isBoosting) * Time.deltaTime;

        characterController.Move(transform.forward * movement.z);

        //Debug.Log("Moving Player" + movement);

    }

    //void AnimationController(AnimationState newstate)
    //{
    //    string motionTitle;
    //    motionTitle = "";
    //    var currState = _state;

    //    switch (newstate)
    //    {
    //        //case AnimationState.Attack:
    //        //    {
    //        //        motionTitle = "Slash";
    //        //        break;
    //        //    }
    //        case AnimationState.Block:
    //            {
    //                motionTitle = "Block Idle";
    //                break;
    //            }
    //        default:
    //            break;
    //    }
    //    _state = newstate;
    //    PlayAnimation(motionTitle);

    //}


    //void PlayAnimation(string motion)
    //{
    //    _animator.Play(motion, 0);
    //}

}
