using System;
using System.Collections.Generic;
using System.Text;

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
            this._id.Remove();
        }

        internal override void OnRenderFrame()
        {
            CommonBulletLauncher.Launch();
        }
    }
}
