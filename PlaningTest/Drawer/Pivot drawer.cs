using GlmSharp;
using PlaningTest.Graphic;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Drawer
{
    public class Pivot_drawer: ShaderDrawable
    {
        public float[] rtcp_sys;
        public float[] mcs_sys;
        public Kinematic _kinematic;
        public Pivot_drawer(OpenGL gl) : base(gl)
        {
            rtcp_sys = new float[5];
            mcs_sys = new float[5];
            _kinematic = new Kinematic();
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
        }
        public void set_mcs(vec3 XYZ, float B_pos, float C_pos)
        {
            mcs_sys[0] = XYZ.x;
            mcs_sys[1] = XYZ.y;
            mcs_sys[2] = XYZ.z;
            mcs_sys[3] = B_pos;
            mcs_sys[4] = C_pos;

            vec3 rtcp_sys_temp= _kinematic.pivot_point(XYZ, B_pos);// tính toán m
            rtcp_sys[0] = rtcp_sys_temp.x;
            rtcp_sys[1] = rtcp_sys_temp.y;
            rtcp_sys[2] = rtcp_sys_temp.z;
        }
        public void load_kinematic(Kinematic _ki)
        {
            _kinematic = _ki;
        }
        public override bool UpdateData()
        {
            m_lines.Clear();
            m_lines = null;
            m_lines = new List<Vert>{
                new Vert(new vec3(mcs_sys[0], mcs_sys[2], mcs_sys[1]), new vec3(1.0f, 0.0f, 0.0f),Color.Yellow.ToVector4()),
                new Vert(new vec3(rtcp_sys[0], rtcp_sys[2], rtcp_sys[1]), new vec3(1.0f, 0.0f, 0.0f),Color.Yellow.ToVector4())
            };
            return true;
        }

    }
}
