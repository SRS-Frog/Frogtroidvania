using Pathfinding;
using UnityEngine;

namespace Enemy.Heron
{
    public class RiseState : State<HeronController>
    {
        private const float NextWaypointRadius = 3f;
        private int currentWaypoint = 0;

        private Path path;
        private Seeker seeker;
        private Rigidbody2D rb;

        private float attackTimer;
        
        public RiseState(HeronController p) : base(p)
        {
            seeker = p.GetComponent<Seeker>();
            rb = p.GetComponent<Rigidbody2D>();
        }
        
        public override void Enter()
        {
            attackTimer = 0;
            UpdatePath();
        }

        public override void Update()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > parent.timePerAttack)
            {
                parent.ChangeState(parent.diveState);
            }

            if (path == null) return;
        
            if (currentWaypoint >= path.vectorPath.Count)
            {
                UpdatePath();
                return;
            };
            
            rb.AddForce((path.vectorPath[currentWaypoint] - parent.transform.position).normalized * Time.deltaTime * parent.speed);
            
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < NextWaypointRadius)
            {
                currentWaypoint++;
            }
        }

        public override void Exit()
        {
            
        }
        
        
        private void UpdatePath() 
        {
            if (!seeker.IsDone()) return;
            
            seeker.StartPath(parent.transform.position, generatePos(), p =>
            {
                path = p;
                currentWaypoint = 0;
            });
        }

        private Vector3 generatePos()
        {
            if (parent.isLeft)
            {
                parent.nodes1 = parent.nodes1.neighbors[Random.Range(0, parent.nodes1.neighbors.Length)];
                return parent.nodes1.transform.position;
            }
            else
            {
                parent.nodes2 = parent.nodes2.neighbors[Random.Range(0, parent.nodes2.neighbors.Length)];
                return parent.nodes2.transform.position;
            }
        }
    }
}