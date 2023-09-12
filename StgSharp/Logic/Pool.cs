using StgSharp.Entities;
using StgSharp.Geometries;
using StgSharp.Math;

namespace StgSharp
{
    public class Pool
    {
        internal LinkedList<Vector4Map> _Vector4mapContainer;
        internal LinkedList<IPlainGeometry> _geometryContainer;
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

        public Pool()
        {
            initContainers();
        }

        private void initContainers()
        {
            _Vector4mapContainer = new LinkedList<Vector4Map>();
            _bulletContainer = new LinkedList<IEntity>();
            _awardContainer = new LinkedList<IEntity>();
            _enemyContainer = new LinkedList<IEntity>();
            _geometryContainer = new LinkedList<IPlainGeometry>();
        }

        public void OnRender()
        {
        }


        

    }

}
