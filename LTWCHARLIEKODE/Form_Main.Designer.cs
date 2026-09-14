namespace LTWCHARLIEKODE
{
    partial class Form_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChangePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManageUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManageRoles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBusiness = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManageMembers = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRegisterPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCreateInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPackagesCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.btnManageTrainers = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEquipmentsCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReports = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRevenueReport = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSeparator = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSystem,
            this.menuAdmin,
            this.menuBusiness,
            this.menuCatalog,
            this.menuReports});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuSystem
            // 
            this.menuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuChangePassword,
            this.toolStripSeparator1,
            this.menuLogout,
            this.menuExit});
            this.menuSystem.Name = "menuSystem";
            this.menuSystem.Size = new System.Drawing.Size(85, 24);
            this.menuSystem.Text = "Hệ thống";
            // 
            // menuChangePassword
            // 
            this.menuChangePassword.Name = "menuChangePassword";
            this.menuChangePassword.Size = new System.Drawing.Size(224, 26);
            this.menuChangePassword.Text = "Đổi mật khẩu";
            this.menuChangePassword.Click += new System.EventHandler(this.menuChangePassword_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(221, 6);
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(224, 26);
            this.menuLogout.Text = "Đăng xuất";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(224, 26);
            this.menuExit.Text = "Thoát";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuAdmin
            // 
            this.menuAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnManageUsers,
            this.btnManageRoles});
            this.menuAdmin.Name = "menuAdmin";
            this.menuAdmin.Size = new System.Drawing.Size(76, 24);
            this.menuAdmin.Text = "Quản trị";
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(224, 26);
            this.btnManageUsers.Text = "Quản lý người dùng";
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnManageRoles
            // 
            this.btnManageRoles.Name = "btnManageRoles";
            this.btnManageRoles.Size = new System.Drawing.Size(224, 26);
            this.btnManageRoles.Text = "Quản lý vai trò";
            this.btnManageRoles.Click += new System.EventHandler(this.btnManageRoles_Click);
            // 
            // menuBusiness
            // 
            this.menuBusiness.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnManageMembers,
            this.btnRegisterPackage,
            this.btnCreateInvoice});
            this.menuBusiness.Name = "menuBusiness";
            this.menuBusiness.Size = new System.Drawing.Size(91, 24);
            this.menuBusiness.Text = "Nghiệp vụ";
            // 
            // btnManageMembers
            // 
            this.btnManageMembers.Name = "btnManageMembers";
            this.btnManageMembers.Size = new System.Drawing.Size(198, 26);
            this.btnManageMembers.Text = "Quản lý hội viên";
            this.btnManageMembers.Click += new System.EventHandler(this.btnManageMembers_Click);
            // 
            // btnRegisterPackage
            // 
            this.btnRegisterPackage.Name = "btnRegisterPackage";
            this.btnRegisterPackage.Size = new System.Drawing.Size(224, 26);
            this.btnRegisterPackage.Text = "Đăng ký gói tập";
            this.btnRegisterPackage.Click += new System.EventHandler(this.btnRegisterPackage_Click);
            // 
            // btnCreateInvoice
            // 
            this.btnCreateInvoice.Name = "btnCreateInvoice";
            this.btnCreateInvoice.Size = new System.Drawing.Size(198, 26);
            this.btnCreateInvoice.Text = "Lập hóa đơn";
            this.btnCreateInvoice.Click += new System.EventHandler(this.btnCreateInvoice_Click);
            // 
            // menuCatalog
            // 
            this.menuCatalog.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPackagesCatalog,
            this.btnManageTrainers,
            this.btnEquipmentsCatalog});
            this.menuCatalog.Name = "menuCatalog";
            this.menuCatalog.Size = new System.Drawing.Size(90, 24);
            this.menuCatalog.Text = "Danh mục";
            // 
            // btnPackagesCatalog
            // 
            this.btnPackagesCatalog.Name = "btnPackagesCatalog";
            this.btnPackagesCatalog.Size = new System.Drawing.Size(219, 26);
            this.btnPackagesCatalog.Text = "Danh mục gói tập";
            this.btnPackagesCatalog.Click += new System.EventHandler(this.btnPackagesCatalog_Click);
            // 
            // btnManageTrainers
            // 
            this.btnManageTrainers.Name = "btnManageTrainers";
            this.btnManageTrainers.Size = new System.Drawing.Size(219, 26);
            this.btnManageTrainers.Text = "Danh mục HLV (PT)";
            this.btnManageTrainers.Click += new System.EventHandler(this.btnManageTrainers_Click);
            // 
            // btnEquipmentsCatalog
            // 
            this.btnEquipmentsCatalog.Name = "btnEquipmentsCatalog";
            this.btnEquipmentsCatalog.Size = new System.Drawing.Size(219, 26);
            this.btnEquipmentsCatalog.Text = "Danh mục thiết bị";
            this.btnEquipmentsCatalog.Click += new System.EventHandler(this.btnEquipmentsCatalog_Click);
            // 
            // menuReports
            // 
            this.menuReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRevenueReport});
            this.menuReports.Name = "menuReports";
            this.menuReports.Size = new System.Drawing.Size(77, 24);
            this.menuReports.Text = "Báo cáo";
            // 
            // btnRevenueReport
            // 
            this.btnRevenueReport.Name = "btnRevenueReport";
            this.btnRevenueReport.Size = new System.Drawing.Size(217, 26);
            this.btnRevenueReport.Text = "Báo cáo doanh thu";
            this.btnRevenueReport.Click += new System.EventHandler(this.btnRevenueReport_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurrentUser,
            this.toolStripStatusLabelSeparator,
            this.lblCurrentRole});
            this.statusStrip1.Location = new System.Drawing.Point(0, 527);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(92, 20);
            this.lblCurrentUser.Text = "Người dùng:";
            // 
            // toolStripStatusLabelSeparator
            // 
            this.toolStripStatusLabelSeparator.Name = "toolStripStatusLabelSeparator";
            this.toolStripStatusLabelSeparator.Size = new System.Drawing.Size(21, 20);
            this.toolStripStatusLabelSeparator.Text = " | ";
            // 
            // lblCurrentRole
            // 
            this.lblCurrentRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRole.Name = "lblCurrentRole";
            this.lblCurrentRole.Size = new System.Drawing.Size(55, 20);
            this.lblCurrentRole.Text = "Vai trò:";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 553);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HỆ THỐNG QUẢN LÝ PHÒNG GYM & FITNESS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuSystem;
        private System.Windows.Forms.ToolStripMenuItem menuChangePassword;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuAdmin;
        private System.Windows.Forms.ToolStripMenuItem btnManageUsers;
        private System.Windows.Forms.ToolStripMenuItem btnManageRoles;
        private System.Windows.Forms.ToolStripMenuItem menuBusiness;
        private System.Windows.Forms.ToolStripMenuItem btnManageMembers;
        private System.Windows.Forms.ToolStripMenuItem btnRegisterPackage;
        private System.Windows.Forms.ToolStripMenuItem btnCreateInvoice;
        private System.Windows.Forms.ToolStripMenuItem menuCatalog;
        private System.Windows.Forms.ToolStripMenuItem btnPackagesCatalog;
        private System.Windows.Forms.ToolStripMenuItem btnManageTrainers;
        private System.Windows.Forms.ToolStripMenuItem btnEquipmentsCatalog;
        private System.Windows.Forms.ToolStripMenuItem menuReports;
        private System.Windows.Forms.ToolStripMenuItem btnRevenueReport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentUser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSeparator;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentRole;
    }
}