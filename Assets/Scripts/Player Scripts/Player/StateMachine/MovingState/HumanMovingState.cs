using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovingState : MovingState
{
    public override bool IsHumanState() {
        return true;
    }
}
