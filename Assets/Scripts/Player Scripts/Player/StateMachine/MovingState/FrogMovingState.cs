using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMovingState : MovingState
{
    public override bool IsHumanState() {
        return false;
    }
}
