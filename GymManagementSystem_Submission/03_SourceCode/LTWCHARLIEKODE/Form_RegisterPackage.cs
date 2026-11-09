using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_RegisterPackage : Form
    {
        private MemberPackages _currentActivePackage = null;

        public Form_RegisterPackage()
        {
            InitializeComponent();
        }

        private void Form_RegisterPackage_Load(object sender, EventArgs e)
        {
            LoadMembers();
            LoadPackages();
            LoadTrainers();
            LoadHistory();
            ResetForm();
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

        private void LoadPackages()
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var packages = db.Packages.Where(p => p.IsActive == true || p.IsActive == null)
                                             .OrderBy(p => p.PackageName)
                                             .ToList();

                    cboPackages.DataSource = packages;
                    cboPackages.DisplayMember = "PackageName";
                    cboPackages.ValueMember = "PackageId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục gói tập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        TrainerId = (int?)t.TrainerId,
                        DisplayName = t.FullName + " - Chuyên môn: " + (t.Specialty ?? "Gym/Fitness")
                    }).ToList();

                    // Thêm tùy chọn Không có HLV
                    var listWithNone = trainers.ToList();
                    listWithNone.Insert(0, new { TrainerId = (int?)null, DisplayName = "-- Không có HLV kèm (Tự tập) --" });

                    cboTrainers.DataSource = listWithNone;
                    cboTrainers.DisplayMember = "DisplayName";
                    cboTrainers.ValueMember = "TrainerId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách HLV: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 2. TÍNH TOÁN NGÀY HẾT HẠN & GIÁ TIỀN

        private void cboMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMembers.SelectedValue == null || !(cboMembers.SelectedValue is int memberId))
            {
                lblCurrentPackageInfo.Text = "Trạng thái gói: Chưa chọn hội viên";
                _currentActivePackage = null;
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    // Lấy gói tập còn hạn mới nhất của hội viên này
                    _currentActivePackage = db.MemberPackages
                        .Where(mp => mp.MemberId == memberId && mp.IsActive == true)
                        .OrderByDescending(mp => mp.EndDate)
                        .FirstOrDefault();

                    if (_currentActivePackage != null)
                    {
                        string pkgName = _currentActivePackage.Packages != null ? _currentActivePackage.Packages.PackageName : "Gói tập";
                        bool isExpired = _currentActivePackage.EndDate < DateTime.Now;

                        if (isExpired)
                        {
                            lblCurrentPackageInfo.Text = $"Gói trước đây: {pkgName} (Đã hết hạn ngày: {_currentActivePackage.EndDate:dd/MM/yyyy})";
                            lblCurrentPackageInfo.ForeColor = Color.FromArgb(180, 40, 40);
                            chkRenew.Checked = false;
                        }
                        else
                        {
                            lblCurrentPackageInfo.Text = $"Đang dùng: {pkgName} (Hạn đến: {_currentActivePackage.EndDate:dd/MM/yyyy}) - Khuyến nghị Gia hạn!";
                            lblCurrentPackageInfo.ForeColor = Color.FromArgb(0, 128, 0);
                        }
                    }
                    else
                    {
                        lblCurrentPackageInfo.Text = "Trạng thái gói: Chưa có gói nào đang hoạt động";
                        lblCurrentPackageInfo.ForeColor = Color.FromArgb(80, 80, 80);
                        chkRenew.Checked = false;
                    }
                }
            }
            catch
            {
                lblCurrentPackageInfo.Text = "Trạng thái gói: Không thể nạp thông tin";
            }

            CalculateEndDate();
        }

        private void cboPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPackages.SelectedItem is Packages pkg)
            {
                lblPackagePrice.Text = $"Đơn giá: {pkg.Price:N0} đ";
                lblPackageDuration.Text = $"Thời hạn: {pkg.DurationDays} ngày";
                lblTotalAmount.Text = $"{pkg.Price:N0} đ";
            }
            else
            {
                lblPackagePrice.Text = "Đơn giá: 0 đ";
                lblPackageDuration.Text = "Thời hạn: 0 ngày";
                lblTotalAmount.Text = "0 đ";
            }

            CalculateEndDate();
        }

        private void chkRenew_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRenew.Checked && _currentActivePackage != null && _currentActivePackage.EndDate > DateTime.Now)
            {
                // Nếu gia hạn và gói cũ còn hạn thì ngày bắt đầu nối tiếp sau ngày kết thúc cũ 1 ngày
                dtpStartDate.Value = _currentActivePackage.EndDate.AddDays(1);
            }
            else
            {
                dtpStartDate.Value = DateTime.Now;
            }

            CalculateEndDate();
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            CalculateEndDate();
        }

        private void CalculateEndDate()
        {
            if (cboPackages.SelectedItem is Packages pkg && pkg.DurationDays > 0)
            {
                dtpEndDate.Value = dtpStartDate.Value.AddDays(pkg.DurationDays);
            }
            else
            {
                dtpEndDate.Value = dtpStartDate.Value.AddDays(30);
            }
        }

        #endregion

        #region 3. NGHIỆP VỤ TRANSACTION: ĐĂNG KÝ GÓI & TẠO HÓA ĐƠN

        private void btnConfirmRegister_Click(object sender, EventArgs e)
        {
            if (cboMembers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Hội viên cần đăng ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboPackages.SelectedItem is Packages pkg))
            {
                MessageBox.Show("Vui lòng chọn Gói tập hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int memberId = (int)cboMembers.SelectedValue;
            int? trainerId = cboTrainers.SelectedValue as int?;
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;
            decimal totalAmount = pkg.Price;

            string confirmMsg = $"XÁC NHẬN ĐĂNG KÝ GÓI TẬP:\n\n" +
                               $"- Hội viên: {cboMembers.Text}\n" +
                               $"- Gói tập: {pkg.PackageName} ({pkg.DurationDays} ngày)\n" +
                               $"- Thời gian: {startDate:dd/MM/yyyy} -> {endDate:dd/MM/yyyy}\n" +
                               $"- HLV kèm: {(trainerId.HasValue ? cboTrainers.Text : "Không")}\n" +
                               $"- Tổng thanh toán: {totalAmount:N0} VNĐ\n\n" +
                               $"Hệ thống sẽ thực hiện giao dịch lưu Hóa đơn, Chi tiết hóa đơn và kích hoạt Gói tập.\nBạn có chắc chắn muốn tiến hành?";

            DialogResult dr = MessageBox.Show(confirmMsg, "Xác nhận giao dịch", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes) return;

            // BẮT ĐẦU TRANSACTION ĐẢM BẢO TOÀN VẸN DỮ LIỆU
            using (var db = new GymManagementDBEntities1())
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 1. Tạo bản ghi Hóa đơn mới (Invoices)
                    int currentUserId = UserSession.UserId > 0 ? UserSession.UserId : 1;
                    var invoice = new Invoices
                    {
                        MemberId = memberId,
                        CreatedByUserId = currentUserId,
                        TotalAmount = totalAmount,
                        CreatedDate = DateTime.Now
                    };
                    db.Invoices.Add(invoice);
                    db.SaveChanges(); // Lấy InvoiceId vừa sinh tự động

                    // 2. Tạo Chi tiết hóa đơn (InvoiceDetails)
                    var invoiceDetail = new InvoiceDetails
                    {
                        InvoiceId = invoice.InvoiceId,
                        PackageId = pkg.PackageId,
                        Price = totalAmount,
                        Quantity = 1
                    };
                    db.InvoiceDetails.Add(invoiceDetail);

                    // 3. Nếu không phải gia hạn nối tiếp, hủy kích hoạt các gói cũ cùng loại của hội viên
                    if (!chkRenew.Checked)
                    {
                        var oldPackages = db.MemberPackages.Where(mp => mp.MemberId == memberId && mp.IsActive == true).ToList();
                        foreach (var oldPkg in oldPackages)
                        {
                            oldPkg.IsActive = false;
                        }
                    }

                    // 4. Tạo bản ghi Gói tập Hội viên mới (MemberPackages)
                    var memberPackage = new MemberPackages
                    {
                        MemberId = memberId,
                        PackageId = pkg.PackageId,
                        TrainerId = trainerId,
                        StartDate = startDate,
                        EndDate = endDate,
                        IsActive = true
                    };
                    db.MemberPackages.Add(memberPackage);

                    // Lưu toàn bộ thay đổi
                    db.SaveChanges();

                    // CAM KẾT GIAO DỊCH (COMMIT)
                    transaction.Commit();

                    string successMsg = $"ĐĂNG KÝ VÀ LẬP HÓA ĐƠN THÀNH CÔNG!\n\n" +
                                       $"• Mã Hóa Đơn: HD-{invoice.InvoiceId:D5}\n" +
                                       $"• Ngày lập: {invoice.CreatedDate:dd/MM/yyyy HH:mm}\n" +
                                       $"• Số tiền đã thu: {totalAmount:N0} đ\n\n" +
                                       $"Bạn có muốn xem/in hóa đơn này không?";

                    DialogResult printDr = MessageBox.Show(successMsg, "Giao dịch thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (printDr == DialogResult.Yes)
                    {
                        // Mở danh sách hóa đơn để xem/in
                        Form_InvoiceList frmInvoice = new Form_InvoiceList(invoice.InvoiceId);
                        frmInvoice.StartPosition = FormStartPosition.CenterParent;
                        frmInvoice.ShowDialog(this);
                    }

                    // Làm mới dữ liệu
                    LoadHistory();
                    cboMembers_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    // ROLLBACK TOÀN BỘ NẾU CÓ BẤT KỲ LỖI NÀO
                    transaction.Rollback();
                    MessageBox.Show($"GIAO DỊCH THẤT BẠI!\nĐã Rollback toàn bộ dữ liệu để đảm bảo an toàn.\nChi tiết lỗi: {ex.Message}",
                                    "Lỗi Transaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region 4. LỊCH SỬ ĐĂNG KÝ & TÌM KIẾM

        private void LoadHistory(string keyword = "")
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var query = from mp in db.MemberPackages
                                join m in db.Members on mp.MemberId equals m.MemberId
                                join p in db.Packages on mp.PackageId equals p.PackageId
                                join t in db.Trainers on mp.TrainerId equals t.TrainerId into ptJoin
                                from pt in ptJoin.DefaultIfEmpty()
                                select new
                                {
                                    MaDangKy = mp.Id,
                                    HoiVien = m.FullName,
                                    SoDienThoai = m.Phone,
                                    GoiTap = p.PackageName,
                                    GiaGoi = p.Price,
                                    HLV = pt != null ? pt.FullName : "Tự tập",
                                    NgayBatDau = mp.StartDate,
                                    NgayKetThuc = mp.EndDate,
                                    TrangThai = mp.EndDate >= DateTime.Now && (mp.IsActive == true) ? "Còn hạn" : "Hết hạn"
                                };

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        query = query.Where(x => x.HoiVien.ToLower().Contains(kw) || (x.SoDienThoai != null && x.SoDienThoai.Contains(kw)));
                    }

                    var list = query.OrderByDescending(x => x.MaDangKy).ToList();

                    dgvHistory.DataSource = list;

                    if (dgvHistory.Columns["MaDangKy"] != null) dgvHistory.Columns["MaDangKy"].HeaderText = "Mã ĐK";
                    if (dgvHistory.Columns["HoiVien"] != null) dgvHistory.Columns["HoiVien"].HeaderText = "Hội Viên";
                    if (dgvHistory.Columns["SoDienThoai"] != null) dgvHistory.Columns["SoDienThoai"].HeaderText = "Số ĐT";
                    if (dgvHistory.Columns["GoiTap"] != null) dgvHistory.Columns["GoiTap"].HeaderText = "Gói Tập";
                    if (dgvHistory.Columns["GiaGoi"] != null)
                    {
                        dgvHistory.Columns["GiaGoi"].HeaderText = "Giá Gói";
                        dgvHistory.Columns["GiaGoi"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvHistory.Columns["HLV"] != null) dgvHistory.Columns["HLV"].HeaderText = "PT Kèm";
                    if (dgvHistory.Columns["NgayBatDau"] != null)
                    {
                        dgvHistory.Columns["NgayBatDau"].HeaderText = "Ngày Bắt Đầu";
                        dgvHistory.Columns["NgayBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dgvHistory.Columns["NgayKetThuc"] != null)
                    {
                        dgvHistory.Columns["NgayKetThuc"].HeaderText = "Ngày Kết Thúc";
                        dgvHistory.Columns["NgayKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dgvHistory.Columns["TrangThai"] != null) dgvHistory.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp lịch sử đăng ký gói: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadHistory(txtSearch.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadHistory();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            dtpStartDate.Value = DateTime.Now;
            chkRenew.Checked = false;
            CalculateEndDate();
            if (cboMembers.Items.Count > 0) cboMembers.SelectedIndex = 0;
            if (cboPackages.Items.Count > 0) cboPackages.SelectedIndex = 0;
            if (cboTrainers.Items.Count > 0) cboTrainers.SelectedIndex = 0;
            cboMembers_SelectedIndexChanged(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
