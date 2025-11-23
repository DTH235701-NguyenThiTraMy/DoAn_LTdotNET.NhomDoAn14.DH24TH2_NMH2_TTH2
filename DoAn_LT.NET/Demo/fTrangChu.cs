using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fTrangChu : Form
    {
        private Database db = new Database();
        private Timer timer = new Timer();
            
        public fTrangChu()
        {
            InitializeComponent();

            
            // Cập nhật thống kê khi mở form
            LoadStatistics();

            // Timer để cập nhật ngày giờ
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss | dd/MM/yyyy");
        }

        private void LoadStatistics()
        {
            // Giáo viên
            DataTable dtGV = db.ExecuteSelect("SELECT COUNT(*) AS SoLuong FROM GiaoVien");
            lblGV.Text = "Giáo viên: " + dtGV.Rows[0]["SoLuong"];

            // Lớp
            DataTable dtLop = db.ExecuteSelect("SELECT COUNT(*) AS SoLuong FROM Lop");
            lblLop.Text = "Lớp: " + dtLop.Rows[0]["SoLuong"];

            // Môn học
            DataTable dtMon = db.ExecuteSelect("SELECT COUNT(*) AS SoLuong FROM MonHoc");
            lblMon.Text = "Môn học: " + dtMon.Rows[0]["SoLuong"];
        }

        

        private void fTrangChu_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }
    }
}
