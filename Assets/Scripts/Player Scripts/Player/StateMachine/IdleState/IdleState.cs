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

    public override void UpdateState(StateManager player)
    {
        jumpState = player.playerController.JumpState();
        string jumpContext = jumpState.ToString();

        if (player.playerController.IsSwitchPressed() && IsHumanState()) // frog-human swapping
        {
            player.SwitchState(player.FrogMovingState);
            player.playerController.clearSwitchPressedInput();    // Need to clear input so that switch only happens once
        }
        else if (player.playerController.IsSwitchPressed() && !IsHumanState()) // frog-human swapping
        {
            player.SwitchState(player.HumanMovingState);
            player.playerController.clearSwitchPressedInput();    // Need to clear input so that switch only happens once
        }

        if (!attributes.isGrounded)
            player.SwitchState(player.AirState);
        else if (player.playerController.IsAttackPressed())
            player.SwitchState(player.AttackState);
        else if (jumpContext == "performed")
        {
            Debug.Log("Jump");
            Jump();
            player.SwitchState(player.AirState);
        }
        else if (player.playerController.IsMovePressed())
            player.SwitchState(player.MovingState);
    }

    public override void FixedUpdateState(StateManager player)
    {
        if (!player.playerController.IsMovePressed()) // if no movement keys pressed
        {
            Stop();
            player.SwitchState(player.IdleState);
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
            attributes.rb.velocity = new Vector2(attributes.rb.velocity.x, attributes.jumpStrength);
        }
    }
}
