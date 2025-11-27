using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
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

        // ===================== KIỂM TRA DỮ LIỆU - LỚP HỌC THPT ======================
        private bool ValidateInput()
        {
            string maLop = txtMaLop.Text.Trim();
            string tenLopHienTai = txtTenLop.Text.Trim();
            string siSoText = txtSiSo.Text.Trim();

            // 1. Kiểm tra không để trống
            if (string.IsNullOrWhiteSpace(maLop))
            {
                MessageBox.Show("Mã lớp không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLop.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenLopHienTai))
            {
                MessageBox.Show("Tên lớp không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLop.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(siSoText))
            {
                MessageBox.Show("Sĩ số không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSiSo.Focus();
                return false;
            }

            // 2. Ràng buộc chính: Mã lớp phải đúng định dạng 10A1 → 12Z12
            if (!Regex.IsMatch(maLop, @"^(10|11|12)[A-Z]([1-9]|1[0-2])$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show(
                    "Mã lớp không hợp lệ!\n\n" +
                    "• Chỉ chấp nhận khối: 10, 11, 12\n" +
                    
                    "Ví dụ hợp lệ:\n" +
                    "   10A1, 10A12, 11B5, 12Z10, 12K12",
                    "Định dạng mã lớp sai",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtMaLop.Focus();
                txtMaLop.SelectAll();
                return false;
            }

            // 3. Chuẩn hóa mã lớp thành chữ hoa (10A1, 11B12,...)
            string maLopChuan = maLop.ToUpper(); // Đơn giản và chính xác nhất
            txtMaLop.Text = maLopChuan;

            // 4. TỰ ĐỘNG ĐIỀN TÊN LỚP = "Lớp 10A1" (nếu chưa có tên riêng)
            string tenLopTuDong = "Lớp " + maLopChuan;

            // Chỉ tự động điền nếu:
            // - Ô tên lớp đang trống
            // - Hoặc đang là tên mặc định cũ (do lần nhập trước)
            // - Hoặc người dùng chưa tự đặt tên riêng
            if (string.IsNullOrWhiteSpace(tenLopHienTai) ||
                tenLopHienTai.Equals("Lớp " + maLop, StringComparison.OrdinalIgnoreCase) ||
                tenLopHienTai.StartsWith("Lớp ") && tenLopHienTai.Length <= 10) // tránh ghi đè tên dài như "Chuyên Toán"
            {
                txtTenLop.Text = tenLopTuDong;
            }
            // Nếu người dùng đã đặt tên riêng (ví dụ: "Chuyên Anh", "Nâng cao") → giữ nguyên, không ghi đè

            // 5. Kiểm tra sĩ số: từ 20 đến 50
            if (!int.TryParse(siSoText, out int siSo))
            {
                MessageBox.Show("Sĩ số phải là một số nguyên!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSiSo.Focus();
                return false;
            }

            if (siSo < 20 || siSo > 50)
            {
                MessageBox.Show("Sĩ số lớp THPT phải từ 20 đến 50 học sinh!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSiSo.Focus();
                return false;
            }

            return true; // Tất cả hợp lệ
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
