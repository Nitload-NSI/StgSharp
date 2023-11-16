using StgSharp.Geometries;
using StgSharp.Math;

namespace StgSharp
{
    public abstract class IEntity
    {
        internal Point _pos;
        internal LinkedList<PlainGeometry> _collisionBox;
        internal Sensor<PlainGeometry> _collisionSensor;
        internal string _texture;
        internal Point _position;
        internal Point _basePoint;
        internal GetLocationHandler _movHandler;
        protected uint _bornTick;

        public LinkedList<PlainGeometry> CollisionBox
        {
            get { return _collisionBox; }
            set { _collisionBox = value; }
        }

        /// <summary>
        /// A sensor used for cheking out if a bullet hit a player or something else
        /// </summary>
        public Sensor<PlainGeometry> CollisionSensor
        {
            get { return _collisionSensor; }
            set { _collisionSensor = value; }
        }

        public string Texture { get; set; }

        /// <summary>
        /// A 2D vector presenting the position of an enemy entity, position can also be visited by X, Y
        /// </summary>
        public Vec2d Location { get; }

        /// <summary>
        /// X position of an enenmy entity
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y position of an enemy entity
        /// </summary>
        public float Y { get; set; }

        public void SetLocation(Point p)
        {
            _pos = p;
        }

        public abstract void OnRenderFrame();

    }
}
