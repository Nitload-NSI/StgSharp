//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ArrayList.cs"
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Logic
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    internal struct LogSeperator
    {

        [FieldOffset(0)] public ulong all;
        [FieldOffset(0)] public long all_s;
        [FieldOffset(4)] public uint high;
        [FieldOffset(4)] public int high_s;

        [FieldOffset(0)] public uint low;

        [FieldOffset(0)] public int low_s;

        public LogSeperator(uint high, uint low)
        {
            this.high = high;
            this.low = low;
        }

    }

    /// <summary>
    /// A linear data structure with dynamic length.
    /// It contains a <see cref="List{T}"/> of <see cref="Array"/>,
    /// each <see cref="Array"/> is twice longer than the former one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicArrayList<T>
    {

        private List<T[]> arrayIndexList;
        private LogSeperator indexpair;
        private int lastMain = 0;

        // main index to the last used element
        private int lastSub = 0;

        private int length;
        private int subLength = 1;

        // sub index to the last used element
        private Queue<int> unusedMainIndex;

        private Queue<int> unusedSubIndex;

        public DynamicArrayList()
        {
            arrayIndexList = [new T[1]];
            length = 0;
            unusedMainIndex = new Queue<int>();
            unusedSubIndex = new Queue<int>();
            unusedMainIndex.Enqueue(0);
            unusedSubIndex.Enqueue(0);
        }

        /// <summary>
        /// Init a <see cref="DynamicArrayList{T}"/> with a certain amount of elements prepared.
        /// </summary>
        /// <param name="count">Amount of elements to be prepared.</param>
        public DynamicArrayList(int count)
        {
            arrayIndexList = new List<T[]>();
            length = count;
            unusedMainIndex = new();
            unusedSubIndex = new();

            SeprateIndex(count);
            int subcount = 1;
            for (int i = 0; i < indexpair.high_s; i++)
            {
                arrayIndexList.Add(new T[count]);
                for (int j = 0; j < subcount; j++)
                {
                    unusedMainIndex.Enqueue(i);
                    unusedSubIndex.Enqueue(j);
                }
                count >>= 1;
            }
            if (indexpair.low_s != 0)
            {
                arrayIndexList.Add(new T[1 >> indexpair.high_s]);
                for (int j = 0; j < indexpair.high_s; j++)
                {
                    unusedMainIndex.Enqueue(indexpair.high_s);
                    unusedSubIndex.Enqueue(j);
                }
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="DynamicArrayList{T}"/>,
        /// and fill the begining ones of elemtns as given instance.
        /// </summary>
        /// <param name="count">Amount of beginning elements to be filled</param>
        /// <param name="instace"></param>
        public DynamicArrayList(int count, T instace)
        {
            arrayIndexList = new List<T[]>();
            length = count;
            unusedMainIndex = new();
            unusedSubIndex = new();

            SeprateIndex(count);
            int subcount = 1;
            for (int i = 0; i < indexpair.high_s; i++)
            {
                arrayIndexList.Add(new T[count]);
                for (int j = 0; j < subcount; j++)
                {
                    unusedMainIndex.Enqueue(i);
                    unusedSubIndex.Enqueue(j);
                }
                count >>= 1;
            }
            if (indexpair.low_s != 0)
            {
                arrayIndexList.Add(new T[1 >> indexpair.high_s]);
                for (int j = 0; j < indexpair.high_s; j++)
                {
                    unusedMainIndex.Enqueue(indexpair.high_s);
                    unusedSubIndex.Enqueue(j);
                }
            }
        }

        internal T this[long index]
        {
            get
            {
                indexpair.all_s = index;
                if (
                    (indexpair.low_s > lastSub) ||
                    (indexpair.high_s > lastMain) ||
                    (indexpair.low_s < 0) ||
                    (indexpair.high_s < 0))
                {
                    throw new IndexOutOfRangeException();
                }
                return arrayIndexList[indexpair.high_s][indexpair.low_s];
            }
            set
            {
                indexpair.all_s = index;
                if (
                    (indexpair.low_s > lastSub) ||
                    (indexpair.high_s > lastMain) ||
                    (indexpair.low_s < 0) ||
                    (indexpair.high_s < 0))
                {
                    throw new IndexOutOfRangeException();
                }
                arrayIndexList[indexpair.high_s][indexpair.low_s] = value;
            }
        }

        public T this[int index]
        {
            get
            {
                if ((index + 1 > length) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                SeprateIndex(index);
                return arrayIndexList[indexpair.high_s][indexpair.low_s];
            }
            set
            {
                if ((index + 1 > length) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                SeprateIndex(index);
                arrayIndexList[indexpair.high_s][indexpair.low_s] = value;
            }
        }

        public T this[uint index]
        {
            get
            {
                if ((index + 1 > length) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                SeprateIndex(index);
                return arrayIndexList[indexpair.high_s][indexpair.low_s];
            }
            set
            {
                if ((index + 1 > length) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                SeprateIndex(index);
                arrayIndexList[indexpair.high_s][indexpair.low_s] = value;
            }
        }

        public int Count => length;

        /// <summary>
        /// Length of this <see cref="DynamicArrayList{T}"/>
        /// </summary>
        public int Length => length;

        public int Add(T item)
        {
            // this list is tite, all item is side by side
            if (unusedMainIndex.Count == 0)
            {
                // no extra space, a new sub array is needed
                if (lastSub + 1 == 1 << lastMain)
                {
                    subLength <<= 1;
                    arrayIndexList.Add(new T[subLength]);
                    lastMain++;
                    lastSub = 0;
                    arrayIndexList[lastMain][0] = item;

                    length++;
                    subLength = 1;
                    return length - 1;
                }

                length++;
                lastSub++;
                arrayIndexList[lastMain][lastSub] = item;
                subLength = 1;
                return length - 1;
            }

            length++;
            int main = unusedMainIndex.Dequeue();
            int sub = unusedSubIndex.Dequeue();
            arrayIndexList[main][sub] = item;

            subLength = 1;
            return sub + (1 << main);
        }

        public void Clear()
        {
            arrayIndexList.Clear();
            arrayIndexList.Add(new T[1]);
            unusedMainIndex.Clear();
            unusedSubIndex.Clear();
            length = 0;
            unusedMainIndex.Enqueue(0);
            unusedSubIndex.Enqueue(0);
        }

        public bool Contains(T item)
        {
            foreach (T[] list in arrayIndexList)
            {
                foreach (T i in list)
                {
                    if (i.Equals(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(DynamicArrayList<T> target)
        {
            target.arrayIndexList.Clear();
            int length = 1;
            foreach (T[] tList in target.arrayIndexList)
            {
                T[] newIndex = new T[length];
                Buffer.BlockCopy(tList, 0, newIndex, 0, length);
                target.arrayIndexList.Add(newIndex);
                length <<= 1;
            }

            target.lastMain = lastMain;
            target.lastSub = lastSub;
            target.length = length;
            target.unusedMainIndex = new Queue<int>(unusedMainIndex.ToArray());
            target.unusedSubIndex = new Queue<int>(unusedSubIndex.ToArray());
        }

        public int IndexOf(T item)
        {
            int index = 1;
            foreach (T[] list in arrayIndexList)
            {
                int subindex = 0;
                foreach (T i in list)
                {
                    if (i.Equals(item))
                    {
                        return (index + subindex) - 1;
                    }
                    subindex++;
                }
                index <<= 1;
            }
            return -1;
        }

        [Obsolete("This method is not same, you may read data form an empty index.", false)]
        public bool Remove(T item)
        {
            int index = 0;
            foreach (T[] list in arrayIndexList)
            {
                int subindex = 0;
                foreach (T i in list)
                {
                    if (i.Equals(item))
                    {
                        list[subindex] = default;
                        if ((subindex == lastSub) && (index == lastMain))
                        {
                            if (lastSub == 0)
                            {
                                lastMain = (lastMain == 0) ? 0 : (lastMain - 1);
                                lastSub = arrayIndexList[lastMain].Length - 1;
                            }
                            else
                            {
                                lastSub--;
                            }
                            length--;
                            return true;
                        }
                        unusedMainIndex.Enqueue(index);
                        unusedSubIndex.Enqueue(subindex);
                        return true;
                    }
                    subindex++;
                }
                index++;
            }
            return false;
        }

        [Obsolete("This method is not same, you may read data form an empty index.", false)]
        public void RemoveAt(int index)
        {
            if ((index > length) || (index < 0))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            SeprateIndex(index);

            //the last element was removed.
            if (index + 1 == length)
            {
                //the last array can be disposed
                if (lastSub == 0)
                {
                    arrayIndexList.RemoveAt(lastMain);
                    lastMain--;
                    lastSub = (length / 2) - 1;
                }
                length--;
                return;
            }

            unusedMainIndex.Enqueue(indexpair.high_s);
            unusedSubIndex.Enqueue(indexpair.low_s);
        }

        public void ShowStructure()
        {
            foreach (T[] l in arrayIndexList)
            {
                foreach (T item in l)
                {
                    Console.Write($"{item}\t");
                }
                Console.Write("\n\n");
            }
        }

        private void SeprateIndex(uint ulongIndex)
        {
            indexpair.high = ulongIndex;
            int mainindex = 0;
            while (indexpair.high > 1)
            {
                indexpair.all >>= 1;
                mainindex++;
            }
            indexpair.high_s = mainindex;
            indexpair.low >>= 32 - mainindex;
        }

        private void SeprateIndex(int longIndex)
        {
            indexpair.high_s = longIndex;
            int mainindex = 0;
            while (indexpair.high > 1)
            {
                indexpair.all >>= 1;
                mainindex++;
            }
            indexpair.high_s = mainindex;
            indexpair.low >>= 32 - mainindex;
        }

    }
}