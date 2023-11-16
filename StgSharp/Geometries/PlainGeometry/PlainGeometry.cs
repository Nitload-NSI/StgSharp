using StgSharp.Math;

namespace StgSharp.Geometries
{

    /// <summary>
    /// 平面几何体。包含三角形，对称四边形，自由多边形等。最多可由16个点参数定义。
    /// </summary>
    public abstract class PlainGeometry
    {
        protected readonly uint bornTick;
        internal readonly Point refOrigin;

        public Point RefOrigin { get { return refOrigin; } }

        public abstract Point RefPoint0 { get; }
        public abstract Point RefPoint1 { get; }
        public abstract Point RefPoint2 { get; }

        public uint BornTime => bornTick;

        internal abstract int[] Indices { get; }

        public PlainGeometry(uint bornTime)
        {
            bornTick = bornTime;
        }

        public PlainGeometry(Counter<uint> MainTimer)
        {
            bornTick = MainTimer.Value;
        }

        public PlainGeometry() { }


        internal abstract void OnRender(uint tick);

        public abstract Line[] GetAllSides();

        public abstract Plain GetPlain();

    }


}
