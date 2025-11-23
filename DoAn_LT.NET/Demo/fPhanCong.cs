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
                // Giáo viên
                DataTable dtGV = db.ExecuteSelect("SELECT MaGV, (HoLot + ' ' + Ten) AS HoTen FROM GiaoVien");
                cmbGV.DataSource = dtGV;
                cmbGV.DisplayMember = "HoTen";
                cmbGV.ValueMember = "MaGV";
                cmbGV.SelectedIndex = -1;

                // Môn học
                DataTable dtMon = db.ExecuteSelect("SELECT MaMon, TenMon FROM MonHoc");
                cmbMon.DataSource = dtMon;
                cmbMon.DisplayMember = "TenMon";
                cmbMon.ValueMember = "MaMon";
                cmbMon.SelectedIndex = -1;

                // Lớp
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
                dtAll = db.ExecuteSelect("SELECT * FROM vw_PhanCong");

                dgvPhanCong.AutoGenerateColumns = false;
                dgvPhanCong.Columns.Clear();

                dgvPhanCong.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Mã PC", DataPropertyName = "MaPC", Name = "MaPC" });
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
            if (cmbGV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giáo viên!");
                cmbGV.Focus();
                return false;
            }

            if (cmbMon.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn môn học!");
                cmbMon.Focus();
                return false;
            }

            if (cmbLop.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                cmbLop.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                string sqlCheck = "SELECT * FROM PhanCong WHERE MaGV=@MaGV AND MaMon=@MaMon AND MaLop=@MaLop";
                SqlParameter[] pCheck = {
                    new SqlParameter("@MaGV", cmbGV.SelectedValue.ToString()),
                    new SqlParameter("@MaMon", cmbMon.SelectedValue.ToString()),
                    new SqlParameter("@MaLop", cmbLop.SelectedValue.ToString())
                };

                if (db.ExecuteSelect(sqlCheck, pCheck).Rows.Count > 0)
                {
                    MessageBox.Show("Phân công đã tồn tại!");
                    return;
                }

                string sql = "INSERT INTO PhanCong(MaGV, MaMon, MaLop) VALUES(@MaGV,@MaMon,@MaLop)";
                if (db.ExecuteNonQuery(sql, pCheck))
                {
                    MessageBox.Show("Thêm thành công!");
                    LoadData();
                    ClearText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm phân công: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPhanCong.CurrentRow == null) return;
            if (!ValidateInput()) return;

            try
            {
                int MaPC = Convert.ToInt32(dgvPhanCong.CurrentRow.Cells["MaPC"].Value);

                string sql = "UPDATE PhanCong SET MaGV=@MaGV, MaMon=@MaMon, MaLop=@MaLop WHERE MaPC=@MaPC";
                SqlParameter[] pUpdate = {
                    new SqlParameter("@MaGV", cmbGV.SelectedValue.ToString()),
                    new SqlParameter("@MaMon", cmbMon.SelectedValue.ToString()),
                    new SqlParameter("@MaLop", cmbLop.SelectedValue.ToString()),
                    new SqlParameter("@MaPC", MaPC)
                };

                if (db.ExecuteNonQuery(sql, pUpdate))
                {
                    MessageBox.Show("Cập nhật thành công!");
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

            DataView dv = new DataView(dtAll);
            dv.RowFilter = $"MaGV LIKE '%{key}%' OR HoTen LIKE '%{key}%'";
            dgvPhanCong.DataSource = dv;
        }

        // ===================== CELL CLICK ======================
        private void dgvPhanCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                cmbGV.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaGV"].Value.ToString();
                cmbMon.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaMon"].Value.ToString();
                cmbLop.SelectedValue = dgvPhanCong.Rows[e.RowIndex].Cells["MaLop"].Value.ToString();
            }
        }

        private void cmbMon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
