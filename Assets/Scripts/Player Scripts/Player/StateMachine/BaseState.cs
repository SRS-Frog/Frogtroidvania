using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(StateManager stateManager, PlayerAttributes attributes);

    public abstract void UpdateState(StateManager stateManager);

    public abstract void FixedUpdateState(StateManager stateManager);

    public abstract void OnCollisionEnter(StateManager stateManager, Collision collision);

    public abstract bool IsHumanState();
}
