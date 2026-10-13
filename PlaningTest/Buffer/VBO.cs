using SharpGL;
using SharpGL.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PlaningTest.Graphic;

namespace PlaningTest.Buffer
{
    public class VBO:IBuffer
    {
        private readonly OpenGL _gl;

        private uint _handle;
        public uint Handle
        {
            get { return _handle; }
            private set { _handle = value; }
        }

        public int Count { get; private set; }

        public BeginMode Mode { get; set; }

        public  IEnumerable<Vert> Data;

        public VBO(OpenGL gl, BeginMode beginMode)
        {
            Mode = beginMode;
            _gl = gl;


            var buffers = new uint[1];
            gl.GenBuffers(1, buffers);
            Handle = buffers[0];

            Count = 0;

        }
        public void release()
        {
            _gl.DeleteBuffers(1, new uint[] { _handle });
        }
        public bool IsCreated() { return _handle != 0; }
        public VBO(OpenGL gl, BeginMode beginMode, IEnumerable<Vert> data)
            : this(gl, beginMode)
        {
            Data = data;
            Buffer(Data);
        }
        public void alocate(IEnumerable<Vert> data)
        {
            Data = data;
            Buffer(Data);
        }
        public IntPtr map()
        {
            _gl.BindBufferARB(OpenGL.GL_ARRAY_BUFFER_ARB, Handle);
            return _gl.MapBuffer(OpenGL.GL_ARRAY_BUFFER, OpenGL.GL_WRITE_ONLY_ARB);
        }
        public void Unmap()
        {
            _gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER_ARB, _handle);
            _gl.UnmapBuffer(OpenGL.GL_ARRAY_BUFFER);
            _gl.BindBufferARB(OpenGL.GL_ARRAY_BUFFER_ARB, 0);
        }
        private void Buffer(IEnumerable<Vert> data)
        {
            using (new Bind(this))
            {
                var gldata = data.ToOpenGLVertices().ToArray();
                Count = gldata.Count();
                var vertsHandle = GCHandle.Alloc(gldata, GCHandleType.Pinned);
                _gl.BufferData(OpenGL.GL_ARRAY_BUFFER, Count * OpenGLVertex.SizeInBytes, vertsHandle.AddrOfPinnedObject(), OpenGL.GL_STATIC_DRAW);
                vertsHandle.Free();
            }
        }

        public void Bind()
        {
            _gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, Handle);
        }
        public void UnBind()
        {
            _gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
        }
    }
}
