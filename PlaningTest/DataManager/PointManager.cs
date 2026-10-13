using GlmSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaningTest.DataManager
{

    public class Pr_Point//Program point
    {
        public string Name;
        public float x;
        public float y;
        public float z;
        public float a;
        public float b;
        public Pr_Point()
        {

        }
        public vec3 GetVec3()//x,y,z
        {
            return new vec3(x, y, z);
        }
        public vec2 GetVec2()//b,c
        {
            return new vec2(a, b);
        }
        //public Pr_Point(float _x, float _y, float _z, float _w)
        //{
        //    x = _x;
        //    y = _y;
        //    z = _z;
        //    w = _w;
        //}
    }
    public class PointManager
    {
        private int idx = 0;
        string filePath = Path.Combine(Application.StartupPath, "Points.txt");
        private List<Pr_Point> mn_point;
        public  PointManager()
        {
            mn_point = new List<Pr_Point>();
            LoadPoints(filePath);
        }
        public void Bạckup()
        {
            mn_point.Clear();
            idx = 0;
            LoadPoints(filePath);
        }
        public List<Pr_Point> GetPoints()
        {
            return mn_point;
        }
        public void SavePoints()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    Debug.WriteLine(filePath);
                    foreach (Pr_Point p in mn_point)
                    {
                        writer.WriteLine($"{p.Name},"+
                                         $"{p.x.ToString(CultureInfo.InvariantCulture)}," +
                                         $"{p.y.ToString(CultureInfo.InvariantCulture)}," +
                                         $"{p.z.ToString(CultureInfo.InvariantCulture)}," +
                                         $"{p.a.ToString(CultureInfo.InvariantCulture)}," +
                                         $"{p.b.ToString(CultureInfo.InvariantCulture)} ");
                    }
                }    

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
        private void LoadPoints(string filePath)
        {
            mn_point.Clear();

            if (!File.Exists(filePath))
                return;

            foreach (string line in File.ReadLines(filePath))
            {
                string[] data = line.Split(',');

                if (data.Length != 6)
                    continue;

                mn_point.Add(new Pr_Point
                {
                    Name=data[0],
                    x = float.Parse(data[1], CultureInfo.InvariantCulture),
                    y = float.Parse(data[2], CultureInfo.InvariantCulture),
                    z = float.Parse(data[3], CultureInfo.InvariantCulture),
                    a = float.Parse(data[4], CultureInfo.InvariantCulture),
                    b = float.Parse(data[5], CultureInfo.InvariantCulture)
                });
                idx++;
            }
        }
        public void Add(Pr_Point _p)
        {
            mn_point.Add(_p);
        }
        public void Remove(int _idx)
        {
            mn_point.RemoveAt(_idx);
        }
        public void Update(int idx, Pr_Point _p)
        {
           mn_point[idx]=_p;
        }
        public void AddRange(List<Pr_Point> _p)
        {
            mn_point.AddRange(_p);
        }
        public void Insert(int _idx,Pr_Point _p)
        {
            mn_point.Insert(_idx, _p);
            idx++;
        }
        public void CreatePoint()
        {
            string _Name = $"{idx++}_Point";
            mn_point.Add(new Pr_Point
            {
                Name = _Name,
                x=0,
                y=0,
                z=0,
                a=0,
                b=0
            }); 
        }
        public int Count()
        {
            return mn_point.Count();
        }
    }
}
