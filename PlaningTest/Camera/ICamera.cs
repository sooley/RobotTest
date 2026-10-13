using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Camera
{
    public interface ICamera
    {
        // Vect3 Postion { get; }
        double Near { get; }
        double Far { get; }

        mat4 Model { get; set; }
        mat4 View { get; }
        mat4 Projection { get; set; }
        mat4 MVP { get; }


        vec3 Eye { get; set; }
        vec3 Target { get; set; }
        vec3 Up { get; set; }

        void Update(double delta);
        void Resize(int width, int height);
    }
}
