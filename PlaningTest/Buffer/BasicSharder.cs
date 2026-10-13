using PlaningTest.Buffer;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotModbusTCp.Buffer
{
    public class BasicSharder:BaseSharder
    {
        public BasicSharder(OpenGL gl)
    : base(gl, @"Basic.vert", @"Basic.frag")
        {
        }
        public BasicSharder(OpenGL gl, IUBO ubo)
    : base(gl, @"Basic.vert", @"Basic.frag")
        {
            // if (this.GetComplieStatus(1)==false) throw new InvalidOperationException("Link loi");
            ubo.BindToShaderProgram(this);
        }
    }
}

