using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        // ===================== KIỂM TRA DỮ LIỆU ======================
        private bool ValidateInput()
        {
            if (txtMaMon.Text.Trim() == "")
            {
                MessageBox.Show("Mã môn không được để trống!");
                txtMaMon.Focus();
                return false;
            }

            if (txtTenMon.Text.Trim() == "")
            {
                MessageBox.Show("Tên môn không được để trống!");
                txtTenMon.Focus();
                return false;
            }

            return true;
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
    }
}
