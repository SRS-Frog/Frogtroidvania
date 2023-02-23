using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlungeState : HumanBaseState
{
    public override void EnterState(HumanStateManager human, HumanAttributes attributes)
    {
        Debug.Log("Hello from Plungestate");
    }

    public override void UpdateState(HumanStateManager human)
    {
        
    }

    public override void FixedUpdateState(HumanStateManager human)
    {
        
    }

    public override void OnCollisionEnter(HumanStateManager human, Collision collision)
    {

    }
}
