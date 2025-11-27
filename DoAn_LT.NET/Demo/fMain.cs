using System;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fMain : Form
    {
        private string username, role, maGV;
        public fMain(string user, string role, string maGV)
        {
            InitializeComponent();

            this.username = user;
            this.role = role;
            this.maGV = maGV;

            lblUser.Text = $"Xin chào: {username} ({role})";
            ApplyRole();

            OpenChild(new fTrangChu());
        }
        // Áp dụng phân quyền
        private void ApplyRole()
        {
            if (role == "user") // giáo viên
            {
                menuQLGV.Enabled = false;
                menuMon.Enabled = false;
                menuLop.Enabled = false;
                menuPhanCong.Enabled = false;
                menuTKB.Enabled = true; // chỉ xem TKB
            }
        }
        // Mở form con
        private void OpenChild(Form child)
        {
            // Đóng tất cả form con đang mở
            foreach (Form f in this.MdiChildren)
                f.Close();

            child.MdiParent = this;
            child.FormBorderStyle = FormBorderStyle.None; // bỏ viền
            child.Dock = DockStyle.Fill;                  // tự động vừa MDI parent
            child.Show();
        }             

        private void menuTrangChu_Click(object sender, EventArgs e)
        {
            OpenChild(new fTrangChu());
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fMain_Load(object sender, EventArgs e)
        {

        }

        private void menuLop_Click(object sender, EventArgs e)
        {
            OpenChild(new fLop());
        }

        private void menuQLGV_Click(object sender, EventArgs e)
        {
            OpenChild(new fGiaoVien());
        }

        private void menuMon_Click(object sender, EventArgs e)
        {
            OpenChild(new fMonHoc());
        }

        private void menuPhanCong_Click(object sender, EventArgs e)
        {
            OpenChild(new fPhanCong());
        }

        private void menuTKB_Click(object sender, EventArgs e)
        {
            OpenChild(new fTKB(this.maGV, this.role));
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            new fLogin().ShowDialog();
            this.Close();
        }
    }
}
