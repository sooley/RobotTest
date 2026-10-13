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
    public partial class PointsForm3 : Form
    {
        public event Action<Pr_Point> PointChanged;
        public Pr_Point mo_temp;
        public PointsForm3()
        {
            InitializeComponent();
        }
        public PointsForm3(Pr_Point Point)
        {
            InitializeComponent();
            NamePointTb.Text = Point.Name.ToString();
            PointXTb.Text = Point.x.ToString();
            PointYTb.Text = Point.y.ToString();
            PointZTb.Text = Point.z.ToString();
            PointATb.Text = Point.a.ToString();
            PointBTb.Text = Point.b.ToString();
        }

        private void GetPointBtn_Click(object sender, EventArgs e)
        {
            PointXTb.Text = Form1.m_motioncontrol_x.getPos().ToString();
            PointYTb.Text = Form1.m_motioncontrol_y.getPos().ToString();
            PointZTb.Text = Form1.m_motioncontrol_z.getPos().ToString();
            PointATb.Text = Form1.m_motioncontrol_b.getPos().ToString();
            PointBTb.Text = Form1.m_motioncontrol_c.getPos().ToString();
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
            PointChanged?.Invoke(mo_temp);
            this.Close();
        }
    }
}
