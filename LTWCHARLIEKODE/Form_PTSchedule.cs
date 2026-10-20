using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_PTSchedule : Form
    {
        private string _connectionString;

        public Form_PTSchedule()
        {
            InitializeComponent();
            _connectionString = "data source=.;initial catalog=GymManagementDB;integrated security=True;";
        }

        private void Form_PTSchedule_Load(object sender, EventArgs e)
        {
            LoadMembers();
            LoadTrainers();
            cboStatus.SelectedIndex = 0;
            ResetForm();
            LoadSchedules();
        }

        #region 1. NẠP DỮ LIỆU DANH MỤC

        private void LoadMembers()
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var members = db.Members.OrderBy(m => m.FullName).Select(m => new
                    {
                        m.MemberId,
                        DisplayName = m.FullName + " (" + (m.Phone ?? "Không SĐT") + ")"
                    }).ToList();

                    cboMembers.DataSource = members;
                    cboMembers.DisplayMember = "DisplayName";
                    cboMembers.ValueMember = "MemberId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách hội viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTrainers()
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var trainers = db.Trainers.OrderBy(t => t.FullName).Select(t => new
                    {
                        t.TrainerId,
                        DisplayName = t.FullName + " (" + (t.Specialty ?? "Gym") + ")"
                    }).ToList();

                    cboTrainers.DataSource = trainers;
                    cboTrainers.DisplayMember = "DisplayName";
                    cboTrainers.ValueMember = "TrainerId";

                    // Bộ lọc HLV
                    var filterList = trainers.ToList();
                    filterList.Insert(0, new { TrainerId = 0, DisplayName = "-- Tất cả Huấn luyện viên --" });
                    cboFilterTrainer.DataSource = filterList;
                    cboFilterTrainer.DisplayMember = "DisplayName";
                    cboFilterTrainer.ValueMember = "TrainerId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 2. THUẬT TOÁN KIỂM TRA TRÙNG LỊCH (OVERLAP DETECTION)

        /// <summary>
        /// Kiểm tra trùng lịch tập theo nguyên lý giao khoảng thời gian:
        /// Khoảng [StartA, EndA] trùng với [StartB, EndB] khi: (StartA < EndB) AND (EndA > StartB).
        /// Kiểm tra cả 2 trường hợp:
        /// 1. Huấn luyện viên đã có lịch với học viên khác trong khung giờ này chưa?
        /// 2. Hội viên đã đăng ký lịch với ai khác trong khung giờ này chưa?
        /// </summary>
        private bool CheckScheduleConflict(int? currentScheduleId, int trainerId, int memberId, DateTime scheduleDate, TimeSpan startTime, TimeSpan endTime, out string conflictMessage)
        {
            conflictMessage = string.Empty;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Kiểm tra Huấn luyện viên có bị trùng lịch không
                string sqlTrainer = @"
                    SELECT TOP 1 s.ScheduleId, t.FullName AS TrainerName, m.FullName AS MemberName, s.StartTime, s.EndTime
                    FROM PTSchedules s
                    JOIN Trainers t ON s.TrainerId = t.TrainerId
                    JOIN Members m ON s.MemberId = m.MemberId
                    WHERE s.TrainerId = @TrainerId
                      AND s.ScheduleDate = @ScheduleDate
                      AND s.Status != N'Đã hủy'
                      AND (@CurrentId IS NULL OR s.ScheduleId != @CurrentId)
                      AND (s.StartTime < @EndTime AND s.EndTime > @StartTime)";

                using (var cmd = new SqlCommand(sqlTrainer, conn))
                {
                    cmd.Parameters.AddWithValue("@TrainerId", trainerId);
                    cmd.Parameters.AddWithValue("@ScheduleDate", scheduleDate.Date);
                    cmd.Parameters.AddWithValue("@StartTime", startTime);
                    cmd.Parameters.AddWithValue("@EndTime", endTime);
                    cmd.Parameters.AddWithValue("@CurrentId", (object)currentScheduleId ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string trainerName = reader["TrainerName"].ToString();
                            string existMember = reader["MemberName"].ToString();
                            TimeSpan eStart = (TimeSpan)reader["StartTime"];
                            TimeSpan eEnd = (TimeSpan)reader["EndTime"];

                            conflictMessage = $"TRÙNG LỊCH HUẤN LUYỆN VIÊN!\n\n" +
                                              $"Huấn luyện viên [{trainerName}] đã có lịch tập với Hội viên [{existMember}] " +
                                              $"vào khung giờ {eStart:hh\\:mm} - {eEnd:hh\\:mm} ngày {scheduleDate:dd/MM/yyyy}.\n\n" +
                                              $"Vui lòng chọn khung giờ khác hoặc đổi Huấn luyện viên!";
                            return true;
                        }
                    }
                }

                // 2. Kiểm tra Hội viên có bị trùng lịch của chính mình không
                string sqlMember = @"
                    SELECT TOP 1 s.ScheduleId, m.FullName AS MemberName, t.FullName AS TrainerName, s.StartTime, s.EndTime
                    FROM PTSchedules s
                    JOIN Trainers t ON s.TrainerId = t.TrainerId
                    JOIN Members m ON s.MemberId = m.MemberId
                    WHERE s.MemberId = @MemberId
                      AND s.ScheduleDate = @ScheduleDate
                      AND s.Status != N'Đã hủy'
                      AND (@CurrentId IS NULL OR s.ScheduleId != @CurrentId)
                      AND (s.StartTime < @EndTime AND s.EndTime > @StartTime)";

                using (var cmd = new SqlCommand(sqlMember, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberId", memberId);
                    cmd.Parameters.AddWithValue("@ScheduleDate", scheduleDate.Date);
                    cmd.Parameters.AddWithValue("@StartTime", startTime);
                    cmd.Parameters.AddWithValue("@EndTime", endTime);
                    cmd.Parameters.AddWithValue("@CurrentId", (object)currentScheduleId ?? DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string memberName = reader["MemberName"].ToString();
                            string ptName = reader["TrainerName"].ToString();
                            TimeSpan eStart = (TimeSpan)reader["StartTime"];
                            TimeSpan eEnd = (TimeSpan)reader["EndTime"];

                            conflictMessage = $"TRÙNG LỊCH HỘI VIÊN!\n\n" +
                                              $"Hội viên [{memberName}] đã có lịch tập với HLV [{ptName}] " +
                                              $"vào khung giờ {eStart:hh\\:mm} - {eEnd:hh\\:mm} ngày {scheduleDate:dd/MM/yyyy}.\n\n" +
                                              $"Một hội viên không thể tập 2 buổi cùng lúc!";
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region 3. THAO TÁC CRUD LỊCH TẬP

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out int memberId, out int trainerId, out DateTime date, out TimeSpan start, out TimeSpan end, out string status, out string notes))
            {
                return;
            }

            // Kiểm tra trùng lịch
            if (CheckScheduleConflict(null, trainerId, memberId, date, start, end, out string conflictMsg))
            {
                MessageBox.Show(conflictMsg, "Cảnh báo trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                        INSERT INTO PTSchedules (MemberId, TrainerId, ScheduleDate, StartTime, EndTime, Status, Notes)
                        VALUES (@MemberId, @TrainerId, @ScheduleDate, @StartTime, @EndTime, @Status, @Notes)";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MemberId", memberId);
                        cmd.Parameters.AddWithValue("@TrainerId", trainerId);
                        cmd.Parameters.AddWithValue("@ScheduleDate", date);
                        cmd.Parameters.AddWithValue("@StartTime", start);
                        cmd.Parameters.AddWithValue("@EndTime", end);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm lịch tập mới với Huấn luyện viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSchedules();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm lịch tập: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtScheduleId.Text) || !int.TryParse(txtScheduleId.Text, out int scheduleId))
            {
                MessageBox.Show("Vui lòng chọn một lịch tập từ danh sách bên dưới để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out int memberId, out int trainerId, out DateTime date, out TimeSpan start, out TimeSpan end, out string status, out string notes))
            {
                return;
            }

            // Kiểm tra trùng lịch loại trừ chính nó
            if (CheckScheduleConflict(scheduleId, trainerId, memberId, date, start, end, out string conflictMsg))
            {
                MessageBox.Show(conflictMsg, "Cảnh báo trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                        UPDATE PTSchedules
                        SET MemberId = @MemberId,
                            TrainerId = @TrainerId,
                            ScheduleDate = @ScheduleDate,
                            StartTime = @StartTime,
                            EndTime = @EndTime,
                            Status = @Status,
                            Notes = @Notes
                        WHERE ScheduleId = @ScheduleId";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
                        cmd.Parameters.AddWithValue("@MemberId", memberId);
                        cmd.Parameters.AddWithValue("@TrainerId", trainerId);
                        cmd.Parameters.AddWithValue("@ScheduleDate", date);
                        cmd.Parameters.AddWithValue("@StartTime", start);
                        cmd.Parameters.AddWithValue("@EndTime", end);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cập nhật thông tin lịch tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSchedules();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật lịch tập: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtScheduleId.Text) || !int.TryParse(txtScheduleId.Text, out int scheduleId))
            {
                MessageBox.Show("Vui lòng chọn lịch tập cần hủy/xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn hủy/xóa lịch tập #{scheduleId} này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes) return;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM PTSchedules WHERE ScheduleId = @ScheduleId";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleId", scheduleId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã xóa lịch tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSchedules();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa lịch: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput(out int memberId, out int trainerId, out DateTime date, out TimeSpan start, out TimeSpan end, out string status, out string notes)
        {
            memberId = 0;
            trainerId = 0;
            date = dtpScheduleDate.Value.Date;
            start = dtpStartTime.Value.TimeOfDay;
            end = dtpEndTime.Value.TimeOfDay;
            status = cboStatus.SelectedItem != null ? cboStatus.SelectedItem.ToString() : "Đã lên lịch";
            notes = txtNotes.Text.Trim();

            if (cboMembers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Hội viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            memberId = (int)cboMembers.SelectedValue;

            if (cboTrainers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Huấn luyện viên (PT)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            trainerId = (int)cboTrainers.SelectedValue;

            if (end <= start)
            {
                MessageBox.Show("Giờ kết thúc buổi tập phải lớn hơn giờ bắt đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((end - start).TotalMinutes < 30)
            {
                MessageBox.Show("Thời lượng một buổi tập tối thiểu là 30 phút!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion

        #region 4. HIỂN THỊ DANH SÁCH & BỘ LỌC

        private void LoadSchedules(int? filterTrainerId = null, DateTime? filterDate = null)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                        SELECT s.ScheduleId AS [Mã Lịch],
                               m.FullName AS [Hội Viên],
                               m.Phone AS [Số ĐT],
                               t.FullName AS [Huấn Luyện Viên],
                               t.Specialty AS [Chuyên Môn],
                               s.ScheduleDate AS [Ngày Tập],
                               CONVERT(VARCHAR(5), s.StartTime, 108) AS [Bắt Đầu],
                               CONVERT(VARCHAR(5), s.EndTime, 108) AS [Kết Thúc],
                               s.Status AS [Trạng Thái],
                               s.Notes AS [Ghi Chú]
                        FROM PTSchedules s
                        JOIN Members m ON s.MemberId = m.MemberId
                        JOIN Trainers t ON s.TrainerId = t.TrainerId
                        WHERE (@TrainerId IS NULL OR @TrainerId = 0 OR s.TrainerId = @TrainerId)
                          AND (@FilterDate IS NULL OR s.ScheduleDate = @FilterDate)
                        ORDER BY s.ScheduleDate DESC, s.StartTime ASC";

                    using (var da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@TrainerId", (object)filterTrainerId ?? DBNull.Value);
                        da.SelectCommand.Parameters.AddWithValue("@FilterDate", (object)filterDate?.Date ?? DBNull.Value);

                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvSchedules.DataSource = dt;

                        if (dgvSchedules.Columns["Ngày Tập"] != null)
                        {
                            dgvSchedules.Columns["Ngày Tập"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp danh sách lịch tập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSchedules.Rows[e.RowIndex];
            txtScheduleId.Text = row.Cells["Mã Lịch"].Value.ToString();

            // Set Member
            string memberName = row.Cells["Hội Viên"].Value.ToString();
            for (int i = 0; i < cboMembers.Items.Count; i++)
            {
                if (cboMembers.GetItemText(cboMembers.Items[i]).Contains(memberName))
                {
                    cboMembers.SelectedIndex = i;
                    break;
                }
            }

            // Set Trainer
            string trainerName = row.Cells["Huấn Luyện Viên"].Value.ToString();
            for (int i = 0; i < cboTrainers.Items.Count; i++)
            {
                if (cboTrainers.GetItemText(cboTrainers.Items[i]).Contains(trainerName))
                {
                    cboTrainers.SelectedIndex = i;
                    break;
                }
            }

            // Set Date
            if (DateTime.TryParse(row.Cells["Ngày Tập"].Value.ToString(), out DateTime sDate))
            {
                dtpScheduleDate.Value = sDate;
            }

            // Set Time
            if (TimeSpan.TryParse(row.Cells["Bắt Đầu"].Value.ToString(), out TimeSpan sTime))
            {
                dtpStartTime.Value = DateTime.Today.Add(sTime);
            }
            if (TimeSpan.TryParse(row.Cells["Kết Thúc"].Value.ToString(), out TimeSpan eTime))
            {
                dtpEndTime.Value = DateTime.Today.Add(eTime);
            }

            // Set Status
            string status = row.Cells["Trạng Thái"].Value?.ToString() ?? "Đã lên lịch";
            cboStatus.SelectedItem = status;

            // Set Notes
            txtNotes.Text = row.Cells["Ghi Chú"].Value?.ToString() ?? string.Empty;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int? trainerId = null;
            if (cboFilterTrainer.SelectedValue != null && int.TryParse(cboFilterTrainer.SelectedValue.ToString(), out int tid) && tid > 0)
            {
                trainerId = tid;
            }

            DateTime? fDate = null;
            if (chkFilterByDate.Checked)
            {
                fDate = dtpFilterDate.Value.Date;
            }

            LoadSchedules(trainerId, fDate);
        }

        private void btnFilterAll_Click(object sender, EventArgs e)
        {
            cboFilterTrainer.SelectedIndex = 0;
            chkFilterByDate.Checked = false;
            LoadSchedules();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtScheduleId.Clear();
            txtNotes.Clear();
            dtpScheduleDate.Value = DateTime.Today;
            dtpStartTime.Value = DateTime.Today.AddHours(8);
            dtpEndTime.Value = DateTime.Today.AddHours(9).AddMinutes(30);
            cboStatus.SelectedIndex = 0;
            if (cboMembers.Items.Count > 0) cboMembers.SelectedIndex = 0;
            if (cboTrainers.Items.Count > 0) cboTrainers.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
