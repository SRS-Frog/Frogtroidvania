using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovingState : HumanBaseState
{
    HumanAttributes attributes;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from the Moving State");
        this.attributes = attributes;
    }

    public override void UpdateState(HumanStateManager human)
    {
        if(!attributes.isGrounded)
        {
            human.SwitchState(human.AirState);
        }
        else if(human.playerController.IsJumpPressed())
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
            Stop();
            human.SwitchState(human.IdleState);
        }
        else
            Move(human.playerController.GetDir());
    }

    public override void OnCollisionEnter(HumanStateManager human, Collision collision)
    {

    }

    // stops the player when no horizontal movement key is pressed
    private void Stop()
    {
        if (attributes.isGrounded)
        {
            attributes.rb.velocity = new Vector2(0, attributes.rb.velocity.y);
        }
    }

    // deals with the the velocity of the player, and calls Flip() when applicable
    public void Move(int dir)
    {
        // changes horizontal velocity of player
        ////Time.deltaTime makes the speed more constant between different computers with different frames per second

        attributes.rb.velocity = new Vector2(dir * 
        attributes.moveSpeed * Time.deltaTime, attributes.rb.velocity.y);

        // flip the player if needed
        if ((attributes.facingRight && dir == -1) || 
            (!attributes.facingRight && dir == 1))
            Flip();
    }

    // add a vertical force to the player
    public void Jump()
    {
        attributes.rb.AddForce(new Vector2(0f, attributes.jumpForce));
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
