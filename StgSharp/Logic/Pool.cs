using Steamworks;
using StgSharp.Math;
using StgSharp.Entities;
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
        internal LinkedList<IPlainGeometry> geometryContainer;
        internal LinkedList<IEntity> _bulletContainer;
        internal LinkedList<IEntity> _awardContainer;
        internal LinkedList<IEntity> _enemyContainer;

        internal Line leftBoader;
        internal Line rightBoader;
        internal Line topBoader;
        internal Line bottomBoader;

        public Line LeftBoarder => leftBoader;
        public Line RightBoarder => rightBoader;
        public Line TopBoarder => topBoader;
        public Line BottomBoarder => bottomBoader;

        internal Sensor<EntityPartical> Sensor;



        public Pool(Line left, Line right, Line top, Line bottom)
        {
            topBoader = top;
            bottomBoader= bottom;
            rightBoader = right;
            leftBoader = left;
        }
        public Pool( Geometries.Rectangle area)
        {
            vec3d vec = area.vertex01._position - area.vertex02._position;
            if ((vec.X==0||vec.Y==0)&&vec.Z==0)
            {
                Line[] barders = area.GetAllSides();
                if (area.vertex01.X>0)
                {
                    if (area.vertex01.Y>0)
                    {
                        topBoader = barders[3];
                        rightBoader = barders[0];
                        bottomBoader = barders[1];
                        leftBoader = barders[2];
                    }
                    else
                    {
                        topBoader = barders[2];
                        rightBoader = barders[3];
                        bottomBoader = barders[0];
                        leftBoader = barders[1];
                    }
                }
                else
                {
                    if (area.vertex01.Y > 0)
                    {
                        topBoader = barders[3];
                        rightBoader = barders[0];
                        bottomBoader = barders[1];
                        leftBoader = barders[2];
                    }
                    else
                    {
                        topBoader = barders[2];
                        rightBoader = barders[3];
                        bottomBoader = barders[0];
                        leftBoader = barders[1];
                    }
                }
            }
        }

        public void OnRender()
        {
            foreach (Enemy e in _enemyContainer)
            {
                
            }
        }

        
    }

}
