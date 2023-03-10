using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovingState : BaseState
{
    PlayerAttributes attributes;

    PlayerController.JumpStates jumpState;

    public override void EnterState(StateManager player, PlayerAttributes attributes)
    {
        Debug.Log("Hello from the Moving State");
        
        this.attributes = attributes;
        attributes.state = PlayerAttributes.PlayerStates.Walking;
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
        {
            player.SwitchState(player.AirState);
        }
        else if (jumpContext == "performed" || jumpContext == "started")
        {
            Debug.Log("Jump");
            Jump();
            player.SwitchState(player.AirState);
        }
        else if (player.playerController.IsAttackPressed())
        {
            player.SwitchState(player.AttackState);
        }
    }

    public override void FixedUpdateState(StateManager player)
    {
        if (!player.playerController.IsMovePressed())
        {
            player.SwitchState(player.IdleState);
        }
        else
            Move(player.playerController.GetDir(), player.playerController.HorizontalVal());
    }
    public override void OnCollisionEnter2D(StateManager player, Collision2D collision)
    {

    }
    public override void OnCollisionStay2D(StateManager player, Collision2D collision)
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

        // animations
        Animate();
    }

    private void Animate()
    {
        if (Mathf.Abs(attributes.rb.velocity.x) >= (attributes.topSpeed * 0.95f)) // if at about 95% of top speed
        {
            attributes.state = PlayerAttributes.PlayerStates.Running;
        }
        else
        {
            attributes.state = PlayerAttributes.PlayerStates.Walking;
        }
    }

    // add a vertical force to the player
    public void Jump()
    {
        if (attributes.isGrounded) // if the player is grounded, jump normally
        {
            attributes.rb.velocity = new Vector2(attributes.rb.velocity.x, attributes.jumpStrength);
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
