//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="PlainInstancingBuffer.cs"
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
using StgSharp.Math;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public class PlainInstancingBuffer<T> : IDisposable,IEnumerable<T> where T : IInstancing<T>
    {

        private CoordinatePlain globalCorrdinnate;
        private List<vec4d> coordList;
        private List<IInstancing<T>> instanceList;
        private List<float> scalingList;
        private Mutex mutex;
        private bool disposedValue;

        #region field of mem opt

        vec4d[] coordArray;
        float[] scaleArray;

        #endregion

        public PlainInstancingBuffer()
        {
            coordList = new List<vec4d>(capacity: 100);
            instanceList = new List<IInstancing<T>>(capacity:100);
            scalingList = new List<float>(capacity: 100);
            coordArray = new vec4d[0];
            scaleArray = new float[0];
            mutex = new Mutex();
        }

        internal List<IInstancing<T>> InstanceList
        {
            get => instanceList;
        }

        internal List<vec4d> CoordList
        {
            get => coordList;
        }

        internal List<float> ScalingList
        {
            get => scalingList;
        }

        public Span<vec4d> CoordSpan
        {
            get
            {
                if (coordArray.Length != coordList.Capacity)
                {
                    FieldInfo fieldInfo = typeof(List<vec4d>).
                        GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo == null)
                    {
                        throw new NullReferenceException();
                    }
                    coordArray = (vec4d[])fieldInfo.GetValue(coordList);
                }
                return new Span<vec4d>(coordArray, 0, coordList.Count);
            }
        }

        public Span<float> ScalingSpan
        {
            get
            {
                if (scaleArray.Length != coordList.Capacity)
                {
                    FieldInfo fieldInfo = typeof(List<float>).
                        GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo == null)
                    {
                        throw new NullReferenceException();
                    }
                    scaleArray = (float[])fieldInfo.GetValue(scalingList);
                }
                return new Span<float>(scaleArray, 0, coordList.Count);
            }
        }

        public int CreateInstanceID()
        {
            mutex.WaitOne();
            int ret = instanceList.Count;
            coordList.Add(new vec4d());
            scalingList.Add(0);
            mutex.ReleaseMutex();
            return ret;
        }

        public void Remove(T instance)
        {
            mutex.WaitOne();
            if (instance.GlobalBuffer != this)
            {
                throw new ArgumentException(
                    message:"instance does not belong to current Particle Buffer",
                    paramName: nameof(instance));
            }
            int index = instance.BufferId;
            IInstancing<T> temp = instanceList[instanceList.Count - 1];
            instanceList[instance.BufferId] = temp;
            temp.BufferId = instance.BufferId;

            coordList[instance.BufferId] = coordList[coordList.Count - 1];
            scalingList[instance.BufferId] = scalingList[scalingList.Count - 1];

            instanceList.RemoveAt(instanceList.Count - 1);
            coordList.RemoveAt(coordList.Count - 1);
            scalingList.RemoveAt(scalingList.Count - 1);
            mutex.ReleaseMutex();
        }

        public void UpdateAllParticles()
        {
            foreach (var particle in instanceList)
            {
                particle.Move();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mutex.Dispose();
                }

                coordList.Clear();
                instanceList.Clear();
                scalingList.Clear();
                disposedValue = true;
            }
        }

        // ~PlainInstancingBuffer()
        // {
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in instanceList)
            {
                yield return (T)item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in instanceList)
            {
                yield return (T)item;
            }
        }
    }
}
