using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Parser
{
    public class ArcProperties
    {
        public ArcProperties()
        {
            radius = 0;
            center = vec3.Zero;
        }
        public bool isClockWise;
        public double radius;
        public vec3 center;
    }
}
