using StgSharp;
using StgSharp.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;


namespace StgSharp.Geometries
{
    public class Point : INode<Point>
    {

        internal vec3d _position;

        public float X
        {
            get { return this._position.X; }
        }

        public float Y
        {
            get { return this._position.Y; }
        }

        internal readonly bool isVertex;

        public bool IsVertex => isVertex;

        /// <summary>
        /// 刷新点的位置，即移动点。当该点是某个几何体的从动点时
        /// （即点坐标由几何体其他点约束或点参数已经在创建几何体时定义）
        /// 请不要再次进行定义，程序将自行忽略此处的代码。
        /// </summary>
        /// <param name="tick">当前游戏刻</param>
        public virtual void Update(uint tick) {}

        public bool IsInGeometry(IPlainGeometry geo)
        {
            throw new NotImplementedException();
        }

        public bool IsOnPlain(IPlainGeometry geo)
        {
            return this._position * geo.GetPlain().plainParameter == 1;
        }

    }
}
