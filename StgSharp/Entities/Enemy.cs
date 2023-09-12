using StgSharp.Controlling;

namespace StgSharp.Entities
{
    public class Enemy : IEntity
    {
        public bool _selectalbe;

        internal Launcher<EntityPartical> _awardLauncher;
        internal Launcher<EntityPartical> _commonBulletLauncher;
        internal Launcher<EntityPartical> _dieBulletLauncher;

        public Launcher<EntityPartical> AwardLauncher { get; set; }
        public Launcher<EntityPartical> CommonBulletLauncher { get; set; }
        public Launcher<EntityPartical> DieBulletLauncher { get; set; }

        internal void InternalDieOperation(Pool home)
        {
            _dieBulletLauncher.Launch();
            _awardLauncher.Launch();
            TimeLine._currentPool._enemyContainer.Remove(this);
        }

        public override void OnRenderFrame()
        {
            CommonBulletLauncher.Launch();
        }
    }
}
