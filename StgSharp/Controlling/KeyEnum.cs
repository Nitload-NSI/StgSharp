//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="KeyEnum.cs"
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
using StgSharp.Graphics;
using StgSharp.Graphics.OpenGL;

using System;

namespace StgSharp
{
    public enum KeyboardKey : int
    {

        UNKNOWN = -1,
        None = 0,
        Space = 32,
        Apostrophe = 39/*'*/,
        Comma = 44/*,*/,
        Minus = 45/*-*/,
        Period = 46/*.*/,
        Slash = 47/*/*/,
        Zero = 48,
        One = 49,
        Two = 50,
        Three = 51,
        Four = 52,
        Five = 53,
        Six = 54,
        Seven = 55,
        Eight = 56,
        Nine = 57,
        Semicolon = 59/*,*/,
        Equal = 61/*=*/,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LeftBracket = 91/*[*/,
        BACKSLASH = 92/*\*/,
        RightBracket = 93/*]*/,
        GraveAccent = 96/*`*/,
        World1 = 161/*non-US#1*/,
        World2 = 162/*non-US#2*/,
        Esc = 256,
        Enter = 257,
        Tab = 258,
        Backspace = 259,
        Insert = 260,
        Delete = 261,
        Right = 262,
        Left = 263,
        Down = 264,
        Up = 265,
        PgUp = 266,
        PgDown = 267,
        Home = 268,
        End = 269,
        CapsLock = 280,
        ScrollLock = 281,
        NumLock = 282,
        PrintScreen = 283,
        Pause = 284,
        F1 = 290,
        F2 = 291,
        F3 = 292,
        F4 = 293,
        F5 = 294,
        F6 = 295,
        F7 = 296,
        F8 = 297,
        F9 = 298,
        F10 = 299,
        F11 = 300,
        F12 = 301,
        F13 = 302,
        F14 = 303,
        F15 = 304,
        F16 = 305,
        F17 = 306,
        F18 = 307,
        F19 = 308,
        F20 = 309,
        F21 = 310,
        F22 = 311,
        F23 = 312,
        F24 = 313,
        F25 = 314,
        NumPad0 = 320,
        NumPad1 = 321,
        NumPad2 = 322,
        NumPad3 = 323,
        NumPad4 = 324,
        NumPad5 = 325,
        NumPad6 = 326,
        NumPad7 = 327,
        NumPad8 = 328,
        NumPad9 = 329,
        NumPadDec = 330,
        NumPadDiv = 331,
        NumPadMul = 332,
        NumPadSub = 333,
        NumPadAdd = 334,
        NumPadAnter = 335,
        NumPadEqual = 336,
        LeftShift = 340,
        LeftControl = 341,
        LeftAlt = 342,
        LeftSuper = 343,
        RightShift = 344,
        RightControl = 345,
        RightAlt = 346,
        RightSuper = 347,
        Menu = 348,
        First = Space,
        Last = Menu,

    }

    #pragma warning disable CA1707
    #pragma warning disable CA1712
    #pragma warning disable CA1008
    #pragma warning disable CA1027
    public enum Joystick : int
    #pragma warning restore CA1027
    {

        Joystick_1 = GLconst.GLFW_JOYSTICK_1,
        Joystick_2 = GLconst.GLFW_JOYSTICK_2,
        Joystick_3 = GLconst.GLFW_JOYSTICK_3,
        Joystick_4 = GLconst.GLFW_JOYSTICK_4,
        Joystick_5 = GLconst.GLFW_JOYSTICK_5,
        Joystick_6 = GLconst.GLFW_JOYSTICK_6,
        Joystick_7 = GLconst.GLFW_JOYSTICK_7,
        Joystick_8 = GLconst.GLFW_JOYSTICK_8,
        Joystick_9 = GLconst.GLFW_JOYSTICK_9,
        Joystick_10 = GLconst.GLFW_JOYSTICK_10,
        Joystick_11 = GLconst.GLFW_JOYSTICK_11,
        Joystick_12 = GLconst.GLFW_JOYSTICK_12,
        Joystick_13 = GLconst.GLFW_JOYSTICK_13,
        Joystick_14 = GLconst.GLFW_JOYSTICK_14,
        Joystick_15 = GLconst.GLFW_JOYSTICK_15,
        Joystick_16 = GLconst.GLFW_JOYSTICK_16,
        First = Joystick_1,
        Last = Joystick_16

    }

    [System.Flags]
    public enum Mouse : int
    {

        Button1 = 0,
        Button2 = 1,
        Button3 = 2,
        Button4 = 3,
        Button5 = 4,
        Button6 = 5,
        Button7 = 6,
        Button8 = 7,
        First = Button1,
        Last = Button8,
        Left = Button1,
        Right = Button2,
        Middle = Button3

    }

    #pragma warning restore CA1707
    #pragma warning restore CA1712
    #pragma warning restore CA7008

    internal enum InputEquipment
    {

        Keyboard,
        Mouse,
        Joystick

    }

    public enum KeyStatus : int
    {

        RELEASE = 0,
        PRESS = 1

    }
}
