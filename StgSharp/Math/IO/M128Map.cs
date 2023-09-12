using StgSharp;
using StgSharp.Controlling;
using StgSharp.Logic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace StgSharp.Math
{
    public unsafe class Vector4Map
    {
        public readonly long mapID;
        internal long mapPtr;
        internal readonly StgSharp.LinkedListNode<long> _unusedVector4Header;
        private const int mapSize = 30 * 30;

        internal StgSharp.LinkedList<long> vector4Map = 
            new StgSharp.LinkedList<long>();

        public bool isFull
        {
            get 
            {
                return _unusedVector4Header == vector4Map._hook;
            }
        }

        public Vector4Map()
        {
            mapPtr = (long)(Marshal.AllocHGlobal(sizeof(Vector4)*mapSize)+15);

            long firstVector4ptr = (mapPtr / 16 + 1) * 16;
            
            vector4Map.Add(0);
            _unusedVector4Header = vector4Map._hook._next;

            for (int i = 0; i < 4*30*30*4; i+=16)
            {
                vector4Map.Add(firstVector4ptr+i);
            }
            
            mapID = Calc.RandomSeed();
        }

        internal long RegistManagedVector4Ptr()
        {
            LinkedListNode<long> Vector4 = _unusedVector4Header._next;
            LinkedListNode<long> Vector4Previous = _unusedVector4Header._previous;

            Vector4._next._previous = _unusedVector4Header.Next;
            Vector4Previous._next = Vector4;
            Vector4._previous = Vector4Previous;
            _unusedVector4Header._next = Vector4._next;
            Vector4._next = _unusedVector4Header;
            _unusedVector4Header._previous = Vector4;

            return Vector4._value;

        }

        internal void RecycleManagedVector4Ptr()
        {
            
        }


    }



}
