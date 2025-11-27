using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fPhanCong : Form
    {
        Database db = new Database();
        private DataTable dtAll;
        public fPhanCong()
        {
            InitializeComponent();
            LoadCombobox();
            LoadData();
        }
        // ===================== LOAD COMBOBOX ======================
        private void LoadCombobox()
        {
            try
            {
                DataTable dtGV = db.ExecuteSelect("SELECT MaGV, (HoLot + ' ' + Ten) AS HoTen FROM GiaoVien");
                cmbGV.DataSource = dtGV;
                cmbGV.DisplayMember = "HoTen";
                cmbGV.ValueMember = "MaGV";
                cmbGV.SelectedIndex = -1;


                DataTable dtMon = db.ExecuteSelect("SELECT MaMon, TenMon FROM MonHoc");
                cmbMon.DataSource = dtMon;
                cmbMon.DisplayMember = "TenMon";
                cmbMon.ValueMember = "MaMon";
                cmbMon.SelectedIndex = -1;


                DataTable dtLop = db.ExecuteSelect("SELECT MaLop, TenLop FROM Lop");
                cmbLop.DataSource = dtLop;
                cmbLop.DisplayMember = "TenLop";
                cmbLop.ValueMember = "MaLop";
                cmbLop.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load combobox: " + ex.Message);
            }
        }

        // ===================== LOAD DATA ======================
        private void LoadData()
        {
            try
            {
                // Giả định vw_PhanCong có MaPC, MaGV, HoTen, MaMon, TenMon, MaLop, TenLop
                dtAll = db.ExecuteSelect("SELECT * FROM vw_PhanCong");


                dgvPhanCong.AutoGenerateColumns = false;
                dgvPhanCong.Columns.Clear();

                // Đã thêm: Visible = false để ẩn cột Mã PC
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã PC", DataPropertyName = "MaPC", Name = "MaPC", Visible = false });

                // Đã thêm: ReadOnly = true để ngăn người dùng sửa trực tiếp trên lưới (đảm bảo dữ liệu chỉ được sửa qua nút Edit)
                dgvPhanCong.ReadOnly = true;

                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã GV", DataPropertyName = "MaGV", Name = "MaGV" });
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Họ tên", DataPropertyName = "HoTen", Name = "HoTen" });
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã môn", DataPropertyName = "MaMon", Name = "MaMon" });
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Tên môn", DataPropertyName = "TenMon", Name = "TenMon" });
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã lớp", DataPropertyName = "MaLop", Name = "MaLop" });
                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Tên lớp", DataPropertyName = "TenLop", Name = "TenLop" });


                dgvPhanCong.DataSource = dtAll;
                dgvPhanCong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        // ===================== CLEAR ======================
        private void ClearText()
        {
            cmbGV.SelectedIndex = -1;
            cmbMon.SelectedIndex = -1;
            cmbLop.SelectedIndex = -1;
            txtSearch.Clear();
        }
        private bool ValidateInput()
        {
            // Kiểm tra SelectedValue có null không trước khi gọi ToString() 
            // và đảm bảo ComboBox đã được chọn (SelectedIndex >= 0)
            if (cmbGV.SelectedIndex == -1 || cmbGV.SelectedValue == null) { MessageBox.Show("Vui lòng chọn giáo viên!"); cmbGV.Focus(); return false; }
            if (cmbMon.SelectedIndex == -1 || cmbMon.SelectedValue == null) { MessageBox.Show("Vui lòng chọn môn học!"); cmbMon.Focus(); return false; }
            if (cmbLop.SelectedIndex == -1 || cmbLop.SelectedValue == null) { MessageBox.Show("Vui lòng chọn lớp!"); cmbLop.Focus(); return false; }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return; // kiểm tra chọn GV, Môn, Lớp

            string maGV = cmbGV.SelectedValue.ToString();
            string maMon = cmbMon.SelectedValue.ToString();
            string maLop = cmbLop.SelectedValue.ToString();

            // Kiểm tra giáo viên đã dạy môn khác chưa
            string sqlCheckGV = "SELECT DISTINCT MaMon FROM PhanCong WHERE MaGV=@MaGV";
            SqlParameter[] pCheckGV = { new SqlParameter("@MaGV", maGV) };
            DataTable dtMonDaDay = db.ExecuteSelect(sqlCheckGV, pCheckGV);

            if (dtMonDaDay.Rows.Count > 0)
            {
                bool daDayMonKhac = false;
                foreach (DataRow row in dtMonDaDay.Rows)
                {
                    string monDaDay = row["MaMon"].ToString();
                    if (monDaDay != maMon)
                    {
                        daDayMonKhac = true;
                        break;
                    }
                }

                if (daDayMonKhac)
                {
                    MessageBox.Show($"Giáo viên này đã được phân công dạy môn khác rồi. Không thể dạy thêm môn mới!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Kiểm tra phân công đã tồn tại chưa
            string sqlCheck = "SELECT MaPC FROM PhanCong WHERE MaGV=@MaGV AND MaMon=@MaMon AND MaLop=@MaLop";
            SqlParameter[] pCheck = {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@MaLop", maLop)
            };
            if (db.ExecuteSelect(sqlCheck, pCheck).Rows.Count > 0)
            {
                MessageBox.Show("Phân công đã tồn tại (Giáo viên đã dạy môn này ở lớp này)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm phân công mới
            string sqlInsert = "INSERT INTO PhanCong(MaGV, MaMon, MaLop) VALUES(@MaGV, @MaMon, @MaLop)";
            SqlParameter[] pInsert = {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@MaLop", maLop)
            };
            try
            {
                if (db.ExecuteNonQuery(sqlInsert, pInsert))
                {
                    MessageBox.Show("Thêm phân công thành công!");
                    LoadData();
                    ClearText();
                }
            }
            catch (SqlException sex)
            {
                if (sex.Number == 547 || sex.Number == 2627)
                {
                    MessageBox.Show($"Lỗi ràng buộc dữ liệu: Vui lòng kiểm tra lại MaGV, MaMon, MaLop hoặc Phân công đã tồn tại.\n\nThông báo chi tiết: {sex.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi SQL Server: " + sex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm phân công: " + ex.Message);
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPhanCong.CurrentRow == null || !ValidateInput()) return;

            try
            {
                int MaPC = Convert.ToInt32(dgvPhanCong.CurrentRow.Cells["MaPC"].Value);
                string maGV = cmbGV.SelectedValue.ToString();
                string maMon = cmbMon.SelectedValue.ToString();
                string maLop = cmbLop.SelectedValue.ToString();

                // 1. KIỂM TRA TRÙNG LẶP (Unique Constraint)
                // Kiểm tra xem tổ hợp (MaGV, MaMon, MaLop) đã tồn tại ở bản ghi KHÁC (MaPC <> @MaPC) hay chưa
                string sqlCheck = "SELECT MaPC FROM PhanCong WHERE MaGV=@MaGV AND MaMon=@MaMon AND MaLop=@MaLop AND MaPC <> @MaPC";

                SqlParameter[] pCheck = {
                    new SqlParameter("@MaGV", maGV),
                    new SqlParameter("@MaMon", maMon),
                    new SqlParameter("@MaLop", maLop),
                    new SqlParameter("@MaPC", MaPC)
                };

                // Phải sử dụng db.ExecuteSelect mới
                if (db.ExecuteSelect(sqlCheck, pCheck).Rows.Count > 0)
                {
                    MessageBox.Show("Phân công đã tồn tại (Giáo viên đã dạy môn này ở lớp này)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. THỰC HIỆN UPDATE
                string sqlUpdate = "UPDATE PhanCong SET MaGV=@MaGV, MaMon=@MaMon, MaLop=@MaLop WHERE MaPC=@MaPC";

                // Tạo mảng tham số mới cho câu lệnh UPDATE
                SqlParameter[] pUpdate = {
                    new SqlParameter("@MaGV", maGV),
                    new SqlParameter("@MaMon", maMon),
                    new SqlParameter("@MaLop", maLop),
                    new SqlParameter("@MaPC", MaPC)
                };

                if (db.ExecuteNonQuery(sqlUpdate, pUpdate))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPhanCong.CurrentRow == null) return;


            try
            {
                int MaPC = Convert.ToInt32(dgvPhanCong.CurrentRow.Cells["MaPC"].Value);
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sql = "DELETE FROM PhanCong WHERE MaPC=@MaPC";
                    SqlParameter[] pDelete = { new SqlParameter("@MaPC", MaPC) };


                    if (db.ExecuteNonQuery(sql, pDelete))
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadData();
                        ClearText();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {           
            ClearText();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim();

            if (key == "")
            {
                dgvPhanCong.DataSource = dtAll;
                return;
            }

            // Đã cải tiến: Mở rộng tìm kiếm sang HoTen, TenMon, TenLop
            DataView dv = new DataView(dtAll);
            dv.RowFilter = $@"
                MaGV LIKE '%{key}%' OR 
                HoTen LIKE '%{key}%' OR 
                TenMon LIKE '%{key}%' OR 
                TenLop LIKE '%{key}%'";
            dgvPhanCong.DataSource = dv;
        }

        // ===================== CELL CLICK ======================
        private void dgvPhanCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Kiểm tra SelectedValue có thể là null trước khi gán
                if (dgvPhanCong.Rows[e.RowIndex].Cells["MaGV"].Value != null)
                {
                    cmbGV.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaGV"].Value.ToString();
                }
                if (dgvPhanCong.Rows[e.RowIndex].Cells["MaMon"].Value != null)
                {
                    cmbMon.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaMon"].Value.ToString();
                }
                if (dgvPhanCong.Rows[e.RowIndex].Cells["MaLop"].Value != null)
                {
                    cmbLop.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
                }
            }
        }

        private void cmbMon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fPhanCong_Load(object sender, EventArgs e)
        {

        }
    }
}
