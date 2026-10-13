using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Buffer
{
    public class TubeSharder: BaseSharder
    {
        public TubeSharder(OpenGL gl)
: base(gl, @"Basic1.vert", @"Basic1.frag")
        {
        }
        public TubeSharder(OpenGL gl, IUBO ubo)
    : base(gl, @"Basic1.vert", @"Basic1.frag")
        {
            // if (this.GetComplieStatus(1)==false) throw new InvalidOperationException("Link loi");
            ubo.BindToShaderProgram(this);
        }
    }
}
