using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAttackState : AttackState
{
    public override bool IsHumanState() {
        return false;
    }
}
