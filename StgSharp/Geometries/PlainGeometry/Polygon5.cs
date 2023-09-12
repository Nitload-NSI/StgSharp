using StgSharp.Controlling;
using StgSharp.Math;

namespace StgSharp.Geometries
{
    public unsafe class Polygon5 : IPlainGeometry
    {

        internal Point _refOrigin;
        internal readonly Point vertex1;
        internal readonly Point vertex2;
        internal readonly Point vertex3;
        internal readonly Point vertex4;
        internal readonly Point vertex5;
        internal readonly int _vertexCount;

        public override Point RefPoint01 => vertex1;

        public override Point RefPoint02 => vertex2;

        public override Point RefPoint03 => vertex3;

        internal GetLocationHandler movP01Operation = default;
        internal GetLocationHandler movP02Operation = default;
        internal GetLocationHandler movP03Operation = default;
        internal GetLocationHandler movP04Operation = default;
        internal GetLocationHandler movP05Operation = default;

        public GetLocationHandler MovP01Operation { get => movP01Operation; }
        public GetLocationHandler MovP02Operation { get => movP02Operation; }
        public GetLocationHandler MovP03Operation { get => movP03Operation; }
        public GetLocationHandler MovP04Operation { get => movP04Operation; }
        public GetLocationHandler MovP05Operation { get => movP05Operation; }

        public unsafe Polygon5()
        {
        }

        public virtual Vec3d movPoint01(uint tick) => movP01Operation.Invoke(TimeLine.tickCounter._value);

        public virtual Vec3d movPoint02(uint tick) => movP02Operation.Invoke(TimeLine.tickCounter._value);

        public virtual Vec3d movPoint03(uint tick) => movP03Operation.Invoke(TimeLine.tickCounter._value);

        public virtual Vec3d movPoint04(uint tick) => movP04Operation.Invoke(TimeLine.tickCounter._value);

        public virtual Vec3d movPoint05(uint tick) => movP05Operation.Invoke(TimeLine.tickCounter._value);


        internal override void OnRender(uint tick)
        {
            uint nowTick = tick - BornTime;
            vertex1.Position = RefOrigin.Position + movPoint01(nowTick);
            vertex2.Position = RefOrigin.Position + movPoint02(nowTick);
            vertex3.Position = RefOrigin.Position + movPoint03(nowTick);
            vertex4.Position = RefOrigin.Position + movPoint04(nowTick);
            vertex5.Position = RefOrigin.Position + movPoint05(nowTick);
        }

        public override Line[] GetAllSides()
        {
            return new Line[5]
                {
                    new Line(vertex1,vertex2),
                    new Line(vertex2,vertex3),
                    new Line(vertex3,vertex4),
                    new Line(vertex4,vertex5),
                    new Line(vertex5,vertex1)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(vertex1, vertex2, vertex3);
        }
    }
}
