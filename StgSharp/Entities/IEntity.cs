//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IEntity.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Geometries;
using StgSharp.Math;

using System.Collections.Generic;

namespace StgSharp
{
    public abstract class IEntity
    {

        protected uint _bornTick;
        internal Point _basePoint;
        internal LinkedList<PlainGeometry> _collisionBox;
        internal Sensor<PlainGeometry> _collisionSensor;
        internal GetLocationHandler _movHandler;
        internal Point _pos;
        internal Point _position;
        internal string _texture;

        public LinkedList<PlainGeometry> CollisionBox
        {
            get => _collisionBox;
            set => _collisionBox = value;
        }

        /// <summary>
        /// A sensor used for cheking out if a bullet hit a player or something else
        /// </summary>
        public Sensor<PlainGeometry> CollisionSensor
        {
            get => _collisionSensor;
            set => _collisionSensor = value;
        }

        /// <summary>
        /// A 2D vector presenting the position of an enemy entity, position can also be visited by X, Y
        /// </summary>
        public Vec2d Location { get; }

        public string Texture { get; set; }

        /// <summary>
        /// X position of an enenmy entity
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y position of an enemy entity
        /// </summary>
        public float Y { get; set; }

        public abstract void OnRenderFrame();

        public void SetLocation(Point p)
        {
            _pos = p;
        }

    }
}