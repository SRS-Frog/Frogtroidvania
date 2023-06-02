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
    
    //Dive State
    [SerializeField] public float horizontalMin;
    [SerializeField] public float diveHeight;
    
    //Feather State
    [SerializeField] public float featherDuration;
    [SerializeField] private GameObject feather;
    [SerializeField] private float spreadAngle;

    #region StateMachine

    public RiseState riseState;
    public Enemy.Heron.IdleState idleState;
    public DiveState diveState;
    public FeatherState featherState;
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
        featherState = new FeatherState(this);

        ChangeState(diveState);
    }

    // Update is called once per frame
    void Update()
    {
        curState.Update();

        isLeft = health.health > 50;
    }

    public void SummonFeathers()
    {
        float angle = -spreadAngle;
        Vector3 dir = player.transform.position - transform.position;

        for (; angle <= spreadAngle; angle += spreadAngle)
        {
            GameObject g = Instantiate(feather, transform.position, Quaternion.identity);
            g.GetComponent<Feather>().Init(Quaternion.Euler(0,0, angle) * dir);
        }
    }
}
