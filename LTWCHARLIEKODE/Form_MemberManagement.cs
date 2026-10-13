using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_MemberManagement : Form
    {
        // Biến xác định quyền hạn
        private bool isFullAccess = false;   // True cho Admin (1) & Lễ Tân (2)
        private bool isPTRole = false;       // True cho PT (3)
        private int? currentTrainerId = null; // TrainerId của PT đang đăng nhập nếu có

        // Biến kiểm soát trạng thái chỉnh sửa và thêm mới
        private bool isEditing = false;      // True khi đang nhấn nút Chỉnh sửa
        private bool isAddingNew = false;    // True khi đang ở chế độ Thêm mới

        public Form_MemberManagement()
        {
            InitializeComponent();
        }

        #region 1. SỰ KIỆN LOAD & PHÂN QUYỀN (RBAC)

        private void Form_MemberManagement_Load(object sender, EventArgs e)
        {
            // Kiểm tra phân quyền truy cập
            if (!CheckPermission())
            {
                return;
            }

            SetupRoleInterface();
            LoadPackageFilterComboBox();
            LoadMembers();
            ResetInputForm();
        }

        /// <summary>
        /// Kiểm tra quyền truy cập của User hiện tại:
        /// - Role 1 (Admin) & Role 2 (Lễ tân): Toàn quyền
        /// - Role 3 (PT): Chỉ được xem hội viên mình hướng dẫn
        /// - Role 4 (Hội viên) hoặc role khác: Khóa truy cập
        /// </summary>
        private bool CheckPermission()
        {
            int roleId = UserSession.RoleId;

            if (roleId == 1 || roleId == 2)
            {
                isFullAccess = true;
                isPTRole = false;
                return true;
            }
            else if (roleId == 3)
            {
                isFullAccess = false;
                isPTRole = true;
                return true;
            }
            else
            {
                // Role Hội viên hoặc người dùng không có quyền
                MessageBox.Show(
                    "Tài khoản Hội viên không có quyền truy cập vào chức năng Quản lý hội viên!",
                    "Truy cập bị từ chối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                this.Close();
                return false;
            }
        }

        /// <summary>
        /// Cấu hình giao diện và trạng thái nút theo Vai trò người dùng
        /// </summary>
        private void SetupRoleInterface()
        {
            if (isFullAccess)
            {
                // ADMIN & LỄ TÂN: Toàn quyền thao tác
                lblRoleBadge.Text = $"Vai trò: {UserSession.RoleName} ({UserSession.FullName}) - Toàn quyền thao tác";
                lblRoleBadge.BackColor = Color.FromArgb(232, 244, 253);
                lblRoleBadge.ForeColor = Color.FromArgb(13, 71, 161);

                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnClear.Visible = true;
                btnGenerateQR.Visible = true;

                // Mặc định ban đầu khóa tất cả các ô ở mức chỉ xem
                SetInputFieldsReadOnly(true);
            }
            else if (isPTRole)
            {
                // PT (HUẤN LUYỆN VIÊN): Chỉ xem hội viên do mình hướng dẫn
                using (var db = new GymManagementDBEntities1())
                {
                    currentTrainerId = GetCurrentTrainerId(db);
                }

                string ptNotice = currentTrainerId.HasValue
                    ? $"Chế độ PT: {UserSession.FullName} - Chỉ xem hội viên đang hướng dẫn"
                    : $"Chế độ PT: {UserSession.FullName} (Chưa liên kết hồ sơ HLV trong danh mục)";

                lblRoleBadge.Text = ptNotice;
                lblRoleBadge.BackColor = Color.FromArgb(254, 243, 199);
                lblRoleBadge.ForeColor = Color.FromArgb(146, 64, 14);

                // Khóa hoàn toàn các chức năng thêm, sửa, xóa
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClear.Visible = false;
                btnGenerateQR.Visible = false;

                // Khóa các ô nhập dữ liệu vĩnh viễn (Chỉ xem)
                SetInputFieldsReadOnly(true);

                // Đổi vị trí nút Làm mới sang bên trái cho gọn gàng
                btnReload.Location = new Point(25, 224);
            }
        }

        /// <summary>
        /// Bật/Tắt chế độ chỉ xem (ReadOnly) cho các ô nhập thông tin hội viên
        /// </summary>
        private void SetInputFieldsReadOnly(bool isReadOnly)
        {
            txtFullName.ReadOnly = isReadOnly;
            txtPhone.ReadOnly = isReadOnly;
            txtEmail.ReadOnly = isReadOnly;
            txtQRCode.ReadOnly = isReadOnly;
            dtpCreatedDate.Enabled = !isReadOnly;
            btnGenerateQR.Enabled = !isReadOnly;

            // Đổi màu nền để người dùng nhận diện trực quan: Xám nhạt = Chỉ xem, Trắng = Cho phép chỉnh sửa
            Color fieldBgColor = isReadOnly ? Color.FromArgb(248, 249, 250) : Color.White;
            txtFullName.BackColor = fieldBgColor;
            txtPhone.BackColor = fieldBgColor;
            txtEmail.BackColor = fieldBgColor;
            txtQRCode.BackColor = fieldBgColor;
        }

        /// <summary>
        /// Tìm mã TrainerId tương ứng với tài khoản PT đang đăng nhập
        /// </summary>
        private int? GetCurrentTrainerId(GymManagementDBEntities1 db)
        {
            string userFullName = (UserSession.FullName ?? "").Trim();
            if (string.IsNullOrEmpty(userFullName)) return null;

            // 1. Tìm trùng tên chính xác
            var trainer = db.Trainers.FirstOrDefault(t => t.FullName.ToLower() == userFullName.ToLower());
            if (trainer != null) return trainer.TrainerId;

            // 2. Tìm theo tên sau khi bỏ hậu tố "(PT)", "(pt)", "PT"
            string cleanName = userFullName
                .Replace("(PT)", "")
                .Replace("(pt)", "")
                .Replace("PT", "")
                .Trim();

            if (!string.IsNullOrEmpty(cleanName))
            {
                trainer = db.Trainers.FirstOrDefault(t =>
                    t.FullName.ToLower() == cleanName.ToLower() ||
                    t.FullName.ToLower().Contains(cleanName.ToLower()) ||
                    cleanName.ToLower().Contains(t.FullName.ToLower())
                );
                if (trainer != null) return trainer.TrainerId;
            }

            return null;
        }

        #endregion

        #region 2. NẠP DỮ LIỆU & BỘ LỌC

        /// <summary>
        /// Nạp danh sách các gói tập vào ComboBox để lọc
        /// </summary>
        private void LoadPackageFilterComboBox()
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var packages = db.Packages
                        .Where(p => p.IsActive == true)
                        .Select(p => new { p.PackageId, p.PackageName })
                        .ToList();

                    var filterList = new List<dynamic>
                    {
                        new { PackageId = 0, PackageName = "-- Tất cả gói tập --" }
                    };

                    foreach (var p in packages)
                    {
                        filterList.Add(new { PackageId = p.PackageId, PackageName = p.PackageName });
                    }

                    cboPackageFilter.DataSource = filterList;
                    cboPackageFilter.DisplayMember = "PackageName";
                    cboPackageFilter.ValueMember = "PackageId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục gói tập: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải danh sách hội viên lên DataGridView.
        /// Áp dụng lọc theo quyền:
        /// - Admin/Lễ tân: Toàn bộ hội viên.
        /// - PT: Chỉ hội viên do PT này đang hướng dẫn.
        /// </summary>
        private void LoadMembers(string keyword = "", int packageIdFilter = 0)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var membersQuery = db.Members.AsQueryable();

                    // 1. Phân quyền cho PT: Chỉ lấy những hội viên mà PT đang hướng dẫn
                    if (isPTRole)
                    {
                        if (currentTrainerId.HasValue)
                        {
                            int trainerId = currentTrainerId.Value;
                            var guidedMemberIds = db.MemberPackages
                                .Where(mp => mp.TrainerId == trainerId)
                                .Select(mp => mp.MemberId)
                                .Distinct()
                                .ToList();

                            membersQuery = membersQuery.Where(m => guidedMemberIds.Contains(m.MemberId));
                        }
                        else
                        {
                            // PT chưa liên kết hồ sơ Trainer -> Không có hội viên hiển thị
                            dgvMembers.DataSource = null;
                            dgvMemberPackages.DataSource = null;
                            return;
                        }
                    }

                    // 2. Tìm kiếm theo từ khóa (Mã HV, Họ tên, SĐT, Email, Mã QR)
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        int.TryParse(kw, out int searchId);

                        membersQuery = membersQuery.Where(m =>
                            m.MemberId == searchId ||
                            m.FullName.ToLower().Contains(kw) ||
                            (m.Phone != null && m.Phone.ToLower().Contains(kw)) ||
                            (m.Email != null && m.Email.ToLower().Contains(kw)) ||
                            (m.QRCode != null && m.QRCode.ToLower().Contains(kw))
                        );
                    }

                    // 3. Lọc theo gói tập nếu người dùng chọn
                    if (packageIdFilter > 0)
                    {
                        var memberIdsInPackage = db.MemberPackages
                            .Where(mp => mp.PackageId == packageIdFilter && mp.IsActive == true)
                            .Select(mp => mp.MemberId)
                            .Distinct()
                            .ToList();

                        membersQuery = membersQuery.Where(m => memberIdsInPackage.Contains(m.MemberId));
                    }

                    // 4. Lấy dữ liệu và map ra thông tin chi tiết
                    var rawList = membersQuery
                        .OrderByDescending(m => m.MemberId)
                        .ToList();

                    var displayList = rawList.Select(m =>
                    {
                        // Lấy thông tin gói tập đang hoạt động mới nhất của hội viên
                        var activePackage = m.MemberPackages
                            .Where(mp => mp.IsActive == true)
                            .OrderByDescending(mp => mp.EndDate)
                            .FirstOrDefault();

                        string packageName = activePackage != null && activePackage.Packages != null
                            ? activePackage.Packages.PackageName
                            : "Chưa đăng ký";

                        string trainerName = activePackage != null && activePackage.Trainers != null
                            ? activePackage.Trainers.FullName
                            : (activePackage != null ? "Không có PT" : "--");

                        string statusText = "Chưa có gói";
                        if (activePackage != null)
                        {
                            if (activePackage.EndDate >= DateTime.Today)
                            {
                                statusText = $"Đang tập (Hết hạn {activePackage.EndDate:dd/MM/yyyy})";
                            }
                            else
                            {
                                statusText = $"Hết hạn ngày {activePackage.EndDate:dd/MM/yyyy}";
                            }
                        }

                        return new
                        {
                            MaHV = m.MemberId,
                            HoVaTen = m.FullName,
                            SoDienThoai = m.Phone ?? "",
                            Email = m.Email ?? "",
                            MaQR = m.QRCode ?? "",
                            NgayThamGia = m.CreatedDate.HasValue ? m.CreatedDate.Value.ToString("dd/MM/yyyy") : "",
                            GoiTapHienTai = packageName,
                            HLVHuongDan = trainerName,
                            TrangThaiGoi = statusText,
                            CreatedDateValue = m.CreatedDate ?? DateTime.Today
                        };
                    }).ToList();

                    dgvMembers.DataSource = displayList;

                    // 5. Định dạng hiển thị đúng 5 cột theo yêu cầu
                    FormatMembersGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách hội viên: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Định dạng DataGridView: Chỉ hiển thị đúng 5 cột: Mã HV, Họ và tên, SĐT, Email, Trạng thái gói tập
        /// </summary>
        private void FormatMembersGrid()
        {
            dgvMembers.ColumnHeadersHeight = 36;
            dgvMembers.RowTemplate.Height = 30;
            dgvMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvMembers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // 1. CỘT MÃ HV
            if (dgvMembers.Columns["MaHV"] != null)
            {
                dgvMembers.Columns["MaHV"].HeaderText = "Mã HV";
                dgvMembers.Columns["MaHV"].Width = 85;
                dgvMembers.Columns["MaHV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMembers.Columns["MaHV"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMembers.Columns["MaHV"].Visible = true;
            }

            // 2. CỘT HỌ VÀ TÊN
            if (dgvMembers.Columns["HoVaTen"] != null)
            {
                dgvMembers.Columns["HoVaTen"].HeaderText = "Họ và tên";
                dgvMembers.Columns["HoVaTen"].Width = 220;
                dgvMembers.Columns["HoVaTen"].Visible = true;
            }

            // 3. CỘT SỐ ĐIỆN THOẠI (SĐT)
            if (dgvMembers.Columns["SoDienThoai"] != null)
            {
                dgvMembers.Columns["SoDienThoai"].HeaderText = "SĐT";
                dgvMembers.Columns["SoDienThoai"].Width = 130;
                dgvMembers.Columns["SoDienThoai"].Visible = true;
            }

            // 4. CỘT EMAIL
            if (dgvMembers.Columns["Email"] != null)
            {
                dgvMembers.Columns["Email"].HeaderText = "Email";
                dgvMembers.Columns["Email"].Width = 210;
                dgvMembers.Columns["Email"].Visible = true;
            }

            // 5. CỘT TRẠNG THÁI GÓI TẬP (Mở rộng tự động toàn bộ chiều rộng còn lại để không bị che)
            if (dgvMembers.Columns["TrangThaiGoi"] != null)
            {
                dgvMembers.Columns["TrangThaiGoi"].HeaderText = "Trạng thái gói tập";
                dgvMembers.Columns["TrangThaiGoi"].MinimumWidth = 250;
                dgvMembers.Columns["TrangThaiGoi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMembers.Columns["TrangThaiGoi"].Visible = true;
            }

            // CÁC CỘT CÒN LẠI: ẨN KHỎI BẢNG (Dữ liệu sẽ hiển thị ở khu vực phía trên khi nhấn chọn)
            if (dgvMembers.Columns["MaQR"] != null)
            {
                dgvMembers.Columns["MaQR"].Visible = false;
            }
            if (dgvMembers.Columns["NgayThamGia"] != null)
            {
                dgvMembers.Columns["NgayThamGia"].Visible = false;
            }
            if (dgvMembers.Columns["GoiTapHienTai"] != null)
            {
                dgvMembers.Columns["GoiTapHienTai"].Visible = false;
            }
            if (dgvMembers.Columns["HLVHuongDan"] != null)
            {
                dgvMembers.Columns["HLVHuongDan"].Visible = false;
            }
            if (dgvMembers.Columns["CreatedDateValue"] != null)
            {
                dgvMembers.Columns["CreatedDateValue"].Visible = false;
            }
        }

        /// <summary>
        /// Nạp lịch sử các gói tập của hội viên được chọn vào bảng phụ
        /// </summary>
        private void LoadMemberPackageHistory(int memberId)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var packageHistory = db.MemberPackages
                        .Where(mp => mp.MemberId == memberId)
                        .OrderByDescending(mp => mp.StartDate)
                        .Select(mp => new
                        {
                            MaDangKy = mp.Id,
                            TenGoi = mp.Packages != null ? mp.Packages.PackageName : "",
                            HLVPhuTrach = mp.Trainers != null ? mp.Trainers.FullName : "Không có PT",
                            NgayBatDau = mp.StartDate,
                            NgayKetThuc = mp.EndDate,
                            TrangThai = (mp.IsActive == true)
                                ? (mp.EndDate >= DateTime.Today ? "Đang hoạt động" : "Hết hạn")
                                : "Đã hủy"
                        })
                        .ToList();

                    var formattedHistory = packageHistory.Select(p => new
                    {
                        p.MaDangKy,
                        p.TenGoi,
                        p.HLVPhuTrach,
                        NgayBatDau = p.NgayBatDau.ToString("dd/MM/yyyy"),
                        NgayKetThuc = p.NgayKetThuc.ToString("dd/MM/yyyy"),
                        p.TrangThai
                    }).ToList();

                    dgvMemberPackages.DataSource = formattedHistory;

                    if (dgvMemberPackages.Columns["MaDangKy"] != null)
                    {
                        dgvMemberPackages.Columns["MaDangKy"].HeaderText = "Mã ĐK";
                        dgvMemberPackages.Columns["MaDangKy"].Width = 70;
                    }
                    if (dgvMemberPackages.Columns["TenGoi"] != null)
                    {
                        dgvMemberPackages.Columns["TenGoi"].HeaderText = "Tên gói tập";
                        dgvMemberPackages.Columns["TenGoi"].Width = 200;
                    }
                    if (dgvMemberPackages.Columns["HLVPhuTrach"] != null)
                    {
                        dgvMemberPackages.Columns["HLVPhuTrach"].HeaderText = "HLV (PT) phụ trách";
                        dgvMemberPackages.Columns["HLVPhuTrach"].Width = 180;
                    }
                    if (dgvMemberPackages.Columns["NgayBatDau"] != null)
                    {
                        dgvMemberPackages.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                        dgvMemberPackages.Columns["NgayBatDau"].Width = 120;
                    }
                    if (dgvMemberPackages.Columns["NgayKetThuc"] != null)
                    {
                        dgvMemberPackages.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                        dgvMemberPackages.Columns["NgayKetThuc"].Width = 120;
                    }
                    if (dgvMemberPackages.Columns["TrangThai"] != null)
                    {
                        dgvMemberPackages.Columns["TrangThai"].HeaderText = "Trạng thái";
                        dgvMemberPackages.Columns["TrangThai"].Width = 130;
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi hiển thị bảng phụ
            }
        }

        /// <summary>
        /// Làm trống form nhập liệu và đưa về chế độ Chỉ xem
        /// </summary>
        private void ResetInputForm()
        {
            isEditing = false;
            isAddingNew = false;

            txtMemberId.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtQRCode.Clear();
            dtpCreatedDate.Value = DateTime.Today;
            txtCurrentPackage.Clear();
            txtCurrentTrainer.Clear();

            dgvMemberPackages.DataSource = null;

            // Luôn khóa các ô thông tin ở mức chỉ xem
            SetInputFieldsReadOnly(true);

            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = false;   // Chưa chọn hội viên thì chưa cho chỉnh sửa
                btnUpdate.Enabled = false; // Chưa bấm Chỉnh sửa thì Cập nhật bị khóa
                btnDelete.Enabled = false;
                btnClear.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnClear.Enabled = false;
            }
        }

        #endregion

        #region 3. TƯƠNG TÁC LỰA CHỌN DÒNG TRÊN BẢNG (CHẾ ĐỘ CHỈ XEM)

        private void dgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvMembers.Rows.Count) return;

            DataGridViewRow row = dgvMembers.Rows[e.RowIndex];

            // Hiển thị đầy đủ thông tin hội viên lên các ô nhập liệu phía trên
            txtMemberId.Text = row.Cells["MaHV"].Value?.ToString();
            txtFullName.Text = row.Cells["HoVaTen"].Value?.ToString();
            txtPhone.Text = row.Cells["SoDienThoai"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtQRCode.Text = row.Cells["MaQR"].Value?.ToString();

            if (row.Cells["CreatedDateValue"].Value != null &&
                DateTime.TryParse(row.Cells["CreatedDateValue"].Value.ToString(), out DateTime createdDate))
            {
                dtpCreatedDate.Value = createdDate;
            }
            else
            {
                dtpCreatedDate.Value = DateTime.Today;
            }

            txtCurrentPackage.Text = row.Cells["GoiTapHienTai"].Value?.ToString();
            txtCurrentTrainer.Text = row.Cells["HLVHuongDan"].Value?.ToString();

            // Nạp lịch sử gói tập của hội viên này vào bảng phụ
            if (int.TryParse(txtMemberId.Text, out int memberId))
            {
                LoadMemberPackageHistory(memberId);
            }

            // ĐẢM BẢO KHÓA TOÀN BỘ Ô NHẬP Ở MỨC CHỈ XEM
            isEditing = false;
            isAddingNew = false;
            SetInputFieldsReadOnly(true);

            // Cập nhật trạng thái các nút
            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;    // Kích hoạt nút Chỉnh sửa
                btnUpdate.Enabled = false; // Nút Cập nhật vẫn BỊ KHÓA cho đến khi ấn Chỉnh sửa
                btnDelete.Enabled = true;
                btnClear.Enabled = true;
            }
        }

        #endregion

        #region 4. CHỨC NĂNG CHỈNH SỬA VÀ CẬP NHẬT (QUY TRÌNH CHỈNH SỬA -> CẬP NHẬT)

        /// <summary>
        /// BƯỚC 1: Nhấn nút "Chỉnh sửa" để mở khóa các ô thông tin cho phép chỉnh sửa
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!isFullAccess)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng chỉnh sửa thông tin hội viên!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtMemberId.Text))
            {
                MessageBox.Show("Vui lòng chọn một hội viên từ danh sách trước khi nhấn Chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kích hoạt chế độ chỉnh sửa
            isEditing = true;
            isAddingNew = false;

            // Mở khóa các ô nhập liệu
            SetInputFieldsReadOnly(false);

            // Cập nhật trạng thái các nút:
            btnEdit.Enabled = false;   // Đang chỉnh sửa thì tạm khóa nút Chỉnh sửa
            btnUpdate.Enabled = true;  // MỞ KHÓA NÚT CẬP NHẬT ĐỂ NGƯỜI DÙNG LƯU
            btnAdd.Enabled = false;    // Tạm khóa thêm mới
            btnDelete.Enabled = false; // Tạm khóa xóa

            txtFullName.Focus();
        }

        /// <summary>
        /// BƯỚC 2: Nhấn nút "Cập nhật" để lưu dữ liệu vào CSDL sau khi đã ấn Chỉnh sửa
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!isFullAccess)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng cập nhật thông tin hội viên!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra bắt buộc phải ấn Chỉnh sửa trước
            if (!isEditing)
            {
                MessageBox.Show(
                    "Vui lòng nhấn nút 'Chỉnh sửa' trước khi thực hiện cập nhật thông tin!",
                    "Yêu cầu thao tác",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Vui lòng chọn hội viên cần cập nhật từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string qrCode = txtQRCode.Text.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Họ và tên hội viên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var member = db.Members.Find(memberId);
                    if (member == null)
                    {
                        MessageBox.Show("Không tìm thấy hội viên này trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra trùng mã QR với hội viên khác nếu có nhập
                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        bool qrDuplicate = db.Members.Any(m => m.MemberId != memberId && m.QRCode.ToLower() == qrCode.ToLower());
                        if (qrDuplicate)
                        {
                            MessageBox.Show($"Mã QR / Thẻ '{qrCode}' đã được sử dụng bởi hội viên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtQRCode.Focus();
                            return;
                        }
                    }

                    // Cập nhật thông tin hội viên
                    member.FullName = fullName;
                    member.Phone = string.IsNullOrEmpty(phone) ? null : phone;
                    member.Email = string.IsNullOrEmpty(email) ? null : email;
                    member.QRCode = string.IsNullOrEmpty(qrCode) ? null : qrCode;
                    member.CreatedDate = dtpCreatedDate.Value;

                    db.SaveChanges();

                    MessageBox.Show(
                        $"Cập nhật thông tin hội viên '{member.FullName}' thành công!",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Kết thúc chế độ chỉnh sửa và khóa lại các ô về mức chỉ xem
                    isEditing = false;
                    SetInputFieldsReadOnly(true);

                    // Trả lại trạng thái các nút
                    btnUpdate.Enabled = false; // Khóa lại nút Cập nhật
                    btnEdit.Enabled = true;    // Mở lại nút Chỉnh sửa
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;

                    LoadMembers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật hội viên: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 5. THÊM MỚI, XÓA, NHẬP LẠI, TẠO MÃ QR

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!isFullAccess)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng thêm hội viên!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu chưa ở chế độ Thêm mới -> Mở form trống để người dùng nhập
            if (!isAddingNew)
            {
                isAddingNew = true;
                isEditing = false;

                // Xóa trắng form nhập và mở khóa
                txtMemberId.Clear();
                txtFullName.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
                txtCurrentPackage.Clear();
                txtCurrentTrainer.Clear();
                dtpCreatedDate.Value = DateTime.Today;
                txtQRCode.Text = GenerateNewQRCode();
                dgvMemberPackages.DataSource = null;

                SetInputFieldsReadOnly(false);

                btnAdd.Text = "Lưu mới";
                btnEdit.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                txtFullName.Focus();
                return;
            }

            // Nếu đang ở chế độ Thêm mới -> Thực hiện Lưu vào CSDL
            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string qrCode = txtQRCode.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập Họ và tên hội viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            // Tự động sinh mã QR nếu để trống
            if (string.IsNullOrEmpty(qrCode))
            {
                qrCode = GenerateNewQRCode();
                txtQRCode.Text = qrCode;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    // Kiểm tra trùng số điện thoại nếu có nhập
                    if (!string.IsNullOrEmpty(phone))
                    {
                        bool phoneExists = db.Members.Any(m => m.Phone == phone);
                        if (phoneExists)
                        {
                            var confirm = MessageBox.Show(
                                $"Số điện thoại '{phone}' đã được đăng ký bởi hội viên khác!\nBạn có muốn tiếp tục thêm không?",
                                "Cảnh báo trùng số điện thoại",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question
                            );
                            if (confirm != DialogResult.Yes)
                            {
                                txtPhone.Focus();
                                return;
                            }
                        }
                    }

                    // Kiểm tra trùng mã QR
                    if (!string.IsNullOrEmpty(qrCode))
                    {
                        bool qrExists = db.Members.Any(m => m.QRCode.ToLower() == qrCode.ToLower());
                        if (qrExists)
                        {
                            MessageBox.Show($"Mã QR / Mã thẻ '{qrCode}' đã tồn tại! Vui lòng chọn hoặc sinh mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtQRCode.Focus();
                            return;
                        }
                    }

                    Members newMember = new Members
                    {
                        FullName = fullName,
                        Phone = string.IsNullOrEmpty(phone) ? null : phone,
                        Email = string.IsNullOrEmpty(email) ? null : email,
                        QRCode = qrCode,
                        CreatedDate = dtpCreatedDate.Value
                    };

                    db.Members.Add(newMember);
                    db.SaveChanges();

                    MessageBox.Show(
                        $"Thêm hội viên '{fullName}' thành công!\nMã hội viên: {newMember.MemberId}\nMã QR: {newMember.QRCode}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    isAddingNew = false;
                    btnAdd.Text = "Thêm mới";
                    SetInputFieldsReadOnly(true);

                    LoadMembers();
                    ResetInputForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm hội viên: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!isFullAccess)
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng xóa hội viên!", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var member = db.Members.Find(memberId);
                    if (member == null)
                    {
                        MessageBox.Show("Không tìm thấy hội viên cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra ràng buộc hóa đơn
                    int invoiceCount = member.Invoices.Count;
                    if (invoiceCount > 0)
                    {
                        MessageBox.Show(
                            $"Không thể xóa hội viên '{member.FullName}' vì đã có {invoiceCount} hóa đơn thanh toán trong hệ thống!\nNhằm đảm bảo tính chính xác dữ liệu doanh thu kế toán, hội viên này không được phép xóa.",
                            "Ràng buộc dữ liệu hóa đơn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                    int packageCount = member.MemberPackages.Count;

                    DialogResult confirm = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa vĩnh viễn hội viên '{member.FullName}' (Mã: {member.MemberId})?\n{(packageCount > 0 ? $"(Bao gồm {packageCount} thông tin đăng ký gói tập kèm theo)" : "")}",
                        "Xác nhận xóa hội viên",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        // Xóa các gói tập đã đăng ký liên quan nếu có
                        if (packageCount > 0)
                        {
                            db.MemberPackages.RemoveRange(member.MemberPackages);
                        }

                        db.Members.Remove(member);
                        db.SaveChanges();

                        MessageBox.Show($"Xóa hội viên '{member.FullName}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadMembers();
                        ResetInputForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa hội viên: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetInputForm();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cboPackageFilter.Items.Count > 0)
            {
                cboPackageFilter.SelectedIndex = 0;
            }
            ResetInputForm();
            LoadMembers();
        }

        private void btnGenerateQR_Click(object sender, EventArgs e)
        {
            txtQRCode.Text = GenerateNewQRCode();
        }

        private string GenerateNewQRCode()
        {
            Random rnd = new Random();
            int randNum = rnd.Next(1000, 9999);
            return $"MEM-{DateTime.Now:yyMMdd}-{randNum}";
        }

        #endregion

        #region 6. TÌM KIẾM VÀ LỌC THEO GÓI

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ExecuteSearchAndFilter();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ExecuteSearchAndFilter();
            }
        }

        private void cboPackageFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPackageFilter.SelectedValue != null && int.TryParse(cboPackageFilter.SelectedValue.ToString(), out _))
            {
                ExecuteSearchAndFilter();
            }
        }

        private void ExecuteSearchAndFilter()
        {
            string keyword = txtSearch.Text.Trim();
            int selectedPackageId = 0;

            if (cboPackageFilter.SelectedValue != null && int.TryParse(cboPackageFilter.SelectedValue.ToString(), out int pkgId))
            {
                selectedPackageId = pkgId;
            }

            LoadMembers(keyword, selectedPackageId);
        }

        #endregion
    }
}
