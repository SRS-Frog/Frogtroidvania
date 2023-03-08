using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAirState : AirState
{
    public override bool IsHumanState() {
        return false;
    }
}
