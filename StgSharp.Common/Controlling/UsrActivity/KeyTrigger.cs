//-----------------------------------------------------------------------
// -----------------------------------------------------------------------
// file="KeyTrigger"
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
using System.Data.Common;
using System.Text;

namespace StgSharp.Controlling.UsrActivity
{
    public readonly struct KeyTrigger : IClickTrigger, IEquatable<KeyTrigger>
    {

        private readonly KeyboardKey _targetKey;
        private readonly KeyStatus _triggerStat;

        public KeyTrigger(KeyboardKey targetKey, KeyStatus targetStat)
        {
            this._targetKey = targetKey;
            this._triggerStat = targetStat;
        }

        public int TargetKeyOrButtonID => (int)_targetKey;

        public KeyStatus TriggeredStatus => _triggerStat;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ((int)_targetKey) + (17 * ((int)_triggerStat));
        }

        public override string ToString()
        {
            return $"Key {_targetKey} triggered when {_triggerStat}";
        }

        public static bool operator !=(KeyTrigger left, KeyTrigger right)
        {
            return !(left == right);
        }

        public static bool operator ==(KeyTrigger left, KeyTrigger right)
        {
            return (left._triggerStat == right._triggerStat) && (left._targetKey == right._targetKey);
        }

        bool IEquatable<KeyTrigger>.Equals(KeyTrigger other)
        {
            return other == this;
        }

    }
}
