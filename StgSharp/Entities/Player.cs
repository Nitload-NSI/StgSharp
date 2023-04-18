using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

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
