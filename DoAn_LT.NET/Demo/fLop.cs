using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QLGV_THPT
{
    
    public partial class fLop : Form
    {
        Database db = new Database();
        private DataTable dtAll;
        public fLop()
        {
            InitializeComponent();
            LoadData();
        }
        // ===================== LOAD DỮ LIỆU ======================
        private void LoadData()
        {
            string sql = "SELECT * FROM Lop";
            dtAll = db.ExecuteSelect(sql); // lưu dữ liệu gốc

            dgvLop.AutoGenerateColumns = false;
            dgvLop.Columns.Clear();

            dgvLop.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mã lớp",
                DataPropertyName = "MaLop",
                Name = "MaLop"
            });

            dgvLop.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Tên lớp",
                DataPropertyName = "TenLop",
                Name = "TenLop"
            });

            dgvLop.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Sĩ số",
                DataPropertyName = "SiSo",
                Name = "SiSo"
            });

            dgvLop.DataSource = dtAll;

            dgvLop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLop.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        
        // ===================== CLEAR TEXTBOX ======================
        private void ClearText()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtSiSo.Clear();
            txtMaLop.Focus();
        }

        // ===================== KIỂM TRA DỮ LIỆU ======================
        private bool ValidateInput()
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Mã lớp không được để trống!");
                txtMaLop.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                MessageBox.Show("Tên lớp không được để trống!");
                txtTenLop.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSiSo.Text))
            {
                MessageBox.Show("Sĩ số không được để trống!");
                txtSiSo.Focus();
                return false;
            }

            // Kiểm tra định dạng MaLop (chỉ chữ và số, 2-10 ký tự)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtMaLop.Text.Trim(), @"^[A-Za-z0-9]{2,10}$"))
            {
                MessageBox.Show("Mã lớp chỉ chứa chữ và số, từ 2 đến 10 ký tự.");
                txtMaLop.Focus();
                return false;
            }

            // Kiểm tra SiSo là số nguyên ≥ 0
            if (!int.TryParse(txtSiSo.Text.Trim(), out int siSo) || siSo < 0)
            {
                MessageBox.Show("Sĩ số phải là số nguyên lớn hơn hoặc bằng 0.");
                txtSiSo.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string sqlCheck = "SELECT * FROM Lop WHERE MaLop = @MaLop";
            SqlParameter[] pCheck = { new SqlParameter("@MaLop", txtMaLop.Text.Trim()) };
            DataTable dt = db.ExecuteSelect(sqlCheck, pCheck);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã lớp đã tồn tại!");
                return;
            }

            string sql = "INSERT INTO Lop (MaLop, TenLop, SiSo) VALUES (@MaLop, @TenLop, @SiSo)";
            SqlParameter[] pInsert = {
                new SqlParameter("@MaLop", txtMaLop.Text.Trim()),
                new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                new SqlParameter("@SiSo", int.Parse(txtSiSo.Text.Trim()))
            };

            if (db.ExecuteNonQuery(sql, pInsert))
            {
                MessageBox.Show("Thêm thành công!");
                LoadData();
                ClearText();
            }
            else
                MessageBox.Show("Thêm thất bại!");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string sql = "UPDATE Lop SET TenLop = @TenLop, SiSo = @SiSo WHERE MaLop = @MaLop";
            SqlParameter[] pUpdate = {
                new SqlParameter("@MaLop", txtMaLop.Text.Trim()),
                new SqlParameter("@TenLop", txtTenLop.Text.Trim()),
                new SqlParameter("@SiSo", int.Parse(txtSiSo.Text.Trim()))
            };

            if (db.ExecuteNonQuery(sql, pUpdate))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearText();
            }
            else
                MessageBox.Show("Cập nhật thất bại!");
        }

        private void fLop_Load(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMaLop.Text.Trim() == "")
            {
                MessageBox.Show("Chọn lớp cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            string sql = "DELETE FROM Lop WHERE MaLop = @MaLop";
            SqlParameter[] pDelete = { new SqlParameter("@MaLop", txtMaLop.Text.Trim()) };

            if (db.ExecuteNonQuery(sql, pDelete))
            {
                MessageBox.Show("Xóa thành công!");
                LoadData();
                ClearText();
            }
            else
                MessageBox.Show("Xóa thất bại!");
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtSiSo.Clear();
            txtSearch.Clear();
            txtMaLop.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim();

            if (key == "")
            {
                // Quay lại dữ liệu gốc
                dgvLop.DataSource = dtAll;
                return;
            }

            // Lọc dữ liệu từ dtAll (không query lại database)
            DataView dv = new DataView(dtAll);
            dv.RowFilter = $"MaLop LIKE '%{key}%' OR TenLop LIKE '%{key}%'";
            dgvLop.DataSource = dv;
        }
        // ===================== CLICK DGV ======================
        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtMaLop.Text = dgvLop.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
                    txtTenLop.Text = dgvLop.Rows[e.RowIndex].Cells["TenLop"].Value.ToString();
                    txtSiSo.Text = dgvLop.Rows[e.RowIndex].Cells["SiSo"].Value.ToString();
                }
            }
            catch { }
        }

        // ===================== CHỈ CHO NHẬP SỐ ======================
        private void txtSiSo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtSiSo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
