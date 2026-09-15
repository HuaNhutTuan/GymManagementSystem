using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_RoleManagement : Form
    {
        public Form_RoleManagement()
        {
            InitializeComponent();
        }

        private void Form_RoleManagement_Load(object sender, EventArgs e)
        {
            LoadRoles();
            ResetInputForm();
        }

        #region 1. NẠP DỮ LIỆU

        private void LoadRoles()
        {
            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var roles = db.Roles.OrderBy(r => r.RoleId).Select(r => new
                    {
                        MaVaiTro = r.RoleId,
                        TenVaiTro = r.RoleName,
                        MoTa = r.Description,
                        SoNguoiDung = r.Users.Count()
                    }).ToList();

                    dgvRoles.DataSource = roles;

                    if (dgvRoles.Columns["MaVaiTro"] != null)
                    {
                        dgvRoles.Columns["MaVaiTro"].HeaderText = "Mã vai trò";
                        dgvRoles.Columns["MaVaiTro"].Width = 100;
                    }
                    if (dgvRoles.Columns["TenVaiTro"] != null)
                    {
                        dgvRoles.Columns["TenVaiTro"].HeaderText = "Tên vai trò";
                        dgvRoles.Columns["TenVaiTro"].Width = 180;
                    }
                    if (dgvRoles.Columns["MoTa"] != null)
                    {
                        dgvRoles.Columns["MoTa"].HeaderText = "Mô tả vai trò";
                        dgvRoles.Columns["MoTa"].Width = 350;
                    }
                    if (dgvRoles.Columns["SoNguoiDung"] != null)
                    {
                        dgvRoles.Columns["SoNguoiDung"].HeaderText = "Số người dùng";
                        dgvRoles.Columns["SoNguoiDung"].Width = 130;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục vai trò: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetInputForm()
        {
            txtRoleId.Clear();
            txtRoleName.Clear();
            txtDescription.Clear();

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        #endregion

        #region 2. TƯƠNG TÁC LỰA CHỌN DÒNG

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvRoles.Rows.Count) return;

            DataGridViewRow row = dgvRoles.Rows[e.RowIndex];

            txtRoleId.Text = row.Cells["MaVaiTro"].Value?.ToString();
            txtRoleName.Text = row.Cells["TenVaiTro"].Value?.ToString();
            txtDescription.Text = row.Cells["MoTa"].Value?.ToString();

            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        #endregion

        #region 3. THÊM, SỬA, XÓA VAI TRÒ

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string roleName = txtRoleName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    bool isExists = db.Roles.Any(r => r.RoleName.ToLower() == roleName.ToLower());
                    if (isExists)
                    {
                        MessageBox.Show($"Vai trò '{roleName}' đã tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtRoleName.Focus();
                        return;
                    }

                    Roles newRole = new Roles()
                    {
                        RoleName = roleName,
                        Description = description
                    };

                    db.Roles.Add(newRole);
                    db.SaveChanges();

                    MessageBox.Show($"Thêm vai trò '{roleName}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadRoles();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm vai trò: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtRoleId.Text, out int roleId))
            {
                MessageBox.Show("Vui lòng chọn vai trò cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roleName = txtRoleName.Text.Trim();
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Tên vai trò không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var role = db.Roles.Find(roleId);
                    if (role == null)
                    {
                        MessageBox.Show("Không tìm thấy vai trò này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra nếu đổi tên trùng với vai trò khác
                    bool isDuplicate = db.Roles.Any(r => r.RoleId != roleId && r.RoleName.ToLower() == roleName.ToLower());
                    if (isDuplicate)
                    {
                        MessageBox.Show($"Tên vai trò '{roleName}' đã được sử dụng bởi vai trò khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    role.RoleName = roleName;
                    role.Description = description;

                    db.SaveChanges();

                    MessageBox.Show($"Cập nhật vai trò '{roleName}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadRoles();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật vai trò: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtRoleId.Text, out int roleId))
            {
                MessageBox.Show("Vui lòng chọn vai trò cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (roleId == 1)
            {
                MessageBox.Show("Không thể xóa vai trò 'Admin' (Quản trị viên) mặc định của hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    var role = db.Roles.Find(roleId);
                    if (role == null) return;

                    // Kiểm tra ràng buộc khóa ngoại người dùng
                    int userCount = role.Users.Count;
                    if (userCount > 0)
                    {
                        MessageBox.Show($"Không thể xóa vai trò '{role.RoleName}' vì đang có {userCount} tài khoản người dùng thuộc vai trò này!\nVui lòng chuyển vai trò của các tài khoản trước khi xóa.", 
                                        "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa vai trò '{role.RoleName}'?", 
                        "Xác nhận xóa", 
                        MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        db.Roles.Remove(role);
                        db.SaveChanges();

                        MessageBox.Show($"Xóa vai trò '{role.RoleName}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadRoles();
                        ResetInputForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa vai trò: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetInputForm();
        }

        #endregion
    }
}
