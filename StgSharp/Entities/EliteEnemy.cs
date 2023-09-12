using StgSharp.Controlling;
using System;

namespace StgSharp.Entities
{
    public class EliteEnemy : IEntity
    {
        public bool _selectalbe;

        internal Launcher<EntityPartical> _awardLauncher;
        internal LinkedList<Launcher<EntityPartical>> _commonBulletLauncher;
        internal Launcher<EntityPartical> _dieBulletLauncher;

        public Launcher<EntityPartical> AwardLauncher { get; set; }
        public LinkedList<Launcher<EntityPartical>> CommonBulletLauncher { get; set; }
        public Launcher<EntityPartical> DieBulletLauncher { get; set; }

        internal void InternalDieOperation(Pool home)
        {
            _dieBulletLauncher.Launch();
            _awardLauncher.Launch();
            TimeLine._currentPool._enemyContainer.Remove(this);
        }

        public override void OnRenderFrame()
        {
            this.OnUpdate();
            foreach (Launcher<EntityPartical> launcher in CommonBulletLauncher)
            {
                launcher.Launch();
            }
            this.OnRender();
        }

        public virtual void OnRender() => new NotImplementedException();

        public virtual void OnUpdate() => new NotImplementedException();
    }
}
