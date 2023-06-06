using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAttackState : AttackState
{
    public override bool IsHumanState() {
        return true;
    }
}
