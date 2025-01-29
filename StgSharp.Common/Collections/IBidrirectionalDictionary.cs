//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IBidrirectionalDictionary.cs"
//     Project: StgSharp
//     AuthorGroup: Nitload Space
//     Copyright (c) Nitload Space. All rights reserved.
//     
//     Permission is hereby granted, free of charge, to any person 
//     obtaining a copy of this software and associated documentation 
//     files (the ¡°Software¡±), to deal in the Software without restriction, 
//     including without limitation the rights to use, copy, modify, merge,
//     publish, distribute, sublicense, and/or sell copies of the Software, 
//     and to permit persons to whom the Software is furnished to do so, 
//     subject to the following conditions:
//     
//     The above copyright notice and 
//     this permission notice shall be included in all copies 
//     or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED ¡°AS IS¡±, 
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StgSharp.Commom.Collections
{
    public interface IBidirectionalDictionary<TFirst, TSecond> : IDictionary<TFirst, TSecond>
    {

        public TFirst this[TSecond key]
        {
            get;
            set;
        }

        public ICollection<TFirst> FirstIndex
        {
            get;
        }

        public ICollection<TSecond> SecondIndex
        {
            get;
        }

        public IReadOnlyDictionary<TFirst, TSecond> Forward
        {
            get;
        }

        public IReadOnlyDictionary<TSecond, TFirst> Reverse
        {
            get;
        }

        /// <summary>
        /// Determines whether the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains
        /// the specified key.
        /// </summary>
        /// <param _name="key"> The value to locate in the <see cref="IBidirectionalDictionary{TFirst, TSecond}" />. </param>
        /// <returns>
        /// true if the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains an element
        /// with the specified key; otherwise, false.
        /// </returns>
        public bool Contains(TSecond key);

        /// <summary>
        /// Determines whether the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains
        /// the specified key.
        /// </summary>
        /// <param _name="key"> The value to locate in the <see cref="IBidirectionalDictionary{TFirst, TSecond}" />. </param>
        /// <returns>
        /// true if the <see cref="IBidirectionalDictionary{TFirst, TSecond}" /> contains an element
        /// with the specified key; otherwise, false.
        /// </returns>
        public bool Contains(TFirst key);

        public bool TryGetValue(TSecond key, out TFirst value);

#pragma warning disable CA1033
        ICollection<TFirst> IDictionary<TFirst, TSecond>.Keys => FirstIndex;

        ICollection<TSecond> IDictionary<TFirst, TSecond>.Values => SecondIndex;

        bool IDictionary<TFirst, TSecond>.ContainsKey(TFirst key) => Contains(
            key);
#pragma warning restore CA1033 
    }
}