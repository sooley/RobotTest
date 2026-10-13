using GlmSharp;
using PlaningTest.Graphic;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlaningTest.Drawer
{
    public class CursorDrawer:ShaderDrawable
    {
        public CursorDrawer(OpenGL gl) : base(gl)
        {
            cur_point = new vec3(0, 0, 0);
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
        }
        private vec3 cur_point;
        private bool is_Update=false;
        public void SetPos(vec3 m_pos)
        {
            cur_point = m_pos;
            if (cur_point != m_pos) is_Update = true;
        }
        public override bool UpdateData()
        {
            m_points.Clear();
            m_points.Add( new Vert(new vec3((float)cur_point.x, (float)cur_point.y, (float)cur_point.z), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()));
            if (is_Update)
            {
                is_Update = false;
                return true;
            }
            else return true;
            
        }
    }
}
