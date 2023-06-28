using StgSharp.Control;
using StgSharp.Math;

namespace StgSharp.Geometries
{
    public class Polygon6 : IPlainGeometry
    {
        internal Point _refOrigin;
        internal Point vertex1;
        internal Point vertex2;
        internal Point vertex3;
        internal Point vertex4;
        internal Point vertex5;
        internal Point vertex6;
        internal readonly int _vertexCount;

        public override Point RefPoint01 => vertex1;

        public override Point RefPoint02 => vertex2;

        public override Point RefPoint03 => vertex3;


        internal GetLocationHandler movP01Operation = default;
        internal GetLocationHandler movP02Operation = default;
        internal GetLocationHandler movP03Operation = default;
        internal GetLocationHandler movP04Operation = default;
        internal GetLocationHandler movP05Operation = default;
        internal GetLocationHandler movP06Operation = default;

        public GetLocationHandler MovP01Operation { get => movP01Operation; }
        public GetLocationHandler MovP02Operation { get => movP02Operation; }
        public GetLocationHandler MovP03Operation { get => movP03Operation; }
        public GetLocationHandler MovP04Operation { get => movP04Operation; }
        public GetLocationHandler MovP05Operation { get => movP05Operation; }
        public GetLocationHandler MovP06Operation { get => movP06Operation; }

        public virtual vec3d movPoint01(uint tick)
        {
            return movP01Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movPoint02(uint tick)
        {
            return movP02Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movPoint03(uint tick)
        {
            return movP03Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movPoint04(uint tick)
        {
            return movP04Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movPoint05(uint tick)
        {
            return movP05Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movPoint06(uint tick)
        {
            return movP06Operation.Invoke(TimeLine.tickCounter._value);
        }

        internal override void OnRender(uint tick)
        {
            uint nowTick = tick - BornTime;
            vertex1.Position = RefOrigin.Position + movPoint01(nowTick);
            vertex2.Position = RefOrigin.Position + movPoint02(nowTick);
            vertex3.Position = RefOrigin.Position + movPoint03(nowTick);
            vertex4.Position = RefOrigin.Position + movPoint04(nowTick);
            vertex5.Position = RefOrigin.Position + movPoint05(nowTick);
            vertex6.Position = RefOrigin.Position + movPoint06(nowTick);
        }

        public override Line[] GetAllSides()
        {
            return new Line[6]
                {
                    new Line(vertex1,vertex2),
                    new Line(vertex2,vertex3),
                    new Line(vertex3,vertex4),
                    new Line(vertex4,vertex5),
                    new Line(vertex5,vertex6),
                    new Line(vertex6,vertex1)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(vertex1, vertex2, vertex3);
        }
    }
}
