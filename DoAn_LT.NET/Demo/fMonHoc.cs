using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fMonHoc : Form
    {
        Database db = new Database();
        private DataTable dtAll;
        public fMonHoc()
        {
            InitializeComponent();
            LoadData();
        }
        // ===================== LOAD DỮ LIỆU ======================
        private void LoadData()
        {
            string sql = "SELECT * FROM MonHoc";
            dtAll = db.ExecuteSelect(sql); // lưu dữ liệu gốc

            dgvMonHoc.AutoGenerateColumns = false;
            dgvMonHoc.Columns.Clear();

            // Cột Mã môn
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mã môn",
                DataPropertyName = "MaMon",
                Name = "MaMon"
            });

            // Cột Tên môn
            dgvMonHoc.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Tên môn",
                DataPropertyName = "TenMon",
                Name = "TenMon"
            });

            dgvMonHoc.DataSource = dtAll;

            dgvMonHoc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMonHoc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        // ===================== CLEAR TEXTBOX ======================
        private void ClearText()
        {
            txtMaMon.Clear();
            txtTenMon.Clear();
            txtMaMon.Focus();
        }

        // ===================== KIỂM TRA DỮ LIỆU - MÔN HỌC ======================
        private bool ValidateInput()
        {
            string maMon = txtMaMon.Text.Trim();
            string tenMon = txtTenMon.Text.Trim();

            // 1. Kiểm tra không để trống
            if (string.IsNullOrWhiteSpace(maMon))
            {
                MessageBox.Show("Mã môn không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaMon.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(tenMon))
            {
                MessageBox.Show("Tên môn không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return false;
            }

            // 2. Ràng buộc Mã môn học
            // → Chỉ chấp nhận chữ cái in hoa và số
            // → Không khoảng trắng, không ký tự đặc biệt
            // → Độ dài từ 2 đến 8 ký tự
            // → Ví dụ hợp lệ: TO, HOA, LI, SINH, SU, DIA, GDCD, ANH12, VAN10
            if (!Regex.IsMatch(maMon, @"^[A-Z0-9]{2,8}$"))
            {
                MessageBox.Show(
                    "Mã môn không hợp lệ!\n\n" +
                    "• Chỉ được dùng chữ cái in hoa (A-Z) và số (0-9)\n" +
                    "• Không khoảng trắng, không ký tự đặc biệt\n" +
                    "• Độ dài từ 2 đến 8 ký tự\n\n" +
                    "Ví dụ đúng:\n" +
                    "   TO, HOA, LI, SINH, DIA, GDCD, ANH12, VAN10",
                    "Định dạng mã môn sai",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtMaMon.Focus();
                txtMaMon.SelectAll();
                return false;
            }

            // Tự động chuyển thành chữ hoa (đề phòng người dùng nhập thường)
            txtMaMon.Text = maMon.ToUpper();

            // 3. Ràng buộc Tên môn học
            // → Không được chứa số
            // → Không được chứa ký tự đặc biệt (chỉ chữ, khoảng trắng và dấu phẩy, dấu chấm nếu cần)
            if (Regex.IsMatch(tenMon, @"\d"))
            {
                MessageBox.Show("Tên môn học không được chứa số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return false;
            }

            // (Tùy chọn) Chỉ cho phép chữ cái, khoảng trắng, dấu gạch ngang, dấu ngoặc
            if (!Regex.IsMatch(tenMon, @"^[\p{L}\s\-().,]+$"))
            {
                MessageBox.Show("Tên môn chỉ được chứa chữ cái, khoảng trắng và một số dấu cơ bản (-().,)",
                    "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return false;
            }

            // 4. Gợi ý danh sách môn học chuẩn THPT (không bắt buộc, chỉ cảnh báo nếu sai chính tả phổ biến)
            string[] monChuan = {
                "Toán học", "Vật lí", "Hóa học", "Sinh học", "Ngữ văn", "Lịch sử", "Địa lí",
                "Giáo dục công dân", "Tin học", "Công nghệ", "Tiếng Anh", "Tiếng Pháp",
                "Tiếng Trung", "Tiếng Nhật", "Tiếng Nga", "Thể dục", "Giáo dục quốc phòng - An ninh",
                "Âm nhạc", "Mĩ thuật"
            };

            // (Tùy chọn) Nếu muốn cảnh báo nhẹ khi tên môn không nằm trong danh sách chuẩn
            // Bỏ comment 2 dòng dưới nếu muốn dùng tính năng này
            // if (!monChuan.Contains(tenMon, StringComparer.OrdinalIgnoreCase))
            //     MessageBox.Show("Cảnh báo: Tên môn có thể chưa đúng chuẩn chương trình THPT.", "Gợi ý", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true; // Tất cả hợp lệ
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string sqlCheck = $"SELECT * FROM MonHoc WHERE MaMon = '{txtMaMon.Text}'";
            DataTable dt = db.ExecuteSelect(sqlCheck);

            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã môn đã tồn tại!");
                return;
            }

            string sql = $"INSERT INTO MonHoc VALUES ('{txtMaMon.Text}', N'{txtTenMon.Text}')";

            if (db.ExecuteNonQuery(sql))
            {
                MessageBox.Show("Thêm thành công!");
                LoadData();
                ClearText();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string sql = $"UPDATE MonHoc SET TenMon = N'{txtTenMon.Text}' WHERE MaMon = '{txtMaMon.Text}'";

            if (db.ExecuteNonQuery(sql))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearText();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMaMon.Text.Trim() == "")
            {
                MessageBox.Show("Chọn môn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            string sql = $"DELETE FROM MonHoc WHERE MaMon = '{txtMaMon.Text}'";

            if (db.ExecuteNonQuery(sql))
            {
                MessageBox.Show("Xóa thành công!");
                LoadData();
                ClearText();
            }
            else
            {
                MessageBox.Show("Xóa thất bại!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaMon.Clear();
            txtTenMon.Clear();
            txtSearch.Clear();
            txtMaMon.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim();

            if (key == "")
            {
                dgvMonHoc.DataSource = dtAll;
                return;
            }

            DataView dv = new DataView(dtAll);
            dv.RowFilter = $"MaMon LIKE '%{key}%' OR TenMon LIKE '%{key}%'";
            dgvMonHoc.DataSource = dv;
        }
        // ===================== CLICK DGV ======================
        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtMaMon.Text = dgvMonHoc.Rows[e.RowIndex].Cells["MaMon"].Value.ToString();
                    txtTenMon.Text = dgvMonHoc.Rows[e.RowIndex].Cells["TenMon"].Value.ToString();
                }
            }
            catch { }
        }

        private void fMonHoc_Load(object sender, EventArgs e)
        {

        }
    }
}
