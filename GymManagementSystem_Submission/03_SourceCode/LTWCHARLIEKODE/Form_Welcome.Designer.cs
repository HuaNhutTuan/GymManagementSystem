namespace LTWCHARLIEKODE
{
    partial class Form_Welcome
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblQuote = new System.Windows.Forms.Label();
            this.lblGreetingSub = new System.Windows.Forms.Label();
            this.lblGreetingHeader = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.flowQuickActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnActionMembers = new System.Windows.Forms.Button();
            this.btnActionPackages = new System.Windows.Forms.Button();
            this.btnActionCheckIn = new System.Windows.Forms.Button();
            this.btnActionPTSchedule = new System.Windows.Forms.Button();
            this.btnActionInvoices = new System.Windows.Forms.Button();
            this.btnActionRevenue = new System.Windows.Forms.Button();
            this.btnActionCatalog = new System.Windows.Forms.Button();
            this.btnActionPassword = new System.Windows.Forms.Button();
            this.lblActionTitle = new System.Windows.Forms.Label();
            this.pnlStats = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalMembers = new System.Windows.Forms.Panel();
            this.lblCountMembers = new System.Windows.Forms.Label();
            this.lblTitleMembers = new System.Windows.Forms.Label();
            this.cardTotalPackages = new System.Windows.Forms.Panel();
            this.lblCountPackages = new System.Windows.Forms.Label();
            this.lblTitlePackages = new System.Windows.Forms.Label();
            this.cardTotalTrainers = new System.Windows.Forms.Panel();
            this.lblCountTrainers = new System.Windows.Forms.Label();
            this.lblTitleTrainers = new System.Windows.Forms.Label();
            this.cardTotalEquipments = new System.Windows.Forms.Panel();
            this.lblCountEquipments = new System.Windows.Forms.Label();
            this.lblTitleEquipments = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblFooterText = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.flowQuickActions.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.cardTotalMembers.SuspendLayout();
            this.cardTotalPackages.SuspendLayout();
            this.cardTotalTrainers.SuspendLayout();
            this.cardTotalEquipments.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(39)))), ((int)(((byte)(72)))));
            this.pnlHeader.Controls.Add(this.lblQuote);
            this.pnlHeader.Controls.Add(this.lblGreetingSub);
            this.pnlHeader.Controls.Add(this.lblGreetingHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlHeader.Size = new System.Drawing.Size(1082, 140);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblQuote
            // 
            this.lblQuote.AutoSize = true;
            this.lblQuote.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(215)))), ((int)(((byte)(0)))));
            this.lblQuote.Location = new System.Drawing.Point(32, 98);
            this.lblQuote.Name = "lblQuote";
            this.lblQuote.Size = new System.Drawing.Size(462, 21);
            this.lblQuote.TabIndex = 2;
            this.lblQuote.Text = "★ \"Kỷ luật là cầu nối giữa mục tiêu và thành công trong thể hình!\"";
            // 
            // lblGreetingSub
            // 
            this.lblGreetingSub.AutoSize = true;
            this.lblGreetingSub.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreetingSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(220)))), ((int)(((byte)(245)))));
            this.lblGreetingSub.Location = new System.Drawing.Point(32, 63);
            this.lblGreetingSub.Name = "lblGreetingSub";
            this.lblGreetingSub.Size = new System.Drawing.Size(430, 25);
            this.lblGreetingSub.TabIndex = 1;
            this.lblGreetingSub.Text = "Xin chào: Đang tải thông tin phiên làm việc...";
            // 
            // lblGreetingHeader
            // 
            this.lblGreetingHeader.AutoSize = true;
            this.lblGreetingHeader.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreetingHeader.ForeColor = System.Drawing.Color.White;
            this.lblGreetingHeader.Location = new System.Drawing.Point(30, 18);
            this.lblGreetingHeader.Name = "lblGreetingHeader";
            this.lblGreetingHeader.Size = new System.Drawing.Size(650, 41);
            this.lblGreetingHeader.TabIndex = 0;
            this.lblGreetingHeader.Text = "CHÀO MỪNG ĐẾN VỚI HỆ THỐNG GYM";
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pnlContent.Controls.Add(this.flowQuickActions);
            this.pnlContent.Controls.Add(this.lblActionTitle);
            this.pnlContent.Controls.Add(this.pnlStats);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 140);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.pnlContent.Size = new System.Drawing.Size(1082, 545);
            this.pnlContent.TabIndex = 1;
            // 
            // flowQuickActions
            // 
            this.flowQuickActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowQuickActions.Controls.Add(this.btnActionMembers);
            this.flowQuickActions.Controls.Add(this.btnActionPackages);
            this.flowQuickActions.Controls.Add(this.btnActionCheckIn);
            this.flowQuickActions.Controls.Add(this.btnActionPTSchedule);
            this.flowQuickActions.Controls.Add(this.btnActionInvoices);
            this.flowQuickActions.Controls.Add(this.btnActionRevenue);
            this.flowQuickActions.Controls.Add(this.btnActionCatalog);
            this.flowQuickActions.Controls.Add(this.btnActionPassword);
            this.flowQuickActions.Location = new System.Drawing.Point(30, 175);
            this.flowQuickActions.Name = "flowQuickActions";
            this.flowQuickActions.Size = new System.Drawing.Size(1022, 335);
            this.flowQuickActions.TabIndex = 2;
            // 
            // btnActionMembers
            // 
            this.btnActionMembers.BackColor = System.Drawing.Color.White;
            this.btnActionMembers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionMembers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionMembers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionMembers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionMembers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionMembers.Location = new System.Drawing.Point(10, 10);
            this.btnActionMembers.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionMembers.Name = "btnActionMembers";
            this.btnActionMembers.Size = new System.Drawing.Size(235, 140);
            this.btnActionMembers.TabIndex = 0;
            this.btnActionMembers.Text = "Quản lý Hội viên\r\n\r\nTra cứu, thêm mới và quản lý hồ sơ tập luyện";
            this.btnActionMembers.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionMembers.UseVisualStyleBackColor = false;
            this.btnActionMembers.Click += new System.EventHandler(this.btnActionMembers_Click);
            // 
            // btnActionPackages
            // 
            this.btnActionPackages.BackColor = System.Drawing.Color.White;
            this.btnActionPackages.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionPackages.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionPackages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionPackages.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionPackages.Location = new System.Drawing.Point(265, 10);
            this.btnActionPackages.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionPackages.Name = "btnActionPackages";
            this.btnActionPackages.Size = new System.Drawing.Size(235, 140);
            this.btnActionPackages.TabIndex = 1;
            this.btnActionPackages.Text = "Đăng ký Gói tập\r\n\r\nKý hợp đồng thẻ tháng, quý, năm & gói PT";
            this.btnActionPackages.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionPackages.UseVisualStyleBackColor = false;
            this.btnActionPackages.Click += new System.EventHandler(this.btnActionPackages_Click);
            // 
            // btnActionCheckIn
            // 
            this.btnActionCheckIn.BackColor = System.Drawing.Color.White;
            this.btnActionCheckIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionCheckIn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionCheckIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionCheckIn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionCheckIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionCheckIn.Location = new System.Drawing.Point(520, 10);
            this.btnActionCheckIn.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionCheckIn.Name = "btnActionCheckIn";
            this.btnActionCheckIn.Size = new System.Drawing.Size(235, 140);
            this.btnActionCheckIn.TabIndex = 2;
            this.btnActionCheckIn.Text = "Điểm danh QR / Thẻ\r\n\r\nQuét mã QR và quẹt thẻ RFID vào phòng < 50ms";
            this.btnActionCheckIn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionCheckIn.UseVisualStyleBackColor = false;
            this.btnActionCheckIn.Click += new System.EventHandler(this.btnActionCheckIn_Click);
            // 
            // btnActionPTSchedule
            // 
            this.btnActionPTSchedule.BackColor = System.Drawing.Color.White;
            this.btnActionPTSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionPTSchedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionPTSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionPTSchedule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionPTSchedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionPTSchedule.Location = new System.Drawing.Point(775, 10);
            this.btnActionPTSchedule.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionPTSchedule.Name = "btnActionPTSchedule";
            this.btnActionPTSchedule.Size = new System.Drawing.Size(235, 140);
            this.btnActionPTSchedule.TabIndex = 3;
            this.btnActionPTSchedule.Text = "Lịch tập PT\r\n\r\nXếp lịch tập cá nhân, chống xung đột giờ dạy";
            this.btnActionPTSchedule.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionPTSchedule.UseVisualStyleBackColor = false;
            this.btnActionPTSchedule.Click += new System.EventHandler(this.btnActionPTSchedule_Click);
            // 
            // btnActionInvoices
            // 
            this.btnActionInvoices.BackColor = System.Drawing.Color.White;
            this.btnActionInvoices.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionInvoices.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionInvoices.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionInvoices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionInvoices.Location = new System.Drawing.Point(10, 170);
            this.btnActionInvoices.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionInvoices.Name = "btnActionInvoices";
            this.btnActionInvoices.Size = new System.Drawing.Size(235, 140);
            this.btnActionInvoices.TabIndex = 4;
            this.btnActionInvoices.Text = "Lập Hóa đơn\r\n\r\nThu phí dịch vụ và quản lý hóa đơn thanh toán";
            this.btnActionInvoices.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionInvoices.UseVisualStyleBackColor = false;
            this.btnActionInvoices.Click += new System.EventHandler(this.btnActionInvoices_Click);
            // 
            // btnActionRevenue
            // 
            this.btnActionRevenue.BackColor = System.Drawing.Color.White;
            this.btnActionRevenue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionRevenue.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionRevenue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionRevenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionRevenue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionRevenue.Location = new System.Drawing.Point(265, 170);
            this.btnActionRevenue.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionRevenue.Name = "btnActionRevenue";
            this.btnActionRevenue.Size = new System.Drawing.Size(235, 140);
            this.btnActionRevenue.TabIndex = 5;
            this.btnActionRevenue.Text = "Báo cáo Doanh thu\r\n\r\nThống kê tài chính, biểu đồ & xuất file Excel (Admin)";
            this.btnActionRevenue.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionRevenue.UseVisualStyleBackColor = false;
            this.btnActionRevenue.Click += new System.EventHandler(this.btnActionRevenue_Click);
            // 
            // btnActionCatalog
            // 
            this.btnActionCatalog.BackColor = System.Drawing.Color.White;
            this.btnActionCatalog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionCatalog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionCatalog.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionCatalog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionCatalog.Location = new System.Drawing.Point(520, 170);
            this.btnActionCatalog.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionCatalog.Name = "btnActionCatalog";
            this.btnActionCatalog.Size = new System.Drawing.Size(235, 140);
            this.btnActionCatalog.TabIndex = 6;
            this.btnActionCatalog.Text = "Danh mục Gói tập\r\n\r\nXem chi tiết quyền lợi các gói Gym & Fitness";
            this.btnActionCatalog.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionCatalog.UseVisualStyleBackColor = false;
            this.btnActionCatalog.Click += new System.EventHandler(this.btnActionCatalog_Click);
            // 
            // btnActionPassword
            // 
            this.btnActionPassword.BackColor = System.Drawing.Color.White;
            this.btnActionPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActionPassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(225)))), ((int)(((byte)(235)))));
            this.btnActionPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActionPassword.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActionPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnActionPassword.Location = new System.Drawing.Point(775, 170);
            this.btnActionPassword.Margin = new System.Windows.Forms.Padding(10);
            this.btnActionPassword.Name = "btnActionPassword";
            this.btnActionPassword.Size = new System.Drawing.Size(235, 140);
            this.btnActionPassword.TabIndex = 7;
            this.btnActionPassword.Text = "Đổi Mật khẩu\r\n\r\nCập nhật mật khẩu bảo vệ tài khoản cá nhân";
            this.btnActionPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnActionPassword.UseVisualStyleBackColor = false;
            this.btnActionPassword.Click += new System.EventHandler(this.btnActionPassword_Click);
            // 
            // lblActionTitle
            // 
            this.lblActionTitle.AutoSize = true;
            this.lblActionTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(39)))), ((int)(((byte)(72)))));
            this.lblActionTitle.Location = new System.Drawing.Point(32, 138);
            this.lblActionTitle.Name = "lblActionTitle";
            this.lblActionTitle.Size = new System.Drawing.Size(325, 28);
            this.lblActionTitle.TabIndex = 1;
            this.lblActionTitle.Text = "TRUY CẬP NHANH CHỨC NĂNG";
            // 
            // pnlStats
            // 
            this.pnlStats.ColumnCount = 4;
            this.pnlStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pnlStats.Controls.Add(this.cardTotalMembers, 0, 0);
            this.pnlStats.Controls.Add(this.cardTotalPackages, 1, 0);
            this.pnlStats.Controls.Add(this.cardTotalTrainers, 2, 0);
            this.pnlStats.Controls.Add(this.cardTotalEquipments, 3, 0);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(30, 20);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.RowCount = 1;
            this.pnlStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlStats.Size = new System.Drawing.Size(1022, 100);
            this.pnlStats.TabIndex = 0;
            // 
            // cardTotalMembers
            // 
            this.cardTotalMembers.BackColor = System.Drawing.Color.White;
            this.cardTotalMembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalMembers.Controls.Add(this.lblCountMembers);
            this.cardTotalMembers.Controls.Add(this.lblTitleMembers);
            this.cardTotalMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalMembers.Location = new System.Drawing.Point(6, 6);
            this.cardTotalMembers.Margin = new System.Windows.Forms.Padding(6);
            this.cardTotalMembers.Name = "cardTotalMembers";
            this.cardTotalMembers.Padding = new System.Windows.Forms.Padding(12);
            this.cardTotalMembers.Size = new System.Drawing.Size(243, 88);
            this.cardTotalMembers.TabIndex = 0;
            // 
            // lblCountMembers
            // 
            this.lblCountMembers.AutoSize = true;
            this.lblCountMembers.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountMembers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(134)))), ((int)(((byte)(222)))));
            this.lblCountMembers.Location = new System.Drawing.Point(12, 38);
            this.lblCountMembers.Name = "lblCountMembers";
            this.lblCountMembers.Size = new System.Drawing.Size(43, 41);
            this.lblCountMembers.TabIndex = 1;
            this.lblCountMembers.Text = "...";
            // 
            // lblTitleMembers
            // 
            this.lblTitleMembers.AutoSize = true;
            this.lblTitleMembers.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleMembers.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleMembers.Location = new System.Drawing.Point(12, 12);
            this.lblTitleMembers.Name = "lblTitleMembers";
            this.lblTitleMembers.Size = new System.Drawing.Size(147, 21);
            this.lblTitleMembers.TabIndex = 0;
            this.lblTitleMembers.Text = "TỔNG HỘI VIÊN";
            // 
            // cardTotalPackages
            // 
            this.cardTotalPackages.BackColor = System.Drawing.Color.White;
            this.cardTotalPackages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalPackages.Controls.Add(this.lblCountPackages);
            this.cardTotalPackages.Controls.Add(this.lblTitlePackages);
            this.cardTotalPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalPackages.Location = new System.Drawing.Point(261, 6);
            this.cardTotalPackages.Margin = new System.Windows.Forms.Padding(6);
            this.cardTotalPackages.Name = "cardTotalPackages";
            this.cardTotalPackages.Padding = new System.Windows.Forms.Padding(12);
            this.cardTotalPackages.Size = new System.Drawing.Size(243, 88);
            this.cardTotalPackages.TabIndex = 1;
            // 
            // lblCountPackages
            // 
            this.lblCountPackages.AutoSize = true;
            this.lblCountPackages.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountPackages.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(172)))), ((int)(((byte)(132)))));
            this.lblCountPackages.Location = new System.Drawing.Point(12, 38);
            this.lblCountPackages.Name = "lblCountPackages";
            this.lblCountPackages.Size = new System.Drawing.Size(43, 41);
            this.lblCountPackages.TabIndex = 1;
            this.lblCountPackages.Text = "...";
            // 
            // lblTitlePackages
            // 
            this.lblTitlePackages.AutoSize = true;
            this.lblTitlePackages.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitlePackages.ForeColor = System.Drawing.Color.Gray;
            this.lblTitlePackages.Location = new System.Drawing.Point(12, 12);
            this.lblTitlePackages.Name = "lblTitlePackages";
            this.lblTitlePackages.Size = new System.Drawing.Size(141, 21);
            this.lblTitlePackages.TabIndex = 0;
            this.lblTitlePackages.Text = "GÓI TẬP ĐANG BÁN";
            // 
            // cardTotalTrainers
            // 
            this.cardTotalTrainers.BackColor = System.Drawing.Color.White;
            this.cardTotalTrainers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalTrainers.Controls.Add(this.lblCountTrainers);
            this.cardTotalTrainers.Controls.Add(this.lblTitleTrainers);
            this.cardTotalTrainers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalTrainers.Location = new System.Drawing.Point(516, 6);
            this.cardTotalTrainers.Margin = new System.Windows.Forms.Padding(6);
            this.cardTotalTrainers.Name = "cardTotalTrainers";
            this.cardTotalTrainers.Padding = new System.Windows.Forms.Padding(12);
            this.cardTotalTrainers.Size = new System.Drawing.Size(243, 88);
            this.cardTotalTrainers.TabIndex = 2;
            // 
            // lblCountTrainers
            // 
            this.lblCountTrainers.AutoSize = true;
            this.lblCountTrainers.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountTrainers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(82)))), ((int)(((byte)(83)))));
            this.lblCountTrainers.Location = new System.Drawing.Point(12, 38);
            this.lblCountTrainers.Name = "lblCountTrainers";
            this.lblCountTrainers.Size = new System.Drawing.Size(43, 41);
            this.lblCountTrainers.TabIndex = 1;
            this.lblCountTrainers.Text = "...";
            // 
            // lblTitleTrainers
            // 
            this.lblTitleTrainers.AutoSize = true;
            this.lblTitleTrainers.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleTrainers.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleTrainers.Location = new System.Drawing.Point(12, 12);
            this.lblTitleTrainers.Name = "lblTitleTrainers";
            this.lblTitleTrainers.Size = new System.Drawing.Size(176, 21);
            this.lblTitleTrainers.TabIndex = 0;
            this.lblTitleTrainers.Text = "HUẤN LUYỆN VIÊN (PT)";
            // 
            // cardTotalEquipments
            // 
            this.cardTotalEquipments.BackColor = System.Drawing.Color.White;
            this.cardTotalEquipments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalEquipments.Controls.Add(this.lblCountEquipments);
            this.cardTotalEquipments.Controls.Add(this.lblTitleEquipments);
            this.cardTotalEquipments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalEquipments.Location = new System.Drawing.Point(771, 6);
            this.cardTotalEquipments.Margin = new System.Windows.Forms.Padding(6);
            this.cardTotalEquipments.Name = "cardTotalEquipments";
            this.cardTotalEquipments.Padding = new System.Windows.Forms.Padding(12);
            this.cardTotalEquipments.Size = new System.Drawing.Size(245, 88);
            this.cardTotalEquipments.TabIndex = 3;
            // 
            // lblCountEquipments
            // 
            this.lblCountEquipments.AutoSize = true;
            this.lblCountEquipments.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountEquipments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(159)))), ((int)(((byte)(67)))));
            this.lblCountEquipments.Location = new System.Drawing.Point(12, 38);
            this.lblCountEquipments.Name = "lblCountEquipments";
            this.lblCountEquipments.Size = new System.Drawing.Size(43, 41);
            this.lblCountEquipments.TabIndex = 1;
            this.lblCountEquipments.Text = "...";
            // 
            // lblTitleEquipments
            // 
            this.lblTitleEquipments.AutoSize = true;
            this.lblTitleEquipments.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleEquipments.ForeColor = System.Drawing.Color.Gray;
            this.lblTitleEquipments.Location = new System.Drawing.Point(12, 12);
            this.lblTitleEquipments.Name = "lblTitleEquipments";
            this.lblTitleEquipments.Size = new System.Drawing.Size(167, 21);
            this.lblTitleEquipments.TabIndex = 0;
            this.lblTitleEquipments.Text = "THIẾT BỊ PHÒNG TẬP";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))));
            this.pnlFooter.Controls.Add(this.lblFooterText);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 685);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1082, 40);
            this.pnlFooter.TabIndex = 2;
            // 
            // lblFooterText
            // 
            this.lblFooterText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooterText.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooterText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(115)))), ((int)(((byte)(130)))));
            this.lblFooterText.Location = new System.Drawing.Point(0, 0);
            this.lblFooterText.Name = "lblFooterText";
            this.lblFooterText.Size = new System.Drawing.Size(1082, 40);
            this.lblFooterText.TabIndex = 0;
            this.lblFooterText.Text = "HỆ THỐNG QUẢN LÝ PHÒNG GYM & FITNESS - [LTWCHARLIEKODE]  |  PHIÊN BẢN CHUYÊN NGHI" +
    "ỆP © 2026";
            this.lblFooterText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 725);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form_Welcome";
            this.Text = "MÀN HÌNH CHÀO MỪNG - GYM MANAGEMENT SYSTEM";
            this.Load += new System.EventHandler(this.Form_Welcome_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.flowQuickActions.ResumeLayout(false);
            this.pnlStats.ResumeLayout(false);
            this.cardTotalMembers.ResumeLayout(false);
            this.cardTotalMembers.PerformLayout();
            this.cardTotalPackages.ResumeLayout(false);
            this.cardTotalPackages.PerformLayout();
            this.cardTotalTrainers.ResumeLayout(false);
            this.cardTotalTrainers.PerformLayout();
            this.cardTotalEquipments.ResumeLayout(false);
            this.cardTotalEquipments.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblGreetingHeader;
        private System.Windows.Forms.Label lblGreetingSub;
        private System.Windows.Forms.Label lblQuote;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel pnlStats;
        private System.Windows.Forms.Panel cardTotalMembers;
        private System.Windows.Forms.Label lblCountMembers;
        private System.Windows.Forms.Label lblTitleMembers;
        private System.Windows.Forms.Panel cardTotalPackages;
        private System.Windows.Forms.Label lblCountPackages;
        private System.Windows.Forms.Label lblTitlePackages;
        private System.Windows.Forms.Panel cardTotalTrainers;
        private System.Windows.Forms.Label lblCountTrainers;
        private System.Windows.Forms.Label lblTitleTrainers;
        private System.Windows.Forms.Panel cardTotalEquipments;
        private System.Windows.Forms.Label lblCountEquipments;
        private System.Windows.Forms.Label lblTitleEquipments;
        private System.Windows.Forms.Label lblActionTitle;
        private System.Windows.Forms.FlowLayoutPanel flowQuickActions;
        private System.Windows.Forms.Button btnActionMembers;
        private System.Windows.Forms.Button btnActionPackages;
        private System.Windows.Forms.Button btnActionCheckIn;
        private System.Windows.Forms.Button btnActionPTSchedule;
        private System.Windows.Forms.Button btnActionInvoices;
        private System.Windows.Forms.Button btnActionRevenue;
        private System.Windows.Forms.Button btnActionCatalog;
        private System.Windows.Forms.Button btnActionPassword;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblFooterText;
    }
}
