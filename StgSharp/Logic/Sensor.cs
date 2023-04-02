using StgSharp.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace StgSharp
{

    public delegate bool SensorRuleHandler(IGeometry target, IGeometry boarder);

    public class Sensor<T> 
    {
        internal IGeometry _boarder;
        internal IGeometry _target;
        internal SensorRuleHandler _rule;
        

        public IGeometry boarder
        {
            get { return _boarder; }
        }

        public IGeometry target
        {
            get { return _target; }
            set { _target = value; }
        }

        public SensorRuleHandler Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public static Sensor<T> operator +(Sensor<T> sensor, SensorRuleHandler rule)
        {
            sensor._rule += rule;
            return sensor;
        }

        public static Sensor<T> operator -(Sensor<T> sensor, SensorRuleHandler rule)
        {
            sensor._rule -= rule;
            return sensor;
        }
    }
}
