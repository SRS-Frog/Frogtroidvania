using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanStateManager : MonoBehaviour
{
    HumanBaseState currentState;
    public HumanIdleState IdleState = new HumanIdleState();
    public HumanMovingState MovingState = new HumanMovingState();
    public HumanAirState AirState = new HumanAirState();
    public HumanAttackState AttackState = new HumanAttackState();
    public HumanPlungeState PlungeState = new HumanPlungeState();
    public HumanDashState DashState = new HumanDashState();

    [HideInInspector] public HumanAttributes humanAttributes;
    [HideInInspector] public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        humanAttributes = GetComponent<HumanAttributes>();
        playerController = GetComponent<PlayerController>();
        currentState = IdleState;

        currentState.EnterState(this, humanAttributes);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        currentState.OnCollisionStay2D(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(HumanBaseState state)
    {
        currentState = state;
        state.EnterState(this, humanAttributes);
    }
}
