using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RobotModbusTCp.Graphic
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vert
    {

        public vec3 Position;
        public vec3 Normal { get; set; }
        public vec4 Colour { get; set; }
        public Vert(vec3 position, vec3 normal, vec4 colour)
        {
            Position = position;
            Normal = normal;
            Colour = colour;
        }
        //public Vect2 TexCoord;
    }
}
