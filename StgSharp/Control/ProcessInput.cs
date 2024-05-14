using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{


    public partial class Form
    {
        Dictionary<KeyboardKey, KeyBoardEvent> keyEventDictionary = new Dictionary<KeyboardKey, KeyBoardEvent>();

        internal void InternalLoadInput(MethodInfo[] methods)
        {
            if (methods.Length == 0)
            {
                return;
            }
            foreach (MethodInfo m in methods)       //find all methods decorated by RenderActivityAttribute
            {
                KeyEventAttribute attr = m.GetCustomAttribute<KeyEventAttribute>(false);
                if (attr == null)
                {
                    continue;
                }
                if (!m.IsStatic && (m.GetParameters().Length != 0) && (m.ReturnType != typeof(void)))
                {
                    throw new NotSupportedException("A render activity cannot have parameters, returning value or be static.");
                }
            }
        }

        internal void InternalProcessInput()
        {

        }

    }

    public delegate void KeyBoardEvent(KeyboardKey key, KeyStatus status);
    public delegate void MouseEvent(Mouse mouse, KeyStatus status);
    public delegate void JoystickEvent(Joystick joystick, KeyStatus status);

}
