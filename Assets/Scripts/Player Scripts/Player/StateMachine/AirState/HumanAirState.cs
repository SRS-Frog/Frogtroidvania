using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAirState : AirState
{
    public override bool IsHumanState() {
        return true;
    }
}
