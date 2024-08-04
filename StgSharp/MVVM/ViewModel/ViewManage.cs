//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewManage.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the “Software”), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED “AS IS”, 
//     WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//     INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//     DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//     ARISING FROM, OUT OF OR IN CONNECTION WITH 
//     THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//     
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;
using StgSharp.MVVM.View;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace StgSharp.MVVM.ViewModel
{
    public partial class ViewModelBase
    {
        private Dictionary<string, ViewBase> allView;

        public T CreateAndBindView<T>(string name, (int x, int y, int z) unitCubeSize)
            where T : ViewBase, new()
        {
            T ret = new T();
            ret.SelfName = name;
            ret.InternalInitialize(name,unitCubeSize,this);
            allView.Add(name, ret);
            return ret;
        }

    }
}
