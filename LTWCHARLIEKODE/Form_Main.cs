using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        #region 1. SỰ KIỆN LOAD FORM & PHÂN QUYỀN (LOAD & PERMISSION)

        private void Form_Main_Load(object sender, EventArgs e)
        {
            // A. Cấu hình giao diện Form cha MDI tràn màn hình
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "HỆ THỐNG QUẢN LÝ PHÒNG GYM & FITNESS - [LTWCHARLIEKODE]";

            // B. Đổi màu nền vùng làm việc MDI (Tùy chọn cho giao diện đẹp)
            foreach (Control control in this.Controls)
            {
                if (control is MdiClient mdiClient)
                {
                    mdiClient.BackColor = Color.FromArgb(240, 242, 245);
                }
            }

            // C. Hiển thị thông tin người dùng đang đăng nhập lên StatusStrip (Thanh trạng thái)
            lblCurrentUser.Text = $"Người dùng: {UserSession.FullName} ({UserSession.Username})";
            lblCurrentRole.Text = $"Vai trò: {UserSession.RoleName}";

            // D. Thực thi Phân quyền giao diện theo RoleId từ UserSession
            ApplyRolePermissions(UserSession.RoleId);
        }

        /// <summary>
        /// Hàm ẩn / mờ các Menu & Nút chức năng dựa trên Vai trò người dùng (RBAC)
        /// RoleId = 1: Admin (Quản trị viên)
        /// RoleId = 2: Lễ Tân (Receptionist)
        /// RoleId = 3: PT (Huấn luyện viên cá nhân)
        /// </summary>
        private void ApplyRolePermissions(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    // ADMIN: Có toàn quyền trên tất cả các chức năng
                    menuSystem.Enabled = true;
                    menuAdmin.Enabled = true;
                    menuBusiness.Enabled = true;
                    menuCatalog.Enabled = true;
                    menuReports.Enabled = true;
                    break;

                case 2:
                    // LỄ TÂN: Tiếp nhận hội viên, bán gói, lập hóa đơn, xem thiết bị/HLV.
                    // KHÔNG có quyền truy cập Quản trị tài khoản & Xem báo cáo doanh thu.
                    menuSystem.Enabled = true;
                    menuAdmin.Enabled = false;          // Ẩn/Mờ Menu Quản trị hệ thống
                    menuBusiness.Enabled = true;         // Bật Menu Nghiệp vụ
                    menuCatalog.Enabled = true;          // Bật Menu Danh mục

                    // Phân quyền chi tiết trên từng nút con:
                    btnRevenueReport.Enabled = false;   // Vẫn thấy menu Báo cáo nhưng khóa nút Doanh thu
                    break;

                case 3:
                    // PT (HUẤN LUYỆN VIÊN): Chỉ xem thông tin Hội viên & Thiết bị phòng tập.
                    // KHÔNG thu tiền, KHÔNG lập hóa đơn, KHÔNG quản lý tài khoản.
                    menuSystem.Enabled = true;
                    menuAdmin.Enabled = false;
                    menuBusiness.Enabled = true;

                    // Khóa các tính năng giao dịch tài chính của Lễ tân
                    btnRegisterPackage.Enabled = false;
                    btnCreateInvoice.Enabled = false;

                    menuCatalog.Enabled = true;
                    menuReports.Enabled = false;
                    break;

                default:
                    // Trường hợp mặc định hoặc Tài khoản bị khóa: Khóa tất cả
                    menuAdmin.Enabled = false;
                    menuBusiness.Enabled = false;
                    menuCatalog.Enabled = false;
                    menuReports.Enabled = false;
                    break;
            }
        }

        #endregion

        #region 2. HÀM DÙNG CHUNG MỞ FORM CON MDI (MDI CHILD MANAGEMENT)

        /// <summary>
        /// Hàm mở Form con bên trong Form_Main.
        /// Đảm bảo nếu Form đã mở rồi thì Active (mang ra trước) chứ không mở trùng lặp nhiều lần.
        /// </summary>
        private void OpenChildForm<T>() where T : Form, new()
        {
            // Tìm xem Form này đã được mở trong danh sách MdiChildren chưa
            Form existingForm = this.MdiChildren.FirstOrDefault(f => f is T);

            if (existingForm != null)
            {
                // Nếu đã mở ➔ Mang Form lên trên cùng
                existingForm.Activate();
                if (existingForm.WindowState == FormWindowState.Minimized)
                {
                    existingForm.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                // Nếu chưa mở ➔ Khởi tạo và hiển thị Form con mới
                T childForm = new T();
                childForm.MdiParent = this; // Đặt Form_Main làm cha
                childForm.StartPosition = FormStartPosition.CenterScreen;
                childForm.Show();
            }
        }

        #endregion

        #region 3. XỬ LÝ SỰ KIỆN MENU HỆ THỐNG (ĐĂNG XUẤT & THOÁT)

        private void menuLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // 1. Xóa thông tin phiên làm việc
                UserSession.Clear();

                // 2. Mở lại Form Đăng nhập và đóng Form_Main
                Form_Login loginForm = new Form_Login();
                this.Hide();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hỏi xác nhận khi bấm nút X đỏ ở góc trên cùng bên phải
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn thoát khỏi chương trình?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Hủy thao tác đóng Form
                }
            }
        }

        #endregion

        #region 4. GỌI MỞ CÁC FORM CHỨC NĂNG (GIAO PHẦN CHO CÁC THÀNH VIÊN NHÓM)

        // --- CÁC FORM DO BẠN (TEAM LEADER) PHỤ TRÁCH ---
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            // Sau khi bạn tạo Form_UserManagement thì bỏ dấu // ở dòng dưới:
            // OpenChildForm<Form_UserManagement>();
        }

        private void btnManageRoles_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_RoleManagement>();
        }

        private void menuChangePassword_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_ChangePassword>();
        }


        // --- CÁC FORM PHÂN CÔNG THÀNH VIÊN 2 (QUẢN LÝ HỘI VIÊN & GÓI TẬP) ---
        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_MemberManagement>();
        }

        private void btnPackagesCatalog_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_PackageManagement>();
        }


        // --- CÁC FORM PHÂN CÔNG THÀNH VIÊN 3 (QUẢN LÝ THU TIỀN & HÓA ĐƠN) ---
        private void btnRegisterPackage_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_RegisterPackage>();
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_InvoiceManagement>();
        }


        // --- CÁC FORM PHÂN CÔNG THÀNH VIÊN 4 (QUẢN LÝ PT & THIẾT BỊ & BÁO CÁO) ---
        private void btnManageTrainers_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_TrainerManagement>();
        }

        private void btnEquipmentsCatalog_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_EquipmentManagement>();
        }

        private void btnRevenueReport_Click(object sender, EventArgs e)
        {
            // OpenChildForm<Form_RevenueReport>();
        }

        #endregion
    }
}