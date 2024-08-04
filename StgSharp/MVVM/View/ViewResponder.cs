//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
//     file="ViewResponder.cs"
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
using StgSharp.Controlling.UsrActivity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.MVVM.View
{
    public partial class ViewBase
    {
        protected interface IViewResponder<out TVIew> where TVIew : ViewBase
        {

            Action? this[ITrigger trigger] { get; set; }

            void CustomizedInitialize(Func<ITrigger, string, Action> keyRegister);

            void ProcessUserInput();

            bool ValidateInitializing();

            void Initialize(Func<string, Action> keyGetter);

        }

        protected abstract class ViewResponder<TView> : IViewResponder<TView> where TView : ViewBase
        {

            private bool _isInitialized;
            private Dictionary<ITrigger, Action> keyCallbackIndex;
            private Dictionary<string, Action> nameToCallbackMap;
            private TView _binding;

            protected ViewResponder(TView binding)
            {
                keyCallbackIndex = new Dictionary<ITrigger, Action>();
                nameToCallbackMap = new Dictionary<string, Action>();
                _binding = binding;
            }

            public Action? this[ITrigger trigger]
            {
                get => keyCallbackIndex.TryGetValue(trigger, out Action callback) ? callback : null;
                set
                {
                    if (keyCallbackIndex.TryGetValue(trigger, out Action callback))
                    {
                        callback += value;
                    }
                    else
                    {
                        keyCallbackIndex.Add(trigger, value ?? throw new ArgumentNullException(nameof(value)));
                    }
                }
            }

            protected TView Binding => _binding;

            public abstract void CustomizedInitialize(Func<ITrigger, string, Action> keyRegister);

            public void ProcessUserInput()
            {
                foreach (KeyValuePair<ITrigger, Action> pair in keyCallbackIndex)
                {
                    if (InternalIO.glfwGetKey(_binding.ViewHandle, pair.Key.TargetKeyOrButtonID) == pair.Key.TriggeredStatus)
                    {
                        pair.Value();
                    }
                }
            }

            public abstract bool ValidateInitializing();

            public void Initialize(Func<string, Action> keyGetter)
            {
                if (_isInitialized)
                {
                    return;
                }
                CustomizedInitialize(
                    (trigger, name) =>
                    {
                        if (!nameToCallbackMap.TryGetValue(name, out Action ret))
                        {
                            ret = keyGetter.Invoke(name) ??
                                throw new InvalidOperationException($"Cannot find method named {name} in binded view model");
                        }
                        this[trigger] = ret;
                        return ret;
                    }
                    );
            }

        }

    }
}
