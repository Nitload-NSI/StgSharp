// -----------------------------------------------------------------------------
// file="TexturePipeline"
// Project: StgSharp
// Copyright (c) Nitload.
// SPDX-License-Identifier: MIT
// -----------------------------------------------------------------------------

using StgSharp.PipeLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class TexturePipeline : IPipeLineConnectionPayload
    {

        private Image _value;

        public TexturePipeline(
               Image i
        )
        {
            _value = i;
        }

        public object? ValueDefault => null;

        public object Value
        {
            get => _value;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value is not Image image) {
                    throw new ArgumentException("Value must be of type Image");
                }
                _value = image;
            }
        }

    }
}
