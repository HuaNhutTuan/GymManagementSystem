using System;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    // 1. Tìm tài khoản theo Username
                    var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

                    if (user == null)
                    {
                        MessageBox.Show($"Tên đăng nhập '{username}' không tồn tại trong hệ thống!", 
                                        "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUsername.Focus();
                        return;
                    }

                    // 2. Kiểm tra trạng thái hoạt động của tài khoản
                    if (user.IsActive != true)
                    {
                        MessageBox.Show($"Tài khoản '{username}' hiện đang bị vô hiệu hóa hoặc bị khóa!", 
                                        "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 3. Kiểm tra mật khẩu (Chuẩn mã băm SHA-256 của '123456' là 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92)
                    string inputHash = SecurityHelper.HashPassword(password);
                    string dbHash = (user.PasswordHash ?? "").Trim();

                    bool isPasswordValid = string.Equals(dbHash, inputHash, StringComparison.OrdinalIgnoreCase)
                                        || (password == "123456" && (dbHash.Equals("8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", StringComparison.OrdinalIgnoreCase)
                                                                  || dbHash.Equals("e9866e4bd1c92eb295aa112921dcbaafcda8f6ee226b9bd6058eef6412ad0fb4", StringComparison.OrdinalIgnoreCase)))
                                        || string.Equals(dbHash, password, StringComparison.OrdinalIgnoreCase);

                    if (!isPasswordValid)
                    {
                        MessageBox.Show("Mật khẩu không chính xác! Vui lòng kiểm tra lại (Mật khẩu mặc định là: 123456).", 
                                        "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.SelectAll();
                        txtPassword.Focus();
                        return;
                    }

                    // 4. Lưu phiên làm việc
                    UserSession.UserId = user.UserId;
                    UserSession.Username = user.Username;
                    UserSession.FullName = user.FullName;
                    UserSession.RoleId = user.RoleId;

                    if (user.Roles != null && !string.IsNullOrEmpty(user.Roles.RoleName))
                    {
                        UserSession.RoleName = user.Roles.RoleName;
                    }
                    else
                    {
                        var role = db.Roles.FirstOrDefault(r => r.RoleId == user.RoleId);
                        UserSession.RoleName = role != null ? role.RoleName : "";
                    }

                    MessageBox.Show($"Đăng nhập thành công! Chào mừng {user.FullName} ({UserSession.RoleName})",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 5. Mở Form_Main
                    Form_Main mainForm = new Form_Main();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form_Login_Load(object sender, EventArgs e)
        {

        }
    }
}