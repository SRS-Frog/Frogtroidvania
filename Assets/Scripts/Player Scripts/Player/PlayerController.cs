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
    private InputAction dashAction;
    private InputAction plungeAction;

    //bool for if keys are pressed
    private bool movePressed;
    private bool jumpPressed;
    private bool attackPressed;
    private bool dashPressed;
    private bool plungePressed;

    //specific for movement
    private int dir;
    private float horizontal;

    public enum JumpStates // FAUSTINE: IDK IF THIS IS GOOD AT ALL, CONSIDER REFACTORING
    {
        started,
        performed,
        canceled
    }
    JumpStates jumpState = JumpStates.canceled;

    private void Awake()
    {
        //playerStates = GetComponent<PlayerStates>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        dashAction = playerInput.actions["Dash"];
        plungeAction = playerInput.actions["Plunge"];

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

        dashAction.started += DashControl;
        dashAction.performed += DashControl;
        dashAction.canceled += DashControl;

        plungeAction.started += PlungeControl;
        plungeAction.performed += PlungeControl;
        plungeAction.canceled += PlungeControl;
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

        dashAction.started -= DashControl;
        dashAction.performed -= DashControl;
        dashAction.canceled -= DashControl;

        plungeAction.started -= PlungeControl;
        plungeAction.performed -= PlungeControl;
        plungeAction.canceled -= PlungeControl;
    }

    private void MoveControl(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            horizontal = moveAction.ReadValue<float>(); // get 1D axis float from movement input
            if (horizontal > 0)
            {
                dir = 1;
                movePressed = true;
            }
            else if(horizontal < 0)
            {
                dir = -1;
                movePressed = true;
            }
            //Debug.Log();
        }
        else if(context.canceled)
        {
            horizontal = 0; // set it to 0
            movePressed = false;
            //Debug.Log("Stop move left");
        }
    }

    private void JumpControl(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jumpPressed = true;
            jumpState = JumpStates.started;
        }
        else if(context.performed)
        {
            jumpPressed = true;
            jumpState = JumpStates.performed;
        } 
        else if (context.canceled)
        {
            jumpPressed = false;
            jumpState = JumpStates.canceled;
        }      
    }

    private void AttackControl(InputAction.CallbackContext context)
    {
        if(context.started)
            attackPressed = true;
        else if(context.canceled)
            attackPressed = false;
        Debug.Log("AttackPressed");
    }

    private void DashControl(InputAction.CallbackContext context)
    {
        if(context.started || context.performed)
        {
            dashPressed = true;
        } else if(context.canceled)
        {
            dashPressed = false;
        }
    }

    private void PlungeControl(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            plungePressed = true;
        }
        else if (context.canceled)
        {
            plungePressed = false;
        }
    }

    private void Update()
    {

    }


    //Getter functions
    public bool IsMovePressed()
    {
        return movePressed;
    }

    public float HorizontalVal()
    {
        return horizontal; // returns the horizontal input value
    }

    public bool IsJumpPressed()
    {
        return jumpPressed;
    }

    public JumpStates JumpState()
    {
        return jumpState;
    }

    public bool IsAttackPressed()
    {
        return attackPressed;
    }

    public bool IsDashPressed()
    {
        return dashPressed;
    }

    public bool IsPlungePressed()
    {
        return plungePressed;
    }

    public int GetDir()
    {
        return dir;
    }
}
