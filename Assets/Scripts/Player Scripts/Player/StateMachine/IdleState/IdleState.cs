using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

// Parent IdleState class
// Concrete child classes -> FrogIdleState and HumandIdleState
public abstract class IdleState : BaseState
{
    PlayerAttributes attributes;

    PlayerController.JumpStates jumpState;

    public override void EnterState(StateManager stateManager, PlayerAttributes attributes)
    {
        //Debug.Log("Hello from IdleState");
        this.attributes = attributes;
    }

    public override void UpdateState(StateManager stateManager)
    {
        jumpState = stateManager.playerController.JumpState();
        string jumpContext = jumpState.ToString();

        if (!attributes.isGrounded)
            stateManager.SwitchState(stateManager.AirState);
        else if (stateManager.playerController.IsSwitchPressed() && IsHumanState()) {
            stateManager.SwitchState(stateManager.FrogIdleState);
            stateManager.playerController.clearSwitchPressedInput();    // Need to clear input so that switch only happens once
        } else if (stateManager.playerController.IsSwitchPressed() && !IsHumanState()) {
            stateManager.SwitchState(stateManager.HumanIdleState);
            stateManager.playerController.clearSwitchPressedInput();    // Need to clear input so that switch only happens once
        } else if(stateManager.playerController.IsAttackPressed())
            stateManager.SwitchState(stateManager.AttackState);
        else if (jumpContext == "performed")
        {
            Debug.Log("Jump");
            Jump();
            stateManager.SwitchState(stateManager.AirState);
        }
        else if(stateManager.playerController.IsMovePressed())
            stateManager.SwitchState(stateManager.MovingState);
    }

    public override void FixedUpdateState(StateManager stateManager)
    {
        if (!stateManager.playerController.IsMovePressed()) // if no movement keys pressed
        {
            Stop();
            stateManager.SwitchState(stateManager.IdleState);
        }
    }

    // Stops the player
    private void Stop()
    {
        if (attributes.isGrounded)
        {
            attributes.rb.velocity = new Vector2(0, attributes.rb.velocity.y);
        }
    }

    public override void OnCollisionEnter2D(StateManager stateManager, Collision2D collision)
    {

    }

    public override void OnCollisionStay2D(StateManager stateManager, Collision2D collision)
    {

    }

    // add a vertical force to the player
    public void Jump()
    {
        if (attributes.isGrounded) // if the player is grounded, jump normally
        {
            attributes.rb.velocity = new Vector2(attributes.rb.velocity.x, attributes.jumpForce);
        }
    }
}
