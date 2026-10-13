using PlaningTest.Graphic;
using SharpGL;
using SharpGL.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RobotModbusTCp.Buffer;
using GlmSharp;
using PlaningTest.Buffer;
using PlaningTest.Static;

namespace PlaningTest.Drawer
{
    public class ShaderDrawable
    {
        private OpenGL _gl;
        public ShaderDrawable(OpenGL gl)
        {
            _gl = gl;
            m_needsUpdateGeometry = true;
            m_Visible = true;
            m_LineWidth = 1.0;
            m_PointSize = 1.0;
            m_vbo = new VBO(_gl, BeginMode.Triangles);
        }
        public void init()
        {

        }

        public void updateGeometry(ISharderProgram program)
        {
            if (!m_vbo.IsCreated())m_vbo = new VBO(_gl, BeginMode.Triangles);
            m_vbo.Bind();
            if (UpdateData())
            {
                List<Vert> vertexData = new List<Vert>(m_triangles);
                vertexData.AddRange(m_lines);
                vertexData.AddRange(m_points);
                // m_vbo = new VBO(_gl, BeginMode.Triangles, vertexData);
                m_vbo.alocate(vertexData);
            }
            else
            {
                m_needsUpdateGeometry = false;
                return;
            }
                m_vao?.release();
                m_vao = new VAO(_gl, program, m_vbo);   
        }
        public void draw(ISharderProgram program)
        {
            if (m_vao==null) return;
            if (!m_Visible) return;
                using (new Bind(m_vao))
                {
                if (m_triangles.Count != 0)
                {
                    _gl.Enable(OpenGL.GL_DEPTH_TEST);
                    _gl.DrawArrays((uint)BeginMode.Triangles, 0, m_triangles.Count);
                    _gl.Disable(OpenGL.GL_DEPTH_TEST);
                }
                    if (m_lines.Count != 0)
                    {
                        _gl.Enable(OpenGL.GL_DEPTH_TEST);
                        _gl.LineWidth(1.0f);
                        _gl.DrawArrays((uint)BeginMode.Lines, m_triangles.Count, m_lines.Count);
                        _gl.Disable(OpenGL.GL_DEPTH_TEST);
                    }
                    if (m_points.Count != 0)
                    {
                        _gl.Enable(OpenGL.GL_DEPTH_TEST);
                        _gl.PointSize(10.0f);
                        _gl.DrawArrays((uint)BeginMode.Points, m_triangles.Count+m_lines.Count, m_points.Count);
                        _gl.Disable(OpenGL.GL_DEPTH_TEST);
                    }
            }    
        }

        public virtual bool UpdateData()
        {
            m_lines = new List<Vert>{
                new Vert(new vec3(0, 0, 0), new vec3(1, 0, 0), new vec4(float.NaN, 0, 0, 0)),
                new Vert(new vec3(10, 0, 0), new vec3(1, 0, 0), new vec4(float.NaN, 0, 0, 0)),
                new Vert(new vec3(0, 0, 0), new vec3(0, 1, 0), new vec4(float.NaN, 0, 0, 0)),
                new Vert(new vec3(0, 10, 0), new vec3(0, 1, 0), new vec4(float.NaN, 0, 0, 0)),
                new Vert(new vec3(0, 0, 0), new vec3(0, 0, 1), new vec4(float.NaN, 0, 0, 0)),
                new Vert(new vec3(0, 0, 10), new vec3(0, 0, 1), new vec4(float.NaN, 0, 0, 0)),
            };
            return true;
        }
        public virtual void update()
        {
            m_needsUpdateGeometry = true;
        }
        public bool needsUpdateGeometry()
        {
            return m_needsUpdateGeometry;
        }
        public virtual vec3 getSize()
        {
            return new vec3(0.0f, 0.0f, 0.0f);
        }    
        public virtual vec3 getMinimumExtremes()
        {
            return new vec3(0.0f, 0.0f, 0.0f);
        }
        public virtual vec3 getMaximumExtremes()
        {
            return new vec3(0.0f, 0.0f, 0.0f);
        }
        public int getVertexCount()
        {
            return m_lines.Count + m_points.Count + m_triangles.Count;
        }   
        public double lineWidth()
        {
            return m_LineWidth;
        }    
        public bool visible()
        {
            return m_Visible;
        }    
        public void setVisible(bool visible)
        {
            m_Visible = visible;
        }    
        public double pointSize()
        {
            return m_PointSize;
        }
        public void setPointSize(double pointSize)
        {
            m_PointSize= pointSize;
        }
        protected double m_LineWidth;
        protected double m_PointSize;
        protected bool m_Visible;
        protected List<Vert> m_lines;
        protected List<Vert> m_points;
        protected List<Vert> m_triangles;
        protected VBO m_vbo;
        //protected bool UpdateData()
        //{

        //}
        protected VAO m_vao;
        private bool m_needsUpdateGeometry;
    }
}
