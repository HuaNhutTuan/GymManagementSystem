using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_TrainerManagement : Form
    {
        private int? selectedTrainerId = null;

        public Form_TrainerManagement()
        {
            InitializeComponent();
        }

        private void Form_TrainerManagement_Load(object sender, EventArgs e)
        {
            cboSpecialty.SelectedIndex = 0;
            cboFilterSpecialty.SelectedIndex = 0; // "Tất cả"
            ResetForm();
            LoadTrainers();
        }

        #region 1. NẠP DỮ LIỆU HUẤN LUYỆN VIÊN & HOA HỒNG

        private void LoadTrainers(string keyword = "", string specialtyFilter = "Tất cả")
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var query = db.Trainers.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        query = query.Where(t => t.FullName.ToLower().Contains(kw) || (t.Phone != null && t.Phone.Contains(kw)));
                    }

                    if (specialtyFilter != "Tất cả")
                    {
                        query = query.Where(t => t.Specialty != null && t.Specialty.Contains(specialtyFilter));
                    }

                    var trainers = query.OrderBy(t => t.TrainerId).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã HLV", typeof(int));
                    dt.Columns.Add("Họ và Tên HLV", typeof(string));
                    dt.Columns.Add("Số Điện Thoại", typeof(string));
                    dt.Columns.Add("Chuyên Môn Đào Tạo", typeof(string));
                    dt.Columns.Add("Học Viên Đang Kèm", typeof(int));
                    dt.Columns.Add("Số Ca Đã Hoàn Thành", typeof(int));
                    dt.Columns.Add("Hoa Hồng Tích Lũy (VNĐ)", typeof(decimal));

                    int totalCompletedSessions = 0;
                    string connStr = "Server=.\\SQLEXPRESS;Database=GymManagementDB;Integrated Security=True;";

                    foreach (var t in trainers)
                    {
                        // 1. Số học viên active đang kèm
                        int activeMembers = db.MemberPackages.Count(mp => mp.TrainerId == t.TrainerId && mp.IsActive == true);

                        // 2. Số ca đã dạy hoàn thành trong PTSchedules
                        int completedCount = 0;
                        try
                        {
                            using (SqlConnection conn = new SqlConnection(connStr))
                            {
                                conn.Open();
                                string sql = "SELECT COUNT(*) FROM PTSchedules WHERE TrainerId = @TrainerId AND Status = N'Đã hoàn thành'";
                                using (SqlCommand cmd = new SqlCommand(sql, conn))
                                {
                                    cmd.Parameters.AddWithValue("@TrainerId", t.TrainerId);
                                    completedCount = (int)cmd.ExecuteScalar();
                                }
                            }
                        }
                        catch { }

                        totalCompletedSessions += completedCount;
                        decimal commission = completedCount * numCommissionRate.Value;

                        dt.Rows.Add(t.TrainerId, t.FullName, t.Phone ?? "", t.Specialty ?? "Thể hình Gym", activeMembers, completedCount, commission);
                    }

                    dgvTrainers.DataSource = dt;
                    FormatTrainersGrid();

                    lblKpiTrainers.Text = $"👨‍🏫 Tổng PT: {trainers.Count}";
                    lblKpiSessions.Text = $"✅ Ca đã hoàn thành: {totalCompletedSessions}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp danh sách HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatTrainersGrid()
        {
            if (dgvTrainers.Columns["Mã HLV"] != null)
            {
                dgvTrainers.Columns["Mã HLV"].Width = 80;
                dgvTrainers.Columns["Mã HLV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvTrainers.Columns["Họ và Tên HLV"] != null)
            {
                dgvTrainers.Columns["Họ và Tên HLV"].Width = 190;
            }
            if (dgvTrainers.Columns["Số Điện Thoại"] != null)
            {
                dgvTrainers.Columns["Số Điện Thoại"].Width = 110;
                dgvTrainers.Columns["Số Điện Thoại"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvTrainers.Columns["Chuyên Môn Đào Tạo"] != null)
            {
                dgvTrainers.Columns["Chuyên Môn Đào Tạo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTrainers.Columns["Chuyên Môn Đào Tạo"].MinimumWidth = 200;
            }
            if (dgvTrainers.Columns["Học Viên Đang Kèm"] != null)
            {
                dgvTrainers.Columns["Học Viên Đang Kèm"].Width = 130;
                dgvTrainers.Columns["Học Viên Đang Kèm"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvTrainers.Columns["Số Ca Đã Hoàn Thành"] != null)
            {
                dgvTrainers.Columns["Số Ca Đã Hoàn Thành"].Width = 140;
                dgvTrainers.Columns["Số Ca Đã Hoàn Thành"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"] != null)
            {
                dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"].Width = 160;
                dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"].DefaultCellStyle.Format = "N0";
                dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"].DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                dgvTrainers.Columns["Hoa Hồng Tích Lũy (VNĐ)"].DefaultCellStyle.ForeColor = Color.FromArgb(40, 167, 69);
            }
        }

        #endregion

        #region 2. CLICK CHỌN & XEM LỊCH DẠY CHI TIẾT

        private void dgvTrainers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvTrainers.Rows.Count) return;

            DataGridViewRow row = dgvTrainers.Rows[e.RowIndex];
            selectedTrainerId = Convert.ToInt32(row.Cells["Mã HLV"].Value);

            txtTrainerId.Text = selectedTrainerId.ToString();
            txtFullName.Text = row.Cells["Họ và Tên HLV"].Value?.ToString() ?? "";
            txtPhone.Text = row.Cells["Số Điện Thoại"].Value?.ToString() ?? "";
            string spec = row.Cells["Chuyên Môn Đào Tạo"].Value?.ToString() ?? "";
            cboSpecialty.SelectedItem = spec;
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = true;

            LoadTrainerScheduleDetails(selectedTrainerId.Value);
        }

        private void LoadTrainerScheduleDetails(int trainerId)
        {
            try
            {
                string connStr = "Server=.\\SQLEXPRESS;Database=GymManagementDB;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"
                        SELECT s.ScheduleId AS [Mã Lịch],
                               s.ScheduleDate AS [Ngày Tập],
                               CONVERT(VARCHAR(5), s.StartTime, 108) + ' - ' + CONVERT(VARCHAR(5), s.EndTime, 108) AS [Khung Giờ],
                               m.FullName AS [Hội Viên],
                               m.Phone AS [SĐT Hội Viên],
                               s.Status AS [Trạng Thái],
                               s.Notes AS [Ghi Chú]
                        FROM PTSchedules s
                        JOIN Members m ON s.MemberId = m.MemberId
                        WHERE s.TrainerId = @TrainerId
                        ORDER BY s.ScheduleDate DESC, s.StartTime DESC";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@TrainerId", trainerId);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Thêm cột tính hoa hồng từng ca
                        dt.Columns.Add("Hoa Hồng Ca (VNĐ)", typeof(decimal));
                        decimal totalCommission = 0;
                        int completedSessions = 0;

                        foreach (DataRow r in dt.Rows)
                        {
                            string st = r["Trạng Thái"]?.ToString() ?? "";
                            if (st == "Đã hoàn thành")
                            {
                                r["Hoa Hồng Ca (VNĐ)"] = numCommissionRate.Value;
                                totalCommission += numCommissionRate.Value;
                                completedSessions++;
                            }
                            else
                            {
                                r["Hoa Hồng Ca (VNĐ)"] = 0m;
                            }
                        }

                        dgvTrainerSchedules.DataSource = dt;

                        if (dgvTrainerSchedules.Columns["Ngày Tập"] != null)
                        {
                            dgvTrainerSchedules.Columns["Ngày Tập"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        }
                        if (dgvTrainerSchedules.Columns["Hoa Hồng Ca (VNĐ)"] != null)
                        {
                            dgvTrainerSchedules.Columns["Hoa Hồng Ca (VNĐ)"].DefaultCellStyle.Format = "N0";
                            dgvTrainerSchedules.Columns["Hoa Hồng Ca (VNĐ)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }

                        lblTotalCommission.Text = $"💰 Tổng hoa hồng thực nhận của HLV: {totalCommission:N0} VNĐ ({completedSessions} ca thực dạy x {numCommissionRate.Value:N0} đ/ca)";
                    }
                }
            }
            catch (Exception ex)
            {
                lblTotalCommission.Text = "Tổng hoa hồng tích lũy: 0 VNĐ";
                System.Diagnostics.Debug.WriteLine($"Lỗi nạp lịch dạy: {ex.Message}");
            }
        }

        #endregion

        #region 3. THAO TÁC CRUD HUẤN LUYỆN VIÊN

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtFullName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập họ và tên Huấn luyện viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var trainer = new Trainers
                    {
                        FullName = name,
                        Phone = txtPhone.Text.Trim(),
                        Specialty = cboSpecialty.Text.Trim()
                    };
                    db.Trainers.Add(trainer);
                    db.SaveChanges();
                }

                MessageBox.Show("Thêm Huấn luyện viên mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadTrainers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!selectedTrainerId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn HLV cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEdit.Enabled = false;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            txtFullName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!selectedTrainerId.HasValue) return;

            string name = txtFullName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Họ tên Huấn luyện viên không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var trainer = db.Trainers.Find(selectedTrainerId.Value);
                    if (trainer != null)
                    {
                        trainer.FullName = name;
                        trainer.Phone = txtPhone.Text.Trim();
                        trainer.Specialty = cboSpecialty.Text.Trim();
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Cập nhật thông tin Huấn luyện viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadTrainers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!selectedTrainerId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn HLV cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa HLV '{txtFullName.Text}' khỏi hệ thống không?",
                                          "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GymManagementDBEntities1())
                    {
                        // Kiểm tra ràng buộc
                        bool hasMembers = db.MemberPackages.Any(mp => mp.TrainerId == selectedTrainerId.Value);
                        if (hasMembers)
                        {
                            MessageBox.Show("Không thể xóa Huấn luyện viên này vì đang có học viên đăng ký theo học!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var trainer = db.Trainers.Find(selectedTrainerId.Value);
                        if (trainer != null)
                        {
                            db.Trainers.Remove(trainer);
                            db.SaveChanges();
                        }
                    }

                    MessageBox.Show("Xóa Huấn luyện viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    LoadTrainers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            selectedTrainerId = null;
            txtTrainerId.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            cboSpecialty.SelectedIndex = 0;

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dgvTrainerSchedules.DataSource = null;
            lblTotalCommission.Text = "💰 Tổng hoa hồng tích lũy: 0 VNĐ (0 ca thực dạy)";
        }

        #endregion

        #region 4. TÌM KIẾM TỨC THÌ & XUẤT EXCEL EPPLUS

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadTrainers(txtSearch.Text, cboFilterSpecialty.SelectedItem?.ToString() ?? "Tất cả");
        }

        private void cboFilterSpecialty_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTrainers(txtSearch.Text, cboFilterSpecialty.SelectedItem?.ToString() ?? "Tất cả");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTrainers.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu Huấn luyện viên để xuất file Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = $"DanhSachHLV_HoaHong_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã HLV", typeof(int));
                    dt.Columns.Add("Họ và Tên Huấn Luyện Viên", typeof(string));
                    dt.Columns.Add("Số Điện Thoại", typeof(string));
                    dt.Columns.Add("Chuyên Môn Đào Tạo", typeof(string));
                    dt.Columns.Add("Học Viên Đang Kèm", typeof(int));
                    dt.Columns.Add("Số Ca Đã Dạy", typeof(int));
                    dt.Columns.Add("Hoa Hồng Tích Lũy (VNĐ)", typeof(decimal));

                    foreach (DataGridViewRow row in dgvTrainers.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(
                            Convert.ToInt32(row.Cells["Mã HLV"].Value ?? 0),
                            row.Cells["Họ và Tên HLV"].Value?.ToString() ?? "",
                            row.Cells["Số Điện Thoại"].Value?.ToString() ?? "",
                            row.Cells["Chuyên Môn Đào Tạo"].Value?.ToString() ?? "",
                            Convert.ToInt32(row.Cells["Học Viên Đang Kèm"].Value ?? 0),
                            Convert.ToInt32(row.Cells["Số Ca Đã Hoàn Thành"].Value ?? 0),
                            Convert.ToDecimal(row.Cells["Hoa Hồng Tích Lũy (VNĐ)"].Value ?? 0)
                        );
                    }

                    ExcelExportHelper.ExportToExcel(dt, sfd.FileName, "HLV_HoaHong", "BÁO CÁO DANH SÁCH HUẤN LUYỆN VIÊN & HOA HỒNG CA DẠY");

                    var result = MessageBox.Show("Xuất danh sách HLV & Bảng tính hoa hồng ra file Excel thành công bằng EPPlus!\nBạn có muốn mở file ngay không?",
                                                 "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
