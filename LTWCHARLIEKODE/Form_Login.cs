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
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashStandard = SecurityHelper.HashPassword(password);
            string hashSalted = SecurityHelper.HashPasswordWithSalt(password);

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var user = db.Users.FirstOrDefault(u => u.Username == username 
                        && (u.PasswordHash == hashStandard || u.PasswordHash == hashSalted || u.PasswordHash == password) 
                        && u.IsActive == true);

                    if (user != null)
                    {
                        UserSession.UserId = user.UserId;
                        UserSession.Username = user.Username;
                        UserSession.FullName = user.FullName;
                        UserSession.RoleId = user.RoleId;
                        UserSession.RoleName = user.Roles != null ? user.Roles.RoleName : "";

                        MessageBox.Show($"Đăng nhập thành công! Chào mừng {user.FullName} ({UserSession.RoleName})",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form_Main mainForm = new Form_Main();
                        this.Hide();
                        mainForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác, hoặc tài khoản đã bị khóa!", 
                                        "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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