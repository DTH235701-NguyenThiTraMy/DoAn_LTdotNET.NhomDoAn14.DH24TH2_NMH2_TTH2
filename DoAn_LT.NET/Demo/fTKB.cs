using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fTKB : Form
    {
        Database db = new Database();
        private string role;
        private string maGV;
        public fTKB(string maGV = null, string role = "admin")
        {
            InitializeComponent();
            this.role = role;

            this.role = role;
            this.maGV = maGV; // quan trọng cho user

            InitControls();
            InitSearch();
        }

        #region Init
        private void InitControls()
        {
            dgvTKB.ReadOnly = true;
            dgvTKB.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTKB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTKB.RowHeadersVisible = false;
            dgvTKB.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTKB.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            btnOK.Click += btnOK_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
        }

        private void InitSearch()
        {
            txtTim.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTim.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTim.KeyDown += TxtTim_KeyDown;

            LoadAutoComplete();
        }

        private void LoadAutoComplete()
        {
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            string query = "";

            if (role == "admin")
            {
                query = @"SELECT HoLot + ' ' + Ten AS TenGV FROM GiaoVien
                          UNION
                          SELECT TenLop FROM Lop";
                DataTable dt = db.ExecuteSelect(query);
                foreach (DataRow r in dt.Rows)
                    col.Add(r[0].ToString());
            }
            else
            {
                // chỉ giáo viên hiện tại
                query = "SELECT HoLot + ' ' + Ten FROM GiaoVien WHERE MaGV=@MaGV";
                SqlParameter[] param = { new SqlParameter("@MaGV", maGV) };
                DataTable dt = db.ExecuteSelect(query, param);
                foreach (DataRow r in dt.Rows)
                    col.Add(r[0].ToString());

                // tự động load TKB của giáo viên
                if (dt.Rows.Count > 0)
                {
                    txtTim.Text = dt.Rows[0][0].ToString();
                    LoadTKB_GridMatrix_ByGV(maGV);
                }
            }

            txtTim.AutoCompleteCustomSource = col;
        }
        #endregion

        #region Helper
        private bool ValidateTKBInput(string input, out string maGV, out string maLop)
        {
            maGV = null; maLop = null;
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Vui lòng nhập tên lớp hoặc giáo viên!");
                return false;
            }

            DataTable dtGV = db.ExecuteSelect("SELECT MaGV FROM GiaoVien WHERE HoLot + ' ' + Ten=@Ten",
                                             new SqlParameter[] { new SqlParameter("@Ten", input) });
            if (dtGV.Rows.Count > 1)
            {
                MessageBox.Show("Có nhiều giáo viên cùng tên, hãy chọn rõ ràng!");
                return false;
            }
            else if (dtGV.Rows.Count == 1)
                maGV = dtGV.Rows[0]["MaGV"].ToString();

            DataTable dtLop = db.ExecuteSelect("SELECT MaLop FROM Lop WHERE TenLop=@Ten",
                                               new SqlParameter[] { new SqlParameter("@Ten", input) });
            if (dtLop.Rows.Count > 1)
            {
                MessageBox.Show("Có nhiều lớp cùng tên, hãy chọn rõ ràng!");
                return false;
            }
            else if (dtLop.Rows.Count == 1)
                maLop = dtLop.Rows[0]["MaLop"].ToString();

            if (maGV == null && maLop == null)
            {
                MessageBox.Show("Không tìm thấy lớp hoặc giáo viên.");
                return false;
            }

            return true;
        }

        private void LoadTKB(string input)
        {
            if (!ValidateTKBInput(input, out string maGV, out string maLop)) return;

            if (!string.IsNullOrEmpty(maLop))
                LoadTKB_GridMatrix(maLop);
            else
                LoadTKB_GridMatrix_ByGV(maGV);
        }

        private (string maGV, string maLop) GetMaGVorLopFromInput()
        {
            string input = txtTim.Text.Trim();
            if (!ValidateTKBInput(input, out string maGV, out string maLop))
                return (null, null);
            return (maGV, maLop);
        }
        #endregion

        #region Load TKB
        private void LoadTKB_GridMatrix(string maLop)
        {
            if (string.IsNullOrEmpty(maLop)) return;

            DataTable dtMatrix = CreateMatrix();
            string query = @"SELECT Thu, Tiet, TenMon, HoLot + ' ' + Ten AS HoTenGV
                             FROM vw_TKB WHERE MaLop=@MaLop ORDER BY Thu, Tiet";
            DataTable tkb = db.ExecuteSelect(query, new SqlParameter[] { new SqlParameter("@MaLop", maLop) });
            FillMatrix(dtMatrix, tkb);
            dgvTKB.DataSource = dtMatrix;
        }

        private void LoadTKB_GridMatrix_ByGV(string maGV)
        {
            if (string.IsNullOrEmpty(maGV)) return;

            DataTable dtMatrix = CreateMatrix();
            string query = @"SELECT Thu, Tiet, TenMon, HoLot + ' ' + Ten AS HoTenGV
                             FROM vw_TKB WHERE MaGV=@MaGV ORDER BY Thu, Tiet";
            DataTable tkb = db.ExecuteSelect(query, new SqlParameter[] { new SqlParameter("@MaGV", maGV) });
            FillMatrix(dtMatrix, tkb);
            dgvTKB.DataSource = dtMatrix;
        }

        private DataTable CreateMatrix()
        {
            DataTable dtMatrix = new DataTable();
            dtMatrix.Columns.Add("Tiết");
            for (int thu = 2; thu <= 7; thu++)
                dtMatrix.Columns.Add("Thứ " + thu);
            for (int tiet = 1; tiet <= 10; tiet++)
            {
                DataRow row = dtMatrix.NewRow();
                row["Tiết"] = tiet;
                dtMatrix.Rows.Add(row);
            }
            return dtMatrix;
        }

        private void FillMatrix(DataTable dtMatrix, DataTable tkb)
        {
            foreach (DataRow r in tkb.Rows)
            {
                int thu = Convert.ToInt32(r["Thu"]);
                int tiet = Convert.ToInt32(r["Tiet"]);
                dtMatrix.Rows[tiet - 1]["Thứ " + thu] = $"{r["TenMon"]}\n{r["HoTenGV"]}";
            }
        }
        #endregion

        #region Events
        private void TxtTim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadTKB(txtTim.Text.Trim());
                e.Handled = true;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            LoadTKB(txtTim.Text.Trim());
        }
        #endregion

        #region Button Actions
        private void btnThem_Click(object sender, EventArgs e)
        {
            var (maGV, maLop) = GetMaGVorLopFromInput();
            if (maGV == null && maLop == null) return;

            fTKB_Edit editForm = !string.IsNullOrEmpty(maLop)
                ? new fTKB_Edit(maLop, null)
                : new fTKB_Edit(null, null, maGV);

            editForm.ShowDialog();
            if (!string.IsNullOrEmpty(maLop))
                LoadTKB_GridMatrix(maLop);
            else
                LoadTKB_GridMatrix_ByGV(maGV);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTKB.SelectedRows.Count == 0) return;

            int maTKB = Convert.ToInt32(dgvTKB.SelectedRows[0].Cells["MaTKB"].Value);
            var (maGV, maLop) = GetMaGVorLopFromInput();
            if (maGV == null && maLop == null) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa tiết học này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = "DELETE FROM ThoiKhoaBieu WHERE MaTKB = @MaTKB";
                SqlParameter[] parameters = { new SqlParameter("@MaTKB", maTKB) };
                if (db.ExecuteNonQuery(query, parameters))
                {
                    if (!string.IsNullOrEmpty(maLop))
                        LoadTKB_GridMatrix(maLop);
                    else
                        LoadTKB_GridMatrix_ByGV(maGV);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTKB.SelectedRows.Count == 0) return;

            int maTKB = Convert.ToInt32(dgvTKB.SelectedRows[0].Cells["MaTKB"].Value);
            var (maGV, maLop) = GetMaGVorLopFromInput();
            if (maGV == null && maLop == null) return;

            fTKB_Edit editForm = !string.IsNullOrEmpty(maLop)
                ? new fTKB_Edit(maLop, maTKB)
                : new fTKB_Edit(null, maTKB, maGV);

            editForm.ShowDialog();
            if (!string.IsNullOrEmpty(maLop))
                LoadTKB_GridMatrix(maLop);
            else
                LoadTKB_GridMatrix_ByGV(maGV);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            var (maGV, maLop) = GetMaGVorLopFromInput();
            if (maGV == null && maLop == null) return;

            if (!string.IsNullOrEmpty(maLop))
                LoadTKB_GridMatrix(maLop);
            else
                LoadTKB_GridMatrix_ByGV(maGV);
        }
        #endregion // Button Events        
    }
}