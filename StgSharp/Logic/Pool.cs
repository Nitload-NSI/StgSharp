using StgSharp.Entities;
using StgSharp.Geometries;

namespace StgSharp
{
    public class Pool
    {
        internal LinkedList<Point> _pointContainer;
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

        internal Pool()
        {
            initContainers();
        }

        private void initContainers()
        {
            _pointContainer = new LinkedList<Point>();
            _bulletContainer = new LinkedList<IEntity>();
            _awardContainer = new LinkedList<IEntity>();
            _enemyContainer = new LinkedList<IEntity>();
            geometryContainer = new LinkedList<IPlainGeometry>();
        }

        public void OnRender()
        {
            foreach (Enemy e in _enemyContainer)
            {

            }
        }


    }

}
