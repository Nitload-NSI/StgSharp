using StgSharp.Geometries;

namespace StgSharp
{

    public delegate bool SensorRuleHandler(IPlainGeometry target, IPlainGeometry boarder);

    public class Sensor<T>
    {
        internal IPlainGeometry _boarder;
        internal IPlainGeometry _target;
        internal SensorRuleHandler _rule;


        public IPlainGeometry boarder
        {
            get { return _boarder; }
        }

        public IPlainGeometry target
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
