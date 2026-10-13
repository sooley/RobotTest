using GlmSharp;
using PlaningTest.Graphic;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Drawer
{
    public class OriginDrawer:ShaderDrawable
    {
        private bool m_updated;
        public OriginDrawer(OpenGL gl) : base(gl)
        {
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
            Origin_Init();
            m_updated = true;
        }
        public override bool UpdateData()
        {
            return m_updated;
        }

        private void Origin_Init()
        {
            m_lines.Clear();
            m_lines.Clear();

            // =====================================================
            // Coordinate system
            // Origin: (0, 0, 0)
            // X = Red
            // Y = Green
            // Z = Blue
            // =====================================================

            float axisLength = 50.0f;
            float arrowLength = 8.0f;
            float arrowWidth = 4.0f;

            // -----------------------------------------------------
            // X AXIS - Red
            // -----------------------------------------------------

            // Main line
            m_lines.Add(new Vert(
                new vec3(0, 0, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(axisLength, 0, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));

            // Arrow head
            m_lines.Add(new Vert(
                new vec3(axisLength, 0, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(axisLength - arrowLength, arrowWidth, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(axisLength, 0, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(axisLength - arrowLength, -arrowWidth, 0),
                new vec3(0, 0, 0),
                new vec4(1.0f, 0.0f, 0.0f, 1.0f)));


            // -----------------------------------------------------
            // Y AXIS - Green
            // -----------------------------------------------------

            // Main line
            m_lines.Add(new Vert(
                new vec3(0, 0, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(0, axisLength, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));

            // Arrow head
            m_lines.Add(new Vert(
                new vec3(0, axisLength, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(arrowWidth, axisLength - arrowLength, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(0, axisLength, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(-arrowWidth, axisLength - arrowLength, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 1.0f, 0.0f, 1.0f)));


            // -----------------------------------------------------
            // Z AXIS - Blue
            // -----------------------------------------------------

            // Main line
            m_lines.Add(new Vert(
                new vec3(0, 0, 0),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(0, 0, axisLength),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            // Arrow head
            m_lines.Add(new Vert(
                new vec3(0, 0, axisLength),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(arrowWidth, 0, axisLength - arrowLength),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(0, 0, axisLength),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            m_lines.Add(new Vert(
                new vec3(-arrowWidth, 0, axisLength - arrowLength),
                new vec3(0, 0, 0),
                new vec4(0.0f, 0.0f, 1.0f, 1.0f)));

            m_updated = true;
            m_updated = true;
        }
    }
}
