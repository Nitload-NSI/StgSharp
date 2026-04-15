using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.UserInterface
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class PanelStateChangeBindingAttribute(
                        string statePath,
                        string panelHostPath
    ) : Attribute
    {

        public string StatePath { get; } = string.IsNullOrWhiteSpace(statePath) ?
                                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(statePath)) :
                                           statePath;

        public string PanelHostPath { get; } = string.IsNullOrWhiteSpace(panelHostPath) ?
                                   throw new ArgumentException("Value cannot be null or whitespace.", nameof(panelHostPath)) :
                                               panelHostPath;

    }
}
