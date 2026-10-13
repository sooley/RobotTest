using SharpGL;
using SharpGL.SceneGraph.Shaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Buffer
{
    public abstract class BaseSharder:ISharderProgram
    {
        private readonly OpenGL _gl;
        public readonly ShaderProgram _program = new ShaderProgram();
        public VertexShader _vert = new VertexShader();
        public FragmentShader _frag = new FragmentShader();
        public uint Handle { get; private set; }

        public BaseSharder(OpenGL gl,string vertFile, string fragFile)
        {
            _gl = gl;
            _program.CreateInContext(gl);

            Handle = _program.ProgramObject;

            _vert.CreateInContext(gl);
            _vert.LoadSource(vertFile);
            _vert.Compile();
            Debug.Write(_vert.InfoLog);

            _frag.CreateInContext(gl);
            _frag.LoadSource(fragFile);
            _frag.Compile();
            Debug.Write(_frag.InfoLog);
            _program.AttachShader(_vert);
            _program.AttachShader(_frag);
            _program.Link();
            Debug.Write(_program.InfoLog);
        }
        public void UnBind()
        {
            _gl.UseProgram(0);
        }
        public void Bind()
        {
            _gl.UseProgram(Handle);
        }
        public bool GetComplieStatus(uint shar)
        {
            int[] parameters = new int[] { 0 };
            _gl.GetShader(shar, OpenGL.GL_COMPILE_STATUS, parameters);
            return _program.LinkStatus == true;
        }
    }
}
