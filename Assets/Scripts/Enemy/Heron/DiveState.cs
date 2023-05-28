using UnityEngine;

namespace Enemy.Heron
{
    public class DiveState : State<HeronController>
    {
        private Rigidbody2D rb;

        private float liftAcceleration = 9.8f;
        private float startingHeight;
        private float bottomHeight;
        private bool bottomed;

        public DiveState(HeronController p) : base(p)
        {
            rb = parent.GetComponent<Rigidbody2D>();
        }

        public override void Enter()
        {
            //Setup
            rb.drag = 0;

            startingHeight = (parent.transform.position.y + parent.player.transform.position.y)/2;
            bottomHeight = parent.player.transform.position.y + 2;
            bottomed = false;
            
            //Initial Velocity
            float height = 
                Mathf.Abs(parent.transform.position.y - parent.player.transform.position.y);
            float hDist = parent.player.transform.position.x - parent.transform.position.x;
            float diveMagnitude = Mathf.Sqrt(Physics.gravity.magnitude * 2 * height);
            
            rb.velocity = 
                Vector2.down * Mathf.Sqrt(Physics.gravity.magnitude * 2 * height) + 
                Vector2.right * (liftAcceleration * hDist / diveMagnitude);
        }

        public override void Update()
        {
            rb.velocity += Vector2.up * liftAcceleration * Time.deltaTime;
            if (!bottomed)
            {
                if (parent.transform.position.y < bottomHeight)
                {
                    bottomed = true;
                }
            } else if (parent.transform.position.y > startingHeight)
            {
                parent.ChangeState(parent.idleState);
            }
        }

        public override void Exit()
        {
            rb.drag = 1.5f;
        }
    }
}