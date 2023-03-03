using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDashState : HumanBaseState
{
    HumanAttributes attributes;

    private float dashTimer;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from DashState");
        this.attributes = attributes;

        dashTimer = 0f; //restart dash timer
        attributes.canDash = false;

        //DASH
        attributes.rb.gravityScale = 0f; // no vertical force
        float dir = attributes.facingRight ? 1 : -1; // ternary operator, if it is facing right, pos dash, left, neg dash
        attributes.rb.velocity = new Vector2(dir * attributes.dashStrength, 0f); // quickly dash
    }

    public override void UpdateState(HumanStateManager human)
    {
        dashTimer += Time.deltaTime; // add to timer
        if(dashTimer >= attributes.dashTime)
        {
            StopDash(human);
        }
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        if (attributes.isGrounded)
        {
            StopDash(human);
            return;
        }
    }

    public override void OnCollisionEnter2D(HumanStateManager human, Collision2D collision)
    {
        StopDash(human);
    }
    public override void OnCollisionStay2D(HumanStateManager human, Collision2D collision)
    {
        StopDash(human);
    }

    private void StopDash(HumanStateManager human) 
    {
        attributes.rb.velocity = Vector2.zero; // no speed
        attributes.rb.gravityScale = attributes.baseGravity; // restore gravity
        human.SwitchState(human.AirState); // go to air state
    }
}

