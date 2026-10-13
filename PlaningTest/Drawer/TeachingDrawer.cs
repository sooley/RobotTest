using GlmSharp;
using PlaningTest.DataManager;
using PlaningTest.Graphic;
using PlaningTest.Programing;
using PlaningTest.Static;
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
    public class TeachingDrawer:ShaderDrawable
    {
        public bool isUpdate=false;
        private ProgramingData _program_data;
        private PointManager _pointManager;
        private Color m_ColorRapid;
        private Color m_ColorLinearMove;
        private Color m_SimuMove;
        private bool Run_Simulate = false;
        private int counter = 0;
        private int simu_count = 0;
        public void Set_Simulate(bool _simu)
        {
            Run_Simulate = _simu;
            counter = 0;
        }
        public TeachingDrawer(OpenGL gl) : base(gl)
        {
            _program_data = new ProgramingData();
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
            m_ColorRapid = Color.Cyan;
            m_ColorLinearMove = Color.Red;
            m_SimuMove = Color.Orange;
        }

        public void LoadProgram(ProgramingData _prog, PointManager _pointmn)
        {
            _program_data = _prog;
            _pointManager = _pointmn;
        }
        public void Update_Count(int _count)
        {
            simu_count = _count;
        }
        public override bool UpdateData()
        {
            if (isUpdate)
            {
                PrepareVectors();
                isUpdate = false;
            }
            if(Run_Simulate)
            {
                Prepare_SimuVectors();
            }    
            return true;
        }
        public void PrepareVectors()
        {
            m_lines.Clear();
            Vert vertex = new Vert();
            vec3 vertex_end = vec3.NaN;
            bool is_Start = false;
            foreach(var temp in _program_data.orders)
            {
                if (temp.move_type == MOVE_TYPE.RAPID_MOVE)
                {
                    vertex.Colour = m_ColorRapid.ToVector4();
                    if (is_Start == false)
                        vertex.Position = new vec3(Form1.m_motioncontrol_x.getPos(), Form1.m_motioncontrol_z.getPos(), Form1.m_motioncontrol_y.getPos());
                    else
                        vertex.Position = vertex_end;
                    m_lines.Add(vertex);
                    vertex.Colour = m_ColorRapid.ToVector4();
                    Pr_Point temp_point2 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    vertex.Position = new vec3(temp_point2.x, temp_point2.z, temp_point2.y);
                    m_lines.Add(vertex);
                    vertex_end = vertex.Position;
                    if(is_Start==false)
                    {
                        is_Start = true;
                    }    
                }
                else if (temp.move_type == MOVE_TYPE.LINEAR_MOVE)
                {
                    vertex.Colour = m_ColorLinearMove.ToVector4();
                    if (is_Start == false)
                        vertex.Position = new vec3(Form1.m_motioncontrol_x.getPos(), Form1.m_motioncontrol_z.getPos(), Form1.m_motioncontrol_y.getPos());
                    else
                        vertex.Position = vertex_end;
                    vertex.Position = vertex_end;
                    m_lines.Add(vertex);
                    vertex.Colour = m_ColorLinearMove.ToVector4();
                    Pr_Point temp_point2 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    vertex.Position = new vec3(temp_point2.x, temp_point2.z, temp_point2.y);
                    m_lines.Add(vertex);
                    vertex_end = vertex.Position;
                    if (is_Start == false)
                    {
                        is_Start = true;
                    }
                }
                else if(temp.move_type==MOVE_TYPE.ARC_MOVE)
                {
                    Pr_Point Pr_point1 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    Pr_Point Pr_point2 = _pointManager.GetPoints().ElementAt(temp._point[1]);
                    Pr_Point Pr_point3 = _pointManager.GetPoints().ElementAt(temp._point[2]);
                    vec3 P1 = new vec3(Pr_point1.x, Pr_point1.y, Pr_point1.z);
                    vec3 P2 = new vec3(Pr_point2.x, Pr_point2.y, Pr_point2.z);
                    vec3 P3 = new vec3(Pr_point3.x, Pr_point3.y, Pr_point3.z);
                    List<vec3> temp_point = MathHandle.CreateArc(P1, P2, P3,5);

                    foreach(var _temp in temp_point)
                    {
                        vertex.Colour = m_ColorLinearMove.ToVector4();
                        vertex.Position = vertex_end;
                        m_lines.Add(vertex);//Start
                        vertex.Colour = m_ColorLinearMove.ToVector4();
                        vertex.Position = new vec3(_temp.x, _temp.z, _temp.y); ;
                        m_lines.Add(vertex);//Start
                        vertex_end = vertex.Position;
                    }
                }  
                else if(temp.move_type==MOVE_TYPE.SPLINE_MOVE)
                {
                    List<vec3> list = new List<vec3>();
                    foreach(var h in temp._point)
                    {
                        Pr_Point temp_point1 = _pointManager.GetPoints().ElementAt(h);
                        list.Add(new vec3(temp_point1.x, temp_point1.y, temp_point1.z));
                    }
                    List<vec3> SplinePoint = MathHandle.CreateCatmullRom(list, 3);
                    foreach (var _temp in SplinePoint)
                    {
                        vertex.Colour = m_ColorLinearMove.ToVector4();
                        vertex.Position = vertex_end;
                        m_lines.Add(vertex);//Start
                        vertex.Colour = m_ColorLinearMove.ToVector4();
                        vertex.Position = new vec3(_temp.x, _temp.z, _temp.y); ;
                        m_lines.Add(vertex);//Start
                        vertex_end = vertex.Position;
                    }

                }    
            }    
        }
        public List<Vert> GetData()
        {
            return m_lines;
        }
        public void Prepare_SimuVectors()
        {
            while(counter<=simu_count)
            {
                Vert a = m_lines[counter];
                a.Colour = m_SimuMove.ToVector4();
                m_lines[counter] = a;
                counter++;
            }
        }
    }
}
