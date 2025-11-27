namespace QLGV_THPT
{
    partial class fTrangChu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fTrangChu));
            this.lblTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMon = new System.Windows.Forms.Label();
            this.lblLop = new System.Windows.Forms.Label();
            this.lblGV = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpSuKien = new System.Windows.Forms.GroupBox();
            this.lstSuKien = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpSuKien.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTime.Font = new System.Drawing.Font("Times New Roman", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.LightSalmon;
            this.lblTime.Location = new System.Drawing.Point(58, 70);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(191, 49);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Ngày giờ:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 28.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(-38, 482);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1638, 92);
            this.label1.TabIndex = 3;
            this.label1.Text = "HỆ THỐNG QUẢN LÝ GIÁO VIÊN THPT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblMon);
            this.groupBox1.Controls.Add(this.lblLop);
            this.groupBox1.Controls.Add(this.lblGV);
            this.groupBox1.Font = new System.Drawing.Font("Times New Roman", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FloralWhite;
            this.groupBox1.Location = new System.Drawing.Point(67, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 258);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thống kê nhanh";
            // 
            // lblMon
            // 
            this.lblMon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMon.AutoSize = true;
            this.lblMon.Location = new System.Drawing.Point(123, 126);
            this.lblMon.Name = "lblMon";
            this.lblMon.Size = new System.Drawing.Size(186, 49);
            this.lblMon.TabIndex = 2;
            this.lblMon.Text = "Môn học:";
            // 
            // lblLop
            // 
            this.lblLop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLop.AutoSize = true;
            this.lblLop.Location = new System.Drawing.Point(205, 197);
            this.lblLop.Name = "lblLop";
            this.lblLop.Size = new System.Drawing.Size(105, 49);
            this.lblLop.TabIndex = 1;
            this.lblLop.Text = "Lớp:";
            // 
            // lblGV
            // 
            this.lblGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblGV.AutoSize = true;
            this.lblGV.Location = new System.Drawing.Point(109, 62);
            this.lblGV.Name = "lblGV";
            this.lblGV.Size = new System.Drawing.Size(201, 49);
            this.lblGV.TabIndex = 0;
            this.lblGV.Text = "Giáo viên:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.grpSuKien);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(72, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1450, 850);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // grpSuKien
            // 
            this.grpSuKien.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpSuKien.BackColor = System.Drawing.Color.Transparent;
            this.grpSuKien.Controls.Add(this.lstSuKien);
            this.grpSuKien.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.grpSuKien.ForeColor = System.Drawing.Color.White;
            this.grpSuKien.Location = new System.Drawing.Point(45, 539);
            this.grpSuKien.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.grpSuKien.Name = "grpSuKien";
            this.grpSuKien.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.grpSuKien.Size = new System.Drawing.Size(1400, 238);
            this.grpSuKien.TabIndex = 5;
            this.grpSuKien.TabStop = false;
            this.grpSuKien.Text = "THÔNG BÁO - SỰ KIỆN";
            // 
            // lstSuKien
            // 
            this.lstSuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSuKien.FormattingEnabled = true;
            this.lstSuKien.ItemHeight = 31;
            this.lstSuKien.Location = new System.Drawing.Point(5, 37);
            this.lstSuKien.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.lstSuKien.Name = "lstSuKien";
            this.lstSuKien.Size = new System.Drawing.Size(1390, 196);
            this.lstSuKien.TabIndex = 0;
            // 
            // fTrangChu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1574, 930);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "fTrangChu";
            this.Text = "fTrangChu";
            this.Load += new System.EventHandler(this.fTrangChu_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpSuKien.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblMon;
        private System.Windows.Forms.Label lblLop;
        private System.Windows.Forms.Label lblGV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grpSuKien;
        private System.Windows.Forms.ListBox lstSuKien;
    }
}