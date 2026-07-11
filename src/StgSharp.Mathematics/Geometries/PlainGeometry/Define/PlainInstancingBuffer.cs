// -----------------------------------------------------------------------------
// file="PlainInstancingBuffer"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.Mathematics;
using StgSharp.Mathematics.Graphics;
using StgSharp.Mathematics.Numeric.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StgSharp.Geometries
{
    public class PlainInstancingBuffer<T> : IDisposable, IEnumerable<T>, IInstancingBuffer
        where T : IInstancing
    {

        private bool disposedValue;

        private CoordinatePlain globalCoordinate;
        private List<Vec4> coordList;
        private List<IInstancing> instanceList;
        private List<float> scalingList;
        private Mutex mutex;

        public PlainInstancingBuffer()
        {
            coordList = new List<Vec4>(capacity:100);
            instanceList = new List<IInstancing>(capacity:100);
            scalingList = new List<float>(capacity:100);
            coordArray = Array.Empty<Vec4>();
            scaleArray = Array.Empty<float>();
            mutex = new Mutex();
        }

        public List<IInstancing> InstanceList => instanceList;

        public List<Vec4> CoordAndRotationList => coordList;

        public List<float> ScalingList => scalingList;

        public Span<Vec4> CoordAndRotationSpan => CollectionsMarshal.AsSpan(coordList);

        public Span<float> ScalingSpan => CollectionsMarshal.AsSpan(scalingList);

        public int CreateInstanceID()
        {
            mutex.WaitOne();
            int ret = instanceList.Count;
            coordList.Add(new Vec4());
            scalingList.Add(0);
            mutex.ReleaseMutex();
            return ret;
        }

        // ~PlainInstancingBuffer()
        // {
        // Dispose(disposing: false);
        // }

        public void Dispose()
        {
            Dispose(disposing:true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (IInstancing item in instanceList) {
                yield return (T)item;
            }
        }

        public void Remove(
                    T instance
        )
        {
            mutex.WaitOne();
            if (instance.GlobalBuffer != this) {
                throw new ArgumentException(
                    message:"instance does not belong to current Particle Buffer",
                    paramName:nameof(instance));
            }
            int index = instance.BufferId;
            IInstancing temp = instanceList[^1];
            instanceList[instance.BufferId] = temp;
            temp.BufferId = instance.BufferId;

            coordList[instance.BufferId] = coordList[^1];
            scalingList[instance.BufferId] = scalingList[^1];

            instanceList.RemoveAt(instanceList.Count - 1);
            coordList.RemoveAt(coordList.Count - 1);
            scalingList.RemoveAt(scalingList.Count - 1);
            mutex.ReleaseMutex();
        }

        public void UpdateAllParticles()
        {
            foreach (IInstancing particle in instanceList) {
                particle.Move();
            }
        }

        protected virtual void Dispose(
                               bool disposing
        )
        {
            if (!disposedValue)
            {
                if (disposing) {
                    mutex.Dispose();
                }

                coordList.Clear();
                instanceList.Clear();
                scalingList.Clear();
                disposedValue = true;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (IInstancing item in instanceList) {
                yield return (T)item;
            }
        }

        IGeometry IInstancingBuffer.TypicalShape => throw new NotImplementedException();

        #region field of mem opt

        private Vec4[] coordArray;
        private float[] scaleArray;

    #endregion
    }
}
