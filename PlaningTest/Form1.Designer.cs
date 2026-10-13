
namespace PlaningTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VisualBox1 = new SharpGL.OpenGLControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Calib_Btn = new System.Windows.Forms.Button();
            this.FileSelectBtn = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CalibPanel = new System.Windows.Forms.Panel();
            this.Caculate_Btn = new System.Windows.Forms.Button();
            this.Calib_Save = new System.Windows.Forms.Button();
            this.Calib_Cancel = new System.Windows.Forms.Button();
            this.Calib3 = new System.Windows.Forms.Button();
            this.Calib2 = new System.Windows.Forms.Button();
            this.Calib1 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.RTCP_Btn = new System.Windows.Forms.Button();
            this.Off_Simulate = new System.Windows.Forms.Button();
            this.Run_Simulate = new System.Windows.Forms.Button();
            this.C_Down = new System.Windows.Forms.Button();
            this.C_Up = new System.Windows.Forms.Button();
            this.B_Down = new System.Windows.Forms.Button();
            this.B_Up = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Down_Button = new System.Windows.Forms.Button();
            this.Up_Button = new System.Windows.Forms.Button();
            this.Right_button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.CursorPosTxt = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ModifyBtn = new System.Windows.Forms.Button();
            this.SpLinePoints = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.AddToBtn = new System.Windows.Forms.Button();
            this.Point3Box = new System.Windows.Forms.TextBox();
            this.Point3_Lb = new System.Windows.Forms.Label();
            this.Point2Box = new System.Windows.Forms.TextBox();
            this.Point2_Lb = new System.Windows.Forms.Label();
            this.Point1Box = new System.Windows.Forms.TextBox();
            this.Point1_Lb = new System.Windows.Forms.Label();
            this.PrgBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MoveTypeBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TaskTypeBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ProgramDataGrid_View = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Front_Btn = new System.Windows.Forms.Button();
            this.Back_Btn = new System.Windows.Forms.Button();
            this.Rear_Btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.VisualBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.CalibPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramDataGrid_View)).BeginInit();
            this.SuspendLayout();
            // 
            // VisualBox1
            // 
            this.VisualBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.VisualBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisualBox1.DrawFPS = false;
            this.VisualBox1.Location = new System.Drawing.Point(0, 0);
            this.VisualBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.VisualBox1.Name = "VisualBox1";
            this.VisualBox1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.VisualBox1.RenderContextType = SharpGL.RenderContextType.FBO;
            this.VisualBox1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.VisualBox1.Size = new System.Drawing.Size(1502, 616);
            this.VisualBox1.TabIndex = 0;
            this.VisualBox1.OpenGLInitialized += new System.EventHandler(this.VisualBox1_OpenGLInitialized);
            this.VisualBox1.OpenGLDraw += new SharpGL.RenderEventHandler(this.VisualBox1_OpenGLDraw);
            this.VisualBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VisualBox1_MouseDown);
            this.VisualBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VisualBox1_MouseMove);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Calib_Btn);
            this.groupBox3.Controls.Add(this.FileSelectBtn);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1227, 63);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // Calib_Btn
            // 
            this.Calib_Btn.Location = new System.Drawing.Point(392, 21);
            this.Calib_Btn.Name = "Calib_Btn";
            this.Calib_Btn.Size = new System.Drawing.Size(95, 35);
            this.Calib_Btn.TabIndex = 5;
            this.Calib_Btn.Text = "Calib";
            this.Calib_Btn.UseVisualStyleBackColor = true;
            this.Calib_Btn.Click += new System.EventHandler(this.Calib_Btn_Click);
            // 
            // FileSelectBtn
            // 
            this.FileSelectBtn.Location = new System.Drawing.Point(192, 21);
            this.FileSelectBtn.Name = "FileSelectBtn";
            this.FileSelectBtn.Size = new System.Drawing.Size(93, 35);
            this.FileSelectBtn.TabIndex = 4;
            this.FileSelectBtn.Text = "Program";
            this.FileSelectBtn.UseVisualStyleBackColor = true;
            this.FileSelectBtn.Click += new System.EventHandler(this.FileSelectBtn_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(291, 21);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 35);
            this.button4.TabIndex = 3;
            this.button4.Text = "Visual";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(50, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(136, 35);
            this.button5.TabIndex = 2;
            this.button5.Text = "Basic Program";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CalibPanel);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.RTCP_Btn);
            this.groupBox1.Controls.Add(this.Off_Simulate);
            this.groupBox1.Controls.Add(this.Run_Simulate);
            this.groupBox1.Controls.Add(this.C_Down);
            this.groupBox1.Controls.Add(this.C_Up);
            this.groupBox1.Controls.Add(this.B_Down);
            this.groupBox1.Controls.Add(this.B_Up);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.Down_Button);
            this.groupBox1.Controls.Add(this.Up_Button);
            this.groupBox1.Controls.Add(this.Right_button);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(1227, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 50, 3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 616);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // CalibPanel
            // 
            this.CalibPanel.BackColor = System.Drawing.SystemColors.Control;
            this.CalibPanel.Controls.Add(this.Caculate_Btn);
            this.CalibPanel.Controls.Add(this.Calib_Save);
            this.CalibPanel.Controls.Add(this.Calib_Cancel);
            this.CalibPanel.Controls.Add(this.Calib3);
            this.CalibPanel.Controls.Add(this.Calib2);
            this.CalibPanel.Controls.Add(this.Calib1);
            this.CalibPanel.Location = new System.Drawing.Point(5, 381);
            this.CalibPanel.Name = "CalibPanel";
            this.CalibPanel.Size = new System.Drawing.Size(263, 115);
            this.CalibPanel.TabIndex = 14;
            this.CalibPanel.Visible = false;
            // 
            // Caculate_Btn
            // 
            this.Caculate_Btn.Location = new System.Drawing.Point(8, 71);
            this.Caculate_Btn.Name = "Caculate_Btn";
            this.Caculate_Btn.Size = new System.Drawing.Size(75, 23);
            this.Caculate_Btn.TabIndex = 5;
            this.Caculate_Btn.Text = "Caculate";
            this.Caculate_Btn.UseVisualStyleBackColor = true;
            this.Caculate_Btn.Click += new System.EventHandler(this.Caculate_Btn_Click);
            // 
            // Calib_Save
            // 
            this.Calib_Save.Location = new System.Drawing.Point(88, 71);
            this.Calib_Save.Name = "Calib_Save";
            this.Calib_Save.Size = new System.Drawing.Size(75, 23);
            this.Calib_Save.TabIndex = 4;
            this.Calib_Save.Text = "Save";
            this.Calib_Save.UseVisualStyleBackColor = true;
            // 
            // Calib_Cancel
            // 
            this.Calib_Cancel.Location = new System.Drawing.Point(169, 71);
            this.Calib_Cancel.Name = "Calib_Cancel";
            this.Calib_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Calib_Cancel.TabIndex = 3;
            this.Calib_Cancel.Text = "Cancel";
            this.Calib_Cancel.UseVisualStyleBackColor = true;
            this.Calib_Cancel.Click += new System.EventHandler(this.Calib_Cancel_Click);
            // 
            // Calib3
            // 
            this.Calib3.Location = new System.Drawing.Point(169, 13);
            this.Calib3.Name = "Calib3";
            this.Calib3.Size = new System.Drawing.Size(75, 23);
            this.Calib3.TabIndex = 2;
            this.Calib3.Text = "Point3";
            this.Calib3.UseVisualStyleBackColor = true;
            this.Calib3.Click += new System.EventHandler(this.Calib3_Click);
            // 
            // Calib2
            // 
            this.Calib2.Location = new System.Drawing.Point(88, 13);
            this.Calib2.Name = "Calib2";
            this.Calib2.Size = new System.Drawing.Size(75, 23);
            this.Calib2.TabIndex = 1;
            this.Calib2.Text = "Point2";
            this.Calib2.UseVisualStyleBackColor = true;
            this.Calib2.Click += new System.EventHandler(this.Calib2_Click);
            // 
            // Calib1
            // 
            this.Calib1.Location = new System.Drawing.Point(7, 13);
            this.Calib1.Name = "Calib1";
            this.Calib1.Size = new System.Drawing.Size(75, 23);
            this.Calib1.TabIndex = 0;
            this.Calib1.Text = "Point1";
            this.Calib1.UseVisualStyleBackColor = true;
            this.Calib1.Click += new System.EventHandler(this.Calib1_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(6, 304);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(112, 42);
            this.button6.TabIndex = 13;
            this.button6.Text = "Test Path";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // RTCP_Btn
            // 
            this.RTCP_Btn.Location = new System.Drawing.Point(151, 304);
            this.RTCP_Btn.Name = "RTCP_Btn";
            this.RTCP_Btn.Size = new System.Drawing.Size(112, 42);
            this.RTCP_Btn.TabIndex = 12;
            this.RTCP_Btn.Text = "RTCP";
            this.RTCP_Btn.UseVisualStyleBackColor = true;
            this.RTCP_Btn.Click += new System.EventHandler(this.RTCP_Btn_Click);
            // 
            // Off_Simulate
            // 
            this.Off_Simulate.Location = new System.Drawing.Point(151, 244);
            this.Off_Simulate.Name = "Off_Simulate";
            this.Off_Simulate.Size = new System.Drawing.Size(112, 42);
            this.Off_Simulate.TabIndex = 11;
            this.Off_Simulate.Text = "Off_Simulate";
            this.Off_Simulate.UseVisualStyleBackColor = true;
            this.Off_Simulate.Click += new System.EventHandler(this.Off_Simulate_Click);
            // 
            // Run_Simulate
            // 
            this.Run_Simulate.Location = new System.Drawing.Point(6, 244);
            this.Run_Simulate.Name = "Run_Simulate";
            this.Run_Simulate.Size = new System.Drawing.Size(112, 42);
            this.Run_Simulate.TabIndex = 10;
            this.Run_Simulate.Text = "Run_Simulate";
            this.Run_Simulate.UseVisualStyleBackColor = true;
            this.Run_Simulate.Click += new System.EventHandler(this.Run_Simulate_Click);
            // 
            // C_Down
            // 
            this.C_Down.Location = new System.Drawing.Point(186, 169);
            this.C_Down.Name = "C_Down";
            this.C_Down.Size = new System.Drawing.Size(82, 42);
            this.C_Down.TabIndex = 9;
            this.C_Down.Text = "C-";
            this.C_Down.UseVisualStyleBackColor = true;
            this.C_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.C_Down_MouseDown);
            this.C_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.C_Down_MouseUp);
            // 
            // C_Up
            // 
            this.C_Up.Location = new System.Drawing.Point(6, 169);
            this.C_Up.Name = "C_Up";
            this.C_Up.Size = new System.Drawing.Size(82, 42);
            this.C_Up.TabIndex = 8;
            this.C_Up.Text = "C+";
            this.C_Up.UseVisualStyleBackColor = true;
            this.C_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.C_Up_MouseDown);
            this.C_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.C_Up_MouseUp);
            // 
            // B_Down
            // 
            this.B_Down.Location = new System.Drawing.Point(6, 121);
            this.B_Down.Name = "B_Down";
            this.B_Down.Size = new System.Drawing.Size(82, 42);
            this.B_Down.TabIndex = 7;
            this.B_Down.Text = "B-";
            this.B_Down.UseVisualStyleBackColor = true;
            this.B_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B_Down_MouseDown);
            this.B_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.B_Down_MouseUp);
            // 
            // B_Up
            // 
            this.B_Up.Location = new System.Drawing.Point(6, 22);
            this.B_Up.Name = "B_Up";
            this.B_Up.Size = new System.Drawing.Size(82, 42);
            this.B_Up.TabIndex = 6;
            this.B_Up.Text = "B+";
            this.B_Up.UseVisualStyleBackColor = true;
            this.B_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B_Up_MouseDown);
            this.B_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.B_Up_MouseUp);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(186, 121);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 42);
            this.button3.TabIndex = 5;
            this.button3.Text = "Z-";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button3_MouseDown);
            this.button3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button3_MouseUp);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(186, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 42);
            this.button2.TabIndex = 4;
            this.button2.Text = "Z+";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            this.button2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button2_MouseUp);
            // 
            // Down_Button
            // 
            this.Down_Button.Location = new System.Drawing.Point(96, 121);
            this.Down_Button.Name = "Down_Button";
            this.Down_Button.Size = new System.Drawing.Size(82, 42);
            this.Down_Button.TabIndex = 3;
            this.Down_Button.Text = "Y-";
            this.Down_Button.UseVisualStyleBackColor = true;
            this.Down_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_Button_MouseDown);
            this.Down_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Down_Button_MouseUp);
            // 
            // Up_Button
            // 
            this.Up_Button.Location = new System.Drawing.Point(96, 21);
            this.Up_Button.Name = "Up_Button";
            this.Up_Button.Size = new System.Drawing.Size(82, 42);
            this.Up_Button.TabIndex = 2;
            this.Up_Button.Text = "Y+";
            this.Up_Button.UseVisualStyleBackColor = true;
            this.Up_Button.Click += new System.EventHandler(this.Up_Button_Click);
            this.Up_Button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Up_Button_MouseDown);
            this.Up_Button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_Button_MouseUp);
            // 
            // Right_button
            // 
            this.Right_button.Location = new System.Drawing.Point(186, 70);
            this.Right_button.Name = "Right_button";
            this.Right_button.Size = new System.Drawing.Size(82, 42);
            this.Right_button.TabIndex = 1;
            this.Right_button.Text = "X+";
            this.Right_button.UseVisualStyleBackColor = true;
            this.Right_button.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Right_button_MouseDown);
            this.Right_button.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Right_button_MouseUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "X-";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Up_Button_Click);
            this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.button1_KeyDown);
            this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.button1_KeyPress);
            this.button1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.button1_KeyUp);
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button1_MouseDown);
            this.button1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button1_MouseUp);
            // 
            // CursorPosTxt
            // 
            this.CursorPosTxt.AutoSize = true;
            this.CursorPosTxt.Location = new System.Drawing.Point(12, 70);
            this.CursorPosTxt.Name = "CursorPosTxt";
            this.CursorPosTxt.Size = new System.Drawing.Size(46, 17);
            this.CursorPosTxt.TabIndex = 3;
            this.CursorPosTxt.Text = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.ModifyBtn);
            this.groupBox2.Controls.Add(this.SpLinePoints);
            this.groupBox2.Controls.Add(this.CancelBtn);
            this.groupBox2.Controls.Add(this.SaveBtn);
            this.groupBox2.Controls.Add(this.AddToBtn);
            this.groupBox2.Controls.Add(this.Point3Box);
            this.groupBox2.Controls.Add(this.Point3_Lb);
            this.groupBox2.Controls.Add(this.Point2Box);
            this.groupBox2.Controls.Add(this.Point2_Lb);
            this.groupBox2.Controls.Add(this.Point1Box);
            this.groupBox2.Controls.Add(this.Point1_Lb);
            this.groupBox2.Controls.Add(this.PrgBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.SpeedBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.MoveTypeBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TaskTypeBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 452);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1227, 164);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // ModifyBtn
            // 
            this.ModifyBtn.Location = new System.Drawing.Point(481, 135);
            this.ModifyBtn.Name = "ModifyBtn";
            this.ModifyBtn.Size = new System.Drawing.Size(75, 23);
            this.ModifyBtn.TabIndex = 18;
            this.ModifyBtn.Text = "Modify";
            this.ModifyBtn.UseVisualStyleBackColor = true;
            this.ModifyBtn.Click += new System.EventHandler(this.ModifyBtn_Click);
            // 
            // SpLinePoints
            // 
            this.SpLinePoints.Location = new System.Drawing.Point(424, 98);
            this.SpLinePoints.Name = "SpLinePoints";
            this.SpLinePoints.Size = new System.Drawing.Size(121, 23);
            this.SpLinePoints.TabIndex = 17;
            this.SpLinePoints.Text = "Add Points";
            this.SpLinePoints.UseVisualStyleBackColor = true;
            this.SpLinePoints.Click += new System.EventHandler(this.SpLinePoints_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(724, 135);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 16;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(643, 135);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 15;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // AddToBtn
            // 
            this.AddToBtn.Location = new System.Drawing.Point(562, 135);
            this.AddToBtn.Name = "AddToBtn";
            this.AddToBtn.Size = new System.Drawing.Size(75, 23);
            this.AddToBtn.TabIndex = 14;
            this.AddToBtn.Text = "Add To";
            this.AddToBtn.UseVisualStyleBackColor = true;
            this.AddToBtn.Click += new System.EventHandler(this.AddToBtn_Click);
            // 
            // Point3Box
            // 
            this.Point3Box.Location = new System.Drawing.Point(891, 99);
            this.Point3Box.Name = "Point3Box";
            this.Point3Box.Size = new System.Drawing.Size(121, 22);
            this.Point3Box.TabIndex = 13;
            this.Point3Box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Point3Box_MouseDown);
            // 
            // Point3_Lb
            // 
            this.Point3_Lb.AutoSize = true;
            this.Point3_Lb.Location = new System.Drawing.Point(807, 102);
            this.Point3_Lb.Name = "Point3_Lb";
            this.Point3_Lb.Size = new System.Drawing.Size(65, 17);
            this.Point3_Lb.TabIndex = 12;
            this.Point3_Lb.Text = "EndPoint";
            // 
            // Point2Box
            // 
            this.Point2Box.Location = new System.Drawing.Point(659, 99);
            this.Point2Box.Name = "Point2Box";
            this.Point2Box.Size = new System.Drawing.Size(121, 22);
            this.Point2Box.TabIndex = 11;
            this.Point2Box.TextChanged += new System.EventHandler(this.Point2Box_TextChanged);
            this.Point2Box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Point2Box_MouseDown);
            // 
            // Point2_Lb
            // 
            this.Point2_Lb.AutoSize = true;
            this.Point2_Lb.Location = new System.Drawing.Point(568, 102);
            this.Point2_Lb.Name = "Point2_Lb";
            this.Point2_Lb.Size = new System.Drawing.Size(85, 17);
            this.Point2_Lb.TabIndex = 10;
            this.Point2_Lb.Text = "Middle Point";
            // 
            // Point1Box
            // 
            this.Point1Box.Location = new System.Drawing.Point(424, 99);
            this.Point1Box.Name = "Point1Box";
            this.Point1Box.Size = new System.Drawing.Size(121, 22);
            this.Point1Box.TabIndex = 9;
            this.Point1Box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Point1Box_MouseDown);
            // 
            // Point1_Lb
            // 
            this.Point1_Lb.AutoSize = true;
            this.Point1_Lb.Location = new System.Drawing.Point(348, 99);
            this.Point1_Lb.Name = "Point1_Lb";
            this.Point1_Lb.Size = new System.Drawing.Size(78, 17);
            this.Point1_Lb.TabIndex = 8;
            this.Point1_Lb.Text = "RuntoPoint";
            // 
            // PrgBox
            // 
            this.PrgBox.Location = new System.Drawing.Point(424, 20);
            this.PrgBox.Name = "PrgBox";
            this.PrgBox.Size = new System.Drawing.Size(121, 22);
            this.PrgBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(348, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Prg Name";
            // 
            // SpeedBox
            // 
            this.SpeedBox.Location = new System.Drawing.Point(659, 57);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(121, 22);
            this.SpeedBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(594, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Speed";
            // 
            // MoveTypeBox
            // 
            this.MoveTypeBox.FormattingEnabled = true;
            this.MoveTypeBox.Location = new System.Drawing.Point(424, 57);
            this.MoveTypeBox.Name = "MoveTypeBox";
            this.MoveTypeBox.Size = new System.Drawing.Size(121, 24);
            this.MoveTypeBox.TabIndex = 3;
            this.MoveTypeBox.SelectedIndexChanged += new System.EventHandler(this.MoveTypeBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(348, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "MoveType";
            // 
            // TaskTypeBox
            // 
            this.TaskTypeBox.FormattingEnabled = true;
            this.TaskTypeBox.Location = new System.Drawing.Point(659, 20);
            this.TaskTypeBox.Name = "TaskTypeBox";
            this.TaskTypeBox.Size = new System.Drawing.Size(121, 24);
            this.TaskTypeBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(578, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Task Type";
            // 
            // ProgramDataGrid_View
            // 
            this.ProgramDataGrid_View.BackgroundColor = System.Drawing.Color.Silver;
            this.ProgramDataGrid_View.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProgramDataGrid_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgramDataGrid_View.Location = new System.Drawing.Point(0, 63);
            this.ProgramDataGrid_View.Name = "ProgramDataGrid_View";
            this.ProgramDataGrid_View.RowHeadersWidth = 51;
            this.ProgramDataGrid_View.RowTemplate.Height = 24;
            this.ProgramDataGrid_View.Size = new System.Drawing.Size(1227, 389);
            this.ProgramDataGrid_View.TabIndex = 7;
            this.ProgramDataGrid_View.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 80);
            this.panel1.TabIndex = 8;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // Front_Btn
            // 
            this.Front_Btn.Location = new System.Drawing.Point(1177, 62);
            this.Front_Btn.Name = "Front_Btn";
            this.Front_Btn.Size = new System.Drawing.Size(50, 50);
            this.Front_Btn.TabIndex = 0;
            this.Front_Btn.Text = "Front";
            this.Front_Btn.UseVisualStyleBackColor = true;
            this.Front_Btn.Click += new System.EventHandler(this.button7_Click);
            // 
            // Back_Btn
            // 
            this.Back_Btn.Location = new System.Drawing.Point(1121, 62);
            this.Back_Btn.Name = "Back_Btn";
            this.Back_Btn.Size = new System.Drawing.Size(50, 50);
            this.Back_Btn.TabIndex = 9;
            this.Back_Btn.Text = "Back";
            this.Back_Btn.UseVisualStyleBackColor = true;
            this.Back_Btn.Click += new System.EventHandler(this.Back_Btn_Click);
            // 
            // Rear_Btn
            // 
            this.Rear_Btn.Location = new System.Drawing.Point(1065, 62);
            this.Rear_Btn.Name = "Rear_Btn";
            this.Rear_Btn.Size = new System.Drawing.Size(50, 50);
            this.Rear_Btn.TabIndex = 10;
            this.Rear_Btn.Text = "Rear";
            this.Rear_Btn.UseVisualStyleBackColor = true;
            this.Rear_Btn.Click += new System.EventHandler(this.Rear_Btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1502, 616);
            this.Controls.Add(this.Rear_Btn);
            this.Controls.Add(this.Back_Btn);
            this.Controls.Add(this.Front_Btn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ProgramDataGrid_View);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CursorPosTxt);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.VisualBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VisualBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.CalibPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProgramDataGrid_View)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl VisualBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Down_Button;
        private System.Windows.Forms.Button Up_Button;
        private System.Windows.Forms.Button Right_button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label CursorPosTxt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox SpeedBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox MoveTypeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox TaskTypeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PrgBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Point1Box;
        private System.Windows.Forms.Label Point1_Lb;
        private System.Windows.Forms.TextBox Point3Box;
        private System.Windows.Forms.Label Point3_Lb;
        private System.Windows.Forms.TextBox Point2Box;
        private System.Windows.Forms.Label Point2_Lb;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button AddToBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView ProgramDataGrid_View;
        private System.Windows.Forms.Button FileSelectBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SpLinePoints;
        private System.Windows.Forms.Button ModifyBtn;
        private System.Windows.Forms.Button C_Down;
        private System.Windows.Forms.Button C_Up;
        private System.Windows.Forms.Button B_Down;
        private System.Windows.Forms.Button B_Up;
        private System.Windows.Forms.Button Run_Simulate;
        private System.Windows.Forms.Button Off_Simulate;
        private System.Windows.Forms.Button RTCP_Btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button Front_Btn;
        private System.Windows.Forms.Button Back_Btn;
        private System.Windows.Forms.Button Rear_Btn;
        private System.Windows.Forms.Panel CalibPanel;
        private System.Windows.Forms.Button Calib_Btn;
        private System.Windows.Forms.Button Calib_Save;
        private System.Windows.Forms.Button Calib_Cancel;
        private System.Windows.Forms.Button Calib3;
        private System.Windows.Forms.Button Calib2;
        private System.Windows.Forms.Button Calib1;
        private System.Windows.Forms.Button Caculate_Btn;
    }
}

