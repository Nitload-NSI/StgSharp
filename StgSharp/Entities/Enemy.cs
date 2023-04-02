using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Entities
{
    public class Enemy : Entity
    {
        public bool _selectalbe;

        public Launcher<EntityPartical> _awardLauncher;
        public Launcher<EntityPartical> _commonBulletLauncher;
        public Launcher<EntityPartical> _dieBulletLauncher;

        


        internal void InternalDieOperation(Pool home)
        {
            _dieBulletLauncher.Launch(Control.GameTimeLine.tickCounter, home._bulletContainer);
            _awardLauncher.Launch(Control.GameTimeLine.tickCounter, home._awardContainer);
            this._id.Remove();
        }

        

    }
}
