using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class HumanAirState : HumanBaseState
{
    HumanAttributes attributes;

    PlayerController.JumpStates jumpState;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        //Debug.Log("Hello from air state");
        this.attributes = attributes;

    }

    public override void UpdateState(HumanStateManager human)
    {

    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        jumpState = human.playerController.JumpState();
        string jumpContext = jumpState.ToString();

        // for small jumps
        if (jumpContext == "canceled" && attributes.rb.velocity.y > 0) // if the jump key was canceled midjump, fall faster than usual
        {
            attributes.rb.velocity = new Vector2(attributes.rb.velocity.x, attributes.rb.velocity.y* 0.5f);
        }

        if (attributes.isGrounded)
        {
            human.SwitchState(human.IdleState);
            return;
        }

        if (attributes.canDash && human.playerController.IsDashPressed()) // if you can dash and dash is pressed, dash
        {
            human.SwitchState(human.DashState); 
            return;
        }

        if (!human.playerController.IsMovePressed()) // if no movement keys pressed
        {
            human.SwitchState(human.IdleState);
        }
        else
            Move(human.playerController.GetDir(), human.playerController.HorizontalVal());
    }

    public override void OnCollisionEnter2D(HumanStateManager human, Collision2D collision)
    {

    }

    public override void OnCollisionStay2D(HumanStateManager human, Collision2D collision)
    {

    }

    // Stops the player
    private void Stop()
    {
        if (attributes.isGrounded)
        {
            attributes.rb.velocity = new Vector2(0, attributes.rb.velocity.y);
        }
    }

    // deals with the the velocity of the player, and calls Flip() when applicable
    public void Move(int dir, float horizontal)
    {
        Debug.Log("Air Moving " + attributes.rb.velocity);

        // changes horizontal velocity of player
        ////Time.deltaTime makes the speed more constant between different computers with different frames per second
        attributes.rb.velocity += new Vector2(horizontal * attributes.acceleration * Time.deltaTime, 0); // move with acceleration

        if (attributes.isHooked) // if the human is hooked, then clamp velocity to frapple top speed (else frapple along!!)
        {
            float clampedX = Vector2.ClampMagnitude( new Vector2 (attributes.rb.velocity.x, 0), attributes.frappleTopSpeed).x; // clamp horizontal value to top speed
            attributes.rb.velocity = new Vector2(clampedX, attributes.rb.velocity.y);
            Debug.Log("Frapple Speed");
        } else
        {
            float clampedX = Vector2.ClampMagnitude(new Vector2(attributes.rb.velocity.x, 0), attributes.topSpeed).x; // clamp horizontal value to top speed
            attributes.rb.velocity = new Vector2(clampedX, attributes.rb.velocity.y);
        }

        // flip the player if needed
        if ((attributes.facingRight && dir == -1) ||
            (!attributes.facingRight && dir == 1))
            Flip();
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
