using GlmSharp;
using PlaningTest.DataManager;
using PlaningTest.Programing;
using PlaningTest.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Motion
{
    public class Trajectory
    {
        private ProgramingData _program_data;
        public Trajectory(ProgramingData _p)
        {
            _program_data = _p;
        }

        private List<Pr_Point> GenerateLine_Points(Pr_Point P1,Pr_Point P2,float Length)
        {
            List<Pr_Point> GenLine_Points = new List<Pr_Point>();
            float Point_Norm = (float)Math.Sqrt(Math.Pow((P2.x - P1.x), 2) + Math.Pow((P2.y - P1.y), 2) + Math.Pow((P2.z - P1.z), 2));
            float fract = Point_Norm / Length;
            float p_x = P1.x;
            float p_y = P1.y;
            float p_z = P1.z;
            float p_a = P1.a;
            float p_b = P1.b;

            float temp = P2.x + fract * (P2.x - P1.x);
            GenLine_Points.Add(P1);
            while (p_x<temp)
            {
                p_x += fract * (P2.x - P1.x);
                p_y += fract * (P2.y - P1.y);
                p_z += fract * (P2.z - P1.z);
                p_a += fract * (P2.a - P1.a);
                p_b += fract * (P2.b - P1.b);
                GenLine_Points.Add(new Pr_Point { x = p_x, y = p_y, z = p_z, a = p_a, b = p_b });
            }
            GenLine_Points.Add(P2);
            return GenLine_Points; 
        }
        private List<Pr_Point> GenerateArc_Points(Pr_Point P1,Pr_Point P2, Pr_Point P3,double length)
        {
            List<Pr_Point> GenLine_Points = new List<Pr_Point>();
            vec3 _p1 = new vec3(P1.x, P1.y, P1.z);
            vec3 _p2 = new vec3(P2.x, P2.y, P2.z);
            vec3 _p3 = new vec3(P3.x, P3.y, P3.z);
            List<vec3> Points = MathHandle.CreateArc(_p1, _p2, _p3, length);
            //Tính nội suy a,b sau đó update vào List GenLine_Points

            return GenLine_Points;
        }
        
    }
}
