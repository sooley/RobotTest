using g3;
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
    public class ObjectDrawer : ShaderDrawable
    {
        private DMesh3 mesh;
        public bool m_updated = false;
        public ObjectDrawer(OpenGL gl) : base(gl)
        {
            cur_point = new vec3(0, 0, 0);
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
        }
        private vec3 cur_point;
        private bool is_Update = false;
        public void SetPos(vec3 m_pos)
        {
            cur_point = m_pos;
            if (cur_point != m_pos) is_Update = true;
        }
        public override bool UpdateData()
        {
            return m_updated;
        }
        public void Loadfile(string file)
        {
            Debug.WriteLine($"File load: {file}");
            mesh = StandardMeshReader.ReadMesh(file);
            if (mesh != null)
            {
                foreach (int tid in mesh.TriangleIndices())
                {
                    Index3i tri = mesh.GetTriangle(tid);
                    Vector3f a = mesh.GetVertexf(tri.a);
                    Vector3f b = mesh.GetVertexf(tri.b);
                    Vector3f c = mesh.GetVertexf(tri.c);
                    Vector3f d = (Vector3f)mesh.GetTriNormal(tid).Normalized;
                    m_triangles.Add(new Vert(new vec3(a.x, a.y, a.z), new vec3(d.x, d.y, d.z), new vec4(1.0F, 0.0F, 0.0F, 0.0F)));
                    m_triangles.Add(new Vert(new vec3(b.x, b.y, b.z), new vec3(d.x, d.y, d.z), new vec4(1.0F, 0.0F, 0.0F, 0.0F)));
                    m_triangles.Add(new Vert(new vec3(c.x, c.y, c.z), new vec3(d.x, d.y, d.z), new vec4(1.0F, 0.0F, 0.0F, 0.0F)));
                }
                m_updated = true;
            }
            Debug.WriteLine($"Load{m_triangles.Count}");
        }

    }
}