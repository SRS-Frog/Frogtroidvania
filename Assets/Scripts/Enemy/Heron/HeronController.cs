using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeronController : MonoBehaviour
{

    #region Base State

    [Serializable]
    struct GraphNode
    {
        public GraphNode(Transform t, int[] g)
        {
            pos = t;
            neighbors = g;
        }

        public Transform pos;
        public int[] neighbors;
    }

    [SerializeField] private GraphNode[] nodes1;
    private int curPosition1 = 0;

    [SerializeField] private GraphNode[] nodes2;
    private int curPosition2 = 0;
    
    //Constants
    private const float NextWaypointRadius = 3f;
    
    private void BaseStateBehaviour()
    {
        if (path == null) return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            UpdatePath();
            return;
        };
            
        rb.AddForce((path.vectorPath[currentWaypoint] - transform.position).normalized * Time.deltaTime * speed);
        // if (rb.velocity.magnitude > speed) rb.velocity = rb.velocity.normalized * speed;
            
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < NextWaypointRadius)
        {
            currentWaypoint++;
        }
    }
    
    private void UpdatePath() 
    {
        if (!seeker.IsDone()) return;
            
        seeker.StartPath(transform.position, generatePos(), p =>
        {
            path = p;
            currentWaypoint = 0;
        });
    }

    private Vector3 generatePos()
    {
        if (isLeft)
        {
            curPosition1 = nodes1[curPosition1].neighbors[Random.Range(0, nodes1[curPosition1].neighbors.Length)];
            return nodes1[curPosition1].pos.position;
        }
        else
        {
            curPosition2 = nodes2[curPosition2].neighbors[Random.Range(0, nodes2[curPosition2].neighbors.Length)];
            return nodes2[curPosition2].pos.position;
        }
    }

    #endregion

    enum States
    {
        BASE_STATE
    }

    private States curState = States.BASE_STATE;
    private bool isLeft = true;

    [SerializeField] private float speed;

    [SerializeField] private Transform bottomLeft1;
    [SerializeField] private Transform upperRight1;

    private Rigidbody2D rb;
    private Health health;
    
    // Astar Variables
    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        
        UpdatePath();
    }

    // Update is called once per frame
    void Update()
    {
        if (curState == States.BASE_STATE)
        {
            BaseStateBehaviour();
        }

        isLeft = health.health > 50;
    }
}
