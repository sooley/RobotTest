using GlmSharp;
using PlaningTest.Buffer;
using PlaningTest.Camera;
using PlaningTest.DataManager;
using PlaningTest.Drawer;
using PlaningTest.Motion;
using PlaningTest.Planner;
using PlaningTest.Programing;
using PlaningTest.Static;
using PlaningTest.User;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;

namespace PlaningTest
{

    public partial class Form1 : Form
    {
        private PointManager _pointManager;
        private LoadFileTest file;
        private ProgramingData _ProgramingData;
        private Kinematic _kinematic;
        private Planner_ring _planner;
        private bool rtcp_mode;
        private bool simulate = false;
        private mat4 Baxismat = mat4.Identity;

        /// <summary>
        private MainCamera _camera;
        private TubeSharder sharder;
        private OriginSharder sharger1;
        private OriginSharder sharger2;
        private OriginSharder sharder3;
        private OriginSharder sharder4;
        private OriginSharder sharder5;
        private OriginSharder sharder6;
        private OriginSharder sharder7;

        private CameraUBO _ubo;
        private CameraUBO _ubo1;
        private CameraUBO _ubo2;
        private CameraUBO _ubo3;// teaching path
        private CameraUBO _ubo4;
        private CameraUBO _ubo5;//pipe intersect path
        private CameraUBO _ubo6;
        private CameraUBO _ubo7;// origin Baxis



        private gcodedrawer _gcodedrawer;
        private CursorDrawer _cursordrawer;
        private HeightMapGridDrawer _heighgridmapdrawer;
        private Heightmapborderdrawer _heightmapborderdrawer;
        private TeachingDrawer _teachingdrawer;
        private Pivot_drawer _Pivotdrawer;
        private PipePathDrawer _pipepathdrawer;
        private OriginDrawer _origin;
        private OriginDrawer _originBaxis;
        private ObjectDrawer _objectdrawer;


        public const float ZOOMSTEP = 1.1f;
        private float m_xRot, m_yRot, m_xLastRot, m_yLastRot;
        private float m_zoom;
        private System.Drawing.Point m_lastPos;
        private float m_xPan, m_yPan, m_xLastPan, m_yLastPan;

        //Motion test
        public static MotionControl m_motioncontrol_x;
        public static MotionControl m_motioncontrol_y;
        public static  MotionControl m_motioncontrol_z;
        public static MotionControl m_motioncontrol_b;
        public static MotionControl m_motioncontrol_c;
        //
        public string fullName = "";
        public string ImageCAxisName = "";
        public Bitmap ImageCAxis;
        private float CAngle = 0;

        //calib point
        private vec3 Point1_Clb;
        private vec3 Point3_Clb;
        private vec3 Point2_Clb;

        private void VisualBox1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            
            OpenGL gl = this.VisualBox1.OpenGL;

            _camera.updateProjection();
            _camera.updateView();
            _ubo.Update(_camera,85,vec3.UnitZ,2,vec3.UnitY,CAngle,new vec3(100,0,0));
            _ubo1.Update(_camera);
            _ubo2.Update(_camera);
            _ubo3.Update(_camera);
            _ubo4.Update(_camera);
            _ubo5.Update(_camera);
            _ubo6.Update(_camera);
            _ubo7.Update(_camera,Baxismat);


            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            _cursordrawer.SetPos(new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_z.getPos(), m_motioncontrol_y.getPos()));
            CursorPosTxt.Text = $"CurPos X:{m_motioncontrol_x.getPos():F1} | Y:{m_motioncontrol_y.getPos():F1} | Z:{m_motioncontrol_z.getPos():F1} | B:{m_motioncontrol_b.getPos():F1} | C:{m_motioncontrol_c.getPos():F1}|Simu{simu_count}";
            
            //_cursordrawer.UpdateData();
            _cursordrawer.updateGeometry(sharder6);
            _cursordrawer.draw(sharder6);

            _objectdrawer.updateGeometry(sharder);
            _objectdrawer.draw(sharder);

            _origin.updateGeometry(sharder6);
            _origin.draw(sharder6);

            _originBaxis.updateGeometry(sharder7);
            _originBaxis.draw(sharder7);

            if (!simulate)
            {
                if(rtcp_mode)
                {
                    rtcp_target_block_t rtcp_sys = _kinematic.rtcp_forward(new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_y.getPos(), m_motioncontrol_z.getPos()), m_motioncontrol_b.getPos(), 0);
                    _Pivotdrawer.set_mcs(new vec3(rtcp_sys.XYZ.x, rtcp_sys.XYZ.y, rtcp_sys.XYZ.z+_kinematic.get_Pivot_Length()), m_motioncontrol_b.getPos(), 0);
                }    
                else
                {
                    _Pivotdrawer.set_mcs(new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_y.getPos(), m_motioncontrol_z.getPos()), m_motioncontrol_b.getPos(), 0);
                }
                CAngle = m_motioncontrol_c.getPos() - (int)(m_motioncontrol_c.getPos() / 360) * 360;
                panel1.Invalidate();

            }


            //_pipepathdrawer.UpdateData();
            _pipepathdrawer.updateGeometry(sharder5);
            _pipepathdrawer.draw(sharder5);

            //_Pivotdrawer.UpdateData();
            _Pivotdrawer.updateGeometry(sharder4);
            _Pivotdrawer.draw(sharder4);

            //_teachingdrawer.UpdateData();
            _teachingdrawer.updateGeometry(sharder3);
            _teachingdrawer.draw(sharder3);

            //_heighgridmapdrawer.UpdateData();
            _heighgridmapdrawer.updateGeometry(sharger1);
            _heighgridmapdrawer.draw(sharger1);



            //_heightmapborderdrawer.UpdateData();
            _heightmapborderdrawer.updateGeometry(sharger2);
            _camera.fitDrawable(_heightmapborderdrawer);
            _heightmapborderdrawer.draw(sharger2);
        }

        private void VisualBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            { }// PickObject(e.X, e.Y);
            m_lastPos = e.Location;
            m_xLastRot = m_xRot;
            m_yLastRot = m_yRot;
            m_xLastPan = m_xPan;
            m_yLastPan = m_yPan;
        }

        private void VisualBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                _camera.stopViewAnimation();
                m_yRot = normalizeAngle((float)(m_yLastRot - (e.Location.X - m_lastPos.X) * 0.5));
                m_xRot = (float)(m_xLastRot + (e.Location.Y - m_lastPos.Y) * 0.5);
                if (m_xRot < -90) m_xRot = -90;
                if (m_xRot > 90) m_xRot = 90;
                _camera.UpdateRot(m_xRot, m_yRot);
                _camera.updateView();
            }
            if (e.Button == MouseButtons.Right)
            {
                m_xPan = m_xLastPan - (float)(e.Location.X - m_lastPos.X) * 1;
                m_yPan = m_yLastPan + (float)(e.Location.Y - m_lastPos.Y) * 1;
                _camera.UpdatePan(m_xPan, m_yPan);
                _camera.updateProjection();
            }
        }
        float normalizeAngle(float angle)
        {
            while (angle < 0) angle += 360;
            while (angle > 360) angle -= 360;
            return angle;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            ProgramDataGrid_View.Parent = this;
            ProgramDataGrid_View.Visible = true;
            SaveBtn.Visible = true;
            ProgramDataGrid_View.Dock = DockStyle.Fill;
            UpdateTeachingProcess();
            PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)//left button
        {
  
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_x.IsRun()&&m_motioncontrol_x.getPos()!=0)
                {
                    m_motioncontrol_x.GoPos(0);
                    m_motioncontrol_x.Start();
                }    
                    
            }
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_x.IsRun())
                {
                    m_motioncontrol_x.Stop();
                }

            }
        }

        private void Right_button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_x.IsRun()&&m_motioncontrol_x.getPos() != 500)
                {
                    m_motioncontrol_x.GoPos(500);
                    m_motioncontrol_x.Start();
                }

            }
        }

        private void Right_button_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_x.IsRun())
                {
                    m_motioncontrol_x.Stop();
                }

            }
        }

        private void Up_Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_y.IsRun() && m_motioncontrol_y.getPos() != 500)
                {
                    m_motioncontrol_y.GoPos(500);
                    m_motioncontrol_y.Start();
                }

            }
        }

        private void Up_Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_y.IsRun())
                {
                    m_motioncontrol_y.Stop();
                }

            }
        }

        private void Down_Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_y.IsRun() && m_motioncontrol_y.getPos() != 0)
                {
                    m_motioncontrol_y.GoPos(0);
                    m_motioncontrol_y.Start();
                }

            }
        }

        private void Down_Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_y.IsRun())
                {
                    m_motioncontrol_y.Stop();
                }

            }
        }

        private void Up_Button_Click(object sender, EventArgs e)
        {

        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_z.IsRun() && m_motioncontrol_z.getPos() != 500)
                {
                    m_motioncontrol_z.GoPos(500);
                    m_motioncontrol_z.Start();
                }

            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_z.IsRun())
                {
                    m_motioncontrol_z.Stop();
                }

            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_z.IsRun() && m_motioncontrol_z.getPos() != 0)
                {
                    m_motioncontrol_z.GoPos(0);
                    m_motioncontrol_z.Start();
                }
            }
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_z.IsRun())
                {
                    m_motioncontrol_z.Stop();
                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        /// </summary>
        private int runline = 0;
        private bool RunStatus = false;
        private void ProgramBinding()
        {

        }
        private void AddToBtn_Click(object sender, EventArgs e)
        {
            int _speed = 0;
            if (!isPrograming&&PrgBox.Text!=null)
            {
                isPrograming = true;
                _ProgramingData.SetFileName(PrgBox.Text+".txt");
                fullName = Path.Combine(Application.StartupPath, _ProgramingData.GetFileName());
                UpdateTeachingProcess();
            }
            
            else if(isPrograming)
            {
                
                SaveBtn.Visible = true;
                int idx = 0;
                MOVE_TYPE temp=MOVE_TYPE.RAPID_MOVE;
                switch (MoveTypeBox.SelectedIndex)
                {
                    case 0: temp = MOVE_TYPE.RAPID_MOVE;
                        List<int> _pointlist = new List<int>();
                        _pointlist.Add(point_temp1);
                        _ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, _pointlist,_speed));
                        ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                        _pointManager.SavePoints();
                        _pointManager.CreatePoint();
                        idx = _pointManager.Count();
                        Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                        point_temp1 = idx - 1;
                        break;
                    case 1: temp = MOVE_TYPE.LINEAR_MOVE;
                        List<int> _pointlist1 = new List<int>();
                        _pointlist1.Add(point_temp1);
                        _speed = int.Parse(SpeedBox.Text);
                        _ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, _pointlist1, _speed));
                        ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                        _pointManager.SavePoints();
                        _pointManager.CreatePoint();
                        idx = _pointManager.Count();
                        Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                        point_temp1 = idx - 1;
                        break;
                    case 2: temp = MOVE_TYPE.ARC_MOVE;
                        List<int> _pointlist2 = new List<int>();
                        _pointlist2.Add(point_temp1);
                        _pointlist2.Add(point_temp2);
                        _pointlist2.Add(point_temp3);
                        _speed = int.Parse(SpeedBox.Text);
                        _ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, _pointlist2, _speed));
                        ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                        _pointManager.SavePoints();
                        _pointManager.CreatePoint();
                        _pointManager.CreatePoint();
                        _pointManager.CreatePoint();
                        idx = _pointManager.Count();
                        Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 3).Name;
                        Point2Box.Text = _pointManager.GetPoints().ElementAt(idx - 2).Name;
                        Point3Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                        point_temp1 = idx - 3;
                        point_temp2 = idx - 2;
                        point_temp3 = idx - 1;
                        break;
                    case 3: temp = MOVE_TYPE.CIRCLE_MOVE;
                        _speed = int.Parse(SpeedBox.Text);
                        //_ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, _pointManager.Count() - 3, _pointManager.Count() - 2, _pointManager.Count()-1, _speed));
                        ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                        break;
                    case 4: temp = MOVE_TYPE.SPLINE_MOVE;
                        _speed = int.Parse(SpeedBox.Text);
                        _ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, SplineIndex, _speed));
                        ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                        break;
                }
            }    
        }

        private void MoveTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {         
            UpdateTeachingProcess();
            _pointManager.Bạckup();
            if (_pointManager.Count() == 0) _pointManager.CreatePoint();
            MOVE_TYPE temp = MOVE_TYPE.RAPID_MOVE;
            int idx = 0;
            switch (MoveTypeBox.SelectedIndex)
            {
                case 0:
                    temp = MOVE_TYPE.RAPID_MOVE;
                    _pointManager.CreatePoint();
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx-1).Name;
                    point_temp1 = idx - 1;
                    break;
                case 1:
                    temp = MOVE_TYPE.LINEAR_MOVE;
                    _pointManager.CreatePoint();
                    SpeedBox.Text = "1000";
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx-1).Name;
                    point_temp1 = idx - 1;
                    break;
                case 2:
                    temp = MOVE_TYPE.ARC_MOVE;
                    SpeedBox.Text = "1000";
                    _pointManager.CreatePoint();
                    _pointManager.CreatePoint();
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx-3).Name;
                    Point2Box.Text = _pointManager.GetPoints().ElementAt(idx-2).Name;
                    Point3Box.Text = _pointManager.GetPoints().ElementAt(idx-1).Name;
                    point_temp1 = idx - 3;
                    point_temp2 = idx - 2;
                    point_temp3 = idx - 1;
                    break;
                case 3:
                    temp = MOVE_TYPE.CIRCLE_MOVE;
                    _pointManager.CreatePoint();
                    _pointManager.CreatePoint();
                    _pointManager.CreatePoint();
                    break;
                case 4:
                    temp = MOVE_TYPE.SPLINE_MOVE;
                    break;
            }
        }

        private int point_temp1 = 0,point_temp2=0,point_temp3=0;
        private List<int> SplineIndex;
        private void Point1Box_MouseDown(object sender, MouseEventArgs e)// Point1
        {
            int idx = 0;
            int temp1 = _pointManager.Count();
            if (e.Button == MouseButtons.Left)
            {
                switch (MoveTypeBox.SelectedIndex)
                {
                    case 0: 
                        idx = temp1 - 1;
                        break;
                    case 1:
                        idx = temp1 - 1;
                        break;
                    case 2:      
                        idx = temp1 - 3;
                        break;
                    case 3:
                        break;
                }
                PointsForm2 points_form = new PointsForm2(this, _pointManager,point_temp1);
                    
                //points_form.PointChanged += (index, p) =>
                //   {
                //       _pointManager.Update(index, p);
                //       this.Point1Box.Text = p.Name;
                //   };
                points_form.ReferencePoint += (index) =>
                {
                    this.Point1Box.Text = _pointManager.GetPoints().ElementAt(index).Name;
                    point_temp1 = index;
                };
                points_form.Show();
                    //if(points_form.ShowDialog()==DialogResult.OK)
                    //{
                    //    _pointManager.Update(idx, points_form.mo_temp);
                    //    this.Point1Box.Text = points_form.mo_temp.Name;
                    //}    
            }
        }

        private void Point2Box_TextChanged(object sender, EventArgs e)
        {

        }

        private void Point2Box_MouseDown(object sender, MouseEventArgs e)
        {
            int idx = 0;
            int temp1 = _pointManager.Count();
            if (e.Button == MouseButtons.Left)
            {
                switch (MoveTypeBox.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        idx = temp1 - 2;
                        break;
                    case 3:
                        break;
                }
                PointsForm2 points_form = new PointsForm2(this, _pointManager,point_temp2);

                //points_form.PointChanged += (index, p) =>
                //{
                //    _pointManager.Update(index, p);
                //    this.Point2Box.Text = p.Name;
                //};
                points_form.ReferencePoint += (index) =>
                  {
                      this.Point2Box.Text = _pointManager.GetPoints().ElementAt(index).Name;
                      point_temp2 = index;
                  };
                points_form.Show();
                //if(points_form.ShowDialog()==DialogResult.OK)
                //{
                //    _pointManager.Update(idx, points_form.mo_temp);
                //    this.Point1Box.Text = points_form.mo_temp.Name;
                //}    
            }
        }

        private void Point3Box_MouseDown(object sender, MouseEventArgs e)
        {
            int idx = 0;
            int temp1 = _pointManager.Count();
            if (e.Button == MouseButtons.Left)
            {
                switch (MoveTypeBox.SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        idx = temp1 - 1;
                        break;
                    case 3:
                        break;
                }
                PointsForm2 points_form = new PointsForm2(this, _pointManager,point_temp3);

                //points_form.PointChanged += (index, p) =>
                //{
                //    _pointManager.Update(index, p);
                //    this.Point3Box.Text = p.Name;
                //};
                points_form.ReferencePoint += (index) =>
                  {
                      this.Point3Box.Text = _pointManager.GetPoints().ElementAt(index).Name;
                      point_temp3 = index;
                  };

                //Hết dòng này là đã reference được điểm


                points_form.Show();
                //if(points_form.ShowDialog()==DialogResult.OK)
                //{
                //    _pointManager.Update(idx, points_form.mo_temp);
                //    this.Point1Box.Text = points_form.mo_temp.Name;
                //}    
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            _pointManager.SavePoints();
            _ProgramingData.Save();
            _teachingdrawer.updateGeometry(sharder3);
            MessageBox.Show("Save Successfully");
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
"Bạn có chắc chắn muốn thoát chương trình không?",
"Xác nhận thoát",
MessageBoxButtons.YesNo,
MessageBoxIcon.Warning,
MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                isPrograming = false;
                isEditing = false;
                _ProgramingData.Cancel();
                UpdateTeachingProcess();
                ProgramDataGrid_View.DataSource = null;
                PrgBox.Text = "";
            }
        }

        private void SpLinePoints_Click(object sender, EventArgs e)
        {
            _pointManager.Bạckup();
            SplineIndex = new List<int>();
            int managerpoint_count = _pointManager.Count();
            if(!isEditing)
            {
                PointForm4 spline_point = new PointForm4(PrgBox.Text);
                spline_point.ModifyPoint += (points) =>
                {
                    _pointManager.AddRange(points);
                    int i = 0;
                    foreach (var _p in points)
                    {
                        SplineIndex.Add(managerpoint_count + i);
                        i++;
                    }

                };
                spline_point.Show();
            }    
            else
            {
                int rowindex = ProgramDataGrid_View.CurrentRow.Index;
                SplineIndex = new List<int>();
                List<int> spline_getpoint = _ProgramingData.GetOrder(rowindex)._point;
                List<Pr_Point> Points = new List<Pr_Point>();
                foreach(var i in spline_getpoint)
                {
                    Points.Add(_pointManager.GetPoints()[i]);
                }    
                PointForm4 spline_point = new PointForm4(PrgBox.Text, Points);
                spline_point.ModifyPoint += (points) =>
                {
                    _pointManager.AddRange(points);
                    int i = 0;
                    foreach (var _p in points)
                    {
                        SplineIndex.Add(managerpoint_count + i);
                        i++;
                    }

                };

                spline_point.Show();

            }    
            _pointManager.SavePoints();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _teachingdrawer.isUpdate = true;
            _teachingdrawer.LoadProgram(_ProgramingData, _pointManager);
            _teachingdrawer.updateGeometry(sharder3);
            groupBox2.Visible = false;
            ProgramDataGrid_View.Dock = DockStyle.Bottom;
            ProgramDataGrid_View.Height = 200;
            
        }

        private void FileSelectBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Json Files (*.json)|*.json|All Files (*.*)|*.*";
            dlg.Title = "Chọn file";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Đường dẫn đầy đủ
                string fullPath = dlg.FileName;

                // Chỉ tên file
                string fileName = Path.GetFileName(dlg.FileName);
                
                // Tên file không có phần mở rộng
                //string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dlg.FileName);
                fullName= Path.Combine(Application.StartupPath, fileName);
                Debug.WriteLine("slect file" + fullName);
                
               _ProgramingData.Load(fullName);

                if(_ProgramingData!=null)
                {
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    ProgramDataGrid_View.Visible = true;
                    groupBox2.Visible = false;
                    isPrograming = true;
                }    
            }
        }

        public Form1()
        {
            InitializeComponent();
            this.CalibPanel.Visible = false;
            this.VisualBox1.MouseWheel += VisualBox1_MouseWheel;
            this.Rear_Btn.Location = new System.Drawing.Point( this.VisualBox1.Right+52, this.VisualBox1.Location.Y+60);
            this.Back_Btn.Location = new System.Drawing.Point(this.VisualBox1.Right+this.Rear_Btn.Width+52, this.VisualBox1.Location.Y+60);
            this.Front_Btn.Location = new System.Drawing.Point(this.VisualBox1.Right + this.Rear_Btn.Width+ this.Back_Btn.Width + 52, this.VisualBox1.Location.Y+60);
            VisualBox1.BackColor = Color.Black;
            ProgramDataGrid_View.Visible = false; 
            groupBox2.Visible = false;
            CursorPosTxt.Parent = this.VisualBox1;
            CursorPosTxt.Text = "CurPos X:0|Y:0|Z:0";
            CursorPosTxt.ForeColor = Color.White;
            CursorPosTxt.Location = new System.Drawing.Point(VisualBox1.Left + 10,VisualBox1.Top + 60);
            CursorPosTxt.BringToFront();
            typeof(Panel).GetProperty(
         "DoubleBuffered",
         System.Reflection.BindingFlags.Instance |
         System.Reflection.BindingFlags.NonPublic)
        .SetValue(panel1, true);



            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit");
            ToolStripMenuItem InsertItem = new ToolStripMenuItem("Insert");
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");

            editItem.Click += ProgramEdit_Click;
            InsertItem.Click += ProgramInsert_Click;
            deleteItem.Click += ProgramDelete_Click;

            menu.Items.Add(editItem);
            menu.Items.Add(InsertItem);
            menu.Items.Add(deleteItem);

            ProgramDataGrid_View.ContextMenuStrip = menu;


            m_motioncontrol_x = new MotionControl();
            m_motioncontrol_y = new MotionControl();
            m_motioncontrol_z = new MotionControl();
            m_motioncontrol_b = new MotionControl();
            m_motioncontrol_c = new MotionControl();

            m_motioncontrol_x.Settravel(0, 500);
            m_motioncontrol_y.Settravel(0, 500);
            m_motioncontrol_z.Settravel(0, 500);
            m_motioncontrol_b.Settravel(-180, 180);
            m_motioncontrol_c.Settravel(-180, 180);
            _kinematic = new Kinematic();// khởi tạo kinematic
            rtcp_mode = false;
        }

        private void ModifyBtn_Click(object sender, EventArgs e)//nút sửa lệnh trong chương trình
        {
            isEditing = false;
            UpdateTeachingProcess();
            MOVE_TYPE move_type_=MOVE_TYPE.RAPID_MOVE;
            List<int> EditList = new List<int>();
            switch(MoveTypeBox.SelectedIndex)
            {
                case 0: move_type_ = MOVE_TYPE.RAPID_MOVE;
                    EditList.Add(point_temp1);
                    break;
                case 1: move_type_ = MOVE_TYPE.LINEAR_MOVE;
                    EditList.Add(point_temp1);
                    break;
                case 2: move_type_ = MOVE_TYPE.ARC_MOVE;
                    EditList.Add(point_temp1);
                    EditList.Add(point_temp2);
                    EditList.Add(point_temp3);
                    break;
                case 3: move_type_ = MOVE_TYPE.CIRCLE_MOVE; break;
                case 4: move_type_ = MOVE_TYPE.SPLINE_MOVE;
                    EditList = SplineIndex;
                    break;
            }
            OrderLine update_order = new OrderLine
            {
                line_number = edit_order.line_number,
                move_type = move_type_,
                speed = SpeedBox.Text==""?0:int.Parse(SpeedBox.Text),
                _point = EditList
            };
            _ProgramingData.Update(update_order);
            ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
            _pointManager.SavePoints();
            UpdateTeachingProcess();
        }
        private OrderLine edit_order;
        private void ProgramEdit_Click(object sender, EventArgs e)
        {
            isEditing = true;
            UpdateTeachingProcess();
            edit_order = new OrderLine();
            int rowIndex = ProgramDataGrid_View.CurrentRow.Index;
            edit_order = _ProgramingData.GetOrder(rowIndex);
            
            switch (edit_order.move_type)
            {
                case MOVE_TYPE.RAPID_MOVE:
                    MoveTypeBox.SelectedIndex = 0;
                    UpdateTeachingProcess();
                    point_temp1 = edit_order._point[0];
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(point_temp1).Name;
                    break;
                case MOVE_TYPE.LINEAR_MOVE:
                    MoveTypeBox.SelectedIndex = 1;
                    UpdateTeachingProcess();
                    point_temp1 = edit_order._point[0];
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(point_temp1).Name;
                    SpeedBox.Text = edit_order.speed.ToString();
                    break;
                case MOVE_TYPE.ARC_MOVE:
                    MoveTypeBox.SelectedIndex = 2;
                    UpdateTeachingProcess();
                    point_temp1 = edit_order._point[0];
                    point_temp2 = edit_order._point[1];
                    point_temp3 = edit_order._point[2];
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(point_temp1).Name;
                    Point2Box.Text = _pointManager.GetPoints().ElementAt(point_temp2).Name;
                    Point3Box.Text = _pointManager.GetPoints().ElementAt(point_temp3).Name;
                    SpeedBox.Text = edit_order.speed.ToString();
                    break;
                case MOVE_TYPE.SPLINE_MOVE:
                    MoveTypeBox.SelectedIndex = 4;
                    UpdateTeachingProcess();
                    SpeedBox.Text = edit_order.speed.ToString();
                    break;
            }    

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void B_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_b.IsRun() && m_motioncontrol_b.getPos() != 180)
                {
                    m_motioncontrol_b.GoPos(180);
                    m_motioncontrol_b.Start();
                }
            }
        }

        private void B_Up_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_b.IsRun())
                {
                    m_motioncontrol_b.Stop();
                }

            }
        }

        private void B_Down_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_b.IsRun() && m_motioncontrol_b.getPos() != -180)
                {
                    m_motioncontrol_b.GoPos(-180);
                    m_motioncontrol_b.Start();
                }

            }
        }

        private void B_Down_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_b.IsRun())
                {
                    m_motioncontrol_b.Stop();
                }

            }
        }

        private System.Timers.Timer _Time_Simulate;
        private List<Pr_Point> list_simu;
        private void Kinematic_Simulate()
        {
            _planner = new Planner_ring();
            list_simu = new List<Pr_Point>();
            foreach (var temp in _ProgramingData.orders)
            {
                if(temp.move_type== MOVE_TYPE.RAPID_MOVE)
                {
                    float count1 = list_simu.Count;
                    Pr_Point temp_point2 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    list_simu.AddRange(_kinematic.rtcp_segment(new vec3(temp_point2.x, temp_point2.y, temp_point2.z), temp_point2.a, temp_point2.b,temp.speed, _planner));
                }  
                if(temp.move_type==MOVE_TYPE.LINEAR_MOVE)
                {
                    Pr_Point temp_point2 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    float count1 = list_simu.Count;
                    list_simu.AddRange(_kinematic.rtcp_segment(new vec3(temp_point2.x, temp_point2.y, temp_point2.z), temp_point2.a, temp_point2.b, temp.speed, _planner));
                }
                else if (temp.move_type == MOVE_TYPE.ARC_MOVE)
                {
                    Pr_Point Pr_point1 = _pointManager.GetPoints().ElementAt(temp._point[0]);
                    Pr_Point Pr_point2 = _pointManager.GetPoints().ElementAt(temp._point[1]);
                    Pr_Point Pr_point3 = _pointManager.GetPoints().ElementAt(temp._point[2]);
                    List<Pr_Point> temp_point = MathHandle.CreateArc5Axis(Pr_point1, Pr_point2, Pr_point3, 5);
                    float count1 = list_simu.Count;
                    foreach (var _temp in temp_point)
                    {
                        list_simu.AddRange(_kinematic.rtcp_segment(new vec3(_temp.x, _temp.y, _temp.z), _temp.a, _temp.b, temp.speed, _planner));
                    }
                }
                else if (temp.move_type == MOVE_TYPE.SPLINE_MOVE)
                {
                    List<Pr_Point> list = new List<Pr_Point>();
                    foreach (var h in temp._point)
                    {
                        Pr_Point temp_point1 = _pointManager.GetPoints().ElementAt(h);
                        list.Add(temp_point1);
                    }
                    List<Pr_Point> SplinePoint = MathHandle.CreateCatmullRom5Axis(list, 3);
                    foreach (var _temp in SplinePoint)
                    {
                        list_simu.AddRange(_kinematic.rtcp_segment(new vec3(_temp.x, _temp.y, _temp.z), _temp.a, _temp.b, temp.speed, _planner));
                    }
                }    
                for(int i=1;i<list_simu.Count;i++)
                {
                    float dx = list_simu[i].x - list_simu[i - 1].x;
                    float dy = list_simu[i].y - list_simu[i - 1].y;
                    float dz = list_simu[i].z - list_simu[i - 1].z;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
                    //Debug.WriteLine($"Segment:{length}");
                }    
            }    
        }
        private void Run_Simulate_Click(object sender, EventArgs e)
        {
            simulate = true;
            Kinematic_Simulate();
            _teachingdrawer.Set_Simulate(true);
            _Time_Simulate = new System.Timers.Timer(20);
            _Time_Simulate.Elapsed += OnTimer;
            _Time_Simulate.AutoReset = true;
            _Time_Simulate.Start();
        }
        int simu_count = 0;
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            //simu_count++; 
            if(simu_count<list_simu.Count)
            {
                rtcp_target_block_t rtcp_sys = _kinematic.rtcp_forward(new vec3(list_simu[simu_count].x, list_simu[simu_count].y, list_simu[simu_count].z), list_simu[simu_count].a, 0);
                _Pivotdrawer.set_mcs(new vec3(rtcp_sys.XYZ.x, rtcp_sys.XYZ.y, rtcp_sys.XYZ.z + _kinematic.get_Pivot_Length()), list_simu[simu_count].a, 0);
                
                m_motioncontrol_x.setPos(list_simu[simu_count].x);
                m_motioncontrol_y.setPos(list_simu[simu_count].y);
                m_motioncontrol_z.setPos(list_simu[simu_count].z);
                m_motioncontrol_b.setPos(list_simu[simu_count].a);
                m_motioncontrol_c.setPos(list_simu[simu_count].b);
                CAngle = m_motioncontrol_c.getPos() - (int)(m_motioncontrol_c.getPos() / 360) * 360;
                panel1.Invalidate();
            }    
            else _Time_Simulate.Stop();
            simu_count++;
        }

        private void Off_Simulate_Click(object sender, EventArgs e)
        {

            simulate = false;
            _Time_Simulate.Stop();
            _teachingdrawer.Set_Simulate(false);
            _teachingdrawer.isUpdate = true;
            simu_count = 0;
            _teachingdrawer.Update_Count(simu_count);
            rtcp_target_block_t rtcp_sys = _kinematic.rtcp_forward(new vec3(list_simu[simu_count].x, list_simu[simu_count].y, list_simu[simu_count].z), list_simu[simu_count].a, 0);
            _Pivotdrawer.set_mcs(new vec3(rtcp_sys.XYZ.x, rtcp_sys.XYZ.y, rtcp_sys.XYZ.z), list_simu[simu_count].a, 0);
        }

        private void RTCP_Btn_Click(object sender, EventArgs e)
        {
            if (rtcp_mode == true) rtcp_mode = false;
            else rtcp_mode = true;

            if (rtcp_mode == true) RTCP_Btn.BackColor = Color.Green;
            else RTCP_Btn.BackColor = SystemColors.Control;

        }

        private Rectangle GetFitRectangle(
    System.Drawing.Size imageSize,
    System.Drawing.Size controlSize)
        {
            float scaleX = (float)controlSize.Width / imageSize.Width;
            float scaleY = (float)controlSize.Height / imageSize.Height;

            float scale = Math.Min(scaleX, scaleY);

            int width = (int)(imageSize.Width * scale);
            int height = (int)(imageSize.Height * scale);

            int x = (controlSize.Width - width) / 2;
            int y = (controlSize.Height - height) / 2;

            return new Rectangle(x, y, width, height);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            e.Graphics.Clear(VisualBox1.BackColor);

            if (ImageCAxis == null)
                return;

            int cx = this.panel1.Size.Width / 2;
            int cy = this.panel1.Size.Height / 2;

            Rectangle dest = GetFitRectangle(
                ImageCAxis.Size,
                this.panel1.Size);

            e.Graphics.DrawImage(ImageCAxis, dest);

            double rad = (CAngle - 90) * Math.PI / 180.0;

            int handLength = 22;
            float x = cx + (float)(Math.Cos(rad) * handLength);
            float y = cy + (float)(Math.Sin(rad) * handLength);


            using (Pen pen = new Pen(Color.Red, 3))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                g.DrawLine(
                    pen,
                    cx,
                    cy,
                    x,
                    y);
            }


        }

        private void C_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_c.IsRun() && m_motioncontrol_c.getPos() != 180)
                {
                    m_motioncontrol_c.GoPos(180);
                    m_motioncontrol_c.Start();
                }
            }
        }

        private void C_Up_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_c.IsRun())
                {
                    m_motioncontrol_c.Stop();
                }

            }
        }

        private void C_Down_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!m_motioncontrol_c.IsRun() && m_motioncontrol_c.getPos() != -180)
                {
                    m_motioncontrol_c.GoPos(-180);
                    m_motioncontrol_c.Start();
                }
            }
        }

        private void C_Down_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_motioncontrol_c.IsRun())
                {
                    m_motioncontrol_c.Stop();
                }

            }
        }
        float DEG2RAD = (float)(Math.PI / 180.0f);
        float RAD2DEG = (float)(180.0f / Math.PI);
        private void button6_Click(object sender, EventArgs e)
        {
            List<vec3> mcs_points = new List<vec3>();

            foreach (vec4 point in _pipepathdrawer.segment_points)
            {
                rtcp_target_block_t out_ = _kinematic.rtcp_forward(point.xyz, 90, point.w);
                mcs_points.Add(out_.XYZ);
                Debug.WriteLine($"C_Deg{point.w}");
            }
            _pipepathdrawer.UpdateMcs(mcs_points);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            m_xRot = 0;
            m_yRot = 0;
            _camera.UpdateRot(m_xRot, m_yRot);
        }

        private void Rear_Btn_Click(object sender, EventArgs e)
        {
            m_xRot = 0;
            m_yRot = 90;
            _camera.UpdateRot(m_xRot, m_yRot);
        }

        private void Back_Btn_Click(object sender, EventArgs e)
        {
            m_xRot = 90;
            m_yRot = 0;
            _camera.UpdateRot(m_xRot, m_yRot);
        }

        private void Calib_Btn_Click(object sender, EventArgs e)
        {
            this.CalibPanel.Visible = true;
        }

        private void Calib_Cancel_Click(object sender, EventArgs e)
        {
            this.CalibPanel.Visible = false;
        }

        private void Calib1_Click(object sender, EventArgs e)
        {
            Point1_Clb = new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_y.getPos(), m_motioncontrol_z.getPos());

        }

        private void Calib2_Click(object sender, EventArgs e)
        {
            Point2_Clb = new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_y.getPos(), m_motioncontrol_z.getPos());
        }

        private void Calib3_Click(object sender, EventArgs e)
        {
            Point3_Clb = new vec3(m_motioncontrol_x.getPos(), m_motioncontrol_y.getPos(), m_motioncontrol_z.getPos());
        }

        private void Caculate_Btn_Click(object sender, EventArgs e)
        {
            vec3 a = Point1_Clb - Point2_Clb;
            vec3 b = Point2_Clb - Point3_Clb;
            vec3 centervec = vec3.Cross(a, b);

            vec3 M = (Point1_Clb + Point2_Clb) * 0.5f;
            vec3 N = (Point2_Clb + Point3_Clb) * 0.5f;

            vec3 n = centervec.Normalized;
            vec3 dir12 = vec3.Cross(n, a);
            vec3 dir23 = vec3.Cross(n, b);

            if(!FindLineIntersection(M,dir12,N,dir23,out vec3 center))
            {
                throw new System.ArgumentException("3 điểm thẳng hàng hoặc không thể xác định tâm đường tròn.");
            }

            vec3 n1 = Point1_Clb - center;
            vec3 n2 = vec3.Cross(n, n1).Normalized;
            n1 = n1.Normalized;
            Baxismat = new mat4(new vec4(n1, 0.0f), new vec4(n, 0.0f), new vec4(n2, 0.0f), new vec4(center, 1.0f));
        }

        private static bool FindLineIntersection(vec3 p1, vec3 d1, vec3 p2, vec3 d2, out vec3 intersection)
        {
            intersection = vec3.Zero;

            vec3 w = p1 - p2;
            float a = vec3.Dot(d1, d1);
            float b = vec3.Dot(d1, d2);
            float c = vec3.Dot(d2, d2);
            float d = vec3.Dot(d1, w);
            float e = vec3.Dot(d2, w);

            float denominator = a * c - b * b;
            if (System.Math.Abs(denominator) < 1e-6f)
            {
                return false; // Hai đường thẳng song song hoặc trùng nhau
            }

            float s = (a * e - b * d) / denominator;
            intersection = p2 + s * d2;
            return true;
        }

        private void ProgramDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
    "Bạn có chắc chắn muốn xóa dòng này không?",
    "Xác nhận xóa",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning,
    MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                int index = ProgramDataGrid_View.CurrentRow.Index;

                if (index >= 0 && index < _ProgramingData.LineCount())
                {
                    _ProgramingData.Delete(index);
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                }
            }
        }
        private void ProgramInsert_Click(object sender, EventArgs e)
        {
            //Insert Data
            int rowIndex = ProgramDataGrid_View.CurrentRow.Index;

            using (var dlg = new InsertForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.Result == InsertMode.Front)
                    {
                        // Insert Front
                        rowIndex = rowIndex+1;
                    }
                    else if (dlg.Result == InsertMode.Rear)
                    {
                        // Insert Rear
                        rowIndex = rowIndex;
                    }
                }
                else return;
            }

            int idx = 0;
            int _speed = 0;
            MOVE_TYPE temp = MOVE_TYPE.RAPID_MOVE;
            switch (MoveTypeBox.SelectedIndex)
            {
                case 0:
                    temp = MOVE_TYPE.RAPID_MOVE;
                    List<int> _pointlist = new List<int>();
                    _pointlist.Add(point_temp1);
                    _ProgramingData.insert(rowIndex,new OrderLine(rowIndex, temp, _pointlist, _speed));
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    _pointManager.SavePoints();
                    _pointManager.CreatePoint();
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                    point_temp1 = idx - 1;
                    break;
                case 1:
                    temp = MOVE_TYPE.LINEAR_MOVE;
                    List<int> _pointlist1 = new List<int>();
                    _pointlist1.Add(point_temp1);
                    _speed = int.Parse(SpeedBox.Text);
                    _ProgramingData.insert(rowIndex, new OrderLine(rowIndex, temp, _pointlist1, _speed));
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    _pointManager.SavePoints();
                    _pointManager.CreatePoint();
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                    point_temp1 = idx - 1;
                    break;
                case 2:
                    temp = MOVE_TYPE.ARC_MOVE;
                    List<int> _pointlist2 = new List<int>();
                    _pointlist2.Add(point_temp1);
                    _pointlist2.Add(point_temp2);
                    _pointlist2.Add(point_temp3);
                    _speed = int.Parse(SpeedBox.Text);
                    _ProgramingData.insert(rowIndex, new OrderLine(rowIndex, temp, _pointlist2, _speed));
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    _pointManager.SavePoints();
                    _pointManager.CreatePoint();
                    _pointManager.CreatePoint();
                    _pointManager.CreatePoint();
                    idx = _pointManager.Count();
                    Point1Box.Text = _pointManager.GetPoints().ElementAt(idx - 3).Name;
                    Point2Box.Text = _pointManager.GetPoints().ElementAt(idx - 2).Name;
                    Point3Box.Text = _pointManager.GetPoints().ElementAt(idx - 1).Name;
                    point_temp1 = idx - 3;
                    point_temp2 = idx - 2;
                    point_temp3 = idx - 1;
                    break;
                case 3:
                    temp = MOVE_TYPE.CIRCLE_MOVE;
                    _speed = int.Parse(SpeedBox.Text);
                    //_ProgramingData.AddOrder(new OrderLine(_ProgramingData.GetIndex(), temp, _pointManager.Count() - 3, _pointManager.Count() - 2, _pointManager.Count()-1, _speed));
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    break;
                case 4:
                    temp = MOVE_TYPE.SPLINE_MOVE;
                    _speed = int.Parse(SpeedBox.Text);
                    _ProgramingData.insert(rowIndex, new OrderLine(rowIndex, temp, SplineIndex, _speed));
                    ProgramDataGrid_View.DataSource = _ProgramingData.Table_Binding(_pointManager);
                    break;
            }
        }// Insert Data
        private void VisualBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                m_xPan -= (float)((float)(e.Location.X- VisualBox1.Width / 2) + m_xPan) * (1 - 1 / ZOOMSTEP);
                m_yPan += (float)((float)(e.Location.Y- VisualBox1.Height / 2) + m_yPan) * (1 - 1 / ZOOMSTEP);
                m_zoom /= ZOOMSTEP;
             //   Debug.WriteLine("Delta x" + (e.Location.X - VisualBox1.Width / 2) + "Deltay"+ (e.Location.Y - VisualBox1.Height / 2));
            }
            else if (e.Delta < 0)
            {
                m_xPan -= (float)((float)(e.Location.X - VisualBox1.Width / 2) + m_xPan) * (1 - ZOOMSTEP);
                m_yPan += (float)((float)(e.Location.Y - VisualBox1.Height / 2) + m_yPan) * (1 - ZOOMSTEP);
                m_zoom *= ZOOMSTEP;

            }
          //  Debug.WriteLine("m_zoom" + m_zoom + "m_yRot" + m_yRot + "Width" + Height);
            _camera.UpdateRot(m_xRot, m_yRot);
            _camera.UpdatePan(m_xPan, m_yPan);
            _camera.UpdateZoom(m_zoom);
            _camera.updateProjection();
            _camera.updateView();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            m_zoom = 1;
            _pointManager = new PointManager();
            _ProgramingData = new ProgramingData();
            m_xRot = 0;
            m_yRot = 0;
            _camera.UpdateRot(m_xRot, m_yRot);
            MoveTypeBox.Items.AddRange(new object[] {
                "RapidMove",
                "LinearMove",
                "ArcMove",
                "CircleMove",
                "Spline"
            });
            MoveTypeBox.SelectedIndex = 0;
            TaskTypeBox.Items.AddRange(new object[] {
                "Speed-Curve",
                "Welding Process"
            });
            TaskTypeBox.SelectedIndex = 0;
            ImageCAxisName= Path.Combine(Application.StartupPath, "CAxis.JPG");
            ImageCAxis = new Bitmap(ImageCAxisName);
        }

        private void VisualBox1_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = VisualBox1.OpenGL;
            _camera = new MainCamera();
            _camera.Resize(VisualBox1.Width, VisualBox1.Height);
            _camera.fitDrawable(_heightmapborderdrawer);


            _gcodedrawer = new gcodedrawer(gl);
            _cursordrawer = new CursorDrawer(gl);
            _teachingdrawer = new TeachingDrawer(gl);
            _Pivotdrawer = new Pivot_drawer(gl);
            _Pivotdrawer.set_mcs(new vec3(0, 0, 0), 90, 0);
            _origin = new OriginDrawer(gl);
            _originBaxis = new OriginDrawer(gl);

            _heighgridmapdrawer = new HeightMapGridDrawer(gl);
            _heighgridmapdrawer.setborderRect(new Rect(0, 0, 500, 500));
            _heightmapborderdrawer = new Heightmapborderdrawer(gl);
            _heightmapborderdrawer.setBorderRect(new Rect(0, 0, 500, 500));
            _heightmapborderdrawer.SetHeight(300);

            _pipepathdrawer = new PipePathDrawer(gl);
            _pipepathdrawer.declare(50, 30, new vec3(0, 0, 0),45, 45, 90, 0);
            _objectdrawer = new ObjectDrawer(gl);
            _objectdrawer.Loadfile(@"C:\Users\Admin\source\repos\PlaningTest\PlaningTest\bin\Debug\truc.STL");

            _ubo = new CameraUBO(gl);
            _ubo1 = new CameraUBO(gl);
            _ubo2 = new CameraUBO(gl);
            _ubo3 = new CameraUBO(gl);
            _ubo4 = new CameraUBO(gl);
            _ubo5 = new CameraUBO(gl);
            _ubo6 = new CameraUBO(gl);
            _ubo7 = new CameraUBO(gl);
           

            sharder = new TubeSharder(gl, _ubo);
            sharger1 = new OriginSharder(gl, _ubo1);
            sharger2 = new OriginSharder(gl, _ubo2);
            sharder3 = new OriginSharder(gl, _ubo3);
            sharder4 = new OriginSharder(gl, _ubo4);
            sharder5 = new OriginSharder(gl, _ubo5);
            sharder6 = new OriginSharder(gl, _ubo6);
            sharder7 = new OriginSharder(gl, _ubo7);

            _gcodedrawer.updateGeometry(sharder);
            _cursordrawer.updateGeometry(sharder);
            _heighgridmapdrawer.updateGeometry(sharger1);
            _heightmapborderdrawer.updateGeometry(sharger2);
            _teachingdrawer.updateGeometry(sharder3);
            _Pivotdrawer.updateGeometry(sharder4);
            _pipepathdrawer.updateGeometry(sharder5);
            _origin.updateGeometry(sharder6);
            _objectdrawer.updateGeometry(sharder);
            _originBaxis.updateGeometry(sharder7);
        }

        #region Process
        //Process teaching
        private bool isPrograming = false;
        private bool isEditing = false;
        private void UpdateTeachingProcess()
        {
            if(!isPrograming)
            {
                SaveBtn.Visible = false;
                TaskTypeBox.Enabled = false;
                MoveTypeBox.Enabled = false;
                Point1Box.Enabled = false;
                Point2Box.Visible = false;
                Point2_Lb.Visible = false;
                Point3Box.Visible = false;
                Point3_Lb.Visible = false;
                CancelBtn.Visible = false;
                SpeedBox.Enabled = false;
                SpLinePoints.Visible = false;
                PrgBox.Enabled = true;
            }    
            else
            {
                string relative = fullName.Substring(Application.StartupPath.Length);

                if (relative.StartsWith("\\"))
                {
                    relative = relative.Substring(1);
                }
                PrgBox.Text = relative;
                PrgBox.Enabled = false;
                if (MoveTypeBox.SelectedIndex == 0)
                {
                    TaskTypeBox.Enabled = true;
                    MoveTypeBox.Enabled = true;
                    Point1Box.Enabled = true;
                    SpeedBox.Enabled = false;
                    Point1Box.Visible = true;
                    Point1_Lb.Visible = true;
                    Point2Box.Visible = false;
                    Point2_Lb.Visible = false;
                    Point3Box.Visible = false;
                    Point3_Lb.Visible = false;
                    SpLinePoints.Visible = false;
                }
                else if (MoveTypeBox.SelectedIndex == 1)
                {
                    TaskTypeBox.Enabled = true;
                    MoveTypeBox.Enabled = true;
                    Point1Box.Enabled = true;
                    SpeedBox.Enabled = true;
                    Point1Box.Visible = true;
                    Point1_Lb.Visible = true;
                    Point2Box.Visible = false;
                    Point2_Lb.Visible = false;
                    Point3Box.Visible = false;
                    Point3_Lb.Visible = false;
                    SpLinePoints.Visible = false;
                }
                else if (MoveTypeBox.SelectedIndex == 2)
                {
                    TaskTypeBox.Enabled = true;
                    MoveTypeBox.Enabled = true;
                    Point1Box.Enabled = true;
                    SpeedBox.Enabled = true;
                    Point1Box.Visible = true;
                    Point1_Lb.Visible = true;
                    Point2Box.Visible = true;
                    Point2_Lb.Visible = true;
                    Point3Box.Visible = true;
                    Point3_Lb.Visible = true;
                    SpLinePoints.Visible = false;
                }
                else if (MoveTypeBox.SelectedIndex == 4)
                {
                    SpLinePoints.Visible = true;
                    Point1Box.Visible = false;
                    Point1_Lb.Visible = false;
                    Point2Box.Visible = false;
                    Point2_Lb.Visible = false;
                    Point3Box.Visible = false;
                    Point3_Lb.Visible = false;
                    SpeedBox.Enabled = true;
                    SpeedBox.Visible = true;
                }
                if (isEditing)
                {
                    SaveBtn.Visible = false;
                    AddToBtn.Visible = false;
                    ModifyBtn.Visible = true;
                    PrgBox.Visible = true;
                    PrgBox.Enabled = true;
                    MoveTypeBox.Enabled = true;
                    SpeedBox.Enabled = true;
                    return;
                }
                AddToBtn.Visible = true;
                SaveBtn.Visible = true;
                CancelBtn.Visible = true;
                ModifyBtn.Visible = false;
   
            }    
        }
        #endregion


    }
}
