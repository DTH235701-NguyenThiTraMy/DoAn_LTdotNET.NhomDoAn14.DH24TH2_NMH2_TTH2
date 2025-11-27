using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
            LoadSuKien();   
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
        private void LoadSuKien()
        {
            lstSuKien.Items.Clear();

            string filePath = "sukien.txt";

            // Nếu chưa có file thì tự tạo
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath,
                        @"Chưa có sự kiện nào
                Admin có thể thêm sự kiện");
            }

            // Đọc dữ liệu
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    lstSuKien.Items.Add("• " + line);
            }

            // Style nhìn đẹp hơn
            lstSuKien.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lstSuKien.BackColor = Color.White;
            lstSuKien.ForeColor = Color.Black;
        }



        private void fTrangChu_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
