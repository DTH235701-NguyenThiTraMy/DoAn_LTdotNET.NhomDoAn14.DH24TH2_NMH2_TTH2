using System.Drawing;

namespace QLGV_THPT
{
    partial class fMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuTrangChu = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQLGV = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPhanCong = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTKB = new System.Windows.Forms.ToolStripMenuItem();
            this.hệThốngToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblUser = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTrangChu,
            this.quảnLýToolStripMenuItem,
            this.hệThốngToolStripMenuItem1,
            this.menuThoat});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1574, 44);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuTrangChu
            // 
            this.menuTrangChu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuTrangChu.Name = "menuTrangChu";
            this.menuTrangChu.Size = new System.Drawing.Size(160, 40);
            this.menuTrangChu.Text = "Trang chủ";
            this.menuTrangChu.Click += new System.EventHandler(this.menuTrangChu_Click);
            // 
            // quảnLýToolStripMenuItem
            // 
            this.quảnLýToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQLGV,
            this.menuLop,
            this.menuMon,
            this.menuPhanCong,
            this.menuTKB});
            this.quảnLýToolStripMenuItem.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quảnLýToolStripMenuItem.Name = "quảnLýToolStripMenuItem";
            this.quảnLýToolStripMenuItem.Size = new System.Drawing.Size(134, 40);
            this.quảnLýToolStripMenuItem.Text = "Quản lý";
            // 
            // menuQLGV
            // 
            this.menuQLGV.Name = "menuQLGV";
            this.menuQLGV.Size = new System.Drawing.Size(340, 44);
            this.menuQLGV.Text = "Giáo viên";
            this.menuQLGV.Click += new System.EventHandler(this.menuQLGV_Click);
            // 
            // menuLop
            // 
            this.menuLop.Name = "menuLop";
            this.menuLop.Size = new System.Drawing.Size(340, 44);
            this.menuLop.Text = "Lớp";
            this.menuLop.Click += new System.EventHandler(this.menuLop_Click);
            // 
            // menuMon
            // 
            this.menuMon.Name = "menuMon";
            this.menuMon.Size = new System.Drawing.Size(340, 44);
            this.menuMon.Text = "Môn";
            this.menuMon.Click += new System.EventHandler(this.menuMon_Click);
            // 
            // menuPhanCong
            // 
            this.menuPhanCong.Name = "menuPhanCong";
            this.menuPhanCong.Size = new System.Drawing.Size(340, 44);
            this.menuPhanCong.Text = "Phân công";
            this.menuPhanCong.Click += new System.EventHandler(this.menuPhanCong_Click);
            // 
            // menuTKB
            // 
            this.menuTKB.Name = "menuTKB";
            this.menuTKB.Size = new System.Drawing.Size(340, 44);
            this.menuTKB.Text = "Thời khóa biểu";
            this.menuTKB.Click += new System.EventHandler(this.menuTKB_Click);
            // 
            // hệThốngToolStripMenuItem1
            // 
            this.hệThốngToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogout});
            this.hệThốngToolStripMenuItem1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hệThốngToolStripMenuItem1.Name = "hệThốngToolStripMenuItem1";
            this.hệThốngToolStripMenuItem1.Size = new System.Drawing.Size(150, 40);
            this.hệThốngToolStripMenuItem1.Text = "Hệ thống";
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(277, 44);
            this.menuLogout.Text = "Đăng xuất";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // menuThoat
            // 
            this.menuThoat.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuThoat.Name = "menuThoat";
            this.menuThoat.Size = new System.Drawing.Size(108, 40);
            this.menuThoat.Text = "Thoát";
            this.menuThoat.Click += new System.EventHandler(this.menuThoat_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(1063, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(151, 36);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "| Xin chào:";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1574, 1029);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Times New Roman", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống quản lý giáo viên THPT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.fMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuTrangChu;
        private System.Windows.Forms.ToolStripMenuItem quảnLýToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuQLGV;
        private System.Windows.Forms.ToolStripMenuItem menuLop;
        private System.Windows.Forms.ToolStripMenuItem menuMon;
        private System.Windows.Forms.ToolStripMenuItem menuPhanCong;
        private System.Windows.Forms.ToolStripMenuItem menuTKB;
        private System.Windows.Forms.ToolStripMenuItem hệThốngToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ToolStripMenuItem menuThoat;
    }
}