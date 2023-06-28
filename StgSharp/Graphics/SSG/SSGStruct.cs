using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace StgSharp.Graphics
{

    /// <summary>
    /// A window struct that has nearly no differencr with GLFWwindow,
    /// but only can be referred by pointers.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SSGwindow
    {

        //struct _GLFWwindow* next in GLFW
        internal SSGwindow* next;

        internal bool resizable;
        internal bool decorated;
        internal bool autoIconify;
        internal bool floating;
        internal bool focusOnShow;
        internal bool mousePassthrough;
        internal bool shouldClose;
        internal void* userPointer;
        internal bool doublebuffer;
        internal GLFWvidmode videoMode;
        internal IntPtr monitor;   //internal GLFWmonitor* monitor;
        internal GLFWcursor* cursor;

        internal int minwidth, minheight;
        internal int maxwidth, maxheight;
        internal int numer, denom;

        internal bool stickyKeys;
        internal bool stickyMouseButtons;
        internal bool lockKeyMods;
        internal int cursorMode;
        internal long mouseButtons;  //internal char[] mouseButtons = new char[8];
        internal CharArray349 keys;  //internal char[] keys = new char[349];

        

        // Virtual cursor position when cursor is disabled
        internal double virtualCursorPosX, virtualCursorPosY;
        internal bool rawMouseMotion;

        internal GLFWcontext context;

        [StructLayout(LayoutKind.Sequential)]
        internal struct CharArray349
        {
            internal long charGroup1;
            internal long charGroup2;
            internal long charGroup3;
            internal long charGroup4;
            internal long charGroup5;
            internal long charGroup6;
            internal long charGroup7;
            internal long charGroup8;
            internal long charGroup9;
            internal long charGroup10;
            internal long charGroup11;
            internal long charGroup12;
            internal long charGroup13;
            internal long charGroup14;
            internal long charGroup15;
            internal long charGroup16;
            internal long charGroup17;
            internal long charGroup18;
            internal long charGroup19;
            internal long charGroup20;
            internal long charGroup21;
            internal long charGroup22;
            internal long charGroup23;
            internal long charGroup24;
            internal long charGroup25;
            internal long charGroup26;
            internal long charGroup27;
            internal long charGroup28;
            internal long charGroup29;
            internal long charGroup30;
            internal long charGroup31;
            internal long charGroup32;
            internal long charGroup33;
            internal long charGroup34;
            internal long charGroup35;
            internal long charGroup36;
            internal long charGroup37;
            internal long charGroup38;
            internal long charGroup39;
            internal long charGroup40;
            internal long charGroup41;
            internal long charGroup42;
            internal long charGroup43;
            internal byte char345;
            internal byte char346;
            internal byte char347;
            internal byte char348;
            internal byte char349;
        }

        public SSGwindow()
        {

        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SSGmonitor
    {
        char[] name = new char[128];
        void* userPointer;

        // Physical dimensions in millimeters.
        int widthMM, heightMM;

        // The window whose video mode is current on this monitor
        //_GLFWwindow* window;
        IntPtr window;

        //GLFWvidmode* modes;
        IntPtr modes;
        int modeCount;
        GLFWvidmode currentMode;

        GLFWgammaramp originalRamp;
        GLFWgammaramp currentRamp;

        // not transformed
        //GLFW_PLATFORM_MONITOR_STATE;

        [StructLayout(LayoutKind.Sequential)]
        internal struct monitorName
        {
            internal int byte1to4;
            internal int byte5to8;
            internal int byte9to12;
            internal int byte13to16;
        }

        public SSGmonitor() { }
    }

}
