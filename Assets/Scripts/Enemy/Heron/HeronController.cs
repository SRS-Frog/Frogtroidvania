using System;
using Enemy.Heron;
using UnityEngine;
using IdleState = Enemy.Heron.IdleState;

public class HeronController : MonoBehaviour
{
    //Rise State

    [SerializeField] public HeronGraphNode nodes1;
    [SerializeField] public HeronGraphNode nodes2;
    [SerializeField] public float timePerAttack;
    
    //Idle State
    [SerializeField] public float waitDuration;
    
    //Dive SState
    [SerializeField] public float horizontalMin;
    [SerializeField] public float diveHeight;

    #region StateMachine

    public RiseState riseState;
    public Enemy.Heron.IdleState idleState;
    public DiveState diveState;
    private State<HeronController> curState;
    
    [NonSerialized] public bool isLeft = true;

    public void ChangeState(State<HeronController> s)
    {
        if(curState != null) curState.Exit();
        curState = s;
        curState.Enter();
    }

    #endregion

    [SerializeField] public float speed;
    [SerializeField] public PlayerController player;
    
    private Health health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();

        riseState = new RiseState(this);
        idleState = new Enemy.Heron.IdleState(this);
        diveState = new DiveState(this);

        ChangeState(diveState);
    }

    // Update is called once per frame
    void Update()
    {
        curState.Update();

        isLeft = health.health > 50;
    }
}
