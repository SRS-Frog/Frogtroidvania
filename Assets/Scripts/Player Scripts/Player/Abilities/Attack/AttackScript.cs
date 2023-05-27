using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : BaseAbilityScript
{
    // Written in ref to this tutorial https://youtu.be/sPiVz1k-fEs
    public float attackRange = 0.75f;  // Circular range of attakc
    public LayerMask enemyLayers;

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
                Debug.Log("Attack");
                // TODO: add more after enemies are merged
            }
        }

    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
