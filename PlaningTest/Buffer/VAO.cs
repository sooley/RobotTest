using GlmSharp;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Buffer
{
    public class VAO : IBuffer
    {
        private readonly OpenGL _gl;
        private readonly ISharderProgram _program;
        private uint _handle;
        public uint Handle
        {
            get { return _handle; }
            private set { _handle = value; }
        }

        public VBO VBO { get; private set; }
        public void release()
        {
            _gl.DeleteBuffers(1, new uint[] { _handle });
        }
        public bool IsCreated() { return _handle != 0; }
        public VAO(OpenGL gl, ISharderProgram program, VBO vbo)
        {
            _gl = gl;
            _program = program;
            VBO = vbo;

            var buffers = new uint[1];
            gl.GenVertexArrays(1, buffers);
            Handle = buffers[0];

            using (new Bind(program))
            using (new Bind(this))
            using (new Bind(vbo))
            {
                var stride = Marshal.SizeOf(typeof(vec3)) * 2 + Marshal.SizeOf(typeof(vec4));

                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, true, stride, IntPtr.Zero);
                gl.BindAttribLocation(program.Handle, 0, "vert_position");

                gl.EnableVertexAttribArray(1);
                gl.VertexAttribPointer(1, 3, OpenGL.GL_FLOAT, true, stride, new IntPtr(Marshal.SizeOf(typeof(vec3))));
                gl.BindAttribLocation(program.Handle, 1, "vert_normal");

                gl.EnableVertexAttribArray(2);
                gl.VertexAttribPointer(2, 4, OpenGL.GL_FLOAT, false, stride, new IntPtr(Marshal.SizeOf(typeof(vec3)) * 2));
                gl.BindAttribLocation(program.Handle, 2, "vert_colour");
            }
        }

        public void Bind()
        {
            _program.Bind();
            _gl.BindVertexArray(Handle);
        }

        public void UnBind()
        {
            _gl.BindVertexArray(0);
            _program.Bind();
        }

        public void Render()
        {
            using (new Bind(_program))
            using (new Bind(this))
            {
                _gl.DrawArrays((uint)VBO.Mode, 0, VBO.Count);
            }
        }
    }
}
