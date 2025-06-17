//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="TriggetFactory.cs"
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
using StgSharp.Math;

using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{
    public static class TriggetBuilder
    {

        private const int JoystickMax = ( int )Joystick.Last + 1;
        private const int JoystickMin = ( int )Joystick.First - 1;
        private const int KeyboardMax = ( int )KeyboardKey.Last + 1;
        private const int KeyboardMin = ( int )KeyboardKey.First - 1;
        private const int MouseMax = ( int )Mouse.Last + 1;
        private const int MouseMin = ( int )Mouse.First - 1;

        private static KeyTrigger keyboardtemp;
        private static MouseTrigger mousetemp;

        public static JoystickTrigger BuildJoystickClickTrigger( int keyCode, KeyStatus status )
        {
            if( keyCode > MouseMin && keyCode < MouseMax ) {
                return new JoystickTrigger( ( Joystick )keyCode, status );
            }
            throw new InvalidOperationException( "Known key code or key is not on joystick." );
        }

        public static KeyTrigger BuildKeyboardClickTrigger( int keyCode, KeyStatus status )
        {
            if( keyCode > KeyboardMin && keyCode < KeyboardMax ) {
                return new KeyTrigger( ( KeyboardKey )keyCode, status );
            }
            throw new InvalidOperationException( "Known key code or kay is not on keyboard." );
        }

        public static MouseTrigger BuildMouseClickTrigger( int keyCode, KeyStatus status )
        {
            if( keyCode >= MouseMin && keyCode <= MouseMax ) {
                return new MouseTrigger( ( Mouse )keyCode, status );
            }
            throw new InvalidOperationException( "Known key code or key is not on mouse." );
        }

        public static IClickTrigger BuildTrigger( int keyCode, KeyStatus status )
        {
            if( keyCode > KeyboardMin && keyCode < KeyboardMax ) {
                return new KeyTrigger( ( KeyboardKey )keyCode, status );
            }
            if( keyCode > JoystickMin && keyCode < JoystickMax ) {
                return new JoystickTrigger( ( Joystick )keyCode, status );
            }
            if( keyCode >= MouseMin && keyCode <= MouseMax ) {
                return new MouseTrigger( ( Mouse )keyCode, status );
            }
            throw new InvalidOperationException( "Known key code." );
        }

    }
}
