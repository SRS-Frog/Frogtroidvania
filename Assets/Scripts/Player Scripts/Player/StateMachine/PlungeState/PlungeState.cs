using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlungeState : BaseState
{
    PlayerAttributes attributes;

    public override void EnterState(StateManager player, PlayerAttributes attributes)
    {
        Debug.Log("Hello from Plungestate");

        this.attributes = attributes;

        attributes.rb.velocity = new Vector2(0f, -attributes.plungeSpeed);
        attributes.canPlunge = false;
    }

    public override void UpdateState(StateManager player)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {
        if (attributes.isGrounded) // if grounded, switch to idle state
        {
            player.SwitchState(player.IdleState);
            return;
        }
    }

    public override void OnCollisionEnter2D(StateManager player, Collision2D collision)
    {

    }

    public override void OnCollisionStay2D(StateManager player, Collision2D collision)
    {

    }
}
