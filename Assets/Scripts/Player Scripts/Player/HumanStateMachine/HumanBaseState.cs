using UnityEngine;

public abstract class HumanBaseState
{
    public abstract void EnterState(HumanStateManager human, HumanAttributes attributes);

    public abstract void UpdateState(HumanStateManager human);

    public abstract void FixedUpdateState(HumanStateManager human);

    public abstract void OnCollisionEnter2D(HumanStateManager human, Collision2D collision);

    public abstract void OnCollisionStay2D(HumanStateManager human, Collision2D collision);
}
