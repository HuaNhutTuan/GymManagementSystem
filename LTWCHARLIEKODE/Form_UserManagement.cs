using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_UserManagement : Form
    {
        public Form_UserManagement()
        {
            InitializeComponent();
        }

        private void Form_UserManagement_Load(object sender, EventArgs e)
        {
            LoadRoles();
            LoadUsers();
            ResetInputForm();
        }

        #region 1. NẠP DỮ LIỆU

        private void LoadRoles()
        {
            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var roles = db.Roles.Select(r => new { r.RoleId, r.RoleName }).ToList();
                    cboRoles.DataSource = roles;
                    cboRoles.DisplayMember = "RoleName";
                    cboRoles.ValueMember = "RoleId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục vai trò: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers(string keyword = "")
        {
            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var query = db.Users.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        query = query.Where(u => u.Username.ToLower().Contains(kw) || u.FullName.ToLower().Contains(kw));
                    }

                    var list = query.OrderBy(u => u.UserId).Select(u => new
                    {
                        MaNV = u.UserId,
                        TenDangNhap = u.Username,
                        HoVaTen = u.FullName,
                        VaiTro = u.Roles != null ? u.Roles.RoleName : "",
                        TrangThai = (u.IsActive == true) ? "Hoạt động" : "Đã khóa",
                        RoleId = u.RoleId,
                        IsActive = u.IsActive
                    }).ToList();

                    dgvUsers.DataSource = list;

                    // Định dạng cột DataGridView
                    if (dgvUsers.Columns["RoleId"] != null) dgvUsers.Columns["RoleId"].Visible = false;
                    if (dgvUsers.Columns["IsActive"] != null) dgvUsers.Columns["IsActive"].Visible = false;

                    if (dgvUsers.Columns["MaNV"] != null)
                    {
                        dgvUsers.Columns["MaNV"].HeaderText = "Mã NV";
                        dgvUsers.Columns["MaNV"].Width = 80;
                    }
                    if (dgvUsers.Columns["TenDangNhap"] != null)
                    {
                        dgvUsers.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
                        dgvUsers.Columns["TenDangNhap"].Width = 160;
                    }
                    if (dgvUsers.Columns["HoVaTen"] != null)
                    {
                        dgvUsers.Columns["HoVaTen"].HeaderText = "Họ và tên";
                        dgvUsers.Columns["HoVaTen"].Width = 220;
                    }
                    if (dgvUsers.Columns["VaiTro"] != null)
                    {
                        dgvUsers.Columns["VaiTro"].HeaderText = "Vai trò";
                        dgvUsers.Columns["VaiTro"].Width = 140;
                    }
                    if (dgvUsers.Columns["TrangThai"] != null)
                    {
                        dgvUsers.Columns["TrangThai"].HeaderText = "Trạng thái";
                        dgvUsers.Columns["TrangThai"].Width = 120;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetInputForm()
        {
            txtUserId.Clear();
            txtUsername.Clear();
            txtUsername.ReadOnly = false;
            txtFullName.Clear();
            chkIsActive.Checked = true;
            if (cboRoles.Items.Count > 0)
            {
                cboRoles.SelectedIndex = 0;
            }
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnToggleActive.Enabled = false;
            btnResetPassword.Enabled = false;
        }

        #endregion

        #region 2. TƯƠNG TÁC LỰA CHỌN DÒNG

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvUsers.Rows.Count) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            txtUserId.Text = row.Cells["MaNV"].Value?.ToString();
            txtUsername.Text = row.Cells["TenDangNhap"].Value?.ToString();
            txtUsername.ReadOnly = true; // Không cho sửa Username khi đã tạo
            txtFullName.Text = row.Cells["HoVaTen"].Value?.ToString();

            if (row.Cells["RoleId"].Value != null && int.TryParse(row.Cells["RoleId"].Value.ToString(), out int roleId))
            {
                cboRoles.SelectedValue = roleId;
            }

            if (row.Cells["IsActive"].Value != null && bool.TryParse(row.Cells["IsActive"].Value.ToString(), out bool isActive))
            {
                chkIsActive.Checked = isActive;
            }

            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnToggleActive.Enabled = true;
            btnResetPassword.Enabled = true;
        }

        #endregion

        #region 3. CHỨC NĂNG THÊM, SỬA, KHÓA, RESET MẬT KHẨU

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (username.Contains(" "))
            {
                MessageBox.Show("Tên đăng nhập không được chứa khoảng trắng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (cboRoles.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò cho tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int roleId = (int)cboRoles.SelectedValue;

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    // Kiểm tra trùng tên đăng nhập
                    bool isExists = db.Users.Any(u => u.Username.ToLower() == username.ToLower());
                    if (isExists)
                    {
                        MessageBox.Show($"Tên đăng nhập '{username}' đã tồn tại! Vui lòng chọn tên khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        return;
                    }

                    // Mật khẩu mặc định 123456
                    string defaultHash = SecurityHelper.HashPassword("123456");

                    Users newUser = new Users()
                    {
                        Username = username,
                        PasswordHash = defaultHash,
                        FullName = fullName,
                        RoleId = roleId,
                        IsActive = chkIsActive.Checked
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show($"Thêm tài khoản '{username}' thành công!\nMật khẩu mặc định là: 123456", 
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm người dùng: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserId.Text, out int userId))
            {
                MessageBox.Show("Vui lòng chọn một tài khoản từ danh sách để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullName = txtFullName.Text.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Họ và tên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            int roleId = (int)cboRoles.SelectedValue;

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var user = db.Users.Find(userId);
                    if (user == null)
                    {
                        MessageBox.Show("Không tìm thấy người dùng này trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    user.FullName = fullName;
                    user.RoleId = roleId;
                    user.IsActive = chkIsActive.Checked;

                    db.SaveChanges();

                    MessageBox.Show($"Cập nhật tài khoản '{user.Username}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật người dùng: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserId.Text, out int userId))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần khóa hoặc mở khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var user = db.Users.Find(userId);
                    if (user == null) return;

                    // Không cho phép tự khóa tài khoản Admin đang đăng nhập
                    if (user.UserId == UserSession.UserId)
                    {
                        MessageBox.Show("Bạn không thể tự khóa tài khoản của chính mình!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool currentStatus = user.IsActive == true;
                    user.IsActive = !currentStatus;

                    db.SaveChanges();

                    string msg = user.IsActive == true 
                        ? $"Đã mở khóa tài khoản '{user.Username}'!" 
                        : $"Đã khóa tài khoản '{user.Username}'!";

                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thay đổi trạng thái tài khoản: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtUserId.Text, out int userId))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần đặt lại mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn đặt lại mật khẩu của tài khoản '{txtUsername.Text}' về '123456'?",
                "Xác nhận đặt lại mật khẩu",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var user = db.Users.Find(userId);
                    if (user == null) return;

                    user.PasswordHash = SecurityHelper.HashPassword("123456");
                    db.SaveChanges();

                    MessageBox.Show($"Đặt lại mật khẩu cho '{user.Username}' thành công!\nMật khẩu mới là: 123456", 
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadUsers();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đặt lại mật khẩu: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ResetInputForm();
            LoadUsers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearch.Text);
        }

        #endregion
    }
}
