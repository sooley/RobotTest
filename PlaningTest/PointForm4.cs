using PlaningTest.DataManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaningTest
{
    public partial class PointForm4 : Form
    {
        public event Action<List<Pr_Point>> ModifyPoint;
        private string prg_name = "";
        public DataTable table;
        private int idx = 0;
        private List<Pr_Point> _sp_point;
        public PointForm4(string prgname)
        {
            InitializeComponent();
            prg_name = prgname;
            _sp_point = new List<Pr_Point>();
        }
        public PointForm4(string prgname, List<Pr_Point>_pointlist)
        {
            InitializeComponent();
            prg_name = prgname;
            _sp_point = _pointlist;
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            int i = 0;
            foreach (var point in _sp_point)
            {
                i++;
                table.Rows.Add(i, point.Name);
            }
            dataGridView1_Points.DataSource = table;
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            _sp_point.Add(new Pr_Point() {Name=$"{prg_name}_{idx}", x = 0, y = 0, z = 0, a = 0, b = 0 });
            idx++;
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            int i = 0;
            foreach (var point in _sp_point)
            {
                i++;
                table.Rows.Add(i, point.Name);
            }
            dataGridView1_Points.DataSource = table;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1_Points.CurrentRow != null)
            {
                int rowIndex = dataGridView1_Points.CurrentRow.Index;

                PointsForm3 points_form2 = new PointsForm3(_sp_point[rowIndex]);
                points_form2.PointChanged += (p) =>
                {
                    _sp_point[rowIndex] = p;
                };
                points_form2.Show();
            }
        }

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
            ModifyPoint?.Invoke(_sp_point);
            this.Close();
        }
    }
}
