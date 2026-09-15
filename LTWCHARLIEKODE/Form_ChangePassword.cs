using System;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_ChangePassword : Form
    {
        public Form_ChangePassword()
        {
            InitializeComponent();
        }

        private void Form_ChangePassword_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Tài khoản: {UserSession.Username} ({UserSession.FullName})";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string currentPwd = txtCurrentPassword.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();
            string confirmPwd = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(currentPwd))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu hiện tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrentPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(newPwd))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPwd.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có độ dài từ 6 ký tự trở lên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPwd != confirmPwd)
            {
                MessageBox.Show("Xác nhận mật khẩu mới không khớp! Vui lòng nhập lại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var user = db.Users.Find(UserSession.UserId);
                    if (user == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin tài khoản người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string currentHash = SecurityHelper.HashPassword(currentPwd);
                    string dbHash = (user.PasswordHash ?? "").Trim();

                    // Kiểm tra mật khẩu hiện tại
                    bool isCurrentValid = string.Equals(dbHash, currentHash, StringComparison.OrdinalIgnoreCase)
                                       || string.Equals(dbHash, currentPwd, StringComparison.OrdinalIgnoreCase);

                    if (!isCurrentValid)
                    {
                        MessageBox.Show("Mật khẩu hiện tại không chính xác! Vui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCurrentPassword.SelectAll();
                        txtCurrentPassword.Focus();
                        return;
                    }

                    // Lưu mật khẩu mới được băm SHA-256
                    user.PasswordHash = SecurityHelper.HashPassword(newPwd);
                    db.SaveChanges();

                    MessageBox.Show("Đổi mật khẩu thành công! Vui lòng ghi nhớ mật khẩu mới cho các lần đăng nhập sau.", 
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đổi mật khẩu: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
