using GLFWDotNet;
using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;


namespace StgSharp.Geometries
{
    public class Point: INode<Point>
    {
        internal StgSharp.ContainerNode<Point> _id;

        internal vec2d _position;

        internal GetLocationHandler getXHandler;
        internal GetLocationHandler getYHandler;

        ContainerNode<Point> INode<Point>.ID
        {
            get { return this._id; }
        }

        public float X
        {
            get { return this._position.X; }
        }

        public float Y
        {
            get { return this._position.Y; }
        }

        void INode<Point>.setID(ContainerNode<Point> id)
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
            this.getXHandler.Invoke(this._position, Control.GameTimeLine.tickCounter);
        }
    }
}
