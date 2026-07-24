// -----------------------------------------------------------------------------
// file="SequenceEmitter"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.RegularAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.RegularAnalysis.Abstraction
{
    public static class SequenceEmitterExtensions
    {

        public static SequenceEmitter<string> AppendLine(
                                              this SequenceEmitter<string> emitter
        )
        {
            return emitter is null ? null! : emitter.Append(string.Empty);
        }

        public static SequenceEmitter<string> AppendLine(
                                              this SequenceEmitter<string> emitter,
                                              string line
        )
        {
            return emitter is null ?
                   null! :
                   emitter.Append(string.IsNullOrEmpty(line) ? string.Empty : line);
        }

        public static SequenceEmitter<string> AppendLine(
                                              this SequenceEmitter<string> emitter,
                                              string line,
                                              bool condition
        )
        {
            return emitter is null ? null! : condition ? emitter.Append(line) : emitter;
        }

    }

    public sealed class SequenceEmitter<T>
    {

        private bool _isRoot;

        private readonly Node _head;
        private Node _tail;

        private SequenceEmitter(
                Node node
        )
        {
            _isRoot = true;
            _head = node;
            _tail = _head;
        }

        public SequenceEmitter()
        {
            _isRoot = true;
            _head = new();
            _tail = _head;
        }

        public int Count { get; private set; }

        public SequenceEmitter<T> Append(
                                  T unit
        )
        {
            if (!_isRoot) {
                throw new InvalidOperationException("This emitter has been appended to another emitter. It is now readonly.");
            }
            _tail.Add(unit);
            Count++;
            return this;
        }

        public SequenceEmitter<T> Append(
                                  SequenceEmitter<T> emitter
        )
        {
            if (emitter == null || emitter.Count == 0) {
                return this;
            }
            if (ReferenceEquals(this, emitter)) {
                throw new InvalidOperationException("Cannot append emitter to itself.");
            }

            if (!emitter._isRoot) {
                throw new InvalidOperationException("The emitter has been appended to another emitter.");
            }
            _tail.Next = emitter._head;
            _tail = emitter._tail;
            Count += emitter.Count;
            emitter._isRoot = false;
            return this;
        }

        public SequenceEmitter<T> Clone()
        {
            if (!_isRoot) {
                throw new InvalidOperationException("This emitter has been appended to another emitter. It is now readonly.");
            }
            SequenceEmitter<T> clone = new();
            Node current = _head;
            return clone;
        }

        public Enumerator GetEnumerator()
        {
            return _isRoot ?
                   new Enumerator(this) :
                   throw new InvalidOperationException("This emitter has been appended to another emitter. You should not get its enumerator.");
        }

        public List<T> ToList()
        {
            List<T> result = new(Count);
            Node current = _head;
            while (current != null)
            {
                result.AddRange(current.IRList);
                current = current.Next!;
            }
            return result;
        }

#pragma warning disable CA1063
#pragma warning disable CA1816 
        public class Enumerator(
                     [NotNull] SequenceEmitter<T> emitter
        ) : IEnumerator<T>
        {

            private int _index = -1;

            private Node _current = null!;
            private readonly Node _root = emitter._head;

            public T Current
            {
                get
                {
                    if (_current is null || _index < 0 || _index >= _current.Count) {
                        throw new InvalidOperationException();
                    }

                    return _current[_index];
                }
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                _current ??= _root;
                int index = _index + 1;
                if (index < _current.Count)
                {
                    _index = index;
                    return true;
                }
                Node current = _current.Next!;
                if (current is null || ReferenceEquals(current, _root))
                {
                    _index = -1;
                    return false;
                }
                _current = current;
                _index = -1;
                return true;
            }

            public void Reset()
            {
                _current = _root;
                _index = -1;
            }

            object IEnumerator.Current => Current!;

        }

#pragma warning restore CA1063
#pragma warning restore CA1816

        private sealed class Node
        {

            public Node()
            {
                IRList = [];
            }

            public Node(
                   int capacity
            )
            {
                IRList = new(capacity);
            }

            public T this[
                     int index
            ]
            {
                get => IRList[index];
                set => IRList[index] = value;
            }

            public Node? Next { get; set; }

            public int Count => IRList.Count;

            public List<T> IRList { get; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(
                        T ir
            )
            {
                IRList.Add(ir);
            }

            public void AddRange(
                        IEnumerable<T> irs
            )
            {
                IRList.AddRange(irs);
            }

        }

    }
}
