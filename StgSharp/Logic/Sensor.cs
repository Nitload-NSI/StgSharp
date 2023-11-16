using StgSharp.Geometries;

namespace StgSharp
{

    public delegate bool SensorRuleHandler(PlainGeometry target, PlainGeometry boarder);

    public class Sensor<T>
    {
        internal PlainGeometry _boarder;
        internal PlainGeometry _target;
        internal SensorRuleHandler _rule;


        public PlainGeometry boarder
        {
            get { return _boarder; }
        }

        public PlainGeometry target
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
