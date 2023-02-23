using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAirState : HumanBaseState
{
    HumanAttributes attributes;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from air state");
        this.attributes = attributes;

    }

    public override void UpdateState(HumanStateManager human)
    {
        
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        if(attributes.isGrounded)
        {
            human.SwitchState(human.IdleState);
            return;
        }
        if(!human.playerController.IsMovePressed())
            human.SwitchState(human.IdleState);
        else
            Move(human.playerController.GetDir());
    }

    public override void OnCollisionEnter(HumanStateManager human, Collision collision)
    {

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
