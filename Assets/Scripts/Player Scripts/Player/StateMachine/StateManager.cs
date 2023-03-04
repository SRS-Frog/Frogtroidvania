using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;
    [HideInInspector] public PlayerAttributes playerAttributes;
    [HideInInspector] public PlayerController playerController;

    // Instantiate Human states
    public IdleState HumanIdleState = new HumanIdleState();
    public AirState HumanAirState = new HumanAirState();
    public MovingState HumanMovingState = new HumanMovingState();
    public AttackState HumanAttackState = new HumanAttackState();
    public PlungeState HumanPlungeState = new HumanPlungeState();

    // Instantiate Frog states 
    public IdleState FrogIdleState = new FrogIdleState();
    public AirState FrogAirState = new FrogAirState();
    public MovingState FrogMovingState = new FrogMovingState();
    public AttackState FrogAttackState = new FrogAttackState();
    public PlungeState FrogPlungeState = new FrogPlungeState();

    // Assign these variables depending on if player is in Frog or Human state
    // Mostly here so I get to rewrite less code
    public IdleState IdleState;
    public MovingState MovingState; 
    public AirState AirState; 
    public AttackState AttackState;
    public PlungeState PlungeState;

    // Start is called before the first frame update
    void Start()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        playerController = GetComponent<PlayerController>();

        IdleState = HumanIdleState; 
        AirState = HumanAirState;
        MovingState = HumanMovingState;
        currentState = IdleState;

        currentState.EnterState(this, playerAttributes);
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        Debug.Log("is Human?" + currentState.IsHumanState());
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        // Handle all property swaps between human and frog mode
        if (state.IsHumanState() != currentState.IsHumanState()) {
            // Swap between human and frog collider
            playerAttributes.humanCollider.enabled = !playerAttributes.humanCollider.enabled;
            playerAttributes.frogCollider.enabled = !playerAttributes.frogCollider.enabled;

            if (state.IsHumanState()) {
                // Switch to Human States, sprites, and attributes
                IdleState = HumanIdleState;
                AirState = HumanAirState;
                MovingState = HumanMovingState;
                PlungeState = HumanPlungeState;
                AttackState = HumanAttackState;
                playerAttributes.spriteRenderer.sprite = playerAttributes.humanSprite;
                playerAttributes.topSpeed = PlayerAttributes.humanTopSpeed;
                playerAttributes.acceleration = PlayerAttributes.humanAcceleration;
                playerAttributes.jumpForce = PlayerAttributes.humanJumpForce;
            } else {
                // Switch to Frog States, sprites and attributes
                IdleState = FrogIdleState;
                AirState = FrogAirState;
                MovingState = FrogMovingState;
                PlungeState = FrogPlungeState;
                AttackState = FrogAttackState;
                playerAttributes.spriteRenderer.sprite = playerAttributes.frogSprite;
                playerAttributes.topSpeed = PlayerAttributes.frogTopSpeed;
                playerAttributes.acceleration = PlayerAttributes.frogAcceleration;
                playerAttributes.jumpForce = PlayerAttributes.frogJumpForce;
            }
        }
        currentState = state;
        state.EnterState(this, playerAttributes);
    }
}
