
namespace PlaningTest
{
    partial class PointsForm2
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
            this.dataGridView1_Points = new System.Windows.Forms.DataGridView();
            this.DetailBtn = new System.Windows.Forms.Button();
            this.ReferenceBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1_Points)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1_Points
            // 
            this.dataGridView1_Points.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1_Points.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView1_Points.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1_Points.Name = "dataGridView1_Points";
            this.dataGridView1_Points.RowHeadersWidth = 51;
            this.dataGridView1_Points.RowTemplate.Height = 24;
            this.dataGridView1_Points.Size = new System.Drawing.Size(503, 450);
            this.dataGridView1_Points.TabIndex = 0;
            // 
            // DetailBtn
            // 
            this.DetailBtn.Location = new System.Drawing.Point(588, 96);
            this.DetailBtn.Name = "DetailBtn";
            this.DetailBtn.Size = new System.Drawing.Size(94, 23);
            this.DetailBtn.TabIndex = 1;
            this.DetailBtn.Text = "Detail";
            this.DetailBtn.UseVisualStyleBackColor = true;
            this.DetailBtn.Click += new System.EventHandler(this.DetailBtn_Click);
            // 
            // ReferenceBtn
            // 
            this.ReferenceBtn.Location = new System.Drawing.Point(588, 52);
            this.ReferenceBtn.Name = "ReferenceBtn";
            this.ReferenceBtn.Size = new System.Drawing.Size(94, 23);
            this.ReferenceBtn.TabIndex = 2;
            this.ReferenceBtn.Text = "Reference";
            this.ReferenceBtn.UseVisualStyleBackColor = true;
            this.ReferenceBtn.Click += new System.EventHandler(this.ReferenceBtn_Click);
            // 
            // PointsForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ReferenceBtn);
            this.Controls.Add(this.DetailBtn);
            this.Controls.Add(this.dataGridView1_Points);
            this.Name = "PointsForm2";
            this.Text = "Quản lí điểm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1_Points)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1_Points;
        private System.Windows.Forms.Button DetailBtn;
        private System.Windows.Forms.Button ReferenceBtn;
    }
}