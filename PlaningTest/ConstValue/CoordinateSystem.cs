using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.ConstValue
{
    public static class CoordinateSystem
    {
        public static float[] G54 = new float[5];// toạ độ phôi
        public static float[] G55 = new float[5];
        public static float[] G56 = new float[5];
        public static float[] G57 = new float[5];
        public static float[] G58 = new float[5];
        public static float[] G59 = new float[5];

        public static float[] mcs_coord = new float[5];// toạ độ máy
        public static void Set_G54_Coord(float x, float y, float z, float b, float c)
        {
            G54[0] = x;
            G54[1] = y;
            G54[2] = z;
            G54[3] = b;
            G54[4] = c;
        }
        public static void Set_G55_Coord(float x,float y,float z,float b,float c)
        {
            G55[0] = x;
            G55[1] = y;
            G55[2] = z;
            G55[3] = b;
            G55[4] = c;
        }
        public static void Set_G56_Coord(float x, float y,float z, float b,float c)
        {
            G56[0] = x;
            G56[1] = y;
            G56[2] = z;
            G56[3] = b;
            G57[4] = c;
        }
    }
}
