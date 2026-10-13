using GlmSharp;
using PlaningTest.Buffer;
using PlaningTest.Camera;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static PlaningTest.Buffer.CameraUBO;
namespace PlaningTest.Buffer
{
    public interface IUBO:IBuffer
    {
        uint Handle { get; }
        void BindToShaderProgram(ISharderProgram program);
    }    
    public abstract class BaseUBO<T>:IUBO where T:struct
    {
        private readonly OpenGL _gl;
        private readonly string _blockName;
        private readonly int _location;
        private readonly int _size;
        private uint _handle;
        private int _modelUniform;
        private int _projectionUniform;
        private int _viewUniform;
        private int _lightposUniform;
        private int _viewposUniform;
        private int _lightcolorUniform;
        private int _objectocolorUniform;
        protected CameraData Data;
        private ISharderProgram _shader;
        public uint Handle
        {
            get { return _handle; }
            private set { _handle = value; }
        }
        protected BaseUBO(OpenGL gl, string blockName, int location, int size)
        {
            _gl = gl;
            _blockName = blockName;
            _location = location;
            _size = size;
            var buffers = new uint[1];
            gl.GenBuffers(1, buffers);
            Handle = buffers[0];
            using (new Bind(this))
            {
                gl.BufferData(OpenGL.GL_ARRAY_BUFFER, _size, (IntPtr)(null), OpenGL.GL_STREAM_DRAW);
                gl.BindBufferRange(OpenGL.GL_ARRAY_BUFFER, (uint)_location, _handle, 0, _size);
            }
        }
        protected BaseUBO(OpenGL gl, string blockName, int location)
            : this(gl, blockName, location, Marshal.SizeOf(default(T)))
        {

        }
        public void BindToShaderProgram(ISharderProgram program)
        {
            _shader = program;
            _modelUniform = _gl.GetUniformLocation(program.Handle, "Model");
            _viewUniform = _gl.GetUniformLocation(program.Handle, "View");
            _projectionUniform = _gl.GetUniformLocation(program.Handle, "Projection");
            _lightposUniform = _gl.GetUniformLocation(program.Handle, "lightPos");
            _viewposUniform = _gl.GetUniformLocation(program.Handle, "viewPos");
            _lightcolorUniform = _gl.GetUniformLocation(program.Handle, "lightColor");
            _objectocolorUniform = _gl.GetUniformLocation(program.Handle, "objectColor");
            // _gl.UniformBlockBinding(program.Handle, _gl.GetUniformBlockIndex(program.Handle, _blockName), _location);
            //_gl.ShaderStorageBlockBinding(program.Handle, (uint)_gl.GetUniformLocation(program.Handle, _blockName), (uint)_location);
        }
        protected void Update(int x,int y,int z)
        {
            //IntPtr pnt= Marshal.AllocHGlobal(Marshal.SizeOf(default(T)));
            //try
            //{
            //    Marshal.StructureToPtr(Data, pnt, false);

            //    using (new Bind(this))
            //    {
            //        _gl.BufferSubData(OpenGL.GL_ARRAY_BUFFER, 0, _size, pnt);
            //    }
            //}
            //finally
            //{
            //    Marshal.FreeHGlobal(pnt);
            //}
            //}
            using (new Bind(_shader))
            {
                mat4 ma = Data.MVP;
                _gl.UniformMatrix4(_viewUniform, 1, false, Data.View.Values1D);
                _gl.UniformMatrix4(_projectionUniform, 1, false, Data.Projection.Values1D);
                _gl.UniformMatrix4(_modelUniform, 1, false, Data.Model.Values1D);
                _gl.Uniform3ARB(_lightposUniform, x, y, z);
                _gl.Uniform3ARB(_viewposUniform, 1.2f, 1.0f, 2.0f);
                _gl.Uniform3ARB(_lightcolorUniform, 1.0f, 1.0f, 1.0f);
                _gl.Uniform3ARB(_objectocolorUniform, 1.0f, 0.5f, 0.31f);
            }
            Debug.WriteLine("X" + x + ",Y" + y + ",z" + z);
        }
        protected void Update2(ICamera camera)
        {
            using (new Bind(_shader))
            {
                mat4 ma = Data.MVP;
                _gl.UniformMatrix4(_viewUniform, 1, false, Data.View.Values1D);
                _gl.UniformMatrix4(_projectionUniform, 1, false, Data.Projection.Values1D);
                _gl.UniformMatrix4(_modelUniform, 1, false, Data.Model.Values1D);
                _gl.Uniform3ARB(_lightposUniform, camera.Eye.x, camera.Eye.y, -camera.Eye.z);
                _gl.Uniform3ARB(_viewposUniform, 0f, 0f, 0f);
                _gl.Uniform3ARB(_lightcolorUniform, 1.0f, 1.0f, 0.0f);
                _gl.Uniform3ARB(_objectocolorUniform, 1.0f, 1.0f, 0.0f);
            }
        }
        public void Bind()
        {
            _gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, _handle);
        }

        public void UnBind()
        {
            _gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
        }
    }
    public class CameraUBO : BaseUBO<CameraUBO.CameraData>
    {
        private float angle_;
        private vec4 direction;
        public struct CameraData
        {
            public mat4 MVP;
            public mat4 Model;
            public mat4 View;
            public mat4 Projection;
            public mat4 NormalMatrix;
        }

        public CameraUBO(OpenGL gl)
            : base(gl, "Camera", 0)
        {

        }
        public void Update(ICamera camera)
        {
            var normal = (camera.Model * camera.View);
            //normal.Invert();
            //normal.Transpose();
            Data = new CameraData
            {
                MVP = camera.MVP,
                Model = camera.Model,
                View = camera.View,
                Projection = camera.Projection,
                NormalMatrix = normal.Inverse.Transposed
            };
            Update2(camera);
        }
        public void Update(ICamera camera, float angle,vec3 rot, float angle2, vec3 rot2,float angle3, vec3 trans)
        {
            angle_ = angle;
            var normal = (camera.Model * camera.View);
            //normal.Invert();
            //normal.Transpose();
            Data = new CameraData
            {
                MVP = camera.MVP,
                Model = mat4.Translate(trans.x, trans.y, trans.z)*mat4.Rotate(glm.Radians(angle), rot) * mat4.Rotate(glm.Radians(angle2), rot2)*camera.Model * mat4.Translate(50, 0, 50)*mat4.Rotate(glm.Radians(angle3), vec3.UnitY)*mat4.Translate(-50, 0, -50),
                View = camera.View,
                Projection = camera.Projection,
                NormalMatrix = normal.Inverse.Transposed
            };
            direction = mat4.Rotate(glm.Radians(angle2), rot2) * mat4.Rotate(glm.Radians(angle), rot) * new vec4(0f, 1f, 0f, 1f);
            Update2(camera);
        }
        public void Update(ICamera camera,mat4 matrix)
        {
            var normal = (camera.Model * camera.View);
            //normal.Invert();
            //normal.Transpose();
            Data = new CameraData
            {
                MVP = camera.MVP,
                Model =  matrix*camera.Model,
                View = camera.View,
                Projection = camera.Projection,
                NormalMatrix = normal.Inverse.Transposed
            };
            Update2(camera);
        }
    }
    public class TubeCameraUBO : BaseUBO<CameraUBO.CameraData>
    {
        private float angle_;
        private vec4 direction;
        public TubeCameraUBO(OpenGL gl)
            : base(gl, "TubeCamera", 0)
        {

        }
        public mat4 getUBOView()
        {
            return Data.View;
        }
        public mat4 getUBOProjection()
        {
            return Data.Projection;
        }
        public mat4 getUBOModel()
        {
            return Data.Model;
        }
        public vec4 getdirection()
        {
            return direction;
        }
        public mat4 GetMVP()
        {
            return Data.MVP;
        }
        public void Update(ICamera camera,float angle,vec3 rot,float angle2,vec3 rot2, vec3 trans)
        {
            angle_ = angle;
            var normal = (camera.Model * camera.View);
            //normal.Invert();
            //normal.Transpose();
            Data = new CameraData
            {
                MVP = camera.MVP,
                Model = camera.Model * mat4.Translate(trans.x, trans.y, trans.z) * mat4.Rotate(glm.Radians(angle2), rot2) * mat4.Rotate(glm.Radians(angle),rot),
                View = camera.View,
                Projection = camera.Projection,
                NormalMatrix = normal.Inverse.Transposed
            };
            direction =mat4.Rotate(glm.Radians(angle2), rot2)*mat4.Rotate(glm.Radians(angle), rot)  * new vec4(0f, 1f, 0f, 1f);
            Update2(camera);
        }
    }   
}
