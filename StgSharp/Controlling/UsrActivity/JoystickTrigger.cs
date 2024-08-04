using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{
    public struct JoystickTrigger : ITrigger, IEquatable<JoystickTrigger>
    {
        public Joystick Stick;

        private KeyStatus _triggeredStat;

        public int TargetKeyOrButtonID => (int)Stick;

        public KeyStatus TriggeredStatus => _triggeredStat;

        public JoystickTrigger(Joystick targetKey)
        {
            this.Stick = targetKey;
        }

        public static bool operator ==(JoystickTrigger left, JoystickTrigger right)
        {
            return left._triggeredStat == right._triggeredStat && left.Stick == right.Stick;
        }

        public static bool operator !=(JoystickTrigger left, JoystickTrigger right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)Stick + 17 * (int)_triggeredStat;
        }

        public override string ToString()
        {
            return $"Key {Stick} triggered when {_triggeredStat}";
        }

        bool IEquatable<JoystickTrigger>.Equals(JoystickTrigger other)
        {
            return other == this;
        }
    }
}
