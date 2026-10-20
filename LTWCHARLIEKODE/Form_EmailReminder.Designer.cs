namespace LTWCHARLIEKODE
{
    partial class Form_EmailReminder
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

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabEmail = new System.Windows.Forms.TabControl();
            this.tabMembers = new System.Windows.Forms.TabPage();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSendSelected = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboFilterDays = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.tabSmtp = new System.Windows.Forms.TabPage();
            this.grpTest = new System.Windows.Forms.GroupBox();
            this.lblTestStatus = new System.Windows.Forms.Label();
            this.btnTestSmtp = new System.Windows.Forms.Button();
            this.txtTestEmail = new System.Windows.Forms.TextBox();
            this.lblTestEmail = new System.Windows.Forms.Label();
            this.btnSaveSmtp = new System.Windows.Forms.Button();
            this.grpSmtp = new System.Windows.Forms.GroupBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.chkSmtpSsl = new System.Windows.Forms.CheckBox();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.txtSenderPassword = new System.Windows.Forms.TextBox();
            this.lblSenderPassword = new System.Windows.Forms.Label();
            this.txtSenderEmail = new System.Windows.Forms.TextBox();
            this.lblSenderEmail = new System.Windows.Forms.Label();
            this.txtSmtpPort = new System.Windows.Forms.TextBox();
            this.lblSmtpPort = new System.Windows.Forms.Label();
            this.txtSmtpHost = new System.Windows.Forms.TextBox();
            this.lblSmtpHost = new System.Windows.Forms.Label();
            this.tabEmail.SuspendLayout();
            this.tabMembers.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).BeginInit();
            this.pnlLog.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.tabSmtp.SuspendLayout();
            this.grpTest.SuspendLayout();
            this.grpSmtp.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(984, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HỆ THỐNG GỬI EMAIL THÔNG BÁO GIA HẠN GÓI TẬP";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabEmail
            // 
            this.tabEmail.Controls.Add(this.tabMembers);
            this.tabEmail.Controls.Add(this.tabSmtp);
            this.tabEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabEmail.Location = new System.Drawing.Point(0, 45);
            this.tabEmail.Name = "tabEmail";
            this.tabEmail.SelectedIndex = 0;
            this.tabEmail.Size = new System.Drawing.Size(984, 555);
            this.tabEmail.TabIndex = 1;
            // 
            // tabMembers
            // 
            this.tabMembers.Controls.Add(this.pnlGrid);
            this.tabMembers.Controls.Add(this.pnlLog);
            this.tabMembers.Controls.Add(this.progressBar);
            this.tabMembers.Controls.Add(this.pnlControls);
            this.tabMembers.Location = new System.Drawing.Point(4, 26);
            this.tabMembers.Name = "tabMembers";
            this.tabMembers.Padding = new System.Windows.Forms.Padding(3);
            this.tabMembers.Size = new System.Drawing.Size(976, 525);
            this.tabMembers.TabIndex = 0;
            this.tabMembers.Text = "✉ Danh sách Hội viên sắp hết hạn";
            this.tabMembers.UseVisualStyleBackColor = true;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvMembers);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(3, 53);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(970, 314);
            this.pnlGrid.TabIndex = 2;
            // 
            // dgvMembers
            // 
            this.dgvMembers.AllowUserToAddRows = false;
            this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMembers.BackgroundColor = System.Drawing.Color.White;
            this.dgvMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMembers.Location = new System.Drawing.Point(0, 0);
            this.dgvMembers.Name = "dgvMembers";
            this.dgvMembers.RowHeadersVisible = false;
            this.dgvMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembers.Size = new System.Drawing.Size(970, 314);
            this.dgvMembers.TabIndex = 0;
            // 
            // pnlLog
            // 
            this.pnlLog.Controls.Add(this.txtLog);
            this.pnlLog.Controls.Add(this.lblLogTitle);
            this.pnlLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLog.Location = new System.Drawing.Point(3, 367);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(970, 140);
            this.pnlLog.TabIndex = 3;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(0, 20);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(970, 120);
            this.txtLog.TabIndex = 1;
            // 
            // lblLogTitle
            // 
            this.lblLogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(970, 20);
            this.lblLogTitle.TabIndex = 0;
            this.lblLogTitle.Text = "Nhật ký tiến trình gửi Email:";
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(3, 507);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(970, 15);
            this.progressBar.TabIndex = 4;
            this.progressBar.Visible = false;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnDeselectAll);
            this.pnlControls.Controls.Add(this.btnSelectAll);
            this.pnlControls.Controls.Add(this.btnSendSelected);
            this.pnlControls.Controls.Add(this.btnRefresh);
            this.pnlControls.Controls.Add(this.cboFilterDays);
            this.pnlControls.Controls.Add(this.lblFilter);
            this.pnlControls.Controls.Add(this.lblSummary);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(3, 3);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(970, 50);
            this.pnlControls.TabIndex = 0;
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselectAll.Location = new System.Drawing.Point(440, 12);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 27);
            this.btnDeselectAll.TabIndex = 6;
            this.btnDeselectAll.Text = "Bỏ chọn";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.Location = new System.Drawing.Point(355, 12);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(80, 27);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "Chọn tất cả";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSendSelected
            // 
            this.btnSendSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnSendSelected.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendSelected.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendSelected.ForeColor = System.Drawing.Color.White;
            this.btnSendSelected.Location = new System.Drawing.Point(530, 8);
            this.btnSendSelected.Name = "btnSendSelected";
            this.btnSendSelected.Size = new System.Drawing.Size(200, 34);
            this.btnSendSelected.TabIndex = 4;
            this.btnSendSelected.Text = "✉ Gửi Email Cho Mục Đã Chọn";
            this.btnSendSelected.UseVisualStyleBackColor = false;
            this.btnSendSelected.Click += new System.EventHandler(this.btnSendSelected_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(245, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(95, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 Quét Lại";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboFilterDays
            // 
            this.cboFilterDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterDays.FormattingEnabled = true;
            this.cboFilterDays.Items.AddRange(new object[] {
            "Sắp hết hạn trong 3 ngày",
            "Sắp hết hạn trong 5 ngày (Mặc định)",
            "Sắp hết hạn trong 7 ngày",
            "Sắp hết hạn trong 14 ngày"});
            this.cboFilterDays.Location = new System.Drawing.Point(65, 13);
            this.cboFilterDays.Name = "cboFilterDays";
            this.cboFilterDays.Size = new System.Drawing.Size(170, 25);
            this.cboFilterDays.TabIndex = 2;
            this.cboFilterDays.SelectedIndex = 1;
            this.cboFilterDays.SelectedIndexChanged += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilter.Location = new System.Drawing.Point(10, 16);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(53, 17);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Bộ lọc:";
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblSummary.Location = new System.Drawing.Point(740, 10);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(220, 32);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Đang quét danh sách...";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSmtp
            // 
            this.tabSmtp.Controls.Add(this.grpTest);
            this.tabSmtp.Controls.Add(this.btnSaveSmtp);
            this.tabSmtp.Controls.Add(this.grpSmtp);
            this.tabSmtp.Location = new System.Drawing.Point(4, 26);
            this.tabSmtp.Name = "tabSmtp";
            this.tabSmtp.Padding = new System.Windows.Forms.Padding(3);
            this.tabSmtp.Size = new System.Drawing.Size(976, 525);
            this.tabSmtp.TabIndex = 1;
            this.tabSmtp.Text = "⚙ Cấu hình máy chủ SMTP (Gmail)";
            this.tabSmtp.UseVisualStyleBackColor = true;
            // 
            // grpTest
            // 
            this.grpTest.Controls.Add(this.lblTestStatus);
            this.grpTest.Controls.Add(this.btnTestSmtp);
            this.grpTest.Controls.Add(this.txtTestEmail);
            this.grpTest.Controls.Add(this.lblTestEmail);
            this.grpTest.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTest.Location = new System.Drawing.Point(20, 355);
            this.grpTest.Name = "grpTest";
            this.grpTest.Size = new System.Drawing.Size(700, 120);
            this.grpTest.TabIndex = 2;
            this.grpTest.TabStop = false;
            this.grpTest.Text = "Thử nghiệm gửi Email (Test Mail)";
            // 
            // lblTestStatus
            // 
            this.lblTestStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblTestStatus.Location = new System.Drawing.Point(20, 75);
            this.lblTestStatus.Name = "lblTestStatus";
            this.lblTestStatus.Size = new System.Drawing.Size(650, 30);
            this.lblTestStatus.TabIndex = 3;
            this.lblTestStatus.Text = "Nhập email của bạn và bấm Gửi thử nghiệm để kiểm tra kết nối...";
            // 
            // btnTestSmtp
            // 
            this.btnTestSmtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnTestSmtp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestSmtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestSmtp.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestSmtp.ForeColor = System.Drawing.Color.White;
            this.btnTestSmtp.Location = new System.Drawing.Point(475, 30);
            this.btnTestSmtp.Name = "btnTestSmtp";
            this.btnTestSmtp.Size = new System.Drawing.Size(150, 32);
            this.btnTestSmtp.TabIndex = 2;
            this.btnTestSmtp.Text = "⚡ Gửi Thử Nghiệm";
            this.btnTestSmtp.UseVisualStyleBackColor = false;
            this.btnTestSmtp.Click += new System.EventHandler(this.btnTestSmtp_Click);
            // 
            // txtTestEmail
            // 
            this.txtTestEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestEmail.Location = new System.Drawing.Point(170, 34);
            this.txtTestEmail.Name = "txtTestEmail";
            this.txtTestEmail.Size = new System.Drawing.Size(285, 24);
            this.txtTestEmail.TabIndex = 1;
            // 
            // lblTestEmail
            // 
            this.lblTestEmail.AutoSize = true;
            this.lblTestEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestEmail.Location = new System.Drawing.Point(20, 37);
            this.lblTestEmail.Name = "lblTestEmail";
            this.lblTestEmail.Size = new System.Drawing.Size(134, 17);
            this.lblTestEmail.TabIndex = 0;
            this.lblTestEmail.Text = "Gửi tới Email nhận thử:";
            // 
            // btnSaveSmtp
            // 
            this.btnSaveSmtp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnSaveSmtp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveSmtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSmtp.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSmtp.ForeColor = System.Drawing.Color.White;
            this.btnSaveSmtp.Location = new System.Drawing.Point(20, 305);
            this.btnSaveSmtp.Name = "btnSaveSmtp";
            this.btnSaveSmtp.Size = new System.Drawing.Size(190, 36);
            this.btnSaveSmtp.TabIndex = 1;
            this.btnSaveSmtp.Text = "💾 Lưu Cấu Hình SMTP";
            this.btnSaveSmtp.UseVisualStyleBackColor = false;
            this.btnSaveSmtp.Click += new System.EventHandler(this.btnSaveSmtp_Click);
            // 
            // grpSmtp
            // 
            this.grpSmtp.Controls.Add(this.lblNote);
            this.grpSmtp.Controls.Add(this.chkSmtpSsl);
            this.grpSmtp.Controls.Add(this.txtDisplayName);
            this.grpSmtp.Controls.Add(this.lblDisplayName);
            this.grpSmtp.Controls.Add(this.txtSenderPassword);
            this.grpSmtp.Controls.Add(this.lblSenderPassword);
            this.grpSmtp.Controls.Add(this.txtSenderEmail);
            this.grpSmtp.Controls.Add(this.lblSenderEmail);
            this.grpSmtp.Controls.Add(this.txtSmtpPort);
            this.grpSmtp.Controls.Add(this.lblSmtpPort);
            this.grpSmtp.Controls.Add(this.txtSmtpHost);
            this.grpSmtp.Controls.Add(this.lblSmtpHost);
            this.grpSmtp.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSmtp.Location = new System.Drawing.Point(20, 20);
            this.grpSmtp.Name = "grpSmtp";
            this.grpSmtp.Size = new System.Drawing.Size(700, 270);
            this.grpSmtp.TabIndex = 0;
            this.grpSmtp.TabStop = false;
            this.grpSmtp.Text = "Thông số máy chủ SMTP";
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblNote.Location = new System.Drawing.Point(170, 195);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(480, 55);
            this.lblNote.TabIndex = 11;
            this.lblNote.Text = "* Chú ý với Gmail: Bạn cần tạo Mật khẩu ứng dụng (App Password 16 ký tự) trong ph" +
    "ần Tài khoản Google -> Bảo mật -> Xác minh 2 bước -> Mật khẩu ứng dụng để gửi ma" +
    "il.";
            // 
            // chkSmtpSsl
            // 
            this.chkSmtpSsl.AutoSize = true;
            this.chkSmtpSsl.Checked = true;
            this.chkSmtpSsl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSmtpSsl.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSmtpSsl.Location = new System.Drawing.Point(450, 32);
            this.chkSmtpSsl.Name = "chkSmtpSsl";
            this.chkSmtpSsl.Size = new System.Drawing.Size(126, 21);
            this.chkSmtpSsl.TabIndex = 10;
            this.chkSmtpSsl.Text = "Bật mã hóa (SSL)";
            this.chkSmtpSsl.UseVisualStyleBackColor = true;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDisplayName.Location = new System.Drawing.Point(170, 160);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(460, 24);
            this.txtDisplayName.TabIndex = 9;
            this.txtDisplayName.Text = "Trung Tâm Thể Hình Gym & Fitness";
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayName.Location = new System.Drawing.Point(20, 163);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(107, 17);
            this.lblDisplayName.TabIndex = 8;
            this.lblDisplayName.Text = "Tên hiển thị gửi:";
            // 
            // txtSenderPassword
            // 
            this.txtSenderPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenderPassword.Location = new System.Drawing.Point(170, 120);
            this.txtSenderPassword.Name = "txtSenderPassword";
            this.txtSenderPassword.PasswordChar = '●';
            this.txtSenderPassword.Size = new System.Drawing.Size(460, 24);
            this.txtSenderPassword.TabIndex = 7;
            // 
            // lblSenderPassword
            // 
            this.lblSenderPassword.AutoSize = true;
            this.lblSenderPassword.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenderPassword.Location = new System.Drawing.Point(20, 123);
            this.lblSenderPassword.Name = "lblSenderPassword";
            this.lblSenderPassword.Size = new System.Drawing.Size(147, 17);
            this.lblSenderPassword.TabIndex = 6;
            this.lblSenderPassword.Text = "Mật khẩu ứng dụng:";
            // 
            // txtSenderEmail
            // 
            this.txtSenderEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenderEmail.Location = new System.Drawing.Point(170, 77);
            this.txtSenderEmail.Name = "txtSenderEmail";
            this.txtSenderEmail.Size = new System.Drawing.Size(460, 24);
            this.txtSenderEmail.TabIndex = 5;
            // 
            // lblSenderEmail
            // 
            this.lblSenderEmail.AutoSize = true;
            this.lblSenderEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenderEmail.Location = new System.Drawing.Point(20, 80);
            this.lblSenderEmail.Name = "lblSenderEmail";
            this.lblSenderEmail.Size = new System.Drawing.Size(107, 17);
            this.lblSenderEmail.TabIndex = 4;
            this.lblSenderEmail.Text = "Email người gửi:";
            // 
            // txtSmtpPort
            // 
            this.txtSmtpPort.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSmtpPort.Location = new System.Drawing.Point(365, 30);
            this.txtSmtpPort.Name = "txtSmtpPort";
            this.txtSmtpPort.Size = new System.Drawing.Size(65, 24);
            this.txtSmtpPort.TabIndex = 3;
            this.txtSmtpPort.Text = "587";
            // 
            // lblSmtpPort
            // 
            this.lblSmtpPort.AutoSize = true;
            this.lblSmtpPort.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmtpPort.Location = new System.Drawing.Point(325, 33);
            this.lblSmtpPort.Name = "lblSmtpPort";
            this.lblSmtpPort.Size = new System.Drawing.Size(35, 17);
            this.lblSmtpPort.TabIndex = 2;
            this.lblSmtpPort.Text = "Port:";
            // 
            // txtSmtpHost
            // 
            this.txtSmtpHost.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSmtpHost.Location = new System.Drawing.Point(170, 30);
            this.txtSmtpHost.Name = "txtSmtpHost";
            this.txtSmtpHost.Size = new System.Drawing.Size(140, 24);
            this.txtSmtpHost.TabIndex = 1;
            this.txtSmtpHost.Text = "smtp.gmail.com";
            // 
            // lblSmtpHost
            // 
            this.lblSmtpHost.AutoSize = true;
            this.lblSmtpHost.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmtpHost.Location = new System.Drawing.Point(20, 33);
            this.lblSmtpHost.Name = "lblSmtpHost";
            this.lblSmtpHost.Size = new System.Drawing.Size(107, 17);
            this.lblSmtpHost.TabIndex = 0;
            this.lblSmtpHost.Text = "Máy chủ SMTP:";
            // 
            // Form_EmailReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(984, 600);
            this.Controls.Add(this.tabEmail);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_EmailReminder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gửi Email nhắc nhở gia hạn gói tập";
            this.Load += new System.EventHandler(this.Form_EmailReminder_Load);
            this.tabEmail.ResumeLayout(false);
            this.tabMembers.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).EndInit();
            this.pnlLog.ResumeLayout(false);
            this.pnlLog.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.tabSmtp.ResumeLayout(false);
            this.grpTest.ResumeLayout(false);
            this.grpTest.PerformLayout();
            this.grpSmtp.ResumeLayout(false);
            this.grpSmtp.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabEmail;
        private System.Windows.Forms.TabPage tabMembers;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvMembers;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSendSelected;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cboFilterDays;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.TabPage tabSmtp;
        private System.Windows.Forms.GroupBox grpTest;
        private System.Windows.Forms.Label lblTestStatus;
        private System.Windows.Forms.Button btnTestSmtp;
        private System.Windows.Forms.TextBox txtTestEmail;
        private System.Windows.Forms.Label lblTestEmail;
        private System.Windows.Forms.Button btnSaveSmtp;
        private System.Windows.Forms.GroupBox grpSmtp;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.CheckBox chkSmtpSsl;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtSenderPassword;
        private System.Windows.Forms.Label lblSenderPassword;
        private System.Windows.Forms.TextBox txtSenderEmail;
        private System.Windows.Forms.Label lblSenderEmail;
        private System.Windows.Forms.TextBox txtSmtpPort;
        private System.Windows.Forms.Label lblSmtpPort;
        private System.Windows.Forms.TextBox txtSmtpHost;
        private System.Windows.Forms.Label lblSmtpHost;
    }
}
