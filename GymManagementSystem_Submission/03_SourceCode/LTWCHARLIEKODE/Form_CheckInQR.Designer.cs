namespace LTWCHARLIEKODE
{
    partial class Form_CheckInQR
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblCameraSelect = new System.Windows.Forms.Label();
            this.cboCamera = new System.Windows.Forms.ComboBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnStopCamera = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.grpCamera = new System.Windows.Forms.GroupBox();
            this.picWebcam = new System.Windows.Forms.PictureBox();
            this.grpManual = new System.Windows.Forms.GroupBox();
            this.lblManualPrompt = new System.Windows.Forms.Label();
            this.txtManualCode = new System.Windows.Forms.TextBox();
            this.btnManualCheckIn = new System.Windows.Forms.Button();
            this.grpMemberInfo = new System.Windows.Forms.GroupBox();
            this.pnlResultHeader = new System.Windows.Forms.Panel();
            this.lblResultStatus = new System.Windows.Forms.Label();
            this.lblResultMessage = new System.Windows.Forms.Label();
            this.lblInfoMemberId = new System.Windows.Forms.Label();
            this.lblInfoFullName = new System.Windows.Forms.Label();
            this.lblInfoPhone = new System.Windows.Forms.Label();
            this.lblInfoEmail = new System.Windows.Forms.Label();
            this.lblInfoPackage = new System.Windows.Forms.Label();
            this.lblInfoExpiry = new System.Windows.Forms.Label();
            this.grpHistory = new System.Windows.Forms.GroupBox();
            this.dgvCheckInHistory = new System.Windows.Forms.DataGridView();
            this.lblTotalCheckInToday = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.grpCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).BeginInit();
            this.grpManual.SuspendLayout();
            this.grpMemberInfo.SuspendLayout();
            this.pnlResultHeader.SuspendLayout();
            this.grpHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckInHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(984, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HỆ THỐNG QUÉT MÃ QR CHECK-IN HỘI VIÊN";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblCameraSelect);
            this.pnlTop.Controls.Add(this.cboCamera);
            this.pnlTop.Controls.Add(this.btnStartCamera);
            this.pnlTop.Controls.Add(this.btnStopCamera);
            this.pnlTop.Controls.Add(this.btnSelectImage);
            this.pnlTop.Controls.Add(this.lblCameraStatus);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 45);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 55);
            this.pnlTop.TabIndex = 1;
            // 
            // lblCameraSelect
            // 
            this.lblCameraSelect.AutoSize = true;
            this.lblCameraSelect.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraSelect.Location = new System.Drawing.Point(15, 18);
            this.lblCameraSelect.Name = "lblCameraSelect";
            this.lblCameraSelect.Size = new System.Drawing.Size(91, 17);
            this.lblCameraSelect.TabIndex = 0;
            this.lblCameraSelect.Text = "Chọn Camera:";
            // 
            // cboCamera
            // 
            this.cboCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCamera.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCamera.FormattingEnabled = true;
            this.cboCamera.Location = new System.Drawing.Point(115, 15);
            this.cboCamera.Name = "cboCamera";
            this.cboCamera.Size = new System.Drawing.Size(220, 25);
            this.cboCamera.TabIndex = 1;
            // 
            // btnStartCamera
            // 
            this.btnStartCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnStartCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCamera.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCamera.ForeColor = System.Drawing.Color.White;
            this.btnStartCamera.Location = new System.Drawing.Point(345, 12);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(105, 32);
            this.btnStartCamera.TabIndex = 2;
            this.btnStartCamera.Text = "▶ Bật Camera";
            this.btnStartCamera.UseVisualStyleBackColor = false;
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);
            // 
            // btnStopCamera
            // 
            this.btnStopCamera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnStopCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopCamera.Enabled = false;
            this.btnStopCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopCamera.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCamera.ForeColor = System.Drawing.Color.White;
            this.btnStopCamera.Location = new System.Drawing.Point(460, 12);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(105, 32);
            this.btnStopCamera.TabIndex = 3;
            this.btnStopCamera.Text = "⏹ Tắt Camera";
            this.btnStopCamera.UseVisualStyleBackColor = false;
            this.btnStopCamera.Click += new System.EventHandler(this.btnStopCamera_Click);
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSelectImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectImage.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectImage.ForeColor = System.Drawing.Color.White;
            this.btnSelectImage.Location = new System.Drawing.Point(575, 12);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(135, 32);
            this.btnSelectImage.TabIndex = 4;
            this.btnSelectImage.Text = "📂 Chọn ảnh QR...";
            this.btnSelectImage.UseVisualStyleBackColor = false;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // lblCameraStatus
            // 
            this.lblCameraStatus.AutoSize = true;
            this.lblCameraStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCameraStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblCameraStatus.Location = new System.Drawing.Point(720, 20);
            this.lblCameraStatus.Name = "lblCameraStatus";
            this.lblCameraStatus.Size = new System.Drawing.Size(120, 15);
            this.lblCameraStatus.TabIndex = 5;
            this.lblCameraStatus.Text = "Đang tải thiết bị...";
            // 
            // grpCamera
            // 
            this.grpCamera.Controls.Add(this.picWebcam);
            this.grpCamera.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCamera.Location = new System.Drawing.Point(15, 110);
            this.grpCamera.Name = "grpCamera";
            this.grpCamera.Size = new System.Drawing.Size(430, 360);
            this.grpCamera.TabIndex = 2;
            this.grpCamera.TabStop = false;
            this.grpCamera.Text = "Khung hình Camera trực tiếp";
            // 
            // picWebcam
            // 
            this.picWebcam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.picWebcam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picWebcam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picWebcam.Location = new System.Drawing.Point(3, 20);
            this.picWebcam.Name = "picWebcam";
            this.picWebcam.Size = new System.Drawing.Size(424, 337);
            this.picWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picWebcam.TabIndex = 0;
            this.picWebcam.TabStop = false;
            // 
            // grpManual
            // 
            this.grpManual.Controls.Add(this.lblManualPrompt);
            this.grpManual.Controls.Add(this.txtManualCode);
            this.grpManual.Controls.Add(this.btnManualCheckIn);
            this.grpManual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpManual.Location = new System.Drawing.Point(15, 480);
            this.grpManual.Name = "grpManual";
            this.grpManual.Size = new System.Drawing.Size(430, 75);
            this.grpManual.TabIndex = 3;
            this.grpManual.TabStop = false;
            this.grpManual.Text = "Nhập tay (Mã QR / Mã hội viên / SĐT)";
            // 
            // lblManualPrompt
            // 
            this.lblManualPrompt.AutoSize = true;
            this.lblManualPrompt.Location = new System.Drawing.Point(10, 32);
            this.lblManualPrompt.Name = "lblManualPrompt";
            this.lblManualPrompt.Size = new System.Drawing.Size(30, 17);
            this.lblManualPrompt.TabIndex = 0;
            this.lblManualPrompt.Text = "Mã:";
            // 
            // txtManualCode
            // 
            this.txtManualCode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManualCode.Location = new System.Drawing.Point(45, 29);
            this.txtManualCode.Name = "txtManualCode";
            this.txtManualCode.Size = new System.Drawing.Size(250, 25);
            this.txtManualCode.TabIndex = 1;
            // 
            // btnManualCheckIn
            // 
            this.btnManualCheckIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnManualCheckIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManualCheckIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualCheckIn.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnManualCheckIn.Location = new System.Drawing.Point(305, 26);
            this.btnManualCheckIn.Name = "btnManualCheckIn";
            this.btnManualCheckIn.Size = new System.Drawing.Size(115, 30);
            this.btnManualCheckIn.TabIndex = 2;
            this.btnManualCheckIn.Text = "Check-in";
            this.btnManualCheckIn.UseVisualStyleBackColor = false;
            this.btnManualCheckIn.Click += new System.EventHandler(this.btnManualCheckIn_Click);
            // 
            // grpMemberInfo
            // 
            this.grpMemberInfo.Controls.Add(this.pnlResultHeader);
            this.grpMemberInfo.Controls.Add(this.lblInfoMemberId);
            this.grpMemberInfo.Controls.Add(this.lblInfoFullName);
            this.grpMemberInfo.Controls.Add(this.lblInfoPhone);
            this.grpMemberInfo.Controls.Add(this.lblInfoEmail);
            this.grpMemberInfo.Controls.Add(this.lblInfoPackage);
            this.grpMemberInfo.Controls.Add(this.lblInfoExpiry);
            this.grpMemberInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpMemberInfo.Location = new System.Drawing.Point(460, 110);
            this.grpMemberInfo.Name = "grpMemberInfo";
            this.grpMemberInfo.Size = new System.Drawing.Size(510, 235);
            this.grpMemberInfo.TabIndex = 4;
            this.grpMemberInfo.TabStop = false;
            this.grpMemberInfo.Text = "Kết quả nhận diện Hội viên";
            // 
            // pnlResultHeader
            // 
            this.pnlResultHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.pnlResultHeader.Controls.Add(this.lblResultStatus);
            this.pnlResultHeader.Controls.Add(this.lblResultMessage);
            this.pnlResultHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlResultHeader.Location = new System.Drawing.Point(3, 20);
            this.pnlResultHeader.Name = "pnlResultHeader";
            this.pnlResultHeader.Size = new System.Drawing.Size(504, 65);
            this.pnlResultHeader.TabIndex = 0;
            // 
            // lblResultStatus
            // 
            this.lblResultStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResultStatus.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultStatus.ForeColor = System.Drawing.Color.White;
            this.lblResultStatus.Location = new System.Drawing.Point(0, 0);
            this.lblResultStatus.Name = "lblResultStatus";
            this.lblResultStatus.Size = new System.Drawing.Size(504, 30);
            this.lblResultStatus.TabIndex = 0;
            this.lblResultStatus.Text = "CHỜ QUÉT MÃ QR...";
            this.lblResultStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResultMessage
            // 
            this.lblResultMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblResultMessage.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultMessage.ForeColor = System.Drawing.Color.White;
            this.lblResultMessage.Location = new System.Drawing.Point(0, 30);
            this.lblResultMessage.Name = "lblResultMessage";
            this.lblResultMessage.Size = new System.Drawing.Size(504, 35);
            this.lblResultMessage.TabIndex = 1;
            this.lblResultMessage.Text = "Vui lòng đưa thẻ hội viên hoặc mã QR vào trước camera";
            this.lblResultMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfoMemberId
            // 
            this.lblInfoMemberId.AutoSize = true;
            this.lblInfoMemberId.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoMemberId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblInfoMemberId.Location = new System.Drawing.Point(20, 95);
            this.lblInfoMemberId.Name = "lblInfoMemberId";
            this.lblInfoMemberId.Size = new System.Drawing.Size(73, 19);
            this.lblInfoMemberId.TabIndex = 1;
            this.lblInfoMemberId.Text = "Mã HV: --";
            // 
            // lblInfoFullName
            // 
            this.lblInfoFullName.AutoSize = true;
            this.lblInfoFullName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblInfoFullName.Location = new System.Drawing.Point(20, 120);
            this.lblInfoFullName.Name = "lblInfoFullName";
            this.lblInfoFullName.Size = new System.Drawing.Size(183, 20);
            this.lblInfoFullName.TabIndex = 2;
            this.lblInfoFullName.Text = "Họ tên: Chưa có dữ liệu";
            // 
            // lblInfoPhone
            // 
            this.lblInfoPhone.AutoSize = true;
            this.lblInfoPhone.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoPhone.Location = new System.Drawing.Point(20, 150);
            this.lblInfoPhone.Name = "lblInfoPhone";
            this.lblInfoPhone.Size = new System.Drawing.Size(53, 17);
            this.lblInfoPhone.TabIndex = 3;
            this.lblInfoPhone.Text = "SĐT: --";
            // 
            // lblInfoEmail
            // 
            this.lblInfoEmail.AutoSize = true;
            this.lblInfoEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoEmail.Location = new System.Drawing.Point(260, 150);
            this.lblInfoEmail.Name = "lblInfoEmail";
            this.lblInfoEmail.Size = new System.Drawing.Size(61, 17);
            this.lblInfoEmail.TabIndex = 4;
            this.lblInfoEmail.Text = "Email: --";
            // 
            // lblInfoPackage
            // 
            this.lblInfoPackage.AutoSize = true;
            this.lblInfoPackage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoPackage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.lblInfoPackage.Location = new System.Drawing.Point(20, 178);
            this.lblInfoPackage.Name = "lblInfoPackage";
            this.lblInfoPackage.Size = new System.Drawing.Size(84, 19);
            this.lblInfoPackage.TabIndex = 5;
            this.lblInfoPackage.Text = "Gói tập: --";
            // 
            // lblInfoExpiry
            // 
            this.lblInfoExpiry.AutoSize = true;
            this.lblInfoExpiry.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoExpiry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.lblInfoExpiry.Location = new System.Drawing.Point(20, 205);
            this.lblInfoExpiry.Name = "lblInfoExpiry";
            this.lblInfoExpiry.Size = new System.Drawing.Size(95, 17);
            this.lblInfoExpiry.TabIndex = 6;
            this.lblInfoExpiry.Text = "Hạn dùng: --";
            // 
            // grpHistory
            // 
            this.grpHistory.Controls.Add(this.dgvCheckInHistory);
            this.grpHistory.Controls.Add(this.lblTotalCheckInToday);
            this.grpHistory.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpHistory.Location = new System.Drawing.Point(460, 355);
            this.grpHistory.Name = "grpHistory";
            this.grpHistory.Size = new System.Drawing.Size(510, 200);
            this.grpHistory.TabIndex = 5;
            this.grpHistory.TabStop = false;
            this.grpHistory.Text = "Lịch sử Check-in gần nhất";
            // 
            // dgvCheckInHistory
            // 
            this.dgvCheckInHistory.AllowUserToAddRows = false;
            this.dgvCheckInHistory.AllowUserToDeleteRows = false;
            this.dgvCheckInHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCheckInHistory.BackgroundColor = System.Drawing.Color.White;
            this.dgvCheckInHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCheckInHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCheckInHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCheckInHistory.Location = new System.Drawing.Point(3, 20);
            this.dgvCheckInHistory.Name = "dgvCheckInHistory";
            this.dgvCheckInHistory.ReadOnly = true;
            this.dgvCheckInHistory.RowHeadersVisible = false;
            this.dgvCheckInHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCheckInHistory.Size = new System.Drawing.Size(504, 152);
            this.dgvCheckInHistory.TabIndex = 0;
            // 
            // lblTotalCheckInToday
            // 
            this.lblTotalCheckInToday.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotalCheckInToday.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCheckInToday.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTotalCheckInToday.Location = new System.Drawing.Point(3, 172);
            this.lblTotalCheckInToday.Name = "lblTotalCheckInToday";
            this.lblTotalCheckInToday.Size = new System.Drawing.Size(504, 25);
            this.lblTotalCheckInToday.TabIndex = 1;
            this.lblTotalCheckInToday.Text = "Tổng lượt quét phiên này: 0";
            this.lblTotalCheckInToday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form_CheckInQR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(984, 570);
            this.Controls.Add(this.grpHistory);
            this.Controls.Add(this.grpMemberInfo);
            this.Controls.Add(this.grpManual);
            this.Controls.Add(this.grpCamera);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form_CheckInQR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quét mã QR Check-in Hội viên";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CheckInQR_FormClosing);
            this.Load += new System.EventHandler(this.Form_CheckInQR_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWebcam)).EndInit();
            this.grpManual.ResumeLayout(false);
            this.grpManual.PerformLayout();
            this.grpMemberInfo.ResumeLayout(false);
            this.grpMemberInfo.PerformLayout();
            this.pnlResultHeader.ResumeLayout(false);
            this.grpHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCheckInHistory)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblCameraSelect;
        private System.Windows.Forms.ComboBox cboCamera;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnStopCamera;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.GroupBox grpCamera;
        private System.Windows.Forms.PictureBox picWebcam;
        private System.Windows.Forms.GroupBox grpManual;
        private System.Windows.Forms.Label lblManualPrompt;
        private System.Windows.Forms.TextBox txtManualCode;
        private System.Windows.Forms.Button btnManualCheckIn;
        private System.Windows.Forms.GroupBox grpMemberInfo;
        private System.Windows.Forms.Panel pnlResultHeader;
        private System.Windows.Forms.Label lblResultStatus;
        private System.Windows.Forms.Label lblResultMessage;
        private System.Windows.Forms.Label lblInfoMemberId;
        private System.Windows.Forms.Label lblInfoFullName;
        private System.Windows.Forms.Label lblInfoPhone;
        private System.Windows.Forms.Label lblInfoEmail;
        private System.Windows.Forms.Label lblInfoPackage;
        private System.Windows.Forms.Label lblInfoExpiry;
        private System.Windows.Forms.GroupBox grpHistory;
        private System.Windows.Forms.DataGridView dgvCheckInHistory;
        private System.Windows.Forms.Label lblTotalCheckInToday;
    }
}
