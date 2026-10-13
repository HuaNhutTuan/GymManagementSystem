using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_PackageManagement : Form
    {
        // Biến kiểm tra quyền: Admin (1) và Lễ Tân (2) được xem số người đăng ký và quản lý gói
        private bool isFullAccess = false;
        private bool isEditing = false; // Trạng thái đang chỉnh sửa

        public Form_PackageManagement()
        {
            InitializeComponent();
            this.dgvPackages.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvPackages_CellFormatting);
            this.txtPrice.Leave += new EventHandler(this.txtPrice_Leave);
        }

        private void Form_PackageManagement_Load(object sender, EventArgs e)
        {
            // 1. Kiểm tra và thiết lập quyền
            KiemTraPhanQuyen();

            // 2. Nạp danh sách các gói tập lên bảng
            LoadPackages();

            // 3. Đưa form về trạng thái chỉ xem ban đầu
            ResetForm();
        }

        #region 1. PHÂN QUYỀN TRUY CẬP (ROLE-BASED PERMISSION)

        // Kiểm tra quyền theo vai trò: Admin & Lễ tân thấy số người đăng ký và có quyền quản lý; PT & Hội viên chỉ xem
        private void KiemTraPhanQuyen()
        {
            int roleId = UserSession.RoleId;
            isFullAccess = (roleId == 1 || roleId == 2);

            if (isFullAccess)
            {
                lblRoleBadge.Text = "Vai trò: " + UserSession.RoleName + " (" + UserSession.FullName + ") - Xem toàn bộ & Quản lý";
                lblRoleBadge.BackColor = Color.FromArgb(232, 244, 253);
                lblRoleBadge.ForeColor = Color.FromArgb(13, 71, 161);

                // Admin & Lễ tân được xem số người đăng ký
                lblRegisteredCount.Visible = true;
                txtRegisteredCount.Visible = true;

                // Bật các nút chức năng quản lý
                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
                btnClear.Visible = true;
            }
            else
            {
                // PT hoặc Hội viên: Chỉ được xem chi tiết các gói tập, KHÔNG được xem số người đăng ký
                if (roleId == 4)
                {
                    lblRoleBadge.Text = "Hội viên: " + UserSession.FullName + " - Xem gói tập (Gói đang tập hiển thị 'Đang áp dụng')";
                    lblRoleBadge.BackColor = Color.FromArgb(237, 247, 237);
                    lblRoleBadge.ForeColor = Color.FromArgb(46, 125, 50);
                }
                else
                {
                    lblRoleBadge.Text = "Vai trò: " + UserSession.RoleName + " (" + UserSession.FullName + ") - Chế độ xem chi tiết";
                    lblRoleBadge.BackColor = Color.FromArgb(254, 243, 199);
                    lblRoleBadge.ForeColor = Color.FromArgb(146, 64, 14);
                }

                // ẨN số người đăng ký với PT và Hội viên theo yêu cầu
                lblRegisteredCount.Visible = false;
                txtRegisteredCount.Visible = false;

                // ẨN các nút thêm, sửa, xóa
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClear.Visible = false;

                btnReload.Location = new Point(25, 195);
            }
        }

        // Tìm MemberId của tài khoản Hội viên đang đăng nhập
        private int? LayMemberIdCuaHoiVien(GymManagementDBEntities1 db)
        {
            if (UserSession.RoleId != 4) return null;

            string fullName = (UserSession.FullName ?? "").Trim();
            string username = (UserSession.Username ?? "").Trim();

            // 1. Tìm chính xác theo họ và tên
            if (!string.IsNullOrEmpty(fullName))
            {
                var m = db.Members.FirstOrDefault(x => x.FullName.ToLower() == fullName.ToLower());
                if (m != null) return m.MemberId;
            }

            // 2. Tìm theo số điện thoại hoặc email nếu username trùng
            if (!string.IsNullOrEmpty(username))
            {
                var m = db.Members.FirstOrDefault(x => x.Phone == username || (x.Email != null && x.Email.ToLower() == username.ToLower()));
                if (m != null) return m.MemberId;
            }

            // 3. Tìm tương đối theo tên
            if (!string.IsNullOrEmpty(fullName))
            {
                var m = db.Members.FirstOrDefault(x => x.FullName.ToLower().Contains(fullName.ToLower()) || fullName.ToLower().Contains(x.FullName.ToLower()));
                if (m != null) return m.MemberId;
            }

            // 4. Nếu username có dạng "hoivien_an" -> bóc tách từ sau dấu gạch dưới
            if (!string.IsNullOrEmpty(username) && username.StartsWith("hoivien_"))
            {
                string suffix = username.Substring("hoivien_".Length).ToLower();
                var m = db.Members.FirstOrDefault(x => x.FullName.ToLower().Contains(suffix));
                if (m != null) return m.MemberId;
            }

            return null;
        }

        #endregion

        #region 2. QUẢN LÝ KHÓA / MỞ KHÓA NHẬP LIỆU

        // Khóa hoặc mở khóa các ô nhập liệu gói tập
        private void KhoaMoNhapLieu(bool choPhepSua)
        {
            txtPackageName.ReadOnly = !choPhepSua;
            txtDurationDays.ReadOnly = !choPhepSua;
            txtPrice.ReadOnly = !choPhepSua;
            cboPackageType.Enabled = choPhepSua;
            chkIsActive.Enabled = choPhepSua;

            Color bg = choPhepSua ? Color.White : Color.FromArgb(248, 249, 250);
            txtPackageName.BackColor = bg;
            txtDurationDays.BackColor = bg;
            txtPrice.BackColor = bg;
        }

        // Đưa form nhập liệu về trạng thái chỉ xem ban đầu
        private void ResetForm()
        {
            isEditing = false;
            txtPackageId.Clear();
            txtPackageName.Clear();
            txtDurationDays.Clear();
            txtPrice.Clear();
            txtRegisteredCount.Clear();
            cboPackageType.SelectedIndex = 0;
            chkIsActive.Checked = true;

            KhoaMoNhapLieu(false);

            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = false;   // Chưa chọn gói thì chưa cho sửa
                btnUpdate.Enabled = false; // Chưa bấm Chỉnh sửa thì Cập nhật bị khóa
                btnDelete.Enabled = false;
                btnClear.Enabled = true;
            }
        }

        #endregion

        #region 3. NẠP DỮ LIỆU & ĐỊNH DẠNG BẢNG

        // Hàm kiểm tra gói tập thuộc loại Có PT hay Không có PT
        private bool KiemTraCoPT(Packages p)
        {
            if (p == null) return false;
            // Nếu trong tên gói có chữ "Có PT" hoặc đơn giá >= 2.5 triệu thì là gói Có PT
            if (!string.IsNullOrEmpty(p.PackageName) && p.PackageName.Contains("Có PT")) return true;
            return p.Price >= 2500000;
        }

        // Hàm lấy tên gói chỉ thể hiện thời gian kéo dài (xóa bỏ chữ Có PT / Không có PT)
        private string LayTenGoiChiThoiGian(string rawName, int durationDays)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                return LayTenGoiTheoSoNgay(durationDays);
            }

            // Loại bỏ các từ khóa phân loại Có PT / Không có PT
            string name = rawName
                .Replace("(Có PT kèm)", "")
                .Replace("(Có PT)", "")
                .Replace("Có PT", "")
                .Replace("(Không có PT)", "")
                .Replace("(Không PT (Tự do))", "")
                .Replace("(Không PT)", "")
                .Replace("Không có PT", "")
                .Replace("Không PT", "")
                .Trim();

            name = name.Trim('-', ' ', '(', ')');

            if (string.IsNullOrWhiteSpace(name))
            {
                return LayTenGoiTheoSoNgay(durationDays);
            }

            return name;
        }

        // Tên gói dự phòng theo số ngày nếu chuỗi bị rỗng
        private string LayTenGoiTheoSoNgay(int durationDays)
        {
            if (durationDays <= 30) return "Gói 1 Tháng";
            if (durationDays <= 90) return "Gói 3 Tháng";
            if (durationDays <= 180) return "Gói 6 Tháng";
            return "Gói 12 Tháng";
        }

        // Nạp danh sách các gói tập lên DataGridView
        // Thứ tự hiển thị: gom theo Có PT / Không có PT, mỗi loại xếp từ trên xuống theo thời hạn (DurationDays)
        private void LoadPackages(string tuKhoa = "", int loaiGoi = 0)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var rawList = db.Packages.ToList();

                    // 1. Tìm kiếm theo tên gói
                    if (!string.IsNullOrWhiteSpace(tuKhoa))
                    {
                        string kw = tuKhoa.Trim().ToLower();
                        rawList = rawList.Where(p => 
                            (p.PackageName != null && p.PackageName.ToLower().Contains(kw)) ||
                            LayTenGoiChiThoiGian(p.PackageName, p.DurationDays).ToLower().Contains(kw)
                        ).ToList();
                    }

                    // 2. Lọc theo loại gói: 0: Tất cả, 1: Có PT, 2: Không có PT
                    if (loaiGoi == 1) // Có PT
                    {
                        rawList = rawList.Where(p => KiemTraCoPT(p)).ToList();
                    }
                    else if (loaiGoi == 2) // Không có PT
                    {
                        rawList = rawList.Where(p => !KiemTraCoPT(p)).ToList();
                    }

                    // 3. Sắp xếp thứ tự hiển thị:
                    // Nhóm Có PT trước (0) và Không có PT sau (1),
                    // Trong mỗi nhóm: Xếp từ trên xuống theo thời hạn tập luyện (DurationDays tăng dần: 30, 90, 180, 365)
                    var sortedList = rawList
                        .OrderBy(p => KiemTraCoPT(p) ? 0 : 1)
                        .ThenBy(p => p.DurationDays)
                        .ToList();

                    // Lấy danh sách ID gói tập mà Hội viên đang tập (nếu đang đăng nhập bằng role Hội viên = 4)
                    List<int> memberActivePackageIds = new List<int>();
                    if (UserSession.RoleId == 4)
                    {
                        int? memberId = LayMemberIdCuaHoiVien(db);
                        if (memberId.HasValue)
                        {
                            memberActivePackageIds = db.MemberPackages
                                .Where(mp => mp.MemberId == memberId.Value && mp.IsActive == true)
                                .Select(mp => mp.PackageId)
                                .ToList();
                        }
                    }

                    // 4. Map ra danh sách hiển thị:
                    // Tên gói: chỉ hiện thời gian kéo dài (đã xóa Có PT / Không PT)
                    // Phân loại: hiện rõ "Có PT" hoặc "Không có PT"
                    // Trạng thái: Với role Hội viên chỉ hiện "Đang áp dụng" nếu đang tập gói đó, các gói khác để trống
                    var displayList = sortedList.Select(p => 
                    {
                        string trangThaiHienThi = "";
                        if (UserSession.RoleId == 4)
                        {
                            // Role Hội viên: Nếu đang tập gói đó thì hiện "Đang áp dụng", các gói còn lại để trống
                            trangThaiHienThi = memberActivePackageIds.Contains(p.PackageId) ? "Đang áp dụng" : "";
                        }
                        else
                        {
                            // Toàn bộ các role còn lại (Admin, Lễ tân, PT): Hiện theo trạng thái gói tập trong hệ thống
                            trangThaiHienThi = (p.IsActive == true) ? "Đang áp dụng" : "Tạm ngưng";
                        }

                        return new
                        {
                            MaGoi = p.PackageId,
                            TenGoi = LayTenGoiChiThoiGian(p.PackageName, p.DurationDays), // Chỉ hiện thời gian kéo dài
                            PhanLoai = KiemTraCoPT(p) ? "Có PT" : "Không có PT",        // Hiện ở mục phân loại
                            ThoiHan = p.DurationDays + " ngày",
                            DonGia = string.Format("{0:N0} VNĐ", p.Price),
                            SoNguoiDangKy = p.MemberPackages.Count, // Số lượng người đăng ký gói này
                            TrangThai = trangThaiHienThi,

                            // Dữ liệu gốc để đổ lên form nhập liệu
                            DonGiaGoc = p.Price,
                            SoNgayGoc = p.DurationDays,
                            IsActiveGoc = p.IsActive == true,
                            IsDangTapGoc = memberActivePackageIds.Contains(p.PackageId)
                        };
                    }).ToList();

                    dgvPackages.DataSource = displayList;

                    // 5. Định dạng cột bảng DataGridView
                    FormatBangGoiTap();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục gói tập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Định dạng cột hiển thị trên DataGridView
        // Căn chỉnh độ rộng, dài đầy đủ từ ngữ và kích hoạt thanh cuộn linh hoạt cho toàn bộ roles
        private void FormatBangGoiTap()
        {
            // Bật thanh trượt cả ngang và dọc để không bao giờ bị che khuất nội dung
            dgvPackages.ScrollBars = ScrollBars.Both;
            dgvPackages.ColumnHeadersHeight = 38;
            dgvPackages.RowTemplate.Height = 34;
            dgvPackages.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvPackages.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            dgvPackages.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvPackages.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvPackages.AllowUserToResizeColumns = true;

            // 1. Mã gói
            if (dgvPackages.Columns["MaGoi"] != null)
            {
                dgvPackages.Columns["MaGoi"].HeaderText = "Mã gói";
                dgvPackages.Columns["MaGoi"].Width = 85;
                dgvPackages.Columns["MaGoi"].MinimumWidth = 70;
                dgvPackages.Columns["MaGoi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPackages.Columns["MaGoi"].Visible = true;
            }

            // 2. Tên gói tập (chỉ hiện thời gian kéo dài của gói tập)
            if (dgvPackages.Columns["TenGoi"] != null)
            {
                dgvPackages.Columns["TenGoi"].HeaderText = "Tên gói tập";
                dgvPackages.Columns["TenGoi"].Width = 180;
                dgvPackages.Columns["TenGoi"].MinimumWidth = 150;
                dgvPackages.Columns["TenGoi"].Visible = true;
            }

            // 3. Phân loại (hiện rõ Có PT / Không có PT)
            if (dgvPackages.Columns["PhanLoai"] != null)
            {
                dgvPackages.Columns["PhanLoai"].HeaderText = "Phân loại";
                dgvPackages.Columns["PhanLoai"].Width = 140;
                dgvPackages.Columns["PhanLoai"].MinimumWidth = 120;
                dgvPackages.Columns["PhanLoai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPackages.Columns["PhanLoai"].Visible = true;
            }

            // 4. Thời hạn
            if (dgvPackages.Columns["ThoiHan"] != null)
            {
                dgvPackages.Columns["ThoiHan"].HeaderText = "Thời hạn";
                dgvPackages.Columns["ThoiHan"].Width = 120;
                dgvPackages.Columns["ThoiHan"].MinimumWidth = 100;
                dgvPackages.Columns["ThoiHan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPackages.Columns["ThoiHan"].Visible = true;
            }

            // 5. Đơn giá
            if (dgvPackages.Columns["DonGia"] != null)
            {
                dgvPackages.Columns["DonGia"].HeaderText = "Đơn giá (VNĐ)";
                dgvPackages.Columns["DonGia"].Width = 160;
                dgvPackages.Columns["DonGia"].MinimumWidth = 140;
                dgvPackages.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPackages.Columns["DonGia"].Visible = true;
            }

            // 6. Số người đăng ký: CHỈ HIỂN THỊ với Admin và Lễ tân
            if (dgvPackages.Columns["SoNguoiDangKy"] != null)
            {
                dgvPackages.Columns["SoNguoiDangKy"].HeaderText = "Số người đăng ký";
                dgvPackages.Columns["SoNguoiDangKy"].Width = 150;
                dgvPackages.Columns["SoNguoiDangKy"].MinimumWidth = 140;
                dgvPackages.Columns["SoNguoiDangKy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPackages.Columns["SoNguoiDangKy"].Visible = isFullAccess; // Ẩn với PT và Hội viên!
            }

            // 7. Trạng thái
            if (dgvPackages.Columns["TrangThai"] != null)
            {
                dgvPackages.Columns["TrangThai"].HeaderText = "Trạng thái";
                dgvPackages.Columns["TrangThai"].MinimumWidth = 150;
                dgvPackages.Columns["TrangThai"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPackages.Columns["TrangThai"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPackages.Columns["TrangThai"].Visible = true;
            }

            // Ẩn các cột dữ liệu gốc
            if (dgvPackages.Columns["DonGiaGoc"] != null) dgvPackages.Columns["DonGiaGoc"].Visible = false;
            if (dgvPackages.Columns["SoNgayGoc"] != null) dgvPackages.Columns["SoNgayGoc"].Visible = false;
            if (dgvPackages.Columns["IsActiveGoc"] != null) dgvPackages.Columns["IsActiveGoc"].Visible = false;
            if (dgvPackages.Columns["IsDangTapGoc"] != null) dgvPackages.Columns["IsDangTapGoc"].Visible = false;
        }

        // Định dạng nổi bật cột Trạng thái
        private void dgvPackages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvPackages.Columns[e.ColumnIndex].Name == "TrangThai" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Đang áp dụng")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(46, 125, 50); // Xanh lá đậm nổi bật
                    e.CellStyle.Font = new Font(dgvPackages.Font, FontStyle.Bold);
                }
                else if (status == "Tạm ngưng")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(198, 40, 40); // Đỏ đậm
                }
            }
        }

        #endregion

        #region 4. SỰ KIỆN CLICK CHỌN DÒNG TRÊN BẢNG

        private void dgvPackages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvPackages.Rows.Count) return;

            DataGridViewRow row = dgvPackages.Rows[e.RowIndex];

            // Đổ thông tin lên các ô phía trên
            txtPackageId.Text = row.Cells["MaGoi"].Value?.ToString();
            txtPackageName.Text = row.Cells["TenGoi"].Value?.ToString();
            txtDurationDays.Text = row.Cells["SoNgayGoc"].Value?.ToString();

            if (row.Cells["DonGiaGoc"].Value != null)
            {
                decimal donGia = Convert.ToDecimal(row.Cells["DonGiaGoc"].Value);
                txtPrice.Text = donGia.ToString("N0");
            }

            cboPackageType.Text = row.Cells["PhanLoai"].Value?.ToString();

            // Hiển thị trạng thái áp dụng theo vai trò
            if (UserSession.RoleId == 4)
            {
                bool isDangTap = (row.Cells["TrangThai"].Value?.ToString() == "Đang áp dụng");
                chkIsActive.Text = isDangTap ? "Gói bạn đang áp dụng" : "Chưa đăng ký gói này";
                chkIsActive.Checked = isDangTap;
            }
            else
            {
                chkIsActive.Text = "Đang áp dụng";
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActiveGoc"].Value);
            }

            // Số người đăng ký chỉ hiển thị nếu là Admin hoặc Lễ tân
            if (isFullAccess && row.Cells["SoNguoiDangKy"].Value != null)
            {
                txtRegisteredCount.Text = row.Cells["SoNguoiDangKy"].Value.ToString();
            }

            // Mặc định luôn KHÓA CHỈ XEM khi click dòng
            isEditing = false;
            KhoaMoNhapLieu(false);

            if (isFullAccess)
            {
                btnAdd.Text = "Thêm mới";
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;     // Bật nút Chỉnh sửa
                btnUpdate.Enabled = false;  // Khóa nút Cập nhật
                btnDelete.Enabled = true;
                btnClear.Enabled = true;
            }
        }

        #endregion

        #region 5. QUY TRÌNH CHỈNH SỬA -> CẬP NHẬT GÓI TẬP

        // BƯỚC 1: Ấn nút "Chỉnh sửa" -> Mở khóa các ô nhập liệu
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPackageId.Text))
            {
                MessageBox.Show("Vui lòng chọn một gói tập từ danh sách trước khi chỉnh sửa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditing = true;
            KhoaMoNhapLieu(true); // Mở khóa nhập liệu

            btnEdit.Enabled = false;   // Tạm khóa Chỉnh sửa
            btnUpdate.Enabled = true;  // MỞ KHÓA NÚT CẬP NHẬT ĐỂ LƯU
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;

            txtPackageName.Focus();
        }

        // BƯỚC 2: Ấn nút "Cập nhật" -> Lưu vào CSDL và tự động khóa lại
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!isEditing)
            {
                MessageBox.Show("Vui lòng nhấn nút 'Chỉnh sửa' trước khi cập nhật!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtPackageId.Text, out int packageId))
            {
                MessageBox.Show("Vui lòng chọn một gói tập từ danh sách trước khi cập nhật!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenGoi = txtPackageName.Text.Trim();
            string soNgayStr = txtDurationDays.Text.Trim();
            string giaStr = txtPrice.Text.Trim().Replace(",", "").Replace(".", "").Replace("VNĐ", "").Replace("đ", "").Trim();

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(tenGoi))
            {
                MessageBox.Show("Tên gói tập không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPackageName.Focus();
                return;
            }

            if (!int.TryParse(soNgayStr, out int durationDays) || durationDays <= 0)
            {
                MessageBox.Show("Số ngày áp dụng phải là số nguyên lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDurationDays.Focus();
                return;
            }

            if (!decimal.TryParse(giaStr, out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá gói tập không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var package = db.Packages.Find(packageId);
                    if (package == null) return;

                    package.PackageName = tenGoi;
                    package.DurationDays = durationDays;
                    package.Price = price;
                    package.IsActive = chkIsActive.Checked;

                    db.SaveChanges();

                    MessageBox.Show("Cập nhật gói tập '" + tenGoi + "' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // TỰ ĐỘNG KHÓA LẠI VỀ CHỈ XEM
                    isEditing = false;
                    KhoaMoNhapLieu(false);

                    btnUpdate.Enabled = false; // Khóa lại nút Cập nhật
                    btnEdit.Enabled = true;    // Mở lại nút Chỉnh sửa
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;

                    LoadPackages();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật gói tập: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 6. THÊM MỚI, XÓA, NHẬP LẠI

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Lần 1: Nhấn "Thêm mới" -> Mở form trống và mở khóa nhập liệu
            if (btnAdd.Text == "Thêm mới")
            {
                txtPackageId.Clear();
                txtPackageName.Clear();
                txtDurationDays.Clear();
                txtPrice.Clear();
                txtRegisteredCount.Clear();
                chkIsActive.Checked = true;
                cboPackageType.SelectedIndex = 0;

                KhoaMoNhapLieu(true); // Mở khóa nhập
                btnAdd.Text = "Lưu mới";
                btnEdit.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                txtPackageName.Focus();
                return;
            }

            // Lần 2: Nhấn "Lưu mới" -> Lưu vào CSDL
            string tenGoi = txtPackageName.Text.Trim();
            string soNgayStr = txtDurationDays.Text.Trim();
            string giaStr = txtPrice.Text.Trim().Replace(",", "").Replace(".", "").Replace("VNĐ", "").Replace("đ", "").Trim();

            if (string.IsNullOrEmpty(tenGoi))
            {
                MessageBox.Show("Vui lòng nhập tên gói tập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPackageName.Focus();
                return;
            }

            if (!int.TryParse(soNgayStr, out int durationDays) || durationDays <= 0)
            {
                MessageBox.Show("Số ngày áp dụng phải là số nguyên dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDurationDays.Focus();
                return;
            }

            if (!decimal.TryParse(giaStr, out decimal price) || price < 0)
            {
                MessageBox.Show("Đơn giá gói tập không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    Packages packageMoi = new Packages
                    {
                        PackageName = tenGoi,
                        DurationDays = durationDays,
                        Price = price,
                        IsActive = chkIsActive.Checked
                    };

                    db.Packages.Add(packageMoi);
                    db.SaveChanges();

                    MessageBox.Show("Thêm gói tập '" + tenGoi + "' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ResetForm();
                    LoadPackages();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm gói tập: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPackageId.Text, out int packageId))
            {
                MessageBox.Show("Vui lòng chọn gói tập cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var package = db.Packages.Find(packageId);
                    if (package == null) return;

                    // Nếu gói tập đã có hội viên đăng ký hoặc có trong hóa đơn -> không cho xóa, chỉ cho tắt kích hoạt
                    int soNguoiDangKy = package.MemberPackages.Count;
                    if (soNguoiDangKy > 0)
                    {
                        MessageBox.Show("Gói tập '" + package.PackageName + "' đang có " + soNguoiDangKy + " lượt đăng ký trong hệ thống!\n" +
                                        "Để đảm bảo toàn vẹn dữ liệu hợp đồng hội viên, bạn chỉ nên chuyển trạng thái gói sang 'Tạm ngưng' thay vì xóa.",
                                        "Ràng buộc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult xacNhan = MessageBox.Show("Bạn có chắc chắn muốn xóa gói tập '" + package.PackageName + "'?",
                                                          "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (xacNhan == DialogResult.Yes)
                    {
                        db.Packages.Remove(package);
                        db.SaveChanges();

                        MessageBox.Show("Xóa gói tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ResetForm();
                        LoadPackages();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa gói tập: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cboTypeFilter.SelectedIndex = 0;
            ResetForm();
            LoadPackages();
        }

        #endregion

        #region 7. TÌM KIẾM VÀ LỌC GÓI TẬP

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

        private void cboTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void ThucHienTimKiem()
        {
            string tuKhoa = txtSearch.Text.Trim();
            int loaiGoi = cboTypeFilter.SelectedIndex; // 0: Tất cả, 1: Có PT, 2: Không có PT
            LoadPackages(tuKhoa, loaiGoi);
        }

        // Tự động định dạng lại số tiền có dấu phẩy phân cách hàng nghìn khi người dùng nhập xong và rời ô
        private void txtPrice_Leave(object sender, EventArgs e)
        {
            string giaStr = txtPrice.Text.Trim().Replace(",", "").Replace(".", "").Replace("VNĐ", "").Replace("đ", "").Trim();
            if (decimal.TryParse(giaStr, out decimal price) && price >= 0)
            {
                txtPrice.Text = price.ToString("N0");
            }
        }

        #endregion
    }
}
