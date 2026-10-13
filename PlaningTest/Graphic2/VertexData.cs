using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Graphic
{
    class VertexData
    {
        public vec3 Position;
        public vec3 Color;
        public vec3 Start;
        public VertexData(vec3 _position, vec3 _Color, vec3 _Start)
        {
            Position = _position;
            Color = _Color;
            Start = _Start;
        }
        //public Vect2 TexCoord;
    }
}
