using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashState : BaseState
{
    PlayerAttributes attributes;

    private float dashTimer;

    public override void EnterState(StateManager player, PlayerAttributes attributes)
    {
        Debug.Log("Hello from DashState");
        this.attributes = attributes;
        attributes.state = PlayerAttributes.PlayerStates.Dashing;

        dashTimer = 0f; //restart dash timer
        attributes.canDash = false;

        //DASH
        attributes.rb.gravityScale = 0f; // no vertical force
        float dir = attributes.facingRight ? 1 : -1; // ternary operator, if it is facing right, pos dash, left, neg dash
        attributes.rb.velocity = new Vector2(dir * attributes.dashStrength, 0f); // quickly dash
    }

    public override void UpdateState(StateManager player)
    {
        dashTimer += Time.deltaTime; // add to timer
        Debug.Log("dashtimer " + dashTimer);
        if(dashTimer >= attributes.dashTime)
        {
            StopDash(player);
        }
    }

    public override void FixedUpdateState(StateManager player)
    {
        if (attributes.isGrounded)
        {
            StopDash(player);
            return;
        }
    }

    public override void OnCollisionEnter2D(StateManager player, Collision2D collision)
    {
        StopDash(player);
    }
    public override void OnCollisionStay2D(StateManager player, Collision2D collision)
    {
        StopDash(player);
    }

    private void StopDash(StateManager player) 
    {
        Debug.Log("stop dash");
        attributes.rb.velocity = Vector2.zero; // no speed
        attributes.rb.gravityScale = attributes.baseGravity; // restore gravity
        player.SwitchState(player.AirState); // go to air state
    }
}

