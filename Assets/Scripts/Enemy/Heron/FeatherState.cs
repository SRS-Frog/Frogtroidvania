using UnityEngine;

namespace Enemy.Heron
{
    public class FeatherState : State<HeronController>
    {
        private float waitDuration;
        private float waitTimer;
        
        public FeatherState(HeronController p) : base(p)
        {
            waitDuration = parent.featherDuration;
        }

        public override void Enter()
        {
            waitTimer = 0f;
            parent.SummonFeathers();
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
            
        }
    }
}