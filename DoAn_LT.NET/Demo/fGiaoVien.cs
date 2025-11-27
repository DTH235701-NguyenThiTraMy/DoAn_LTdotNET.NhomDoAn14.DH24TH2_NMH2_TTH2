using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fGiaoVien : Form
    {
        Database db = new Database();
        private DataTable dtAll;
        public fGiaoVien()
        {
            InitializeComponent();
            LoadData();
        }
        // ==================== Load dữ liệu ====================
        private void LoadData(string keyword = "")
        {
            string sql = "SELECT MaGV, HoLot, Ten, GioiTinh, NgaySinh, SDT, DiaChi FROM GiaoVien";
            dtAll = db.ExecuteSelect(sql);

            dgvGiaoVien.AutoGenerateColumns = false;
            dgvGiaoVien.Columns.Clear();

            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã GV", DataPropertyName = "MaGV", Name = "MaGV" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Họ lót", DataPropertyName = "HoLot", Name = "HoLot" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Tên", DataPropertyName = "Ten", Name = "Ten" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Giới tính", DataPropertyName = "GioiTinh", Name = "GioiTinh" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Ngày sinh", DataPropertyName = "NgaySinh", Name = "NgaySinh" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "SĐT", DataPropertyName = "SDT", Name = "SDT" });
            dgvGiaoVien.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Địa chỉ", DataPropertyName = "DiaChi", Name = "DiaChi" });

            dgvGiaoVien.DataSource = dtAll;
            dgvGiaoVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGiaoVien.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void ClearInputs()
        {
            txtMaGV.Clear();
            txtHoLot.Clear();
            txtTen.Clear();
            rbNam.Checked = false;
            rbNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();
            txtMaGV.Focus();
        }
        // ===================== KIỂM TRA DỮ LIỆU ======================
        private bool ValidateInput()
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtMaGV.Text))
            {
                MessageBox.Show("Mã GV không được để trống!");
                txtMaGV.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtHoLot.Text))
            {
                MessageBox.Show("Họ lót không được để trống!");
                txtHoLot.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Tên không được để trống!");
                txtTen.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }
            // === RÀNG BUỘC MỚI: Mã GV phải bắt đầu bằng "GV" (không phân biệt hoa/thường), theo sau là chữ/số, tổng 4-10 ký tự ===
            string maGV = txtMaGV.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(maGV, @"^GV[A-Za-z0-9]{2,8}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Mã GV phải bắt đầu bằng \"GV\" !");
                txtMaGV.Focus();
                return false;
            }
            string hoLot = txtHoLot.Text.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(hoLot, @"\d"))
            {
                MessageBox.Show("Họ lót không được chứa số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoLot.Focus();
                return false;
            }
            string ten = txtTen.Text.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(ten, @"\d"))
            {
                MessageBox.Show("Tên không được chứa số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return false;
            }
            // Kiểm tra giới tính
            if (!rbNam.Checked && !rbNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                return false;
            }
            string sdt = txtSDT.Text.Trim();
            // Kiểm tra SDT (10-11 số)
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại phải bắt đầu bằng 0 và có đúng 10 số",
                    "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            // Kiểm tra ngày sinh
            if (dtpNgaySinh.Value.Date >= DateTime.Now.Date)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại.");
                dtpNgaySinh.Focus();
                return false;
            }

            return true; // tất cả hợp lệ
        }
        private void txtMaGV_TextChanged(object sender, EventArgs e)
        {
            if (txtMaGV.Text.Length > 2)
            {
                txtMaGV.Text = txtMaGV.Text.Substring(0, 2).ToUpper() + txtMaGV.Text.Substring(2);
                txtMaGV.SelectionStart = txtMaGV.Text.Length;
            }
            else
            {
                txtMaGV.Text = txtMaGV.Text.ToUpper();
                txtMaGV.SelectionStart = txtMaGV.Text.Length;
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            txtMaGV_TextChanged(sender, e);
            string sqlCheck = "SELECT * FROM GiaoVien WHERE MaGV=@MaGV";
            SqlParameter[] pCheck = { new SqlParameter("@MaGV", txtMaGV.Text.Trim()) };
            DataTable dt = db.ExecuteSelect(sqlCheck, pCheck);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Mã GV đã tồn tại!");
                return;
            }

            string gioiTinh = rbNam.Checked ? "Nam" : rbNu.Checked ? "Nữ" : "";
            string sql = @"INSERT INTO GiaoVien (MaGV, HoLot, Ten, GioiTinh, NgaySinh, SDT, DiaChi)
                           VALUES (@MaGV, @HoLot, @Ten, @GioiTinh, @NgaySinh, @SDT, @DiaChi)";
            SqlParameter[] pInsert = {
                new SqlParameter("@MaGV", txtMaGV.Text.Trim()),
                new SqlParameter("@HoLot", txtHoLot.Text.Trim()),
                new SqlParameter("@Ten", txtTen.Text.Trim()),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", dtpNgaySinh.Value),
                new SqlParameter("@SDT", txtSDT.Text.Trim()),
                new SqlParameter("@DiaChi", txtDiaChi.Text.Trim())
            };

            if (db.ExecuteNonQuery(sql, pInsert))
            {
                MessageBox.Show("Thêm thành công!");
                LoadData();
                ClearInputs();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string gioiTinh = rbNam.Checked ? "Nam" : rbNu.Checked ? "Nữ" : "";
            string sql = @"UPDATE GiaoVien SET HoLot=@HoLot, Ten=@Ten, GioiTinh=@GioiTinh,
                           NgaySinh=@NgaySinh, SDT=@SDT, DiaChi=@DiaChi WHERE MaGV=@MaGV";
            SqlParameter[] pUpdate = {
                new SqlParameter("@MaGV", txtMaGV.Text.Trim()),
                new SqlParameter("@HoLot", txtHoLot.Text.Trim()),
                new SqlParameter("@Ten", txtTen.Text.Trim()),
                new SqlParameter("@GioiTinh", gioiTinh),
                new SqlParameter("@NgaySinh", dtpNgaySinh.Value),
                new SqlParameter("@SDT", txtSDT.Text.Trim()),
                new SqlParameter("@DiaChi", txtDiaChi.Text.Trim())
            };

            if (db.ExecuteNonQuery(sql, pUpdate))
            {
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearInputs();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaGV.Text.Trim() == "")
            {
                MessageBox.Show("Chọn giáo viên cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            string sql = "DELETE FROM GiaoVien WHERE MaGV=@MaGV";
            SqlParameter[] pDelete = { new SqlParameter("@MaGV", txtMaGV.Text.Trim()) };

            if (db.ExecuteNonQuery(sql, pDelete))
            {
                MessageBox.Show("Xóa thành công!");
                LoadData();
                ClearInputs();
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvGiaoVien.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.FileName = "DanhSachGiaoVien.xlsx";


                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        DataTable dt = ((DataTable)dgvGiaoVien.DataSource).Copy();
                        wb.Worksheets.Add(dt, "GiaoVien");
                        wb.SaveAs(sfd.FileName);
                        MessageBox.Show("Xuất Excel thành công!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string key = txtTimKiem.Text.Trim();
            if (key == "")
            {
                dgvGiaoVien.DataSource = dtAll;
                return;
            }

            DataView dv = new DataView(dtAll);
            dv.RowFilter = $"MaGV LIKE '%{key}%' OR HoLot LIKE '%{key}%' OR Ten LIKE '%{key}%'";
            dgvGiaoVien.DataSource = dv;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        // ===================== CLICK DGV ======================
        private void dgvGiaoVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvGiaoVien.Rows[e.RowIndex];
                txtMaGV.Text = row.Cells["MaGV"].Value.ToString();
                txtHoLot.Text = row.Cells["HoLot"].Value.ToString();
                txtTen.Text = row.Cells["Ten"].Value.ToString();
                string gt = row.Cells["GioiTinh"].Value.ToString();
                rbNam.Checked = gt == "Nam";
                rbNu.Checked = gt == "Nữ";
                dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
            }
        }

        private void fGiaoVien_Load(object sender, EventArgs e)
        {

        }
    }
}
