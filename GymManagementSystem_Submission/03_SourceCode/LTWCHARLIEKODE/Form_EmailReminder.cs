using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_EmailReminder : Form
    {
        private List<ExpiringMemberDto> memberList = new List<ExpiringMemberDto>();
        private bool isSending = false;

        public Form_EmailReminder()
        {
            InitializeComponent();
        }

        private void Form_EmailReminder_Load(object sender, EventArgs e)
        {
            LoadSmtpConfigToUi();
            InitGridColumns();
            LoadExpiringMembers();
        }

        private void InitGridColumns()
        {
            dgvMembers.AutoGenerateColumns = false;
            dgvMembers.Columns.Clear();

            DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn
            {
                Name = "ColSelect",
                HeaderText = "Chọn",
                DataPropertyName = "IsSelected",
                Width = 50
            };
            dgvMembers.Columns.Add(chkCol);

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "Mã HV", DataPropertyName = "MemberId", Width = 65, ReadOnly = true });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "Họ và Tên", DataPropertyName = "FullName", Width = 150, ReadOnly = true });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColEmail", HeaderText = "Email", DataPropertyName = "Email", Width = 180, ReadOnly = true });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPhone", HeaderText = "SĐT", DataPropertyName = "Phone", Width = 100, ReadOnly = true });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPackage", HeaderText = "Gói tập", DataPropertyName = "PackageName", Width = 160, ReadOnly = true });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColEndDate", HeaderText = "Ngày hết hạn", DataPropertyName = "EndDate", Width = 105, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatus", HeaderText = "Tình trạng hạn", DataPropertyName = "Status", Width = 120, ReadOnly = true });
        }

        private void LoadSmtpConfigToUi()
        {
            var cfg = EmailReminderService.CurrentConfig;
            txtSmtpHost.Text = cfg.Host;
            txtSmtpPort.Text = cfg.Port.ToString();
            chkSmtpSsl.Checked = cfg.EnableSsl;
            txtSenderEmail.Text = cfg.SenderEmail;
            txtSenderPassword.Text = cfg.SenderPassword;
            txtDisplayName.Text = cfg.DisplayName;
        }

        private void LoadExpiringMembers()
        {
            try
            {
                int threshold = 5;
                if (cboFilterDays.SelectedIndex == 0) threshold = 3;
                else if (cboFilterDays.SelectedIndex == 1) threshold = 5;
                else if (cboFilterDays.SelectedIndex == 2) threshold = 7;
                else if (cboFilterDays.SelectedIndex == 3) threshold = 14;

                memberList = EmailReminderService.GetExpiringMembers(threshold);
                dgvMembers.DataSource = null;
                dgvMembers.DataSource = memberList;

                lblSummary.Text = $"Tìm thấy {memberList.Count} hội viên có gói tập sắp hết hạn (< {threshold} ngày) hoặc vừa hết hạn.";

                // Tô màu các dòng
                foreach (DataGridViewRow row in dgvMembers.Rows)
                {
                    if (row.DataBoundItem is ExpiringMemberDto dto)
                    {
                        if (dto.DaysLeft < 0)
                        {
                            row.DefaultCellStyle.ForeColor = Color.Crimson;
                        }
                        else if (dto.DaysLeft <= 2)
                        {
                            row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                        }
                        else
                        {
                            row.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hội viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadExpiringMembers();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (var m in memberList) m.IsSelected = true;
            dgvMembers.Refresh();
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (var m in memberList) m.IsSelected = false;
            dgvMembers.Refresh();
        }

        private async void btnSendSelected_Click(object sender, EventArgs e)
        {
            if (isSending) return;

            var targets = memberList.Where(m => m.IsSelected).ToList();
            if (targets.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất 1 hội viên để gửi email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var invalidEmailCount = targets.Count(m => string.IsNullOrWhiteSpace(m.Email) || !m.Email.Contains("@"));
            if (invalidEmailCount > 0)
            {
                var dialogResult = MessageBox.Show(
                    $"Có {invalidEmailCount} hội viên không có email hợp lệ và sẽ bị bỏ qua.\nBạn có muốn tiếp tục gửi cho các hội viên còn lại không?",
                    "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.Yes) return;
            }

            var validTargets = targets.Where(m => !string.IsNullOrWhiteSpace(m.Email) && m.Email.Contains("@")).ToList();
            if (validTargets.Count == 0)
            {
                MessageBox.Show("Không có hội viên nào có email hợp lệ để gửi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailReminderService.CurrentConfig.SenderPassword))
            {
                MessageBox.Show("Bạn chưa nhập Mật khẩu ứng dụng (App Password) trong tab 'Cấu hình SMTP'!\nVui lòng vào tab Cấu hình SMTP để điền mật khẩu trước khi gửi.",
                                "Thiếu cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tabEmail.SelectedTab = tabSmtp;
                return;
            }

            isSending = true;
            btnSendSelected.Enabled = false;
            btnRefresh.Enabled = false;
            progressBar.Maximum = validTargets.Count;
            progressBar.Value = 0;
            progressBar.Visible = true;
            txtLog.Clear();
            AppendLog($"Bắt đầu gửi email nhắc nhở cho {validTargets.Count} hội viên...");

            int successCount = 0;
            int errorCount = 0;

            await Task.Run(() =>
            {
                for (int i = 0; i < validTargets.Count; i++)
                {
                    var m = validTargets[i];
                    try
                    {
                        EmailReminderService.SendReminderEmail(m.Email, m.FullName, m.PackageName, m.EndDate, m.DaysLeft);
                        successCount++;
                        this.Invoke(new Action(() =>
                        {
                            AppendLog($"[✓] Đã gửi thành công tới: {m.FullName} ({m.Email})");
                            progressBar.Value = i + 1;
                        }));
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        this.Invoke(new Action(() =>
                        {
                            AppendLog($"[✗] Lỗi gửi tới {m.FullName} ({m.Email}): {ex.Message}");
                            progressBar.Value = i + 1;
                        }));
                    }
                }
            });

            AppendLog($"--- HOÀN TẤT: Thành công {successCount}/{validTargets.Count}, Thất bại {errorCount} ---");
            MessageBox.Show($"Quá trình gửi hoàn tất!\n• Thành công: {successCount}\n• Thất bại: {errorCount}",
                            "Kết quả gửi Email", MessageBoxButtons.OK, MessageBoxIcon.Information);

            isSending = false;
            btnSendSelected.Enabled = true;
            btnRefresh.Enabled = true;
            progressBar.Visible = false;
        }

        private void AppendLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void btnSaveSmtp_Click(object sender, EventArgs e)
        {
            try
            {
                int port = 587;
                int.TryParse(txtSmtpPort.Text.Trim(), out port);

                SmtpConfig config = new SmtpConfig
                {
                    Host = txtSmtpHost.Text.Trim(),
                    Port = port,
                    EnableSsl = chkSmtpSsl.Checked,
                    SenderEmail = txtSenderEmail.Text.Trim(),
                    SenderPassword = txtSenderPassword.Text.Trim(),
                    DisplayName = txtDisplayName.Text.Trim()
                };

                EmailReminderService.SaveConfig(config);
                MessageBox.Show("Đã lưu cấu hình máy chủ Email (SMTP) thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cấu hình SMTP: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnTestSmtp_Click(object sender, EventArgs e)
        {
            string testTo = txtTestEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(testTo))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ email nhận để thử nghiệm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTestEmail.Focus();
                return;
            }

            btnTestSmtp.Enabled = false;
            lblTestStatus.Text = "Đang kết nối SMTP và gửi email thử...";
            lblTestStatus.ForeColor = Color.Blue;

            try
            {
                // Lưu tạm cấu hình hiện tại trên form trước khi test
                btnSaveSmtp_Click(null, null);

                await Task.Run(() =>
                {
                    EmailReminderService.SendTestEmail(testTo);
                });

                lblTestStatus.Text = "✓ Gửi thử thành công! Vui lòng kiểm tra hộp thư.";
                lblTestStatus.ForeColor = Color.ForestGreen;
                MessageBox.Show("Gửi thử nghiệm thành công! Cấu hình SMTP hoàn toàn chính xác.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblTestStatus.Text = "✗ Gửi thử thất bại: " + ex.Message;
                lblTestStatus.ForeColor = Color.Crimson;
                MessageBox.Show("Gửi thử nghiệm thất bại!\nChi tiết: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestSmtp.Enabled = true;
            }
        }
    }
}
