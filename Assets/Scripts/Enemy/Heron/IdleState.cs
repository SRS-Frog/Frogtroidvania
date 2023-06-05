using UnityEngine;

namespace Enemy.Heron
{
    public class IdleState : State<HeronController>
    {
        private float waitDuration;
        private float waitTimer;

        private SpriteRenderer sp;
        private Animator anim;
        
        public IdleState(HeronController p) : base(p)
        {
            waitDuration = parent.waitDuration;
            
            sp = p.GetComponent<SpriteRenderer>();
            anim = p.GetComponent<Animator>();
        }

        public override void Enter()
        {
            waitTimer = 0f;
            anim.SetBool("Hover", true);
        }

        public override void Update()
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > waitDuration)
            {
                parent.ChangeState(Random.Range(0, 2) == 1 ? parent.diveState : parent.featherState);
            }

            sp.flipX = parent.transform.position.x < parent.player.transform.position.x;
        }

        public override void Exit()
        {
            anim.SetBool("Hover", false);
        }
    }
}