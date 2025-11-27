namespace QLGV_THPT
{
    partial class fTKB_Edit
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.cbbGiaoVienMon = new System.Windows.Forms.ComboBox();
            this.lstThu = new System.Windows.Forms.ListBox();
            this.lstTiet = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 36);
            this.label2.TabIndex = 2;
            this.label2.Text = "Giáo viên -";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(976, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 36);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tiết:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(627, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 36);
            this.label4.TabIndex = 4;
            this.label4.Text = "Thứ:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 36);
            this.label5.TabIndex = 5;
            this.label5.Text = "Môn:";
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(314, 594);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(291, 92);
            this.btnLuu.TabIndex = 6;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(795, 585);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(291, 92);
            this.btnHuy.TabIndex = 14;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // cbbGiaoVienMon
            // 
            this.cbbGiaoVienMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbGiaoVienMon.FormattingEnabled = true;
            this.cbbGiaoVienMon.Location = new System.Drawing.Point(92, 128);
            this.cbbGiaoVienMon.Name = "cbbGiaoVienMon";
            this.cbbGiaoVienMon.Size = new System.Drawing.Size(381, 44);
            this.cbbGiaoVienMon.TabIndex = 15;
            this.cbbGiaoVienMon.SelectedIndexChanged += new System.EventHandler(this.cbbGiaoVienMon_SelectedIndexChanged);
            // 
            // lstThu
            // 
            this.lstThu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstThu.FormattingEnabled = true;
            this.lstThu.ItemHeight = 36;
            this.lstThu.Location = new System.Drawing.Point(633, 117);
            this.lstThu.Name = "lstThu";
            this.lstThu.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstThu.Size = new System.Drawing.Size(146, 436);
            this.lstThu.TabIndex = 16;
            // 
            // lstTiet
            // 
            this.lstTiet.FormattingEnabled = true;
            this.lstTiet.ItemHeight = 36;
            this.lstTiet.Location = new System.Drawing.Point(982, 117);
            this.lstTiet.Name = "lstTiet";
            this.lstTiet.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTiet.Size = new System.Drawing.Size(154, 436);
            this.lstTiet.TabIndex = 17;
            // 
            // fTKB_Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1375, 742);
            this.Controls.Add(this.lstTiet);
            this.Controls.Add(this.lstThu);
            this.Controls.Add(this.cbbGiaoVienMon);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "fTKB_Edit";
            this.Text = "fTKB_Edit";
            this.Load += new System.EventHandler(this.fTKB_Edit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.ComboBox cbbGiaoVienMon;
        private System.Windows.Forms.ListBox lstThu;
        private System.Windows.Forms.ListBox lstTiet;
    }
}