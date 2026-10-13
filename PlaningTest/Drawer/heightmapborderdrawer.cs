using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using PlaningTest.Graphic;
using GlmSharp;
using System.Diagnostics;

namespace PlaningTest.Drawer
{
    public class Heightmapborderdrawer:ShaderDrawable
    {
        public Heightmapborderdrawer(OpenGL gl):base(gl)
        {
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
        }
        private Rect m_borderRect { get; set; }
        private int m_height { get; set; }
        public Rect borderRect()
        {
            return m_borderRect;
        }
        public void SetHeight(int height)
        {
            m_height = height;
        }
        public override vec3 getMaximumExtremes()
        {
            return new vec3((float)(m_borderRect.X + m_borderRect.Width),(float)(m_borderRect.Y + m_borderRect.Height), m_height);
        }
        public override vec3 getMinimumExtremes()
        {
            return new vec3((float)(m_borderRect.X), (float)(m_borderRect.Y), 0);
        }
        public void setBorderRect(Rect _borderRect)
        {
            m_borderRect = _borderRect;
            update();
        }    
        public override bool UpdateData()
        {
            float x = (float)m_borderRect.X;
            float y = (float)m_borderRect.Y;
            float w = (float)m_borderRect.Width;
            float d = (float)m_borderRect.Height;
            m_lines = new List<Vert>{
                new Vert(new vec3(x, m_height ,y), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x+w, m_height,y), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x+w, m_height ,y), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x+w, m_height,y+d), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x+w, m_height ,y+d), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x, m_height,y+d), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x, m_height ,y+d), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x, m_height,y), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x, 0 ,y), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x, m_height,y), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x+w, 0 ,y), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x+w, m_height,y), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x+w, 0 ,y+d), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x+w, m_height,y+d), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

                new Vert(new vec3(x, 0 ,y+d), new vec3(1.0f, 0.0f, 0.0f),Color.Green.ToVector4()),
                new Vert(new vec3(x, m_height,y+d), new vec3(1.0f, 0.0f, 0.0f), Color.Green.ToVector4()),

            };
            return true;
        }
        public bool UpdateData2(List<Vert> a)
        {
            m_triangles = a;
            return true;
        }
    }
}
