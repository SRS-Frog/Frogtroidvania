using UnityEngine;

namespace Enemy.Heron
{
    public class DiveState : State<HeronController>
    {
        private Rigidbody2D rb;
        private SpriteRenderer sp;
        private Animator anim;

        private float liftAcceleration = 9.8f;
        private float startingHeight;
        private float bottomHeight;
        private bool bottomed;
        private bool prepared;
        
        public DiveState(HeronController p) : base(p)
        {
            rb = parent.GetComponent<Rigidbody2D>();
            sp = p.GetComponent<SpriteRenderer>();
            anim = p.GetComponent<Animator>();
        }

        public override void Enter()
        {
            //Setup
            rb.drag = 0;

            startingHeight = (parent.transform.position.y + parent.player.transform.position.y)/2;
            bottomHeight = parent.player.transform.position.y;
            bottomed = false;
            prepared = false;
        }

        public override void Update()
        {
            if (prepared)
            {
                rb.velocity += Vector2.up * liftAcceleration * Time.deltaTime;
                sp.flipX = rb.velocity.x > 0;
                
                if (!bottomed)
                {
                    if (parent.transform.position.y < bottomHeight + 2 && rb.velocity.y > 0)
                    {
                        bottomed = true;
            
                        anim.SetBool("Dive", false);
                    }
                }
                else if (parent.transform.position.y > startingHeight)
                {
                    parent.ChangeState(parent.riseState);
                }
            }
            else
            {
                Vector3 horizontalGoal = (parent.player.transform.position.x > parent.transform.position.x ? Vector3.left : Vector3.right) * (parent.horizontalMin * (parent.enraged ? 2f : 1));
                Vector3 verticalGoal = Vector3.up * (parent.diveHeight * (parent.enraged ? 0.5f : 1));
                Vector3 goalPos = parent.player.transform.position + horizontalGoal + verticalGoal;
                
                rb.AddForce((goalPos - parent.transform.position).normalized * parent.speed * Time.deltaTime);
                sp.flipX = rb.velocity.x > 0;
                
                if (Vector2.Distance(parent.transform.position, goalPos) < 5f)
                {
                    //Initial Velocity
                    float height = 
                        Mathf.Abs(parent.transform.position.y - parent.player.transform.position.y) - 1;
                    float hDist = parent.player.transform.position.x - parent.transform.position.x;
                    float diveMagnitude = Mathf.Sqrt(Physics.gravity.magnitude * 2 * height);
            
                    rb.velocity = 
                        Vector2.down * Mathf.Sqrt(Physics.gravity.magnitude * 2 * height) + 
                        Vector2.right * (liftAcceleration * hDist / diveMagnitude);

                    prepared = true;
            
                    anim.SetBool("Dive", true);
                }
            }
        }

        public override void Exit()
        {
            rb.drag = 1.5f;
        }
    }
}