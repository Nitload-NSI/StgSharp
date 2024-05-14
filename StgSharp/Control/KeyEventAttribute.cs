using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    [AttributeUsage( AttributeTargets.Method)]
    public class KeyEventAttribute:Attribute
    {
        internal int _keyTarget;
        internal int _equipment;
        internal int _keyStat;

        public KeyEventAttribute(KeyboardKey key ,KeyStatus status)
        {
            _equipment = (int)InputEquipment.Keyboard;
            _keyTarget = (int)key;
            _keyStat = (int)status;
        }

        public KeyEventAttribute(Joystick joystick, KeyStatus status)
        {
            _equipment = (int)InputEquipment.Keyboard;
            _keyTarget = (int)joystick;
            _keyStat = (int)status;
        }

        public KeyEventAttribute(Mouse mouse, KeyStatus status)
        {
            _equipment = (int)InputEquipment.Keyboard;
            _keyTarget = (int)mouse;
            _keyStat = (int)status;
        }

    }

}
