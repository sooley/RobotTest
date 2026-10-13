using SharpGL;
using PlaningTest.Graphic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GlmSharp;
using System.Drawing;

namespace PlaningTest.Drawer
{
    public class HeightMapGridDrawer:ShaderDrawable
    {
        public HeightMapGridDrawer(OpenGL gl) : base(gl)
        {
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
        }
        public System.Drawing.Point GridSize()
        {
            return m_gridSize;
        }
        public void setGridSize(System.Drawing.Point gridSize)
        {
            m_gridSize = gridSize;
        }
        public Rect borderRect()
        {
            return m_borderRect;
        }    
        public void setborderRect(Rect borderRect)
        {
            m_borderRect = borderRect;
        }
        public double zTop()
        { return m_zTop; }
        public void setZtop(double zTop)
        {
            m_zTop = zTop;
        }
        public double ZBottom()
        {
            return m_zBottom;
        }
        public override bool UpdateData()
        {
            //Grid parameter
            int gridPointsX = 10;
            int gridPointsY = 10;
            float gridStepX = (float)(gridPointsX > 1 ? m_borderRect.Width / (gridPointsX - 1) : 0);
            float gridStepY = (float)(gridPointsY > 1 ? m_borderRect.Height / (gridPointsY - 1) : 0);

            m_lines.Clear();
            m_points.Clear();
            //horizontal line
            for(int i=0;i<gridPointsY;i++)
            {
                for(int j=1;j<gridPointsX;j++)
                {
                    m_lines.Add(new Vert(new vec3((float)m_borderRect.X + gridStepX * (j - 1), 0, (float)m_borderRect.Y + gridStepY * i), new vec3(1.0f, 0.0f, 0.0f), Color.FromArgb(45,45,45).ToVector4()));
                    m_lines.Add(new Vert(new vec3((float)m_borderRect.X + gridStepX * j, 0, (float)m_borderRect.Y + gridStepY * i), new vec3(1.0f, 0.0f, 0.0f), Color.FromArgb(45, 45, 45).ToVector4()));
                }    
            }
            //vẻtical line
            for (int j = 0; j < gridPointsY; j++)
            {
                for (int i = 1; i < gridPointsX; i++)
                {
                    m_lines.Add(new Vert(new vec3((float)m_borderRect.X + gridStepX * j, 0, (float)m_borderRect.Y + gridStepY * (i-1)), new vec3(1.0f, 0.0f, 0.0f), Color.FromArgb(45, 45, 45).ToVector4()));
                    m_lines.Add(new Vert(new vec3((float)m_borderRect.X + gridStepX * j, 0, (float)m_borderRect.Y + gridStepY * i), new vec3(1.0f, 0.0f, 0.0f), Color.FromArgb(45, 45, 45).ToVector4()));
                }
            }
            return true;
        }
        public void setZBottom(double zBottom)
        {
             m_zBottom= zBottom;
        }
        private System.Drawing.Point m_gridSize;
        private Rect m_borderRect;
        double m_zTop;
        double m_zBottom;

    }
}
