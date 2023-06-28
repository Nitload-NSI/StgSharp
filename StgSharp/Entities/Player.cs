namespace StgSharp
{
    public abstract class Player : IEntity
    {
        internal float _velocity;

        public float Velocity
        {
            get { return _velocity; }
        }



    }
}
