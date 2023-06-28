namespace StgSharp.Math
{
    public static partial class Calc
    {

        public static float Cos(float x)
        {
            float refer = x / 360.0f;
            refer -= (int)refer;

            refer *= 40;
            int a = (int)refer;   //大分度，查表
            refer -= a;    //小分度，拟合

            float refer2 = refer * refer;


            return -sinData[a] * (0.1570796327f * refer - 6.459640975e-4f * refer * refer2)
                + cosData[a] * (1 - 0.0123370055f * refer2 + 2.536695709e-5f * refer2 * refer2);
        }
    }
}
