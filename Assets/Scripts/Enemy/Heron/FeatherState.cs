using UnityEngine;

namespace Enemy.Heron
{
    public class FeatherState : State<HeronController>
    {
        private float waitDuration;
        private float waitTimer;
        
        private Animator anim;
        private SpriteRenderer sp;
        
        public FeatherState(HeronController p) : base(p)
        {
            waitDuration = parent.featherDuration;
            anim = parent.GetComponent<Animator>();
            sp = parent.GetComponent<SpriteRenderer>();
        }

        public override void Enter()
        {
            waitTimer = 0f;
            parent.SummonFeathers();
            sp.flipX = parent.transform.position.x < parent.player.transform.position.x;
            anim.SetBool("Feather", true);
        }

        public override void Update()
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > waitDuration)
            {
                parent.ChangeState(parent.riseState);
            }
        }

        public override void Exit()
        {
            anim.SetBool("Feather", false);
        }
    }
}