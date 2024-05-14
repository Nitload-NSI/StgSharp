using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp
{
    public class UniformEmptyReferenceException : Exception
    {
        public UniformEmptyReferenceException(string program, string name):
            base($"Location -1 returned when trying to find uniform {name} in shader program {program}")
        {
            
        }
    }
}
