using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Graphics
{
    public class FormFormatter
    {
        
    }

    public abstract unsafe partial class Form
    {

        static internal Dictionary<IntPtr, Form> activatedForms ;

        public static void ShowForm<T>(FormFormatter f) where T : Form
        {
            if (activatedForms == null)
            {
                activatedForms = new Dictionary<IntPtr, Form>();
            }
        }



    }
}
