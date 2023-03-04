using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanIdleState : IdleState
{
    public override bool IsHumanState() {
        return true;
    }
}
