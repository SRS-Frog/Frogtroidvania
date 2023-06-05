using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private PlayerStates playerStates;

    private PlayerInput playerInput;

    //store our controls
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction switchAction;
    private InputAction dashAction;
    private InputAction plungeAction;
    private InputAction attackAction;

    // for attack animation
    public delegate void AttackAction();
    public static event AttackAction OnAttack;

    //bool for if keys are pressed
    private bool movePressed;
    private bool jumpPressed;
    private bool switchPressed;
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
        switchAction = playerInput.actions["Switch"];
        dashAction = playerInput.actions["Dash"];
        plungeAction = playerInput.actions["Plunge"];
        attackAction = playerInput.actions["Attack"];
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);

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

        switchAction.started += SwitchControl;
        switchAction.performed += SwitchControl;
        switchAction.canceled += SwitchControl;

        dashAction.started += DashControl;
        dashAction.performed += DashControl;
        dashAction.canceled += DashControl;

        plungeAction.started += PlungeControl;
        plungeAction.performed += PlungeControl;
        plungeAction.canceled += PlungeControl;

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

        switchAction.started += SwitchControl;
        switchAction.performed += SwitchControl;
        switchAction.canceled += SwitchControl;

        dashAction.started -= DashControl;
        dashAction.performed -= DashControl;
        dashAction.canceled -= DashControl;

        plungeAction.started -= PlungeControl;
        plungeAction.performed -= PlungeControl;
        plungeAction.canceled -= PlungeControl;

        attackAction.started -= AttackControl;
        attackAction.performed -= AttackControl;
        attackAction.canceled -= AttackControl;
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

    private void SwitchControl(InputAction.CallbackContext context)
    {
        if (context.started)
            switchPressed = true;
        else if (context.canceled)
            switchPressed = false;
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
        Debug.Log("Plunge Pressed");
    }

    private void AttackControl(InputAction.CallbackContext context)
    {
        if (OnAttack != null) OnAttack();
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

    public bool IsSwitchPressed()
    {
        return switchPressed;
    }

    public void clearSwitchPressedInput()
    {
        switchPressed = false;
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
