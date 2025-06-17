//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="IExpConvertableFrom.cs"
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Script.Express
{
    // Base interface with one generic parameter renamed to T0
    public interface IExpConvertableFrom<T0> where T0: IExpConvertableFrom<T0>
    {

        void FromInstance( T0 entity );

        bool IsConvertableTo( string entityTypeName );

    }

    // 2 generic parameters
    public interface IExpConvertableFrom<T0, T1> : IExpConvertableFrom<T0>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
    {

        void FromInstance( T1 entity );

    }

    // 3 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2> : IExpConvertableFrom<T0, T1>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
    {

        void FromInstance( T2 entity );

    }

    // 4 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3> : IExpConvertableFrom<T0, T1, T2>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
    {

        void FromInstance( T3 entity );

    }

    // 5 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4> : IExpConvertableFrom<T0, T1, T2, T3>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
    {

        void FromInstance( T4 entity );

    }

    // 6 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5> : IExpConvertableFrom<T0, T1, T2, T3, T4>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
    {

        void FromInstance( T5 entity );

    }

    // 7 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
    {

        void FromInstance( T6 entity );

    }

    // 8 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
    {

        void FromInstance( T7 entity );

    }

    // 9 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
    {

        void FromInstance( T8 entity );

    }

    // 10 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
    {

        void FromInstance( T9 entity );

    }

    // 11 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
    {

        void FromInstance( T10 entity );

    }

    // 12 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
        where T11: IExpConvertableFrom<T11>
    {

        void FromInstance( T11 entity );

    }

    // 13 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
        where T11: IExpConvertableFrom<T11>
        where T12: IExpConvertableFrom<T12>
    {

        void FromInstance( T12 entity );

    }

    // 14 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
        where T11: IExpConvertableFrom<T11>
        where T12: IExpConvertableFrom<T12>
        where T13: IExpConvertableFrom<T13>
    {

        void FromInstance( T13 entity );

    }

    // 15 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
        where T11: IExpConvertableFrom<T11>
        where T12: IExpConvertableFrom<T12>
        where T13: IExpConvertableFrom<T13>
        where T14: IExpConvertableFrom<T14>
    {

        void FromInstance( T14 entity );

    }

    // 16 generic parameters
    public interface IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IExpConvertableFrom<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        where T0: IExpConvertableFrom<T0>
        where T1: IExpConvertableFrom<T1>
        where T2: IExpConvertableFrom<T2>
        where T3: IExpConvertableFrom<T3>
        where T4: IExpConvertableFrom<T4>
        where T5: IExpConvertableFrom<T5>
        where T6: IExpConvertableFrom<T6>
        where T7: IExpConvertableFrom<T7>
        where T8: IExpConvertableFrom<T8>
        where T9: IExpConvertableFrom<T9>
        where T10: IExpConvertableFrom<T10>
        where T11: IExpConvertableFrom<T11>
        where T12: IExpConvertableFrom<T12>
        where T13: IExpConvertableFrom<T13>
        where T14: IExpConvertableFrom<T14>
        where T15: IExpConvertableFrom<T15>
    {

        void FromInstance( T15 entity );

    }
}
