//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="JoystickTrigger"
// Project: StgSharp
// AuthorGroup: Nitload
// Copyright (c) Nitload. All rights reserved.
//     
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//     
// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{
    public readonly struct JoystickTrigger : IClickTrigger, IEquatable<JoystickTrigger>
    {

        private readonly Joystick _stick;
        private readonly KeyStatus _status;

        public JoystickTrigger(Joystick targetKey, KeyStatus status)
        {
            this._stick = targetKey;
            _status = status;
        }

        public int TargetKeyOrButtonID => (int)_stick;

        public Joystick Stick
        {
            get => _stick;
        }

        public KeyStatus TriggeredStatus => _status;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ((int)_stick) + (17 * ((int)_status));
        }

        public override string ToString()
        {
            return $"Key {_stick} triggered when {_status}";
        }

        public static bool operator !=(JoystickTrigger left, JoystickTrigger right)
        {
            return !(left == right);
        }

        public static bool operator ==(JoystickTrigger left, JoystickTrigger right)
        {
            return (left._status == right._status) && (left._stick == right._stick);
        }

        bool IEquatable<JoystickTrigger>.Equals(JoystickTrigger other)
        {
            return other == this;
        }

    }
}
