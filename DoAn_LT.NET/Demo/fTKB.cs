using ClosedXML.Excel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fTKB : Form
    {
        private readonly Database db = new Database();
        private readonly string role;         // "admin" hoặc "user"
        private readonly string maGVLogin;    // MaGV đang đăng nhập (null nếu admin)

        private string currentMaLop;
        private string currentMaGV;

        public fTKB(string maGV = null, string role = "admin")
        {
            InitializeComponent();
            this.role = role;
            this.maGVLogin = maGV;

            KhoiTaoForm();
            ApDungQuyenGiaoVien();
                        
        }
        private void KhoiTaoForm()
        {
            dgvTKB.ReadOnly = true;
            dgvTKB.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvTKB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTKB.RowHeadersVisible = false;

            dgvTKB.CellPainting += dgvTKB_CellPainting;

            //dgvTKB.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvTKB.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            dgvTKB.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTKB.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvTKB.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTKB.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTKB.RowTemplate.Height = 60;

            dgvTKB.AllowUserToAddRows = false;

            // Gắn sự kiện
            btnOK.Click += (s, e) => TimKiem();
            btnThem.Click += (s, e) => ThemTietHoc();
            btnSua.Click += (s, e) => SuaTietHoc();
            btnXoa.Click += (s, e) => XoaTietHoc();
            btnLamMoi.Click += (s, e) => TaiLaiDuLieu();
            btnXuatExcel.Click += (s, e) => XuatExcel(dgvTKB);
            txtTim.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { TimKiem(); e.SuppressKeyPress = true; } };
        }

        //Tên môn in đậm
        private void dgvTKB_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Chỉ xử lý các cột Thứ
            if (!dgvTKB.Columns[e.ColumnIndex].HeaderText.StartsWith("Thứ")) return;

            string text = e.FormattedValue?.ToString();
            if (string.IsNullOrWhiteSpace(text)) return;

            // Tách dữ liệu theo xuống dòng (xử lý cho cả \n và \r\n)
            string[] parts = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (parts.Length < 2) return;

            e.PaintBackground(e.CellBounds, true);

            string mon = parts[0];
            string gv = parts[1];

            using (Font monFont = new Font("Segoe UI", 11, FontStyle.Bold))
            using (Font gvFont = new Font("Segoe UI", 10, FontStyle.Regular))
            {
                StringFormat format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };

                Rectangle rect = e.CellBounds;

                // Vẽ tên môn (đậm)
                e.Graphics.DrawString(mon, monFont, Brushes.Black,
                    new Rectangle(rect.X, rect.Y + 5, rect.Width, rect.Height / 2),
                    format);

                // Vẽ tên giáo viên (không đậm)
                format.LineAlignment = StringAlignment.Far;
                e.Graphics.DrawString(gv, gvFont, Brushes.Black,
                    new Rectangle(rect.X, rect.Y, rect.Width, rect.Height - 5),
                    format);
            }

            e.Handled = true;
        }


        private void ApDungQuyenGiaoVien()
        {
            if (role == "user" && !string.IsNullOrEmpty(maGVLogin))
            {
                txtTim.Visible = false;
                btnOK.Visible = false;
                lblTimKiem.Text = "Thời khóa biểu của bạn";

                string hoTen = LayHoTenGiaoVien(maGVLogin);
                this.Text = $"TKB - {hoTen}";
                LoadTKB_CuaGiaoVien(maGVLogin);

                // Khóa chỉnh sửa cho giáo viên
                btnThem.Visible = false; // Thay vì Enabled = false
                btnSua.Visible = false;  
                btnXoa.Visible = false;
            }
            else
            {
                TaiAutoComplete();
            }
        }

        private void TaiAutoComplete()
        {
            var source = new AutoCompleteStringCollection();
            string sql = "SELECT HoLot + ' ' + Ten FROM GiaoVien UNION SELECT TenLop FROM Lop";
            DataTable dt = db.ExecuteSelect(sql);
            foreach (DataRow r in dt.Rows)
                source.Add(r[0].ToString());

            txtTim.AutoCompleteCustomSource = source;
            txtTim.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTim.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private string LayHoTenGiaoVien(string maGV)
        {
            string sql = "SELECT HoLot + ' ' + Ten FROM GiaoVien WHERE MaGV = @MaGV";
            // ĐÃ SỬA LỖI CÚ PHÁP: Bọc SqlParameter vào mảng []
            var dt = db.ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@MaGV", maGV) });
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "Giáo viên";
        }

        private void TimKiem()
        {
            if (role == "user") return;

            string input = txtTim.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            string maLop = TimMaLop(input);
            string maGV = maLop == null ? TimMaGV(input) : null;

            if (maLop != null)
            {
                currentMaLop = maLop;
                currentMaGV = null;
                this.Text = $"TKB - Lớp {input}";
                LoadTKB_TheoLop(maLop);
            }
            else if (maGV != null)
            {
                currentMaGV = maGV;
                currentMaLop = null;
                this.Text = $"TKB - {input}";
                LoadTKB_CuaGiaoVien(maGV);
            }
            else
            {
                MessageBox.Show("Không tìm thấy lớp hoặc giáo viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string TimMaLop(string ten)
        {
            var dt = db.ExecuteSelect("SELECT MaLop FROM Lop WHERE TenLop = @Ten", new SqlParameter[] { new SqlParameter("@Ten", ten) });
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
        }
        private string TimMaGV(string ten)
        {
            var dt = db.ExecuteSelect("SELECT MaGV FROM GiaoVien WHERE HoLot + ' ' + Ten = @Ten", new SqlParameter[] { new SqlParameter("@Ten", ten) });
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
        }
        private void LoadTKB_TheoLop(string maLop)
        {
            currentMaLop = maLop;
            currentMaGV = null;
            DataTable matrix = TaoBangMatrix();
            string sql = "SELECT Thu, Tiet, TenMon, HoLot + ' ' + Ten AS HoTenGV, MaTKB FROM vw_TKB WHERE MaLop = @MaLop ORDER BY Thu, Tiet";
            
            var data = db.ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@MaLop", maLop) });
            DienDuLieuVaoMatrix(matrix, data);
            dgvTKB.DataSource = matrix;
            AnCotMaTKB();
        }

        private void LoadTKB_CuaGiaoVien(string maGV)
        {
            currentMaGV = maGV;
            currentMaLop = null;
            DataTable matrix = TaoBangMatrix();

            // ĐÃ SỬA: Thêm TenLop vào câu truy vấn
            string sql = "SELECT Thu, Tiet, TenMon, HoLot + ' ' + Ten AS HoTenGV, TenLop, MaTKB FROM vw_TKB WHERE MaGV = @MaGV ORDER BY Thu, Tiet";

            var data = db.ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@MaGV", maGV) });
            DienDuLieuVaoMatrix(matrix, data);
            dgvTKB.DataSource = matrix;
            AnCotMaTKB();
        }

        private DataTable TaoBangMatrix()
        {
            var dt = new DataTable();
            dt.Columns.Add("Tiết", typeof(string));

            for (int i = 2; i <= 7; i++)
            {
                dt.Columns.Add($"Thứ {i}", typeof(string));
                dt.Columns.Add($"MaTKB_Thu{i}", typeof(int));   //  thêm cột mã TKB cho mỗi ngày
            }

            for (int i = 1; i <= 10; i++)
            {
                var row = dt.NewRow();
                row["Tiết"] = i.ToString();

                for (int j = 2; j <= 7; j++)
                    row[$"MaTKB_Thu{j}"] = 0;   //  khởi tạo

                dt.Rows.Add(row);
            }

            return dt;
        }

        private void DienDuLieuVaoMatrix(DataTable matrix, DataTable data)
        {
            // Kiểm tra xem DataRow có cột TenLop hay không.
            bool hasTenLop = data.Columns.Contains("TenLop");

            // Xác định chế độ hiển thị TKB của GV: (cả Admin xem GV và User xem TKB của mình)
            bool isViewingGVTKB = !string.IsNullOrEmpty(currentMaGV) || hasTenLop;

            foreach (DataRow r in data.Rows)
            {
                int thu = Convert.ToInt32(r["Thu"]);
                int tiet = Convert.ToInt32(r["Tiet"]);
                string mon = r["TenMon"].ToString();
                string gv = r["HoTenGV"].ToString();
                int maTKB = Convert.ToInt32(r["MaTKB"]);

                string displayContent;

                if (isViewingGVTKB && hasTenLop)
                {
                    // SỬA ĐỔI: Nếu đang xem TKB của GV, hiển thị Môn \n Lớp
                    string lop = r["TenLop"].ToString();
                    displayContent = $"{mon}\n{lop}";
                }
                else
                {
                    // Giữ nguyên logic cũ: Nếu đang xem TKB của Lớp, hiển thị Môn \n GV
                    displayContent = $"{mon}\n{gv}";
                }

                // Gán dữ liệu vào matrix
                matrix.Rows[tiet - 1][$"Thứ {thu}"] = displayContent;
                matrix.Rows[tiet - 1][$"MaTKB_Thu{thu}"] = maTKB;
            }
        }

        private void AnCotMaTKB()
        {
            foreach (DataGridViewColumn col in dgvTKB.Columns)
            {
                if (col.Name.StartsWith("MaTKB_"))
                    col.Visible = false;
            }
        }

        private int LayMaTKB_DangChon()
        {
            if (dgvTKB.SelectedCells.Count == 0) return 0;

            var cell = dgvTKB.SelectedCells[0];
            int row = cell.RowIndex;
            int col = cell.ColumnIndex;

            string colName = dgvTKB.Columns[col].Name;

            if (!colName.StartsWith("Thứ")) return 0;

            string thu = colName.Replace("Thứ ", "");
            string maCol = $"MaTKB_Thu{thu}";

            if (!dgvTKB.Columns.Contains(maCol)) return 0;

            object val = dgvTKB.Rows[row].Cells[maCol].Value;
            if (val == DBNull.Value || val == null) return 0;

            return Convert.ToInt32(val);
        }

        private void ThemTietHoc()
        {
            if (string.IsNullOrEmpty(currentMaLop))
            {
                MessageBox.Show("Vui lòng chọn một lớp trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Giả định fTKB_Edit nhận maGVLogin để kiểm tra quyền
            var f = new fTKB_Edit(currentMaLop, null, maGVLogin);
            if (f.ShowDialog() == DialogResult.OK) TaiLaiDuLieu();
        }

        private void SuaTietHoc()
        {
            int maTKB = LayMaTKB_DangChon();
            if (maTKB <= 0)
            {
                MessageBox.Show("Vui lòng chọn một tiết học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var f = new fTKB_Edit(currentMaLop, maTKB, maGVLogin);
            if (f.ShowDialog() == DialogResult.OK) TaiLaiDuLieu();
        }

        private void XoaTietHoc()
        {
            int maTKB = LayMaTKB_DangChon();
            if (maTKB <= 0)
            {
                MessageBox.Show("Vui lòng chọn một tiết học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xóa tiết học này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                db.ExecuteNonQuery("DELETE FROM ThoiKhoaBieu WHERE MaTKB = @MaTKB", new SqlParameter[] { new SqlParameter("@MaTKB", maTKB) });
                TaiLaiDuLieu();
            }
        }

        private void TaiLaiDuLieu()
        {
            if (!string.IsNullOrEmpty(currentMaLop))
                LoadTKB_TheoLop(currentMaLop);
            else if (!string.IsNullOrEmpty(currentMaGV))
                LoadTKB_CuaGiaoVien(currentMaGV);
        }
        private void XuatExcel(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", FileName = "ThoiKhoaBieu.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("ThoiKhoaBieu");

                            // Ghi header
                            for (int i = 0; i < dgv.Columns.Count; i++)
                            {
                                if (!dgv.Columns[i].Visible) continue; // Bỏ cột ẩn (ví dụ cột MaTKB_)
                                worksheet.Cell(1, i + 1).Value = dgv.Columns[i].HeaderText;
                                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            }

                            int rowExcel = 2;
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                int colExcel = 1;
                                for (int j = 0; j < dgv.Columns.Count; j++)
                                {
                                    if (!dgv.Columns[j].Visible) continue; // Bỏ cột ẩn, không tăng colExcel

                                    var cellValue = dgv.Rows[i].Cells[j].Value?.ToString() ?? "";

                                    if (dgv.Columns[j].HeaderText.StartsWith("Thứ") && !string.IsNullOrEmpty(cellValue))
                                    {
                                        var parts = cellValue.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (parts.Length >= 2)
                                        {
                                            var cell = worksheet.Cell(rowExcel, colExcel);
                                            cell.Value = ""; // Xóa trước khi thêm rich text
                                            var richText = cell.GetRichText();
                                            richText.AddText(parts[0]).SetBold();  // Tên môn in đậm
                                            richText.AddText(Environment.NewLine);
                                            richText.AddText(parts[1]);             // Tên giáo viên bình thường
                                            cell.Style.Alignment.WrapText = true;
                                            worksheet.Row(rowExcel).Height = 30;   // Tăng chiều cao cho vừa đủ
                                        }
                                        else
                                        {
                                            worksheet.Cell(rowExcel, colExcel).Value = cellValue;
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Cell(rowExcel, colExcel).Value = cellValue;
                                    }

                                    colExcel++;  // Chỉ tăng khi ghi dữ liệu vào Excel
                                }


                                rowExcel += 2;  // Tăng 2 dòng để đủ chỗ tên môn + GV
                            }



                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            
        }     

        
        private void btnThem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            
        }

        private void fTKB_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}