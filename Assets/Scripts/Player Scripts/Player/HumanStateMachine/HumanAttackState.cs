using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackState : HumanBaseState
{
    HumanAttributes attributes;
    private bool attacking;
    private float attackTimer = 0f;

    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from AttackState");
        this.attributes = attributes;

        attacking = true;
        attributes.attackArea.SetActive(attacking);
    }

    public override void UpdateState(HumanStateManager human)
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
            human.SwitchState(human.IdleState);
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        
    }

    public override void OnCollisionEnter2D(HumanStateManager human, Collision2D collision)
    {

    }
    public override void OnCollisionStay2D(HumanStateManager human, Collision2D collision)
    {

    }
}
