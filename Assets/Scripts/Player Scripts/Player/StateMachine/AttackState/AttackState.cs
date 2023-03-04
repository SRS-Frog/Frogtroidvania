using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : BaseState
{
    PlayerAttributes attributes;
    private bool attacking;
    private float attackTimer = 0f;

    public override void EnterState(StateManager player, PlayerAttributes attributes)
    {
        Debug.Log("Hello from AttackState");
        this.attributes = attributes;

        attacking = true;
        attributes.attackArea.SetActive(attacking);
    }

    public override void UpdateState(StateManager player)
    {
        if(attacking) {
            attackTimer += Time.deltaTime;
            if(attackTimer >= attributes.timeToAttack) {
                attackTimer = 0;
                attacking = false;
                attributes.attackArea.SetActive(attacking);
            }
        }
        else
            player.SwitchState(player.IdleState);
    }

    public override void FixedUpdateState(StateManager player)
    {
        
    }

    public override void OnCollisionEnter(StateManager player, Collision collision)
    {

    }
}
