using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class HumanIdleState : HumanBaseState
{
    HumanAttributes attributes;

    PlayerController.JumpStates jumpState;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        //Debug.Log("Hello from IdleState");
        this.attributes = attributes;
    }

    public override void UpdateState(HumanStateManager human)
    {
        jumpState = human.playerController.JumpState();
        string jumpContext = jumpState.ToString();

        if (!attributes.isGrounded)
            human.SwitchState(human.AirState);
        else if(human.playerController.IsAttackPressed())
            human.SwitchState(human.AttackState);
        else if (jumpContext == "performed")
        {
            Debug.Log("Jump");
            Jump();
            human.SwitchState(human.AirState);
        }
        else if(human.playerController.IsMovePressed())
            human.SwitchState(human.MovingState);
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        if (!human.playerController.IsMovePressed()) // if no movement keys pressed
        {
            Stop();
            human.SwitchState(human.IdleState);
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

    public override void OnCollisionEnter(HumanStateManager human, Collision collision)
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
