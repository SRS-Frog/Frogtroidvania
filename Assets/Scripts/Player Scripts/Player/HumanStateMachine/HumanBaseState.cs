using UnityEngine;

public abstract class HumanBaseState
{
    public abstract void EnterState(HumanStateManager human, HumanAttributes attributes);

    public abstract void UpdateState(HumanStateManager human);

    public abstract void FixedUpdateState(HumanStateManager human);

    public abstract void OnCollisionEnter(HumanStateManager human, Collision collision);
}
