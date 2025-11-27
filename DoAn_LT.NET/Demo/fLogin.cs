using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fLogin : Form
    {
        Database db = new Database();
        public fLogin()
        {
            InitializeComponent();
            txtPass.PasswordChar = '●';
            this.AcceptButton = btnLogin;
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            try
            {
                string sql = @"SELECT Username, Role, MaGV FROM TaiKhoan 
                               WHERE Username=@u AND Password=@p";
                SqlParameter[] parameters = {
                    new SqlParameter("@u", username),
                    new SqlParameter("@p", password)
                };

                DataTable dt = db.ExecuteSelect(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    string role = dt.Rows[0]["Role"].ToString();
                    string maGV = dt.Rows[0]["MaGV"] == DBNull.Value ? "" : dt.Rows[0]["MaGV"].ToString();

                    fMain main = new fMain(username, role, maGV);
                    this.Show();
                    this.Hide();
                    main.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi");
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
