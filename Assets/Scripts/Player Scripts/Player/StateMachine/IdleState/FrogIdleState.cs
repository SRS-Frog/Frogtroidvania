using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIdleState : IdleState
{
    public override bool IsHumanState() {
        return false;
    }
}
