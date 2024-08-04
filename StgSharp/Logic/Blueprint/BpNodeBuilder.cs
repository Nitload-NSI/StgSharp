using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Logic
{
    public static class BpNodeConverter
    {
        public static IConvertableToBlueprintNode AsBlueprintNodeMainExecution(this BlueprintNodeOperation startUp)
        {
            if (startUp == null)
            {
                throw new ArgumentNullException(nameof(startUp));
            }
            var attributeArray = startUp.Method.GetCustomAttributes(typeof(BpNodeActionAttribute), false);
            var attri = (BpNodeActionAttribute)attributeArray[0];
            if (attributeArray.Length < 0 || attri == null)
            {
                throw new InvalidCastException($"Delegate {nameof(startUp)} is not marked by {typeof(BpNodeActionAttribute)}");
            }
            return new DefaultConvertableToBlueprintNode(startUp,attri.InputPort, attri.OutputPort);
        }

        public static IConvertableToBlueprintNode AsBlueprintNodeMainExecution(this Action startUp)
        {
            if (startUp == null)
            {
                throw new ArgumentNullException(nameof(startUp));
            }
            var attributeArray = startUp.Method.GetCustomAttributes(typeof(BpNodeActionAttribute), false);
            var attri = (BpNodeActionAttribute)attributeArray[0];
            if (attributeArray.Length < 0 || attri == null)
            {
                throw new InvalidCastException($"Delegate {nameof(startUp)} is not marked by {typeof(BpNodeActionAttribute)}");
            }
            return new CrossFrameOperation(startUp,attri.OperationSpan);
        }

    }
}
