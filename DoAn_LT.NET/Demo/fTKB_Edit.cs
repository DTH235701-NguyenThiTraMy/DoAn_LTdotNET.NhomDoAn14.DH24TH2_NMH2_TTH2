using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fTKB_Edit : Form
    {
        private readonly Database db = new Database();
        private readonly string maLop;
        private readonly int? maTKB; // null = thêm mới
        private readonly string maGVLogin; // nếu là giáo viên đăng nhập

        public fTKB_Edit(string maLop, int? maTKB = null, string maGVLogin = null)
        {
            InitializeComponent();
            this.maLop = maLop;
            this.maTKB = maTKB;
            this.maGVLogin = maGVLogin;

            this.Text = maTKB.HasValue ? "Sửa tiết học" : "Thêm tiết học mới";
            LoadComboboxes();
            LoadThuVaTiet();
            if (maTKB.HasValue)
                LoadDuLieuSua();

            if (maTKB.HasValue)
            {
                lstThu.SelectionMode = SelectionMode.One;
                lstTiet.SelectionMode = SelectionMode.One;
            }
            else
            {
                // Đảm bảo thiết lập cho chế độ thêm mới (có thể chọn nhiều)
                lstThu.SelectionMode = SelectionMode.MultiExtended;
                lstTiet.SelectionMode = SelectionMode.MultiExtended;
            }
        }
        private void LoadComboboxes()
        {
            string sql = @"
                SELECT PC.MaGV, GV.HoLot + ' ' + GV.Ten AS HoTenGV,
                       PC.MaMon, MH.TenMon
                FROM PhanCong PC
                JOIN GiaoVien GV ON PC.MaGV = GV.MaGV
                JOIN MonHoc MH ON PC.MaMon = MH.MaMon
                WHERE PC.MaLop = @MaLop
                ORDER BY HoTenGV";

            var dt = db.ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@MaLop", maLop) });

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Lớp này chưa được phân công giáo viên nào!\nVui lòng phân công trước.",
                    "Chưa có phân công", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            var dtSource = dt.Copy();
            dtSource.Columns.Add("HienThi", typeof(string), "HoTenGV + ' - ' + TenMon");

            cbbGiaoVienMon.DisplayMember = "HienThi";
            cbbGiaoVienMon.ValueMember = "MaGV";
            cbbGiaoVienMon.DataSource = dtSource;
            cbbGiaoVienMon.SelectedIndex = -1;
            cbbGiaoVienMon.Tag = dtSource;
        }

        private void LoadThuVaTiet()
        {
            for (int i = 2; i <= 7; i++) lstThu.Items.Add("Thứ " + i);
            for (int i = 1; i <= 10; i++) lstTiet.Items.Add("Tiết " + i);
            
        }

        private void LoadDuLieuSua()
        {
            string sql = "SELECT MaGV, MaMon, Thu, Tiet FROM ThoiKhoaBieu WHERE MaTKB = @MaTKB";
            var dt = db.ExecuteSelect(sql, new SqlParameter[] { new SqlParameter("@MaTKB", maTKB.Value) });
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                var source = (DataTable)cbbGiaoVienMon.Tag;
                var foundRows = source.Select($"MaGV = '{row["MaGV"]}' AND MaMon = '{row["MaMon"]}'");
                if (foundRows.Length > 0)
                    cbbGiaoVienMon.SelectedIndex = source.Rows.IndexOf(foundRows[0]);
                lstThu.SelectedIndex = (int)row["Thu"] - 2;
                lstTiet.SelectedIndex = (int)row["Tiet"] - 1;
            }
        }

        private bool ValidateInput()
        {
            if (cbbGiaoVienMon.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giáo viên và môn học!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Thay đổi: Kiểm tra số lượng mục được chọn
            if (lstThu.SelectedIndices.Count == 0 || lstTiet.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một Thứ và một Tiết học!", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            
            return true;
        }

        private bool IsTrungTietLop(int thu, int tiet)
        {
            string sql = "SELECT COUNT(*) FROM ThoiKhoaBieu WHERE MaLop = @MaLop AND Thu = @Thu AND Tiet = @Tiet";
            var parameters = new[]
            {
                new SqlParameter("@MaLop", maLop),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet)
            };

            if (maTKB.HasValue)
                sql += " AND MaTKB <> @MaTKB";

            if (maTKB.HasValue)
                parameters = parameters.Concat(new[] { new SqlParameter("@MaTKB", maTKB.Value) }).ToArray();

            return Convert.ToInt32(db.ExecuteScalar(sql, parameters)) > 0;
        }

        private bool IsTrungLichGiaoVien(string maGV, int thu, int tiet)
        {
            string sql = "SELECT COUNT(*) FROM ThoiKhoaBieu WHERE MaGV = @MaGV AND Thu = @Thu AND Tiet = @Tiet";
            var parameters = new[]
            {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet)
            };

            if (maTKB.HasValue)
                sql += " AND MaTKB <> @MaTKB";

            if (maTKB.HasValue)
                parameters = parameters.Concat(new[] { new SqlParameter("@MaTKB", maTKB.Value) }).ToArray();

            return Convert.ToInt32(db.ExecuteScalar(sql, parameters)) > 0;
        }

        private void ThemTietHocMoi(string maGV, string maMon, int thu, int tiet)
        {
            string sql = @"INSERT INTO ThoiKhoaBieu (MaLop, MaGV, MaMon, Thu, Tiet)
                           VALUES (@MaLop, @MaGV, @MaMon, @Thu, @Tiet)";
            var parameters = new[]
            {
                new SqlParameter("@MaLop", maLop),
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet)
            };
            db.ExecuteNonQuery(sql, parameters);
        }

        private void CapNhatTietHoc(string maGV, string maMon, int thu, int tiet)
        {
            string sql = @"UPDATE ThoiKhoaBieu
                           SET MaGV = @MaGV, MaMon = @MaMon, Thu = @Thu, Tiet = @Tiet
                           WHERE MaTKB = @MaTKB";
            var parameters = new[]
            {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@MaMon", maMon),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet),
                new SqlParameter("@MaTKB", maTKB.Value)
            };
            db.ExecuteNonQuery(sql, parameters);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var selectedRow = ((DataRowView)cbbGiaoVienMon.SelectedItem).Row;
            string maGV = selectedRow["MaGV"].ToString();
            string maMon = selectedRow["MaMon"].ToString();
            
            // Kiểm tra quyền: Giáo viên chỉ được chỉnh tiết của mình

            if (!string.IsNullOrEmpty(maGVLogin) && maGV != maGVLogin)
            {
                MessageBox.Show("Bạn chỉ được chỉnh sửa tiết học của mình!", "Không có quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            {
                if (maTKB.HasValue)
                {
                    // CHẾ ĐỘ SỬA (chỉ có 1 cặp Thu, Tiet)
                    int thu = lstThu.SelectedIndex + 2;
                    int tiet = lstTiet.SelectedIndex + 1;

                    if (IsTrungTietLop(thu, tiet))
                    {
                        MessageBox.Show("Lớp đã có tiết học vào thời gian này!", "Trùng lịch lớp",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (IsTrungLichGiaoVien(maGV, thu, tiet))
                    {
                        MessageBox.Show("Giáo viên đã có tiết dạy khác vào thời gian này!", "Trùng lịch giáo viên",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    CapNhatTietHoc(maGV, maMon, thu, tiet);
                }
                else
                {
                    // CHẾ ĐỘ THÊM MỚI (có thể có nhiều cặp Thu, Tiet)
                    // Lấy tất cả các "Thứ" và "Tiết" đã chọn
                    var selectedThus = lstThu.SelectedIndices.Cast<int>().Select(index => index + 2).ToList();
                    var selectedTiets = lstTiet.SelectedIndices.Cast<int>().Select(index => index + 1).ToList();

                    int soLuongTietThemThanhCong = 0;
                    bool coTrungLich = false;

                    // Lặp qua tất cả các tổ hợp (Thứ, Tiết) đã chọn
                    foreach (int thu in selectedThus)
                    {
                        foreach (int tiet in selectedTiets)
                        {
                            if (IsTrungTietLop(thu, tiet) || IsTrungLichGiaoVien(maGV, thu, tiet))
                            {
                                coTrungLich = true;
                                MessageBox.Show($"Trùng lịch: Thứ {thu} Tiết {tiet} đã có giáo viên khác hoặc lớp khác dạy!",
                                    "Lỗi Trùng Lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                continue; // Bỏ qua cặp trùng lịch
                            }

                            ThemTietHocMoi(maGV, maMon, thu, tiet);
                            soLuongTietThemThanhCong++;
                        }
                    }

                    if (soLuongTietThemThanhCong > 0)
                    {
                        string message = $"Đã thêm thành công {soLuongTietThemThanhCong} tiết học mới.";
                        if (coTrungLich)
                        {
                            MessageBox.Show(message + "\n(Có một số tiết bị bỏ qua do trùng lịch.)", "Thành công",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (coTrungLich)
                    {
                        // Không thêm được tiết nào nhưng có trùng lịch (đã cảnh báo bên trong vòng lặp)
                        return;
                    }
                    else
                    {
                        // Trường hợp không lặp qua được (chỉ xảy ra nếu ValidateInput() lỗi)
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void fTKB_Edit_Load(object sender, EventArgs e)
        {

        }

        private void cbbGiaoVienMon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}