namespace Enemy.Heron
{
    public abstract class State<T>
    {
        protected T parent;
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        
        public State(T p)
        {
            parent = p;
        }
    }
}