using StgSharp.Graphics.OpenGL;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Commom.Collections
{
    public interface IPoolable
    {
        public int ID { get; }
    }

    public class CacheIndexPool<T> : IEnumerable<T> where T : IPoolable
    {
        private ConcurrentDictionary<int, T> _cache;
        private Func<T> _defaultCreater;

        public CacheIndexPool(Func<T> defaultCreater)
        {
            _cache = new ConcurrentDictionary<int, T>();
            _defaultCreater = defaultCreater;
        }

        public T this[int index]
        {
            get
            {
                if (_cache.TryGetValue(index, out T value))
                {
                    return value;
                }
                T newValue = _defaultCreater();
                _cache.TryAdd(index, newValue);
                return newValue;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T CreateObject()
        {
            T _new = _defaultCreater();
            _cache.AddOrUpdate(1, _new,static (_,origin)=>origin);
            throw new NotImplementedException();
            return _new;
        }
    }
}
