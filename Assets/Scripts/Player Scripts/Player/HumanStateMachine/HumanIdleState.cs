using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanIdleState : HumanBaseState
{
    HumanAttributes attributes;
    
    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        //Debug.Log("Hello from IdleState");
        this.attributes = attributes;
    }

    public override void UpdateState(HumanStateManager human)
    {
        if(!attributes.isGrounded)
            human.SwitchState(human.AirState);
        else if(human.playerController.IsAttackPressed())
            human.SwitchState(human.AttackState);
        else if(human.playerController.IsJumpPressed() && attributes.isGrounded)
        {
            Debug.Log("Idle Jump");
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
        attributes.rb.AddForce(new Vector2(0f, attributes.jumpForce));
    }
}
