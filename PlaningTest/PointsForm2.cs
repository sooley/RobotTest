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
    public partial class PointsForm2 : Form
    {
        public event Action<int> ReferencePoint;
        public DataTable table;
        private PointManager Pointmn2;
        public PointsForm2(Form1 _form, PointManager _Pointmn,int cursor)
        {
            InitializeComponent();
            table = new DataTable();
            Pointmn2 = _Pointmn;
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            int i = 0;
            foreach (var point in _Pointmn.GetPoints())
            {
                i++;
                table.Rows.Add(i, point.Name);
            }



            dataGridView1_Points.DataSource = table;
            dataGridView1_Points.ClearSelection();
            dataGridView1_Points.CurrentCell = dataGridView1_Points.Rows[cursor].Cells[0];
            dataGridView1_Points.Rows[cursor].Selected = true;
            dataGridView1_Points.FirstDisplayedScrollingRowIndex = cursor;
        }

        private void DetailBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1_Points.CurrentRow != null)
            {
                int rowIndex = dataGridView1_Points.CurrentRow.Index;

                PointsForm3 points_form2 = new PointsForm3(Pointmn2.GetPoints().ElementAt(rowIndex));
                points_form2.PointChanged += (p) =>
                {
                    Pointmn2.Update(rowIndex, p);
                };
                points_form2.Show();
            }
        }

        private void ReferenceBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1_Points.CurrentRow != null)
            {
                int rowIndex = dataGridView1_Points.CurrentRow.Index;
                ReferencePoint?.Invoke(rowIndex);
                this.Close();
            }
        }
    }
}
