using StgSharp.Controlling;
using StgSharp.Math;
using StgSharp.Math.Linear;
using System.Numerics;

namespace StgSharp.Geometries
{
    public class Polygon6 : PlainGeometry
    {
        internal Point _refOrigin;
        internal GetLocationHandler movVertex0Operation;
        internal GetLocationHandler movVertex1Operation;
        internal GetLocationHandler movVertex2Operation;
        internal GetLocationHandler movVertex3Operation;
        internal GetLocationHandler movVertex4Operation;
        internal GetLocationHandler movVertex5Operation;
        internal mat6 vertexMat;


        public GetLocationHandler MovVertex0Operation { get => movVertex0Operation; }
        public GetLocationHandler MovVertex1Operation { get => movVertex1Operation; }
        public GetLocationHandler MovVertex2Operation { get => movVertex2Operation; }
        public GetLocationHandler MovVertex3Operation { get => movVertex3Operation; }
        public GetLocationHandler MovVertex4Operation { get => movVertex4Operation; }
        public GetLocationHandler MovVertex5Operation { get => movVertex5Operation; }

        public override Point RefPoint0 => Vertex0;
        public override Point RefPoint1 => Vertex1;
        public override Point RefPoint2 => Vertex2;

        internal override int[] Indices
        {
            get
            {
                return new int[12] {
                    0,1,2,
                    0,2,3,
                    0,3,4,
                    0,4,5};
            }
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

        public Point Vertex5
        {
            get => new Point(vertexMat.vec5);
            set { vertexMat.vec5 = value.position.vec; }
        }

        public override Line[] GetAllSides()
        {
            return new Line[6]
                {
                    new Line(Vertex0,Vertex1),
                    new Line(Vertex1,Vertex2),
                    new Line(Vertex2,Vertex3),
                    new Line(Vertex3,Vertex4),
                    new Line(Vertex4,Vertex5),
                    new Line(Vertex5,Vertex0)
                };
        }

        public override Plain GetPlain()
        {
            return new Plain(Vertex0, Vertex1, Vertex2);
        }

        public virtual vec3d movVertex0(uint tick)
        {
            return movVertex0Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movVertex1(uint tick)
        {
            return movVertex1Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movVertex2(uint tick)
        {
            return movVertex2Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movVertex3(uint tick)
        {
            return movVertex3Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movVertex4(uint tick)
        {
            return movVertex4Operation.Invoke(TimeLine.tickCounter._value);
        }

        public virtual vec3d movVertex5(uint tick)
        {
            return movVertex5Operation.Invoke(TimeLine.tickCounter._value);
        }

        internal override void OnRender(uint tick)
        {
            uint nowTick = tick - BornTime;
            Vector4 origin = RefOrigin.Position.vec;
            vertexMat.vec0 = origin + movVertex0(nowTick).vec;
            vertexMat.vec1 = origin + movVertex1(nowTick).vec;
            vertexMat.vec2 = origin + movVertex2(nowTick).vec;
            vertexMat.vec3 = origin + movVertex3(nowTick).vec;
            vertexMat.vec4 = origin + movVertex4(nowTick).vec;
            vertexMat.vec5 = origin + movVertex5(nowTick).vec;
        }
    }
}
