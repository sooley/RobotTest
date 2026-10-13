using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Buffer
{
    public class OriginSharder : BaseSharder
    {
        public OriginSharder(OpenGL gl)
    : base(gl, @"Basic.vert", @"Basic.frag")
        {
        }
        public OriginSharder(OpenGL gl, CameraUBO ubo)
    : base(gl, @"Basic.vert", @"Basic.frag")
        {
            // if (this.GetComplieStatus(1)==false) throw new InvalidOperationException("Link loi");
            ubo.BindToShaderProgram(this);
        }
    }
}

