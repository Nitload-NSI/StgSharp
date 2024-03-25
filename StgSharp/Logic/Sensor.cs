//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="Sensor.cs"
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
using StgSharp.Geometries;

namespace StgSharp
{

    public delegate bool SensorRuleHandler(PlainGeometry target, PlainGeometry boarder);

    public class Sensor<T>
    {

        internal PlainGeometry _boarder;
        internal SensorRuleHandler _rule;
        internal PlainGeometry _target;


        public PlainGeometry boarder => _boarder;

        public SensorRuleHandler Rule
        {
            get => _rule;
            set => _rule = value;
        }

        public PlainGeometry target
        {
            get => _target;
            set => _target = value;
        }

        public static Sensor<T> operator -(Sensor<T> sensor, SensorRuleHandler rule)
        {
            sensor._rule -= rule;
            return sensor;
        }

        public static Sensor<T> operator +(Sensor<T> sensor, SensorRuleHandler rule)
        {
            sensor._rule += rule;
            return sensor;
        }

    }
}
