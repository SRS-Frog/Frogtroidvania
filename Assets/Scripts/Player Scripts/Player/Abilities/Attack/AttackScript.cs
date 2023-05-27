using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : BaseAbilityScript
{
    // Written in ref to this tutorial https://youtu.be/sPiVz1k-fEs
    public float attackRange = 0.75f;  // Circular range of attakc
    public LayerMask enemyLayers;
    public int attackPoints = 10;    // How many hitpoints the attack does

    // 
    public float attackRate = 2f;       // How many attacks per second
    private float nextAttackTime = 0f;  // Use to calculate when you can attack again
    

    public override bool IsHumanAbility() {
        return true;
    }

    public override bool IsFrogAbility() {
        return false;
    }

    // assign stateManager variable in parent class
    public override void Awake() {
        base.stateManager = transform.parent.gameObject.GetComponent<StateManager>();
    }

    // Attack logic here
    public void TriggerAttack() {
        if (CanTriggerAbility()) {
            // Check if enough time has passed to attack again
            if (Time.time >= nextAttackTime) {
                nextAttackTime = Time.time + 1f/attackRate;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies) {
                    enemy.GetComponent<Health>().Damage(attackPoints);
                }
            }
        }

    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
