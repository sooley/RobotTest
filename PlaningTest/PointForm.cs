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
    public partial class PointForm : Form
    {
        public event Action<int,Pr_Point> PointChanged;
        public event Action<int> ReferencePoint;

        private int select_pointid = 0;
        private PointManager Pointmn2;
        public Pr_Point mo_temp;// biến thay đổi dữ liệu cho dialog
        public DataTable table;
        public Form1 form1_data;

        public PointForm()
        {
            InitializeComponent();
        }
        public PointForm(Form1 _form, PointManager _Pointmn,int idx)
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            int i = 0;
            foreach(var point in _Pointmn.GetPoints())
            {
                i++;
                table.Rows.Add(i, point.Name);
            }
            dataGridView1_Point.DataSource = table;
            Pr_Point temp = _Pointmn.GetPoints().ElementAt(idx);
            Pointmn2 = _Pointmn;
            form1_data = _form;
            select_pointid = idx;



            NamePointTb.Text = temp.Name.ToString();
            PointXTb.Text = temp.x.ToString();
            PointYTb.Text = temp.y.ToString();
            PointZTb.Text = temp.z.ToString();
            PointATb.Text = temp.a.ToString();
            PointBTb.Text = temp.b.ToString();
        }

        private void PointForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)// GetPoint
        {
            PointXTb.Text = Form1.m_motioncontrol_x.getPos().ToString();
            PointYTb.Text = Form1.m_motioncontrol_y.getPos().ToString();
            PointZTb.Text = Form1.m_motioncontrol_z.getPos().ToString();
            PointATb.Text = "0";
            PointBTb.Text = "0";
        }

        private void ModifyBtn_Click(object sender, EventArgs e)
        {
             mo_temp = new Pr_Point
            {
                Name = NamePointTb.Text,
                x = float.Parse(PointXTb.Text),
                y = float.Parse(PointYTb.Text),
                z = float.Parse(PointZTb.Text),
                a = float.Parse(PointATb.Text),
                b = float.Parse(PointBTb.Text)
            };
            PointChanged?.Invoke(select_pointid,mo_temp);
            this.Close();
            //Pointmn2.Update(select_pointid,mo_temp);
            //DialogResult = DialogResult.OK;
        }

        private void button1_Click_1(object sender, EventArgs e)//nút reference
        {
            if (dataGridView1_Point.CurrentRow != null)
            {
                int rowIndex = dataGridView1_Point.CurrentRow.Index;

                ReferencePoint?.Invoke(rowIndex);
            }
        }
    }
}
