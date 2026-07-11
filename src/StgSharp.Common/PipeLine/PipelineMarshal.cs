// -----------------------------------------------------------------------------
// file="PipelineMarshal"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.PipeLine
{
    public static class PipelineMarshal
    {

        public static IEnumerable<PipelineNode> GetNodeSortedEnumeration(
                                                PipelineScheduler scheduler
        )
        {
            ArgumentNullException.ThrowIfNull(scheduler);
            return new PipelineNodeEnumerator(scheduler)
            {
                _count = scheduler._allNode.Count,
            };
        }

        private class PipelineNodeEnumerator : ICollection<PipelineNode>
        {

            private readonly List<List<PipelineNode>> _mt, _c;

            public int _count;

            public PipelineNodeEnumerator(
                   PipelineScheduler scheduler
            )
            {
                if (!scheduler.TryGetAllNode(out _mt, out _c)) {
                    throw new InvalidOperationException("Binded scheduler is not sorted.");
                }
            }

            public bool IsReadOnly => true;

            public int Count => _count;

            public IEnumerator<PipelineNode> GetEnumerator()
            {
                for (int i = 1; i < _mt.Count; i++)
                {
                    foreach (PipelineNode node in _mt[i]) {
                        yield return node;
                    }
                    foreach (PipelineNode node in _c[i]) {
                        yield return node;
                    }
                }
            }

            void ICollection<PipelineNode>.Add(
                                           PipelineNode item
            )
            {
                throw new NotSupportedException();
            }

            void ICollection<PipelineNode>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<PipelineNode>.Contains(
                                           PipelineNode item
            )
            {
                throw new NotSupportedException();
            }

            void ICollection<PipelineNode>.CopyTo(
                                           PipelineNode[] array,
                                           int arrayIndex
            )
            {
                throw new NotSupportedException();
            }

            bool ICollection<PipelineNode>.Remove(
                                           PipelineNode item
            )
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

    }
}
