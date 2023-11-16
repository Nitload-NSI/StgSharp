using StgSharp.Controlling;
using StgSharp.Math;
using StgSharp.Math.Linear;
using System.Numerics;

namespace StgSharp.Geometries
{
    public unsafe class Polygon5 : PlainGeometry
    {
        internal Point _refOrigin;
        internal GetLocationHandler movP01Operation = default;
        internal GetLocationHandler movP02Operation = default;
        internal GetLocationHandler movP03Operation = default;
        internal GetLocationHandler movP04Operation = default;
        internal GetLocationHandler movP05Operation = default;
        internal mat5 vertexMat;

        public unsafe Polygon5()
        {
        }

        public Polygon5(
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z,
            float v3x, float v3y, float v3z,
            float v4x, float v4y, float v4z
            )
        {
            vertexMat.vec0 = new Vector4(v0x, v0y, v0z, 0);
            vertexMat.vec1 = new Vector4(v1x, v1y, v1z, 0);
            vertexMat.vec2 = new Vector4(v2x, v2y, v2z, 0);
            vertexMat.vec3 = new Vector4(v3x, v3y, v3z, 0);
            vertexMat.vec4 = new Vector4(v4x, v4y, v4z, 0);
        }


        public GetLocationHandler MovP01Operation { get => movP01Operation; }
        public GetLocationHandler MovP02Operation { get => movP02Operation; }
        public GetLocationHandler MovP03Operation { get => movP03Operation; }
        public GetLocationHandler MovP04Operation { get => movP04Operation; }
        public GetLocationHandler MovP05Operation { get => movP05Operation; }

        public override Point RefPoint0 => Vertex0;
        public override Point RefPoint1 => Vertex1;
        public override Point RefPoint2 => Vertex2;

        public mat5 VertexBuffer
        {
            get { return vertexMat; }
        }

        public Point Vertex0
        {
            get => new Point(vertexMat.vec0);
            set { vertexMat.vec0 = value.position.vec; }
        }

        public Point Vertex1
        {
            get => new Point(vertexMat.vec1);
            set { vertexMat.vec1 = value.position.vec; }
        }

        public Point Vertex2
        {
            get => new Point(vertexMat.vec2);
            set { vertexMat.vec2 = value.position.vec; }
        }

        public Point Vertex3
        {
            get => new Point(vertexMat.vec3);
            set { vertexMat.vec3 = value.position.vec; }
        }

        public Point Vertex4
        {
            get => new Point(vertexMat.vec4);
            set { vertexMat.vec4 = value.position.vec; }
        }

        internal override int[] Indices
        {
            get
            {
                return new int[9]{
                    0,1,2,
                    0,2,3,
                    0,3,4};
            }
        }
        public override Line[] GetAllSides()
        {
            return new Line[5]
                {
                    new Line(Vertex0,Vertex1),
                    new Line(Vertex1,Vertex2),
                    new Line(Vertex2,Vertex3),
                    new Line(Vertex3,Vertex4),
                    new Line(Vertex4,Vertex0)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(Vertex0, Vertex1, Vertex2);
        }

        public virtual vec3d movVertex01(uint tick) => movP01Operation.Invoke(TimeLine.tickCounter._value);

        public virtual vec3d movVertex02(uint tick) => movP02Operation.Invoke(TimeLine.tickCounter._value);

        public virtual vec3d movVertex03(uint tick) => movP03Operation.Invoke(TimeLine.tickCounter._value);

        public virtual vec3d movVertex04(uint tick) => movP04Operation.Invoke(TimeLine.tickCounter._value);

        public virtual vec3d movVertex05(uint tick) => movP05Operation.Invoke(TimeLine.tickCounter._value);


        internal override void OnRender(uint tick)
        {
            uint nowTick = tick - BornTime;
            vertexMat.vec0 = (RefOrigin.Position + movVertex01(nowTick)).vec;
            vertexMat.vec1 = (RefOrigin.Position + movVertex02(nowTick)).vec;
            vertexMat.vec2 = (RefOrigin.Position + movVertex03(nowTick)).vec;
            vertexMat.vec3 = (RefOrigin.Position + movVertex04(nowTick)).vec;
            vertexMat.vec4 = (RefOrigin.Position + movVertex05(nowTick)).vec;
        }
    }
}
