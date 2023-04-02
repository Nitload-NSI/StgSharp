using Steamworks;
using StgSharp;
using StgSharp.Entities;
using StgSharp.Geometries;
using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace StgSharp
{
    public class Pool:INode<Pool>
    {
        internal LinkedList<Point> pointContainer;
        internal LinkedList<IGeometry> geometryContainer;
        internal LinkedList<Entity> _bulletContainer;
        internal LinkedList<Entity> _awardContainer;
        internal LinkedList<Entity> _enemyContainer;

        internal Line leftBoader;
        internal Line rightBoader;
        internal Line topBoader;
        internal Line bottomBoader;

        public Line LeftBoarder => leftBoader;
        public Line RightBoarder => rightBoader;
        public Line TopBoarder => topBoader;
        public Line BottomBoarder => bottomBoader;

        internal Sensor<EntityPartical> Sensor;

        private ContainerNode<Pool> _id;



        public Pool(Line left, Line right, Line top, Line bottom)
        {
            topBoader = top;
            bottomBoader= bottom;
            rightBoader = right;
            leftBoader = left;
        }
        public Pool( Geometries.Rectangle area)
        {
            
        }


        ContainerNode<Pool> INode<Pool>.ID => _id;

        public void OnRender()
        {
            foreach (Enemy e in _enemyContainer)
            {
                
            }
        }

        void INode<Pool>.setID(ContainerNode<Pool> id)
        {
            if (_id == default)
            {
                _id = id;
            }
            else
            {
                throw new ArgumentException("Cannot set ID twicee!");
            }
        }
    }

}
