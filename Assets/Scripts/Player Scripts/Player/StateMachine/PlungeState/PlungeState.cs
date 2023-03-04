using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlungeState : BaseState
{
    public override void EnterState(StateManager player, PlayerAttributes attributes)
    {
        Debug.Log("Hello from Plungestate");
    }

    public override void UpdateState(StateManager player)
    {
        
    }

    public override void FixedUpdateState(StateManager player)
    {
        
    }

    public override void OnCollisionEnter(StateManager player, Collision collision)
    {

    }
}
