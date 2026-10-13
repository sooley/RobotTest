using GlmSharp;
using PlaningTest.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Static
{
    public static class MathHandle
    {
        public static double Hypot(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
        public static vec3 Tranformnormal(vec3 a, mat4 m)
        {
            vec4 tmp = m * new vec4(a.x, a.y, a.z, 1);
            return new vec3(tmp.x, tmp.y, tmp.z);
        }
        public static vec4 Tranformnormal4(vec3 a, mat4 m)
        {
            vec4 tmp = m * new vec4(a.x, a.y, a.z, 1);
            return tmp;
        }
        
        public static float deg2rad(float deg)
        {
            return deg * (float)Math.PI / 180.0f;
        }

        public static List<vec3> CreateArc(vec3 P1, vec3 P2, vec3 P3, double StepLength)
        {
            var list = new List<vec3>();
            vec3 u = P2 - P1;
            vec3 v = P3 - P1;
            vec3 n = vec3.Cross(u, v);
            if(n.Length < 1e-8)
            {
                return list;
            }
            n = n.Normalized;
            vec3 ex = u.Normalized;
            vec3 ey = vec3.Cross(n, ex).Normalized;
            double d = u.Length;
            double x3 = vec3.Dot(v, ex);
            double y3 = vec3.Dot(v, ey);

            double xc = d /2.0;

            double yc = (x3 * x3 + y3 * y3 - d * x3) / (2.0 * y3);

            float R = (float)Math.Sqrt(xc * xc + yc * yc);

            vec3 center = P1 + ex*(float)xc + ey * (float)yc;


            vec3 r1 = P1 - center;
            vec3 r2 = P2 - center;
            vec3 r3 = P3 - center;

            double a1 = Math.Atan2(vec3.Dot(r1, ey), vec3.Dot(r1, ex));
            double a2 = Math.Atan2(vec3.Dot(r2, ey), vec3.Dot(r2, ex));
            double a3 = Math.Atan2(vec3.Dot(r3, ey), vec3.Dot(r3, ex));

            double ccwMid = a2;
            double ccwEnd = a3;

            while (ccwMid < a1)
                ccwMid += 2 * Math.PI;

            while (ccwEnd < a1)
                ccwEnd += 2 * Math.PI;

            bool useCCW = ccwMid <= ccwEnd;

            if (useCCW)
            {
                a2 = ccwMid;
                a3 = ccwEnd;
            }
            else
            {
                while (a2 > a1)
                    a2 -= 2 * Math.PI;

                while (a3 > a1)
                    a3 -= 2 * Math.PI;
            }


            double arcLength = Math.Abs(a3 - a1) * R;

            int count = Math.Max(2, (int)(arcLength / StepLength));

            for (int i = 0; i <= count; i++)
            {
                double t = (double)i / count;

                double ang = a1 + (a3 - a1) * t;

                vec3 p =
                    center +
                    ex * (float)(R * Math.Cos(ang)) +
                    ey * (float)(R * Math.Sin(ang));

                list.Add(p);
            }
            return list;
        }
        public static vec3 Catmull(vec3 p0, vec3 p1, vec3 p2, vec3 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f *
            (
                (2 * p1) +
                (-p0 + p2) * t +
                (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
                (-p0 + 3 * p1 - 3 * p2 + p3) * t3
            );
        }
        public static vec2 Catmull2(vec2 p0, vec2 p1, vec2 p2, vec2 p3, float t)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return 0.5f *
            (
                (2 * p1) +
                (-p0 + p2) * t +
                (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
                (-p0 + 3 * p1 - 3 * p2 + p3) * t3
            );
        }    

        public static List<Pr_Point> CreateArc5Axis(Pr_Point P1, Pr_Point P2, Pr_Point P3, double StepLength)
        {
            var list = new List<Pr_Point>();
            vec3 u = P2.GetVec3() - P1.GetVec3();
            vec3 v = P3.GetVec3() - P1.GetVec3();
            vec3 n = vec3.Cross(u, v);
            if (n.Length < 1e-8)
            {
                return list;
            }
            n = n.Normalized;
            vec3 ex = u.Normalized;
            vec3 ey = vec3.Cross(n, ex).Normalized;
            double d = u.Length;
            double x3 = vec3.Dot(v, ex);
            double y3 = vec3.Dot(v, ey);

            double xc = d / 2.0;

            double yc = (x3 * x3 + y3 * y3 - d * x3) / (2.0 * y3);

            float R = (float)Math.Sqrt(xc * xc + yc * yc);

            vec3 center = P1.GetVec3() + ex * (float)xc + ey * (float)yc;


            vec3 r1 = P1.GetVec3() - center;
            vec3 r2 = P2.GetVec3() - center;
            vec3 r3 = P3.GetVec3() - center;

            double a1 = Math.Atan2(vec3.Dot(r1, ey), vec3.Dot(r1, ex));
            double a2 = Math.Atan2(vec3.Dot(r2, ey), vec3.Dot(r2, ex));
            double a3 = Math.Atan2(vec3.Dot(r3, ey), vec3.Dot(r3, ex));

            double ccwMid = a2;
            double ccwEnd = a3;

            while (ccwMid < a1)
                ccwMid += 2 * Math.PI;

            while (ccwEnd < a1)
                ccwEnd += 2 * Math.PI;

            bool useCCW = ccwMid <= ccwEnd;

            if (useCCW)
            {
                a2 = ccwMid;
                a3 = ccwEnd;
            }
            else
            {
                while (a2 > a1)
                    a2 -= 2 * Math.PI;

                while (a3 > a1)
                    a3 -= 2 * Math.PI;
            }


            double arcLength = Math.Abs(a2 - a1) * R;

            int count = Math.Max(2, (int)(arcLength / StepLength));

            for (int i = 0; i <= count; i++)
            {
                double t = (double)i / count;

                double ang = a1 + (a2 - a1) * t;

                double  b = P1.a + (P2.a - P1.a) *t;
                double c = P1.b + (P2.b - P1.b) * t;

                vec3 p =
                    center +
                    ex * (float)(R * Math.Cos(ang)) +
                    ey * (float)(R * Math.Sin(ang));

                list.Add(new Pr_Point { x=p.x,y=p.y,z=p.z,a=(float)b, b=(float)c});
            }

            arcLength = Math.Abs(a3 - a2) * R;

            count = Math.Max(2, (int)(arcLength / StepLength));

            for (int i = 0; i <= count; i++)
            {
                double t = (double)i / count;

                double ang = a2 + (a3 - a2) * t;

                double b = P2.a + (P3.a - P2.a) * t;
                double c = P2.b + (P3.b - P2.b) * t;

                vec3 p =
                    center +
                    ex * (float)(R * Math.Cos(ang)) +
                    ey * (float)(R * Math.Sin(ang));

                list.Add(new Pr_Point { x = p.x, y = p.y, z = p.z, a = (float)b, b = (float)c });
            }
            return list;
        }
        public static List<vec3> CreateCatmullRom(List<vec3> pts, float stepLength)
        {
            List<vec3> dense = new List<vec3>();

            if (pts.Count < 2)
                return dense;

            //-----------------------------------
            // Bước 1
            // Sinh thật nhiều điểm
            //-----------------------------------

            const float dt = 0.05f;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                vec3 p0 = (i == 0) ? pts[i] : pts[i - 1];
                vec3 p1 = pts[i];
                vec3 p2 = pts[i + 1];
                vec3 p3 = (i == pts.Count - 2) ? pts[i + 1] : pts[i + 2];

                for (float t = 0; t < 1.0f; t += dt)
                    dense.Add(Catmull(p0, p1, p2, p3, t));
            }

            dense.Add(pts.Last());

            //-----------------------------------
            // Bước 2
            // Lấy điểm cách đều
            //-----------------------------------

            List<vec3> result = new List<vec3>();

            result.Add(dense[0]);

            vec3 last = dense[0];

            for (int i = 1; i < dense.Count; i++)
            {
                float d = (dense[i] - last).Length;

                if (d >= stepLength)
                {
                    result.Add(dense[i]);
                    last = dense[i];
                }
            }

            if ((result.Last() - pts.Last()).Length > 0.001f)
                result.Add(pts.Last());

            return result;
        }

        public static List<Pr_Point> CreateCatmullRom5Axis(List<Pr_Point> pts, float stepLength)
        {
            List<Pr_Point> dense = new List<Pr_Point>();

            if (pts.Count < 2)
                return dense;

            //-----------------------------------
            // Bước 1
            // Sinh thật nhiều điểm
            //-----------------------------------

            const float dt = 0.01f;

            for (int i = 0; i < pts.Count - 1; i++)
            {
                vec3 p0 = (i == 0) ? pts[i].GetVec3() : pts[i - 1].GetVec3();
                vec3 p1 = pts[i].GetVec3();
                vec3 p2 = pts[i + 1].GetVec3();
                vec3 p3 = (i == pts.Count - 2) ? pts[i + 1].GetVec3() : pts[i + 2].GetVec3();

                vec2 _p0 = (i == 0) ? pts[i].GetVec2() : pts[i - 1].GetVec2();
                vec2 _p1 = pts[i].GetVec2();
                vec2 _p2 = pts[i + 1].GetVec2();
                vec2 _p3 = (i == pts.Count - 2) ? pts[i + 1].GetVec2() : pts[i + 2].GetVec2();
                for (float t = 0; t < 1.0f; t += dt)
                {
                    vec3 a = Catmull(p0, p1, p2, p3, t);
                    vec2 b = Catmull2(_p0, _p1, _p2, _p3, t);
                    dense.Add(new Pr_Point { x=a.x,y=a.y,z=a.z,a=b.x,b=b.y});
                }    
                    
            }

            dense.Add(pts.Last());

            //-----------------------------------
            // Bước 2
            // Lấy điểm cách đều
            //-----------------------------------

            List<Pr_Point> result = new List<Pr_Point>();

            result.Add(dense[0]);

            Pr_Point last = dense[0];

            for (int i = 1; i < dense.Count; i++)
            {
                float d = (dense[i].GetVec3() - last.GetVec3()).Length;

                if (d >= stepLength)
                {
                    result.Add(dense[i]);
                    last = dense[i];
                }
            }

            if ((result.Last().GetVec3() - pts.Last().GetVec3()).Length > 0.001f)
                result.Add(pts.Last());

            return result;
        }

        public static vec3 ProjectVectorOnLane(vec3 v, vec3 normal)
        {
            vec3 n = normal.Normalized;
            float dotProduct = vec3.Dot(v, n);
            vec3 projection = v - (dotProduct * n);
            return projection;
        }



    }
}
