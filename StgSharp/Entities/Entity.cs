using StgSharp.Math;
using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    public class Entity:INode<Entity>
    {
        internal Point _pos;
        internal IGeometry _collisionBox;
        internal Sensor<IGeometry> _collisionSensor;
        internal ContainerNode<Entity> _id;
        internal String _texture;
        internal Point _position;
        internal Point _basePoint;
        internal GetLocationHandler _movHandler;

        public IGeometry CollisionBox 
        {
            get { return _collisionBox; }
            set { _collisionBox = value; }
        }
        

        /// <summary>
        /// A sensor used for cheking out if a bullet hit a player or something else
        /// </summary>
        public Sensor<IGeometry> CollisionSensor 
        {
            get { return _collisionSensor; }
            set { _collisionSensor = value; } 
        }

        public string Texture { get; set; }

        /// <summary>
        /// A 2D vector presenting the position of an enemy entity, position can also be visited by X, Y
        /// </summary>
        public vec2d Location { get; }

        /// <summary>
        /// X position of an enenmy entity
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y position of an enemy entity
        /// </summary>
        public float Y { get; set; }

        ContainerNode<Entity> INode<Entity>.ID
        {
            get { return _id; } 
        }

        public void SetLocation(Point p)
        {
            _pos = p;
        }

        void INode<Entity>.setID(ContainerNode<Entity> id)
        {
            if (id == null)
            {
                _id = id;
            }
            else
            {
                throw new ArgumentException("Cannot set id twice!");
            }
        }

        public void OnRender()
        {
            _position._position = _movHandler.Invoke(_basePoint._position, Control.GameTimeLine.tickCounter);
        }

    }
}
