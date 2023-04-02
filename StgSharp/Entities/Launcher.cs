using StgSharp.Entities;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp
{
    public class Launcher<T>:INode<Launcher<T>> where T : Entity
    {
        /*
         *用于发射其他的实体，主要是子弹 和 掉落物
         */

        internal vec2d _center;     //发射器的参考位置，运动轨迹的方程应当基于这个参考点构建
        private ContainerNode<Launcher<T>> _id;
        internal Counter<int> _internalParameter;       //发射器的内部计时器
        internal EntityPartical _bulletSample;        //发射的子弹类型
        internal uint[] launchtick;     //发射子弹的内部计时

        public EntityPartical Bullet
        {
            get { return _bulletSample; }
            set { _bulletSample = value; }
        }

        internal void Launch(Counter<uint> outerTickCounter, LinkedList<Entity> container)
        {
            
        }


        ContainerNode<Launcher<T>> INode<Launcher<T>>.ID
        {
            get { return _id; }
        }

        void INode<Launcher<T>>.setID(ContainerNode<Launcher<T>> id)
        {
            if (_id == default(ContainerNode<Launcher<T>>))
            {
                throw new ArgumentException();
            }
            else
            {
                _id = id;
            }
        }
    }
}
