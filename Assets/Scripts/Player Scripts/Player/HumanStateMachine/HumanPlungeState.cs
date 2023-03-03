using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlungeState : HumanBaseState
{
    HumanAttributes attributes;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from Plungestate");

        this.attributes = attributes;

        attributes.rb.velocity = new Vector2(0f, -attributes.plungeSpeed);
        attributes.canPlunge = false;
    }

    public override void UpdateState(HumanStateManager human)
    {
        
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        if(attributes.isGrounded) // if grounded, switch to idle state
        {
            human.SwitchState(human.IdleState);
            return;
        }
    }

    public override void OnCollisionEnter2D(HumanStateManager human, Collision2D collision)
    {

    }

    public override void OnCollisionStay2D(HumanStateManager human, Collision2D collision)
    {

    }
}
