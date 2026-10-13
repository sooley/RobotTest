
namespace PlaningTest
{
    partial class InsertForm
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
            this.InsertBackBtn = new System.Windows.Forms.Button();
            this.InsertFrontBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InsertBackBtn
            // 
            this.InsertBackBtn.Location = new System.Drawing.Point(41, 57);
            this.InsertBackBtn.Name = "InsertBackBtn";
            this.InsertBackBtn.Size = new System.Drawing.Size(87, 41);
            this.InsertBackBtn.TabIndex = 0;
            this.InsertBackBtn.Text = "InsertBack";
            this.InsertBackBtn.UseVisualStyleBackColor = true;
            this.InsertBackBtn.Click += new System.EventHandler(this.InsertBackBtn_Click);
            // 
            // InsertFrontBtn
            // 
            this.InsertFrontBtn.Location = new System.Drawing.Point(157, 57);
            this.InsertFrontBtn.Name = "InsertFrontBtn";
            this.InsertFrontBtn.Size = new System.Drawing.Size(87, 41);
            this.InsertFrontBtn.TabIndex = 1;
            this.InsertFrontBtn.Text = "InsertFront";
            this.InsertFrontBtn.UseVisualStyleBackColor = true;
            this.InsertFrontBtn.Click += new System.EventHandler(this.InsertFrontBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(265, 57);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(87, 41);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // InsertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 142);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.InsertFrontBtn);
            this.Controls.Add(this.InsertBackBtn);
            this.Name = "InsertForm";
            this.Text = "InsertForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button InsertBackBtn;
        private System.Windows.Forms.Button InsertFrontBtn;
        private System.Windows.Forms.Button CancelBtn;
    }
}