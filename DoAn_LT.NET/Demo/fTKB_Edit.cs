using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLGV_THPT
{
    public partial class fTKB_Edit : Form
    {
        Database db = new Database();
        private string maLop;
        private int? maTKB; // null = thêm, có giá trị = sửa
        private string maGV;

        public fTKB_Edit(string maLop, int? maTKB, string maGV = null)
        {
            InitializeComponent();
            this.maLop = maLop;
            this.maTKB = maTKB;
            this.maGV = maGV;

            InitControls();
            LoadPhanCong();
            LoadThuTiet();

            if (maTKB.HasValue)
                LoadData();
        }
        private void InitControls()
        {
            btnLuu.Click +=btnLuu_Click;
            btnHuy.Click += (s, e) => this.Close();
        }
        private void LoadPhanCong()
        {
            // Combo Giáo viên + Môn học theo phân công của lớp
            string query = @"
                SELECT PC.MaGV, GV.HoLot + ' ' + GV.Ten AS HoTenGV,
                       PC.MaMon, MH.TenMon
                FROM PhanCong PC
                JOIN GiaoVien GV ON PC.MaGV = GV.MaGV
                JOIN MonHoc MH ON PC.MaMon = MH.MaMon
                WHERE PC.MaLop = @MaLop";

            SqlParameter[] parameters = { new SqlParameter("@MaLop", maLop) };
            DataTable dt = db.ExecuteSelect(query, parameters);

            // Load Giáo viên
            DataView dvGV = new DataView(dt);
            dvGV = dvGV.ToTable(true, "MaGV", "HoTenGV").DefaultView;
            cbbGV.DataSource = dvGV.ToTable();
            cbbGV.DisplayMember = "HoTenGV";
            cbbGV.ValueMember = "MaGV";
            cbbGV.SelectedIndex = -1;

            // Load Môn
            DataView dvMon = new DataView(dt);
            dvMon = dvMon.ToTable(true, "MaMon", "TenMon").DefaultView;
            cbbMon.DataSource = dvMon.ToTable();
            cbbMon.DisplayMember = "TenMon";
            cbbMon.ValueMember = "MaMon";
            cbbMon.SelectedIndex = -1;
        }
        private void LoadThuTiet()
        {
            // Thứ 2 - 7
            for (int i = 2; i <= 7; i++)
                cbbThu.Items.Add(i);
            // Tiết 1 - 10
            for (int i = 1; i <= 10; i++)
                cbbTiet.Items.Add(i);

            cbbThu.SelectedIndex = -1;
            cbbTiet.SelectedIndex = -1;
        }
        private void LoadData()
        {
            string query = "SELECT * FROM ThoiKhoaBieu WHERE MaTKB = @MaTKB";
            SqlParameter[] parameters = { new SqlParameter("@MaTKB", maTKB.Value) };
            DataTable dt = db.ExecuteSelect(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cbbGV.SelectedValue = row["MaGV"].ToString();
                cbbMon.SelectedValue = row["MaMon"].ToString();
                cbbThu.SelectedItem = Convert.ToInt32(row["Thu"]);
                cbbTiet.SelectedItem = Convert.ToInt32(row["Tiet"]);
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cbbGV.SelectedIndex == -1 || cbbMon.SelectedIndex == -1 ||
                cbbThu.SelectedIndex == -1 || cbbTiet.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Giáo viên, Môn, Thứ, Tiết!");
                return;
            }

            string maGV = cbbGV.SelectedValue.ToString();
            string maMon = cbbMon.SelectedValue.ToString();
            int thu = Convert.ToInt32(cbbThu.SelectedItem);
            int tiet = Convert.ToInt32(cbbTiet.SelectedItem);

            // Kiểm tra trùng tiết lớp
            string checkLop = "SELECT COUNT(*) FROM ThoiKhoaBieu WHERE MaLop=@MaLop AND Thu=@Thu AND Tiet=@Tiet";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@MaLop", maLop),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet)
            };
            if (maTKB.HasValue)
            {
                checkLop += " AND MaTKB <> @MaTKB";
                parameters.Add(new SqlParameter("@MaTKB", maTKB.Value));
            }
            object result = db.ExecuteScalar(checkLop, parameters.ToArray());
            if (Convert.ToInt32(result) > 0)
            {
                MessageBox.Show("Tiết học này đã có trong lớp!");
                return;
            }
            // Kiểm tra trùng tiết giáo viên
            string checkGV = "SELECT COUNT(*) FROM ThoiKhoaBieu WHERE MaGV=@MaGV AND Thu=@Thu AND Tiet=@Tiet";
            List<SqlParameter> parametersGV = new List<SqlParameter>
            {
                new SqlParameter("@MaGV", maGV),
                new SqlParameter("@Thu", thu),
                new SqlParameter("@Tiet", tiet)
            };
            if (maTKB.HasValue)
            {
                checkGV += " AND MaTKB <> @MaTKB";
                parametersGV.Add(new SqlParameter("@MaTKB", maTKB.Value));
            }
            object resultGV = db.ExecuteScalar(checkGV, parametersGV.ToArray());
            if (Convert.ToInt32(resultGV) > 0)
            {
                MessageBox.Show("Giáo viên đã có tiết học khác vào cùng thời gian!");
                return;
            }

            // Thêm hoặc sửa
            if (maTKB.HasValue)
            {
                string queryUpdate = @"UPDATE ThoiKhoaBieu 
                                       SET MaGV=@MaGV, MaMon=@MaMon, Thu=@Thu, Tiet=@Tiet 
                                       WHERE MaTKB=@MaTKB";
                SqlParameter[] paramUpdate = {
                    new SqlParameter("@MaGV", maGV),
                    new SqlParameter("@MaMon", maMon),
                    new SqlParameter("@Thu", thu),
                    new SqlParameter("@Tiet", tiet),
                    new SqlParameter("@MaTKB", maTKB.Value)
                };
                db.ExecuteNonQuery(queryUpdate, paramUpdate);
            }
            else
            {
                string queryInsert = @"INSERT INTO ThoiKhoaBieu (MaLop, MaGV, MaMon, Thu, Tiet)
                                       VALUES (@MaLop, @MaGV, @MaMon, @Thu, @Tiet)";
                SqlParameter[] paramInsert = {
                    new SqlParameter("@MaLop", maLop),
                    new SqlParameter("@MaGV", maGV),
                    new SqlParameter("@MaMon", maMon),
                    new SqlParameter("@Thu", thu),
                    new SqlParameter("@Tiet", tiet)
                };
                db.ExecuteNonQuery(queryInsert, paramInsert);
            }

            MessageBox.Show("Lưu thành công!");
            this.Close();
        }

        private void fTKB_Edit_Load(object sender, EventArgs e)
        {

        }
    }
}