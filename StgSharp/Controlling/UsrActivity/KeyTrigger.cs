using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{

    public struct KeyTrigger: ITrigger,IEquatable<KeyTrigger>
    {
        private KeyboardKey _targetKey;
        private KeyStatus _triggerStat;

        public int TargetKeyOrButtonID => (int)_targetKey;

        public KeyStatus TriggeredStatus => _triggerStat;

        public KeyTrigger(KeyboardKey targetKey, KeyStatus targetStat) 
        {
            this._targetKey = targetKey;
            this._triggerStat = targetStat;
        }

        public static bool operator ==(KeyTrigger left, KeyTrigger right)
        {
            return left._triggerStat == right._triggerStat && left._targetKey == right._targetKey;
        }

        public static bool operator !=(KeyTrigger left, KeyTrigger right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)_targetKey + 17 * (int)_triggerStat;
        }

        public override string ToString()
        {
            return $"Key {_targetKey} triggered when {_triggerStat}";
        }

        bool IEquatable<KeyTrigger>.Equals(KeyTrigger other)
        {
            return other == this;
        }
    }
}
