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

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
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
