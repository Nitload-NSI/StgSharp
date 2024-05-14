using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Math
{
    public static partial class Scaler
    {
        public static float MaxOf(params float[] stream)
        {
            float ret = stream[0];
            foreach (var num in stream)
            {
                if (ret < num)
                {
                    ret = num;
                }
            }
            return ret;
        }
        public static float MinOf(params float[] stream)
        {
            float ret = stream[0];
            foreach (var num in stream)
            {
                if (ret > num)
                {
                    ret = num;
                }
            }
            return ret;
        }



    }
}
