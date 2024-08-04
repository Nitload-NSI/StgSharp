using StgSharp.Graphics;
using StgSharp.MVVM;
using StgSharp.MVVM.ViewModel;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Touhou
{
    public abstract class TouhouWindowMain : ViewModelBase
    {
        protected TouhouWindowMain(ViewPort vp):base(vp,60)
        {
        }



    }
}
