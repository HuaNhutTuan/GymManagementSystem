using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_Welcome : Form
    {
        public Form_Welcome()
        {
            InitializeComponent();
        }

        private void Form_Welcome_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ApplyWelcomeRole(UserSession.RoleId);
            LoadSystemStatisticsAsync();
        }

        /// <summary>
        /// Tùy biến tiêu đề và giao diện Chào mừng theo đúng Vai trò đăng nhập
        /// RoleId = 1: Admin
        /// RoleId = 4: Khách hàng / Hội viên
        /// RoleId = 2: Lễ tân
        /// RoleId = 3: Huấn luyện viên
        /// </summary>
        private void ApplyWelcomeRole(int roleId)
        {
            string fullName = string.IsNullOrEmpty(UserSession.FullName) ? "Người dùng" : UserSession.FullName;
            string username = string.IsNullOrEmpty(UserSession.Username) ? "" : UserSession.Username;
            string dateStr = DateTime.Now.ToString("dddd, 'ngày' dd/MM/yyyy", new System.Globalization.CultureInfo("vi-VN"));

            switch (roleId)
            {
                case 1:
                    // ADMIN (QUẢN TRỊ VIÊN)
                    lblGreetingHeader.Text = "CHÀO MỪNG QUẢN TRỊ VIÊN HỆ THỐNG (ADMIN)";
                    lblGreetingHeader.ForeColor = Color.FromArgb(255, 230, 100);
                    lblGreetingSub.Text = $"Xin chào Quản trị viên: {fullName} ({username})  |  Vai trò: Toàn quyền Quản trị  |  {dateStr}";
                    lblQuote.Text = "★ Chúc bạn một ngày quản trị và điều hành trung tâm thể thao đạt hiệu quả tối ưu!";
                    
                    // Admin có toàn quyền tất cả chức năng
                    btnActionMembers.Visible = true;
                    btnActionPackages.Visible = true;
                    btnActionCheckIn.Visible = true;
                    btnActionPTSchedule.Visible = true;
                    btnActionInvoices.Visible = true;
                    btnActionRevenue.Visible = true;
                    btnActionCatalog.Visible = true;
                    btnActionPassword.Visible = true;
                    break;

                case 4:
                    // KHÁCH HÀNG / HỘI VIÊN
                    lblGreetingHeader.Text = "CHÀO MỪNG QUÝ KHÁCH HÀNG / HỘI VIÊN";
                    lblGreetingHeader.ForeColor = Color.FromArgb(120, 220, 255);
                    lblGreetingSub.Text = $"Kính chào Quý khách: {fullName} ({username})  |  {dateStr}";
                    lblQuote.Text = "★ \"Chúc quý khách có những buổi tập tràn đầy năng lượng và đạt được vóc dáng mong ước!\"";

                    // Khách hàng: Ẩn các nút quản lý tài chính và nhân sự
                    btnActionMembers.Visible = false;
                    btnActionPackages.Visible = false;
                    btnActionInvoices.Visible = false;
                    btnActionRevenue.Visible = false;

                    // Tùy biến nội dung các nút dành cho khách hàng
                    btnActionCheckIn.Text = "Điểm danh QR\r\n\r\nQuét mã QR cá nhân để vào phòng tập luyện";
                    btnActionPTSchedule.Text = "Lịch tập của Tôi\r\n\r\nXem lịch hẹn tập cùng Huấn luyện viên cá nhân";
                    btnActionCatalog.Text = "Khám phá Gói tập\r\n\r\nXem bảng giá và quyền lợi các gói Gym & Fitness";
                    btnActionPassword.Text = "Đổi Mật khẩu\r\n\r\nCập nhật mật khẩu bảo vệ tài khoản hội viên";
                    break;

                case 2:
                    // NHÂN VIÊN LỄ TÂN
                    lblGreetingHeader.Text = "CHÀO MỪNG NHÂN VIÊN LỄ TÂN";
                    lblGreetingHeader.ForeColor = Color.FromArgb(180, 240, 160);
                    lblGreetingSub.Text = $"Xin chào: {fullName} ({username})  |  Bộ phận Lễ tân & Chăm sóc khách hàng  |  {dateStr}";
                    lblQuote.Text = "★ Nụ cười và sự tận tâm của bạn mang lại trải nghiệm tuyệt vời nhất cho từng hội viên!";

                    btnActionRevenue.Visible = false; // Lễ tân không xem báo cáo tài chính cấp cao
                    break;

                case 3:
                    // HUẤN LUYỆN VIÊN (PT)
                    lblGreetingHeader.Text = "CHÀO MỪNG HUẤN LUYỆN VIÊN CÁ NHÂN (PT)";
                    lblGreetingHeader.ForeColor = Color.FromArgb(255, 180, 100);
                    lblGreetingSub.Text = $"Xin chào HLV: {fullName} ({username})  |  Đội ngũ Huấn luyện viên Chuyên nghiệp  |  {dateStr}";
                    lblQuote.Text = "★ Đồng hành và truyền cảm hứng bứt phá giới hạn thể chất đến từng hội viên!";

                    btnActionRevenue.Visible = false;
                    btnActionInvoices.Visible = false;
                    btnActionPackages.Visible = false;
                    btnActionPTSchedule.Text = "Lịch dạy Huấn luyện\r\n\r\nKiểm tra lịch dạy hôm nay và danh sách học viên";
                    break;

                default:
                    // Mặc định
                    lblGreetingHeader.Text = "CHÀO MỪNG ĐẾN VỚI HỆ THỐNG GYM & FITNESS";
                    lblGreetingSub.Text = $"Xin chào: {fullName}  |  {dateStr}";
                    lblQuote.Text = "★ Chúc bạn một ngày tập luyện và làm việc tràn đầy năng lượng!";
                    break;
            }
        }

        /// <summary>
        /// Nạp các số liệu thống kê trung tâm từ Database bất đồng bộ để không treo giao diện
        /// </summary>
        private async void LoadSystemStatisticsAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                    {
                        int memberCount = 0;
                        int packageCount = 0;
                        int trainerCount = 0;
                        int equipCount = 0;

                        try { memberCount = db.Members.Count(); } catch { }
                        try { packageCount = db.Packages.Count(p => p.IsActive == true); } catch { }
                        try { trainerCount = db.Trainers.Count(); } catch { }
                        try { equipCount = db.Equipments.Count(); } catch { }

                        this.Invoke(new Action(() =>
                        {
                            lblCountMembers.Text = memberCount.ToString("N0");
                            lblCountPackages.Text = packageCount.ToString("N0");
                            lblCountTrainers.Text = trainerCount.ToString("N0");
                            lblCountEquipments.Text = equipCount.ToString("N0");
                        }));
                    }
                });
            }
            catch
            {
                // Nếu đang offline hoặc lỗi kết nối, giữ hiển thị an toàn
                lblCountMembers.Text = "--";
                lblCountPackages.Text = "--";
                lblCountTrainers.Text = "--";
                lblCountEquipments.Text = "--";
            }
        }

        #region XỬ LÝ SỰ KIỆN TRUY CẬP NHANH CÁC CHỨC NĂNG

        private void btnActionMembers_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_MemberManagement>();
        }

        private void btnActionPackages_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_RegisterPackage>();
        }

        private void btnActionCheckIn_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_CheckInQR>();
        }

        private void btnActionPTSchedule_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_PTSchedule>();
        }

        private void btnActionInvoices_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_InvoiceList>();
        }

        private void btnActionRevenue_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_RevenueReport>();
        }

        private void btnActionCatalog_Click(object sender, EventArgs e)
        {
            (this.MdiParent as Form_Main)?.OpenChildForm<Form_PackageManagement>();
        }

        private void btnActionPassword_Click(object sender, EventArgs e)
        {
            Form_ChangePassword pwdForm = new Form_ChangePassword();
            pwdForm.StartPosition = FormStartPosition.CenterParent;
            pwdForm.ShowDialog(this);
        }

        #endregion
    }
}
