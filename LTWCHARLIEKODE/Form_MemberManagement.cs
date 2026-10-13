using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_MemberManagement : Form
    {
        // Biến kiểm tra quyền và trạng thái
        private bool isFullAccess = false;   // Quyền toàn phần cho Admin (RoleId = 1) và Lễ Tân (RoleId = 2)
        private bool isPTRole = false;       // Quyền huấn luyện viên PT (RoleId = 3)
        private int? currentTrainerId = null;// Mã HLV của PT đang đăng nhập
        private bool isEditing = false;      // Đang ở chế độ chỉnh sửa thông tin

        public Form_MemberManagement()
        {
            InitializeComponent();
        }

        private void Form_MemberManagement_Load(object sender, EventArgs e)
        {
            // 1. Kiểm tra phân quyền truy cập
            if (!KiemTraPhanQuyen())
            {
                return;
            }

            // 2. Thiết lập giao diện theo vai trò
            CaiDatGiaoDienTheoRole();

            // 3. Nạp danh mục gói tập vào ComboBox lọc
            LoadComboBoxGoiTap();

            // 4. Nạp danh sách hội viên lên bảng
            LoadMembers();

            // 5. Đưa form về trạng thái ban đầu (Chỉ xem)
            ResetForm();
        }

        #region 1. PHÂN QUYỀN TRUY CẬP (ROLE-BASED PERMISSION)

        // Kiểm tra quyền đăng nhập: Role 1, 2 toàn quyền; Role 3 chỉ xem; Role 4 hoặc khác bị khóa
        private bool KiemTraPhanQuyen()
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
                MessageBox.Show("Tài khoản Hội viên không có quyền truy cập vào chức năng Quản lý hội viên!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return false;
            }
        }

        // Cài đặt hiển thị các nút và nhãn quyền theo vai trò
        private void CaiDatGiaoDienTheoRole()
        {
            if (isFullAccess)
            {
                lblRoleBadge.Text = "Vai trò: " + UserSession.RoleName + " (" + UserSession.FullName + ") - Toàn quyền";
                lblRoleBadge.BackColor = Color.FromArgb(232, 244, 253);
                lblRoleBadge.ForeColor = Color.FromArgb(13, 71, 161);

                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnClear.Visible = true;
                btnGenerateQR.Visible = true;
            }
            else if (isPTRole)
            {
                using (var db = new GymManagementDBEntities1())
                {
                    currentTrainerId = LayMaTrainerCuaPT(db);
                }

                lblRoleBadge.Text = "Chế độ PT: " + UserSession.FullName + " - Chỉ xem hội viên đang hướng dẫn";
                lblRoleBadge.BackColor = Color.FromArgb(254, 243, 199);
                lblRoleBadge.ForeColor = Color.FromArgb(146, 64, 14);

                // Khóa và ẩn hoàn toàn các chức năng thêm, sửa, xóa với PT
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClear.Visible = false;
                btnGenerateQR.Visible = false;

                btnReload.Location = new Point(25, 224);
            }
        }

        // Tìm mã Trainer tương ứng với tài khoản PT đang đăng nhập
        private int? LayMaTrainerCuaPT(GymManagementDBEntities1 db)
        {
            string fullName = (UserSession.FullName ?? "").Trim();
            if (string.IsNullOrEmpty(fullName)) return null;

            // 1. Tìm đúng tên PT
            var trainer = db.Trainers.FirstOrDefault(t => t.FullName.ToLower() == fullName.ToLower());
            if (trainer != null) return trainer.TrainerId;

            // 2. Nếu tên tài khoản có gắn đuôi (PT) -> bỏ đuôi để tìm
            string cleanName = fullName.Replace("(PT)", "").Replace("PT", "").Trim().ToLower();
            trainer = db.Trainers.FirstOrDefault(t => t.FullName.ToLower().Contains(cleanName) || cleanName.Contains(t.FullName.ToLower()));
            return trainer != null ? (int?)trainer.TrainerId : null;
        }

        #endregion

        #region 2. QUẢN LÝ KHÓA / MỞ KHÓA NHẬP LIỆU

        // Khóa hoặc mở khóa các ô thông tin hội viên
        private void KhoaMoNhapLieu(bool choPhepSua)
        {
            txtFullName.ReadOnly = !choPhepSua;
            txtPhone.ReadOnly = !choPhepSua;
            txtEmail.ReadOnly = !choPhepSua;
            txtQRCode.ReadOnly = !choPhepSua;
            dtpCreatedDate.Enabled = choPhepSua;
            btnGenerateQR.Enabled = choPhepSua;

            // Đổi màu nền: Xám nhạt = Chỉ xem, Trắng = Cho phép chỉnh sửa
            Color bg = choPhepSua ? Color.White : Color.FromArgb(248, 249, 250);
            txtFullName.BackColor = bg;
            txtPhone.BackColor = bg;
            txtEmail.BackColor = bg;
            txtQRCode.BackColor = bg;
        }

        // Làm sạch form nhập và đưa về chế độ chỉ xem
        private void ResetForm()
        {
            isEditing = false;
            txtMemberId.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtQRCode.Clear();
            dtpCreatedDate.Value = DateTime.Today;
            txtCurrentPackage.Clear();
            txtCurrentTrainer.Clear();
            dgvMemberPackages.DataSource = null;

            KhoaMoNhapLieu(false);

            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = false;   // Chưa chọn hội viên thì chưa cho Chỉnh sửa
                btnUpdate.Enabled = false; // Chưa bấm Chỉnh sửa thì Cập nhật bị khóa
                btnDelete.Enabled = false;
                btnClear.Enabled = true;
            }
        }

        #endregion

        #region 3. NẠP DỮ LIỆU & HIỂN THỊ (5 CỘT TRÊN BẢNG CHÍNH)

        // Nạp danh sách các gói tập vào ComboBox để lọc
        private void LoadComboBoxGoiTap()
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var packages = db.Packages
                        .Where(p => p.IsActive == true)
                        .Select(p => new { p.PackageId, p.PackageName })
                        .ToList();

                    packages.Insert(0, new { PackageId = 0, PackageName = "-- Tất cả gói tập --" });

                    cboPackageFilter.DataSource = packages;
                    cboPackageFilter.DisplayMember = "PackageName";
                    cboPackageFilter.ValueMember = "PackageId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục gói tập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải danh sách hội viên lên DataGridView
        private void LoadMembers(string tuKhoa = "", int packageIdFilter = 0)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var query = db.Members.AsQueryable();

                    // 1. Phân quyền cho PT: Chỉ lấy hội viên do PT này đang kèm
                    if (isPTRole)
                    {
                        if (currentTrainerId.HasValue)
                        {
                            int trainerId = currentTrainerId.Value;
                            var hvCuaPT = db.MemberPackages
                                .Where(mp => mp.TrainerId == trainerId)
                                .Select(mp => mp.MemberId)
                                .Distinct()
                                .ToList();

                            query = query.Where(m => hvCuaPT.Contains(m.MemberId));
                        }
                        else
                        {
                            dgvMembers.DataSource = null;
                            dgvMemberPackages.DataSource = null;
                            return;
                        }
                    }

                    // 2. Tìm kiếm từ khóa (Mã HV, Tên, SĐT, Email, QR)
                    if (!string.IsNullOrWhiteSpace(tuKhoa))
                    {
                        string kw = tuKhoa.Trim().ToLower();
                        int.TryParse(kw, out int searchId);

                        query = query.Where(m =>
                            (searchId > 0 && m.MemberId == searchId) ||
                            m.FullName.ToLower().Contains(kw) ||
                            (m.Phone != null && m.Phone.ToLower().Contains(kw)) ||
                            (m.Email != null && m.Email.ToLower().Contains(kw)) ||
                            (m.QRCode != null && m.QRCode.ToLower().Contains(kw))
                        );
                    }

                    // 3. Lọc theo gói tập nếu có chọn
                    if (packageIdFilter > 0)
                    {
                        var hvTrongGoi = db.MemberPackages
                            .Where(mp => mp.PackageId == packageIdFilter && mp.IsActive == true)
                            .Select(mp => mp.MemberId)
                            .Distinct()
                            .ToList();

                        query = query.Where(m => hvTrongGoi.Contains(m.MemberId));
                    }

                    // 4. Lấy dữ liệu và map ra thông tin
                    var rawList = query.OrderByDescending(m => m.MemberId).ToList();

                    var displayList = rawList.Select(m =>
                    {
                        // Lấy gói tập active mới nhất của hội viên
                        var goiActive = m.MemberPackages
                            .Where(mp => mp.IsActive == true)
                            .OrderByDescending(mp => mp.EndDate)
                            .FirstOrDefault();

                        string tenGoi = goiActive != null && goiActive.Packages != null ? goiActive.Packages.PackageName : "Chưa đăng ký";
                        string tenPT = goiActive != null && goiActive.Trainers != null ? goiActive.Trainers.FullName : (goiActive != null ? "Không có PT" : "--");

                        string trangThai = "Chưa có gói";
                        if (goiActive != null)
                        {
                            trangThai = (goiActive.EndDate >= DateTime.Today)
                                ? "Đang tập (Hết hạn " + goiActive.EndDate.ToString("dd/MM/yyyy") + ")"
                                : "Hết hạn ngày " + goiActive.EndDate.ToString("dd/MM/yyyy");
                        }

                        return new
                        {
                            MaHV = m.MemberId,
                            HoVaTen = m.FullName,
                            SoDienThoai = m.Phone ?? "",
                            Email = m.Email ?? "",
                            TrangThaiGoi = trangThai,

                            // Các trường ẩn dùng khi click dòng
                            MaQR = m.QRCode ?? "",
                            GoiTapHienTai = tenGoi,
                            HLVHuongDan = tenPT,
                            CreatedDateValue = m.CreatedDate ?? DateTime.Today
                        };
                    }).ToList();

                    dgvMembers.DataSource = displayList;

                    // 5. Cấu hình bảng chỉ hiển thị đúng 5 cột
                    FormatBangHoiVien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hội viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Định dạng DataGridView: chỉ hiển thị đúng 5 cột, các cột khác ẩn đi
        private void FormatBangHoiVien()
        {
            dgvMembers.ScrollBars = ScrollBars.Both;
            dgvMembers.ColumnHeadersHeight = 38;
            dgvMembers.RowTemplate.Height = 34;
            dgvMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvMembers.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            dgvMembers.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvMembers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvMembers.AllowUserToResizeColumns = true;

            // 1. Mã HV
            if (dgvMembers.Columns["MaHV"] != null)
            {
                dgvMembers.Columns["MaHV"].HeaderText = "Mã HV";
                dgvMembers.Columns["MaHV"].Width = 85;
                dgvMembers.Columns["MaHV"].MinimumWidth = 75;
                dgvMembers.Columns["MaHV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMembers.Columns["MaHV"].Visible = true;
            }

            // 2. Họ và tên
            if (dgvMembers.Columns["HoVaTen"] != null)
            {
                dgvMembers.Columns["HoVaTen"].HeaderText = "Họ và tên";
                dgvMembers.Columns["HoVaTen"].Width = 220;
                dgvMembers.Columns["HoVaTen"].MinimumWidth = 180;
                dgvMembers.Columns["HoVaTen"].Visible = true;
            }

            // 3. SĐT
            if (dgvMembers.Columns["SoDienThoai"] != null)
            {
                dgvMembers.Columns["SoDienThoai"].HeaderText = "SĐT";
                dgvMembers.Columns["SoDienThoai"].Width = 130;
                dgvMembers.Columns["SoDienThoai"].MinimumWidth = 110;
                dgvMembers.Columns["SoDienThoai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMembers.Columns["SoDienThoai"].Visible = true;
            }

            // 4. Email
            if (dgvMembers.Columns["Email"] != null)
            {
                dgvMembers.Columns["Email"].HeaderText = "Email";
                dgvMembers.Columns["Email"].Width = 210;
                dgvMembers.Columns["Email"].MinimumWidth = 170;
                dgvMembers.Columns["Email"].Visible = true;
            }

            // 5. Trạng thái gói tập (Fill hết phần còn lại để không bao giờ bị cắt chữ)
            if (dgvMembers.Columns["TrangThaiGoi"] != null)
            {
                dgvMembers.Columns["TrangThaiGoi"].HeaderText = "Trạng thái gói tập";
                dgvMembers.Columns["TrangThaiGoi"].MinimumWidth = 220;
                dgvMembers.Columns["TrangThaiGoi"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMembers.Columns["TrangThaiGoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvMembers.Columns["TrangThaiGoi"].Visible = true;
            }

            // Ẩn tất cả các cột còn lại
            if (dgvMembers.Columns["MaQR"] != null) dgvMembers.Columns["MaQR"].Visible = false;
            if (dgvMembers.Columns["GoiTapHienTai"] != null) dgvMembers.Columns["GoiTapHienTai"].Visible = false;
            if (dgvMembers.Columns["HLVHuongDan"] != null) dgvMembers.Columns["HLVHuongDan"].Visible = false;
            if (dgvMembers.Columns["CreatedDateValue"] != null) dgvMembers.Columns["CreatedDateValue"].Visible = false;
        }

        // Nạp lịch sử các gói tập của hội viên vào bảng phụ
        private void LoadLichSuGoiTap(int memberId)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var history = db.MemberPackages
                        .Where(mp => mp.MemberId == memberId)
                        .OrderByDescending(mp => mp.StartDate)
                        .ToList()
                        .Select(mp => new
                        {
                            MaDangKy = mp.Id,
                            TenGoi = mp.Packages != null ? mp.Packages.PackageName : "",
                            HLVPhuTrach = mp.Trainers != null ? mp.Trainers.FullName : "Không có PT",
                            NgayBatDau = mp.StartDate.ToString("dd/MM/yyyy"),
                            NgayKetThuc = mp.EndDate.ToString("dd/MM/yyyy"),
                            TrangThai = (mp.IsActive == true)
                                ? (mp.EndDate >= DateTime.Today ? "Đang hoạt động" : "Hết hạn")
                                : "Đã hủy"
                        }).ToList();

                    dgvMemberPackages.DataSource = history;

                    dgvMemberPackages.ScrollBars = ScrollBars.Both;
                    dgvMemberPackages.ColumnHeadersHeight = 36;
                    dgvMemberPackages.RowTemplate.Height = 32;
                    dgvMemberPackages.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvMemberPackages.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                    dgvMemberPackages.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    dgvMemberPackages.AllowUserToResizeColumns = true;

                    if (dgvMemberPackages.Columns["MaDangKy"] != null)
                    {
                        dgvMemberPackages.Columns["MaDangKy"].HeaderText = "Mã ĐK";
                        dgvMemberPackages.Columns["MaDangKy"].Width = 75;
                        dgvMemberPackages.Columns["MaDangKy"].MinimumWidth = 65;
                        dgvMemberPackages.Columns["MaDangKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dgvMemberPackages.Columns["TenGoi"] != null)
                    {
                        dgvMemberPackages.Columns["TenGoi"].HeaderText = "Tên gói tập";
                        dgvMemberPackages.Columns["TenGoi"].Width = 160;
                        dgvMemberPackages.Columns["TenGoi"].MinimumWidth = 140;
                    }
                    if (dgvMemberPackages.Columns["HLVPhuTrach"] != null)
                    {
                        dgvMemberPackages.Columns["HLVPhuTrach"].HeaderText = "HLV (PT) phụ trách";
                        dgvMemberPackages.Columns["HLVPhuTrach"].Width = 160;
                        dgvMemberPackages.Columns["HLVPhuTrach"].MinimumWidth = 140;
                    }
                    if (dgvMemberPackages.Columns["NgayBatDau"] != null)
                    {
                        dgvMemberPackages.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                        dgvMemberPackages.Columns["NgayBatDau"].Width = 115;
                        dgvMemberPackages.Columns["NgayBatDau"].MinimumWidth = 100;
                        dgvMemberPackages.Columns["NgayBatDau"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dgvMemberPackages.Columns["NgayKetThuc"] != null)
                    {
                        dgvMemberPackages.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                        dgvMemberPackages.Columns["NgayKetThuc"].Width = 115;
                        dgvMemberPackages.Columns["NgayKetThuc"].MinimumWidth = 100;
                        dgvMemberPackages.Columns["NgayKetThuc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dgvMemberPackages.Columns["TrangThai"] != null)
                    {
                        dgvMemberPackages.Columns["TrangThai"].HeaderText = "Trạng thái";
                        dgvMemberPackages.Columns["TrangThai"].MinimumWidth = 130;
                        dgvMemberPackages.Columns["TrangThai"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvMemberPackages.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi nếu hội viên chưa có gói tập hợp lệ
                System.Diagnostics.Debug.WriteLine("Lỗi nạp lịch sử gói: " + ex.Message);
            }
        }

        #endregion

        #region 4. SỰ KIỆN CLICK CHỌN DÒNG TRÊN BẢNG

        private void dgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvMembers.Rows.Count) return;

            DataGridViewRow row = dgvMembers.Rows[e.RowIndex];

            // Hiển thị thông tin lên các ô phía trên
            txtMemberId.Text = row.Cells["MaHV"].Value?.ToString();
            txtFullName.Text = row.Cells["HoVaTen"].Value?.ToString();
            txtPhone.Text = row.Cells["SoDienThoai"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtQRCode.Text = row.Cells["MaQR"].Value?.ToString();

            if (DateTime.TryParse(row.Cells["CreatedDateValue"].Value?.ToString(), out DateTime dt))
                dtpCreatedDate.Value = dt;
            else
                dtpCreatedDate.Value = DateTime.Today;

            txtCurrentPackage.Text = row.Cells["GoiTapHienTai"].Value?.ToString();
            txtCurrentTrainer.Text = row.Cells["HLVHuongDan"].Value?.ToString();

            // Nạp lịch sử gói tập vào bảng phụ
            if (int.TryParse(txtMemberId.Text, out int memberId))
            {
                LoadLichSuGoiTap(memberId);
            }

            // Mặc định luôn KHÓA CHỈ XEM khi click chọn dòng
            isEditing = false;
            KhoaMoNhapLieu(false);

            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;     // Bật nút Chỉnh sửa
                btnUpdate.Enabled = false;  // Khóa nút Cập nhật (phải ấn Chỉnh sửa trước)
                btnDelete.Enabled = true;
                btnClear.Enabled = true;
            }
        }

        #endregion

        #region 5. QUY TRÌNH CHỈNH SỬA -> CẬP NHẬT

        // BƯỚC 1: Ấn nút "Chỉnh sửa" -> Mở khóa các ô nhập liệu
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMemberId.Text))
            {
                MessageBox.Show("Vui lòng chọn một hội viên từ danh sách trước khi chỉnh sửa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = true;
            KhoaMoNhapLieu(true); // Mở khóa cho phép sửa

            btnEdit.Enabled = false;   // Tạm khóa Chỉnh sửa
            btnUpdate.Enabled = true;  // MỞ KHÓA NÚT CẬP NHẬT ĐỂ LƯU
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;

            txtFullName.Focus();
        }

        // BƯỚC 2: Ấn nút "Cập nhật" -> Lưu vào CSDL và tự động khóa lại
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Bắt buộc phải qua nút Chỉnh sửa trước
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút 'Chỉnh sửa' trước khi cập nhật!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtMemberId.Text, out int memberId))
            {
                MessageBox.Show("Vui lòng chọn một hội viên từ danh sách trước khi cập nhật!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hoTen = txtFullName.Text.Trim();
            string sdt = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string maQR = txtQRCode.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Họ và tên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var member = db.Members.Find(memberId);
                    if (member == null) return;

                    // Kiểm tra trùng mã QR với hội viên khác
                    if (!string.IsNullOrEmpty(maQR))
                    {
                        bool trungQR = db.Members.Any(m => m.MemberId != memberId && m.QRCode.ToLower() == maQR.ToLower());
                        if (trungQR)
                        {
                            MessageBox.Show("Mã QR '" + maQR + "' đã được sử dụng bởi hội viên khác!",
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtQRCode.Focus();
                            return;
                        }
                    }

                    // Cập nhật thông tin
                    member.FullName = hoTen;
                    member.Phone = string.IsNullOrEmpty(sdt) ? null : sdt;
                    member.Email = string.IsNullOrEmpty(email) ? null : email;
                    member.QRCode = string.IsNullOrEmpty(maQR) ? null : maQR;
                    member.CreatedDate = dtpCreatedDate.Value;

                    db.SaveChanges();

                    MessageBox.Show("Cập nhật hội viên '" + hoTen + "' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // TỰ ĐỘNG KHÓA LẠI VỀ CHỈ XEM
                    isEditing = false;
                    KhoaMoNhapLieu(false);

                    btnUpdate.Enabled = false; // Khóa lại nút Cập nhật
                    btnEdit.Enabled = true;    // Mở lại nút Chỉnh sửa
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;

                    LoadMembers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 6. THÊM MỚI, XÓA, NHẬP LẠI, TẠO MÃ QR

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Lần 1: Nhấn "Thêm mới" -> Mở form trống và mở khóa nhập
            if (btnAdd.Text == "Thêm mới")
            {
                txtMemberId.Clear();
                txtFullName.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
                txtCurrentPackage.Clear();
                txtCurrentTrainer.Clear();
                dtpCreatedDate.Value = DateTime.Today;
                txtQRCode.Text = "MEM-" + DateTime.Now.ToString("yyMMdd") + "-" + (new Random()).Next(1000, 9999);
                dgvMemberPackages.DataSource = null;

                KhoaMoNhapLieu(true); // Mở khóa để nhập
                btnAdd.Text = "Lưu mới";
                btnEdit.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                txtFullName.Focus();
                return;
            }

            // Lần 2: Nhấn "Lưu mới" -> Lưu vào CSDL
            string hoTen = txtFullName.Text.Trim();
            string sdt = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string maQR = txtQRCode.Text.Trim();

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên hội viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(maQR))
            {
                maQR = "MEM-" + DateTime.Now.ToString("yyMMdd") + "-" + (new Random()).Next(1000, 9999);
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    // Kiểm tra trùng mã QR
                    if (db.Members.Any(m => m.QRCode.ToLower() == maQR.ToLower()))
                    {
                        MessageBox.Show("Mã QR '" + maQR + "' đã tồn tại, vui lòng đổi mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Members memberMoi = new Members
                    {
                        FullName = hoTen,
                        Phone = string.IsNullOrEmpty(sdt) ? null : sdt,
                        Email = string.IsNullOrEmpty(email) ? null : email,
                        QRCode = maQR,
                        CreatedDate = dtpCreatedDate.Value
                    };

                    db.Members.Add(memberMoi);
                    db.SaveChanges();

                    MessageBox.Show("Thêm hội viên '" + hoTen + "' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
                    LoadMembers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm hội viên: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
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
                    if (member == null) return;

                    // Không cho xóa nếu đã có hóa đơn
                    if (member.Invoices.Count > 0)
                    {
                        MessageBox.Show("Không thể xóa hội viên '" + member.FullName + "' vì đã phát sinh hóa đơn trong hệ thống!",
                                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult xacNhan = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên '" + member.FullName + "'?",
                                                          "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (xacNhan == DialogResult.Yes)
                    {
                        if (member.MemberPackages.Count > 0)
                        {
                            db.MemberPackages.RemoveRange(member.MemberPackages);
                        }

                        db.Members.Remove(member);
                        db.SaveChanges();

                        MessageBox.Show("Xóa hội viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ResetForm();
                        LoadMembers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa hội viên: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            if (cboPackageFilter.Items.Count > 0)
            {
                cboPackageFilter.SelectedIndex = 0;
            }
            ResetForm();
            LoadMembers();
        }

        private void btnGenerateQR_Click(object sender, EventArgs e)
        {
            txtQRCode.Text = "MEM-" + DateTime.Now.ToString("yyMMdd") + "-" + (new Random()).Next(1000, 9999);
        }

        #endregion

        #region 7. TÌM KIẾM VÀ LỌC THEO GÓI

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ThucHienTimKiem();
            }
        }

        private void cboPackageFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPackageFilter.SelectedValue != null && int.TryParse(cboPackageFilter.SelectedValue.ToString(), out _))
            {
                ThucHienTimKiem();
            }
        }

        private void ThucHienTimKiem()
        {
            string tuKhoa = txtSearch.Text.Trim();
            int packageId = 0;
            if (cboPackageFilter.SelectedValue != null && int.TryParse(cboPackageFilter.SelectedValue.ToString(), out int pId))
            {
                packageId = pId;
            }
            LoadMembers(tuKhoa, packageId);
        }

        #endregion
    }
}
