using PlaningTest.DataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaningTest.Programing
{
    public enum MOVE_TYPE
    {
        RAPID_MOVE=1,
        LINEAR_MOVE=2,
        ARC_MOVE=3,
        CIRCLE_MOVE=4,
        SPLINE_MOVE=5
    }
    public class OrderLine
    {
        public int line_number { get; set; }
        public MOVE_TYPE move_type { get; set; }
        //public int point1 { get; set; }
        //public int point2 { get; set; }
        //public int point3 { get; set; }
        public List<int>_point{get;set;}
        public int speed { get; set; }

        public OrderLine()
        {
        }
        public OrderLine(int _line, MOVE_TYPE _mt, List<int>_p,int _speed)
        {
            _point = new List<int>();
            line_number = _line;
            move_type = _mt;
            _point = _p;
            speed = _speed;
        }
    }
    public class ProgramingData
    {
        private string filename = "";
        private int idx = 0;
        public List<OrderLine> orders { get; set; }
        public ProgramingData()
        {
            orders = new List<OrderLine>();
            idx = 0;
        }
        public void SetFileName(string _filename)
        {
            filename = _filename;
        }
        public string GetFileName()
        {
            return filename;
        }
        public void Update(OrderLine _p)
        {
            orders[_p.line_number] = _p;
        }
        public void insert(int idx,OrderLine _p)//insert OrderLine
        {
            _p.line_number = idx;
            orders.Insert(idx, _p);
            int _idx = idx + 1;
            while(_idx<orders.Count)
            {
                orders[_idx].line_number += 1;
                _idx++;
            }    
        }
        public void Cancel()
        {
            orders = new List<OrderLine>();
            filename = "";
            idx = 0;
        }
        public int LineCount()
        {
            return orders.Count;
        }
        public void Delete(int idx)
        {
            orders.RemoveAt(idx);
            while(idx<orders.Count)
            {
                orders[idx].line_number -= 1;
                idx++;
            }    
        }
        public void AddOrder(OrderLine _p)
        {
            idx = orders.Count();
            orders.Add(new OrderLine(idx, _p.move_type,_p._point,_p.speed));
        }
        public void DeleteOrder(int idx)
        {
            orders.RemoveAt(idx);
            for(int i=idx;i<orders.Count;i++)
            {
                orders.ElementAt(idx).line_number = idx;
            }    
        }
        public void InsertOrder(int idx, OrderLine _p)
        {
            orders.Insert(idx,_p);
            for (int i = idx; i < orders.Count; i++)
            {
                orders.ElementAt(idx+1).line_number = idx+1;
            }
        }
        public DataTable  Table_Binding(PointManager _pointmn)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("MoveType", typeof(string));
            table.Columns.Add("Point1", typeof(string));
            table.Columns.Add("Point2", typeof(string));
            table.Columns.Add("Point3", typeof(string));
            table.Columns.Add("Speed", typeof(string));
            int i = 0;
            string movetype = "";
            foreach (var order in orders)
            {
                i++;
                if (order.move_type == MOVE_TYPE.RAPID_MOVE)
                {
                    movetype = "RapidMove";
                    table.Rows.Add(i, movetype, _pointmn.GetPoints().ElementAt(order._point[0]).Name,"","","");
                }
                else if (order.move_type == MOVE_TYPE.LINEAR_MOVE)
                {
                    movetype = "LinearMove";
                    table.Rows.Add(i, movetype, _pointmn.GetPoints().ElementAt(order._point[0]).Name, "", "", order.speed.ToString());
                }
                else if (order.move_type == MOVE_TYPE.ARC_MOVE)
                {
                    movetype = "ArcMove";
                    table.Rows.Add(i, movetype, _pointmn.GetPoints().ElementAt(order._point[0]).Name, _pointmn.GetPoints().ElementAt(order._point[1]).Name, _pointmn.GetPoints().ElementAt(order._point[2]).Name, order.speed.ToString());
                }
                else if (order.move_type == MOVE_TYPE.CIRCLE_MOVE) 
                {
                    movetype = "CircleMove";
                    table.Rows.Add(i, movetype, _pointmn.GetPoints()[order._point[0]].Name, _pointmn.GetPoints()[order._point[1]].Name, _pointmn.GetPoints()[order._point[2]].Name, order.speed.ToString());
                }
                else if(order.move_type==MOVE_TYPE.SPLINE_MOVE)
                {
                    movetype = "SPLineMove";
                    table.Rows.Add(i, movetype);
                }    
                 
            }
            return table;
        }
        public OrderLine GetOrder(int idx)
        {
            return orders.ElementAt(idx);
        }    
        public int GetIndex()
        {
            return idx;
        }
        ///Json serialine
        public void Save()
        {
            string filePath= Path.Combine(Application.StartupPath, filename); 
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }
        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tồn tại file");
                return;
            }    
               

            string json = File.ReadAllText(filePath);

            ProgramingData  _data= JsonSerializer.Deserialize<ProgramingData>(json)
            ?? new ProgramingData();
            idx = _data.GetIndex();
            filename = filePath;
            Debug.WriteLine("aaaa"+filename);
            orders = _data.orders;
        }

    }
}
