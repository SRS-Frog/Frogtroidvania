using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private PlayerStates playerStates;

    private PlayerInput playerInput;

    //store our controls
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    //bool for if keys are pressed
    private bool movePressed;
    private bool jumpPressed;
    private bool attackPressed;

    //specific for movement
    private int dir;

    private void Awake()
    {
        //playerStates = GetComponent<PlayerStates>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];

        //player
    }

    private void OnEnable()
    {
        moveAction.started += MoveControl;
        moveAction.performed += MoveControl;
        moveAction.canceled += MoveControl;

        jumpAction.started += JumpControl;
        jumpAction.performed += JumpControl;
        jumpAction.canceled += JumpControl;

        attackAction.started += AttackControl;
        attackAction.performed += AttackControl;
        attackAction.canceled += AttackControl;
    }

    private void OnDisable()
    {
        moveAction.started -= MoveControl;
        moveAction.performed -= MoveControl;
        moveAction.canceled -= MoveControl;

        jumpAction.started -= JumpControl;
        jumpAction.performed -= JumpControl;
        jumpAction.canceled -= JumpControl;

        attackAction.started -= AttackControl;
        attackAction.performed -= AttackControl;
        attackAction.canceled -= AttackControl;
    }

    private void MoveControl(InputAction.CallbackContext context)
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        if(context.started)
        {
            if(direction.x == 1)
            {
                dir = 1;
                movePressed = true;
            }
            else if(direction.x == -1)
            {
                dir = -1;
                movePressed = true;
            }
            //Debug.Log();
        }
        else if(context.canceled && (direction.x != 1 || direction.x != -1))
        {
            movePressed = false;
            //Debug.Log("Stop move left");
        }
    }

    private void JumpControl(InputAction.CallbackContext context)
    {
        if(context.started)
            jumpPressed = true;
        else if(context.canceled)
            jumpPressed = false;
    }

    private void AttackControl(InputAction.CallbackContext context)
    {
        if(context.started)
            attackPressed = true;
        else if(context.canceled)
            attackPressed = false;
        Debug.Log("AttackPressed");
    }

    private void Update()
    {
        //Vector2 move = moveAction.ReadValue<Vector2>();
        //Debug.Log(move);
    }


    //Getter functions
    public bool IsMovePressed() // edited to pass vector 2 instead
    {
        return movePressed;
    }
    public bool IsJumpPressed()
    {
        return jumpPressed;
    }
    public bool IsAttackPressed()
    {
        return attackPressed;
    }

    public int GetDir()
    {
        return dir;
    }
}
