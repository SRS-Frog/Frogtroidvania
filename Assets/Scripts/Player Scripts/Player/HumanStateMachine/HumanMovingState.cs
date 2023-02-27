using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovingState : HumanBaseState
{
    HumanAttributes attributes;

    PlayerController.JumpStates jumpState;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from the Moving State");
        this.attributes = attributes;
    }

    public override void UpdateState(HumanStateManager human)
    {
        jumpState = human.playerController.JumpState();
        string jumpContext = jumpState.ToString();

        if (!attributes.isGrounded)
        {
            human.SwitchState(human.AirState);
        }
        else if(jumpContext == "performed" || jumpContext == "started")
        {
            Debug.Log("Jump");
            Jump();
            human.SwitchState(human.AirState);
        }
        else if(human.playerController.IsAttackPressed())
        {
            human.SwitchState(human.AttackState);
        }
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        
        if(!human.playerController.IsMovePressed())
        {
            human.SwitchState(human.IdleState);
        }
        else
            Move(human.playerController.GetDir(), human.playerController.HorizontalVal());
    }

    public override void OnCollisionEnter(HumanStateManager human, Collision collision)
    {

    }

    // deals with the the velocity of the player, and calls Flip() when applicable
    public void Move(int dir, float horizontal)
    {
        Debug.Log("Moving " + attributes.rb.velocity);
        // changes horizontal velocity of player
        ////Time.deltaTime makes the speed more constant between different computers with different frames per second
        attributes.rb.velocity += new Vector2(horizontal * attributes.acceleration * Time.deltaTime, 0); // move with acceleration
        float clampedX = Vector2.ClampMagnitude(new Vector2(attributes.rb.velocity.x, 0), attributes.topSpeed).x; // clamp horizontal value to top speed
        attributes.rb.velocity = new Vector2(clampedX, attributes.rb.velocity.y);

        // flip the player if needed
        if ((attributes.facingRight && dir == -1) || 
            (!attributes.facingRight && dir == 1))
            Flip();
    }

    // add a vertical force to the player
    public void Jump()
    {
        if (attributes.isGrounded) // if the player is grounded, jump normally
        {
            attributes.rb.velocity = new Vector2(attributes.rb.velocity.x, attributes.jumpForce);
        }
    }

    // flip the player
    private void Flip()
    {
        attributes.facingRight = !attributes.facingRight;

        // flips the sprite AND its colliders
        Vector3 theScale = attributes.transform.localScale;
        theScale.x *= -1;
        attributes.transform.localScale = theScale;
    }
}
