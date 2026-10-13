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
    public class PipePathDrawer: ShaderDrawable
    {
        public bool isUpdate = false;
        public float R1 { get; set; }
        public float R2 { get; set; }
        private float transx { get; set; }
        private float transy { get; set; }
        private float _height { get; set; }
        private float _angle { get; set; }
        private float _anglephi { get; set; }

        private float angleaxis_ { get; set; }

        public List<vec3> mcs_points { get; set; }

        float DEG2RAD = (float)(Math.PI / 180.0f);
        float RAD2DEG = (float)(180.0f/Math.PI);
        public PipePathDrawer(OpenGL gl) : base(gl)
        {
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
            segment_points = new List<vec4>();
            mcs_points = new List<vec3>();
        }

        public List<vec4> segment_points { get; set; }
        public override bool UpdateData()
        {
            if (isUpdate)
            {
                segment_points.Clear();
                PrepareVectors();
                isUpdate = false;
            }
            return true;
        }
        public void UpdateMcs(List<vec3> a)
        {
            mcs_points = a;
            isUpdate = true;
        }
        public void declare(float _r1,float _r2,vec3 trans,float angleaxis, float angle, float anglephi, float height)
        {
            R1 = _r1;
            R2 = _r2;
            _height = height;
            transx = trans.x;
            transy = trans.y;
            angleaxis_ = angleaxis;
            _angle = angle;
            _anglephi = anglephi;
            m_lines.Clear();
            isUpdate = true;
        }

        public void PrepareVectors()
        {
            int segments = 360;
            float phi = 90 - _anglephi;
            double r_phi = phi * Math.PI / 180;
            float car = (float)Math.Cos(glm.Radians(90 - _angle));
            float sar = (float)Math.Sin(glm.Radians(90 - _angle));
            float so = (float)Math.Sin(r_phi);
            float co = (float)Math.Cos(r_phi);
            float CosTh = (float)Math.Cos(angleaxis_ * DEG2RAD);
            float SinTh = (float)Math.Sin(angleaxis_ * DEG2RAD);
            bool isStart = true;
            Vert vertex_end = new Vert();
            float temp1 = 0;
            segment_points.Clear();
            m_lines.Clear();
            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)(i * 2 * Math.PI / segments);
                float sa = (float)Math.Sin(angle);
                float ca = (float)Math.Cos(angle);
                float sqrtTerm = (float)Math.Sqrt(R1 * R1 - Math.Pow(R2 * sa, 2));
                temp1 = R2 * (float)Math.Sin(i * DEG2RAD);
                float C = (float)Math.Round(Math.Asin(temp1 / R1) * RAD2DEG);
                float y = -R2 * co * sa + so * sqrtTerm;
                float x = transx - (-R2 * ca + car * sqrtTerm) / sar;
                float z = so * R2 * sa + co * sqrtTerm;

                float X = x * CosTh + z * SinTh;
                float Y = y;
                float Z = -x * SinTh + z * CosTh;

                segment_points.Add(new vec4(X, Y, Z,C));
                if (!isStart)
                {
                    m_lines.Add(vertex_end);
                    m_lines.Add(new Vert(new vec3(X, Z, Y), new vec3(0, -1, 0), Color.Bisque.ToVector4()));
                }
                else isStart = false;
                vertex_end = new Vert(new vec3(X, Z, Y), new vec3(0, -1, 0), Color.Bisque.ToVector4());
            }
            isStart = false;
            foreach(var point in mcs_points)
            {
                if (!isStart)
                {
                    m_lines.Add(vertex_end);
                    m_lines.Add(new Vert(new vec3(point.x, point.z, point.y), new vec3(0, -1, 0), Color.Red.ToVector4()));
                }
                else isStart = false;
                vertex_end = new Vert(new vec3(point.x, point.z, point.y), new vec3(0, -1, 0), Color.Red.ToVector4());
            }    
            //Vẽ toạ độ Rtcp

        }

    }
}
