//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Launcher.cs"
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
using StgSharp.Entities;
using StgSharp.Math;

namespace StgSharp
{
    public class Launcher<T> where T : IEntity
    {

        internal EntityPartical _bulletSample;        //发射的子弹类型
        /*
         *用于发射其他的实体，主要是子弹 和 掉落物
         */

        internal Vec2d _center;     //发射器的参考位置，运动轨迹的方程应当基于这个参考点构建
        internal Counter<int> _internalParameter;       //发射器的内部计时器
        internal uint[] launchtick;     //发射子弹的内部计时

        public EntityPartical Bullet
        {
            get => _bulletSample;
            set => _bulletSample = value;
        }

        internal virtual void Launch()
        {

        }



    }
}
