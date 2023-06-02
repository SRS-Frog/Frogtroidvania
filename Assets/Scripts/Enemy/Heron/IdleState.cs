using UnityEngine;

namespace Enemy.Heron
{
    public class IdleState : State<HeronController>
    {
        private float waitDuration;
        private float waitTimer;
        
        public IdleState(HeronController p) : base(p)
        {
            waitDuration = parent.waitDuration;
        }

        public override void Enter()
        {
            waitTimer = 0f;
        }

        public override void Update()
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > waitDuration)
            {
                parent.ChangeState(Random.Range(0, 2) == 1 ? parent.diveState : parent.featherState);
            }
        }

        public override void Exit()
        {
            
        }
    }
}