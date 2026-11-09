namespace LTWCHARLIEKODE
{
    partial class Form_TrainerManagement
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblKpiTrainers = new System.Windows.Forms.Label();
            this.lblKpiSessions = new System.Windows.Forms.Label();
            this.grpTrainerInfo = new System.Windows.Forms.GroupBox();
            this.lblTrainerId = new System.Windows.Forms.Label();
            this.txtTrainerId = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblSpecialty = new System.Windows.Forms.Label();
            this.cboSpecialty = new System.Windows.Forms.ComboBox();
            this.lblCommissionRate = new System.Windows.Forms.Label();
            this.numCommissionRate = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilterSpecialty = new System.Windows.Forms.Label();
            this.cboFilterSpecialty = new System.Windows.Forms.ComboBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.dgvTrainers = new System.Windows.Forms.DataGridView();
            this.grpScheduleDetails = new System.Windows.Forms.GroupBox();
            this.dgvTrainerSchedules = new System.Windows.Forms.DataGridView();
            this.lblTotalCommission = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.grpTrainerInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommissionRate)).BeginInit();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainers)).BeginInit();
            this.grpScheduleDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainerSchedules)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(43)))), ((int)(((byte)(73)))));
            this.pnlHeader.Controls.Add(this.lblKpiSessions);
            this.pnlHeader.Controls.Add(this.lblKpiTrainers);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1084, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(465, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "💪 QUẢN LÝ HUẤN LUYỆN VIÊN (PT) & HOA HỒNG CA DẠY";
            // 
            // lblKpiTrainers
            // 
            this.lblKpiTrainers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKpiTrainers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblKpiTrainers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiTrainers.ForeColor = System.Drawing.Color.White;
            this.lblKpiTrainers.Location = new System.Drawing.Point(740, 15);
            this.lblKpiTrainers.Name = "lblKpiTrainers";
            this.lblKpiTrainers.Size = new System.Drawing.Size(150, 30);
            this.lblKpiTrainers.TabIndex = 1;
            this.lblKpiTrainers.Text = "👨‍🏫 Tổng PT: 0";
            this.lblKpiTrainers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKpiSessions
            // 
            this.lblKpiSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKpiSessions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblKpiSessions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiSessions.ForeColor = System.Drawing.Color.White;
            this.lblKpiSessions.Location = new System.Drawing.Point(905, 15);
            this.lblKpiSessions.Name = "lblKpiSessions";
            this.lblKpiSessions.Size = new System.Drawing.Size(160, 30);
            this.lblKpiSessions.TabIndex = 2;
            this.lblKpiSessions.Text = "✅ Ca đã hoàn thành: 0";
            this.lblKpiSessions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpTrainerInfo
            // 
            this.grpTrainerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTrainerInfo.Controls.Add(this.btnClear);
            this.grpTrainerInfo.Controls.Add(this.btnDelete);
            this.grpTrainerInfo.Controls.Add(this.btnUpdate);
            this.grpTrainerInfo.Controls.Add(this.btnEdit);
            this.grpTrainerInfo.Controls.Add(this.btnAdd);
            this.grpTrainerInfo.Controls.Add(this.numCommissionRate);
            this.grpTrainerInfo.Controls.Add(this.lblCommissionRate);
            this.grpTrainerInfo.Controls.Add(this.cboSpecialty);
            this.grpTrainerInfo.Controls.Add(this.lblSpecialty);
            this.grpTrainerInfo.Controls.Add(this.txtPhone);
            this.grpTrainerInfo.Controls.Add(this.lblPhone);
            this.grpTrainerInfo.Controls.Add(this.txtFullName);
            this.grpTrainerInfo.Controls.Add(this.lblFullName);
            this.grpTrainerInfo.Controls.Add(this.txtTrainerId);
            this.grpTrainerInfo.Controls.Add(this.lblTrainerId);
            this.grpTrainerInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpTrainerInfo.Location = new System.Drawing.Point(18, 70);
            this.grpTrainerInfo.Name = "grpTrainerInfo";
            this.grpTrainerInfo.Size = new System.Drawing.Size(1048, 115);
            this.grpTrainerInfo.TabIndex = 1;
            this.grpTrainerInfo.TabStop = false;
            this.grpTrainerInfo.Text = "Thông Tin Huấn Luyện Viên";
            // 
            // lblTrainerId
            // 
            this.lblTrainerId.AutoSize = true;
            this.lblTrainerId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTrainerId.Location = new System.Drawing.Point(15, 30);
            this.lblTrainerId.Name = "lblTrainerId";
            this.lblTrainerId.Size = new System.Drawing.Size(49, 15);
            this.lblTrainerId.TabIndex = 0;
            this.lblTrainerId.Text = "Mã HLV:";
            // 
            // txtTrainerId
            // 
            this.txtTrainerId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTrainerId.Location = new System.Drawing.Point(70, 26);
            this.txtTrainerId.Name = "txtTrainerId";
            this.txtTrainerId.ReadOnly = true;
            this.txtTrainerId.Size = new System.Drawing.Size(60, 23);
            this.txtTrainerId.TabIndex = 1;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFullName.Location = new System.Drawing.Point(145, 30);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(61, 15);
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "Họ và tên:";
            // 
            // txtFullName
            // 
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFullName.Location = new System.Drawing.Point(212, 26);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(185, 23);
            this.txtFullName.TabIndex = 3;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPhone.Location = new System.Drawing.Point(415, 30);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(79, 15);
            this.lblPhone.TabIndex = 4;
            this.lblPhone.Text = "Số điện thoại:";
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPhone.Location = new System.Drawing.Point(500, 26);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(130, 23);
            this.txtPhone.TabIndex = 5;
            // 
            // lblSpecialty
            // 
            this.lblSpecialty.AutoSize = true;
            this.lblSpecialty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSpecialty.Location = new System.Drawing.Point(645, 30);
            this.lblSpecialty.Name = "lblSpecialty";
            this.lblSpecialty.Size = new System.Drawing.Size(78, 15);
            this.lblSpecialty.TabIndex = 6;
            this.lblSpecialty.Text = "Chuyên môn:";
            // 
            // cboSpecialty
            // 
            this.cboSpecialty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboSpecialty.FormattingEnabled = true;
            this.cboSpecialty.Items.AddRange(new object[] {
            "Thể hình & Tăng cơ (Bodybuilding)",
            "Giảm cân & Cardio (Fitness)",
            "Yoga & Thư giãn (Yoga Coach)",
            "Boxing & Kickfit đối kháng",
            "Pilates & Phục hồi chức năng"});
            this.cboSpecialty.Location = new System.Drawing.Point(728, 26);
            this.cboSpecialty.Name = "cboSpecialty";
            this.cboSpecialty.Size = new System.Drawing.Size(160, 23);
            this.cboSpecialty.TabIndex = 7;
            // 
            // lblCommissionRate
            // 
            this.lblCommissionRate.AutoSize = true;
            this.lblCommissionRate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCommissionRate.Location = new System.Drawing.Point(900, 30);
            this.lblCommissionRate.Name = "lblCommissionRate";
            this.lblCommissionRate.Size = new System.Drawing.Size(51, 15);
            this.lblCommissionRate.TabIndex = 8;
            this.lblCommissionRate.Text = "HH/Ca:";
            // 
            // numCommissionRate
            // 
            this.numCommissionRate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numCommissionRate.Increment = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numCommissionRate.Location = new System.Drawing.Point(955, 26);
            this.numCommissionRate.Maximum = new decimal(new int[] { 2000000, 0, 0, 0 });
            this.numCommissionRate.Minimum = new decimal(new int[] { 50000, 0, 0, 0 });
            this.numCommissionRate.Name = "numCommissionRate";
            this.numCommissionRate.Size = new System.Drawing.Size(80, 23);
            this.numCommissionRate.TabIndex = 9;
            this.numCommissionRate.ThousandsSeparator = true;
            this.numCommissionRate.Value = new decimal(new int[] { 150000, 0, 0, 0 });
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(18, 68);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 32);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "+ Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(130, 68);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 32);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "✏ Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(242, 68);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 32);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "💾 Lưu thay đổi";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(364, 68);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 32);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "🗑 Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(466, 68);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 32);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Làm mới";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearch.Controls.Add(this.btnExportExcel);
            this.grpSearch.Controls.Add(this.cboFilterSpecialty);
            this.grpSearch.Controls.Add(this.lblFilterSpecialty);
            this.grpSearch.Controls.Add(this.txtSearch);
            this.grpSearch.Controls.Add(this.lblSearch);
            this.grpSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.grpSearch.Location = new System.Drawing.Point(18, 195);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1048, 60);
            this.grpSearch.TabIndex = 2;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm kiếm tức thì & Lọc chuyên môn";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(15, 25);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(53, 17);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Tra cứu:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(75, 22);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(260, 24);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblFilterSpecialty
            // 
            this.lblFilterSpecialty.AutoSize = true;
            this.lblFilterSpecialty.Location = new System.Drawing.Point(365, 25);
            this.lblFilterSpecialty.Name = "lblFilterSpecialty";
            this.lblFilterSpecialty.Size = new System.Drawing.Size(81, 17);
            this.lblFilterSpecialty.TabIndex = 2;
            this.lblFilterSpecialty.Text = "Chuyên môn:";
            // 
            // cboFilterSpecialty
            // 
            this.cboFilterSpecialty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterSpecialty.FormattingEnabled = true;
            this.cboFilterSpecialty.Items.AddRange(new object[] {
            "Tất cả",
            "Thể hình & Tăng cơ (Bodybuilding)",
            "Giảm cân & Cardio (Fitness)",
            "Yoga & Thư giãn (Yoga Coach)",
            "Boxing & Kickfit đối kháng",
            "Pilates & Phục hồi chức năng"});
            this.cboFilterSpecialty.Location = new System.Drawing.Point(455, 22);
            this.cboFilterSpecialty.Name = "cboFilterSpecialty";
            this.cboFilterSpecialty.Size = new System.Drawing.Size(220, 24);
            this.cboFilterSpecialty.TabIndex = 3;
            this.cboFilterSpecialty.SelectedIndexChanged += new System.EventHandler(this.cboFilterSpecialty_SelectedIndexChanged);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(845, 18);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(185, 30);
            this.btnExportExcel.TabIndex = 4;
            this.btnExportExcel.Text = "📊 Xuất Excel (EPPlus)";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // dgvTrainers
            // 
            this.dgvTrainers.AllowUserToAddRows = false;
            this.dgvTrainers.AllowUserToDeleteRows = false;
            this.dgvTrainers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrainers.BackgroundColor = System.Drawing.Color.White;
            this.dgvTrainers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrainers.Location = new System.Drawing.Point(18, 265);
            this.dgvTrainers.MultiSelect = false;
            this.dgvTrainers.Name = "dgvTrainers";
            this.dgvTrainers.ReadOnly = true;
            this.dgvTrainers.RowHeadersWidth = 35;
            this.dgvTrainers.RowTemplate.Height = 30;
            this.dgvTrainers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrainers.Size = new System.Drawing.Size(1048, 185);
            this.dgvTrainers.TabIndex = 3;
            this.dgvTrainers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrainers_CellClick);
            // 
            // grpScheduleDetails
            // 
            this.grpScheduleDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpScheduleDetails.Controls.Add(this.lblTotalCommission);
            this.grpScheduleDetails.Controls.Add(this.dgvTrainerSchedules);
            this.grpScheduleDetails.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpScheduleDetails.Location = new System.Drawing.Point(18, 460);
            this.grpScheduleDetails.Name = "grpScheduleDetails";
            this.grpScheduleDetails.Size = new System.Drawing.Size(1048, 210);
            this.grpScheduleDetails.TabIndex = 4;
            this.grpScheduleDetails.TabStop = false;
            this.grpScheduleDetails.Text = "Lịch Trình Ca Dạy & Chi Tiết Hoa Hồng Của HLV (Chọn HLV phía trên để xem)";
            // 
            // dgvTrainerSchedules
            // 
            this.dgvTrainerSchedules.AllowUserToAddRows = false;
            this.dgvTrainerSchedules.AllowUserToDeleteRows = false;
            this.dgvTrainerSchedules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrainerSchedules.BackgroundColor = System.Drawing.Color.White;
            this.dgvTrainerSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrainerSchedules.Location = new System.Drawing.Point(15, 25);
            this.dgvTrainerSchedules.MultiSelect = false;
            this.dgvTrainerSchedules.Name = "dgvTrainerSchedules";
            this.dgvTrainerSchedules.ReadOnly = true;
            this.dgvTrainerSchedules.RowHeadersWidth = 30;
            this.dgvTrainerSchedules.RowTemplate.Height = 28;
            this.dgvTrainerSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrainerSchedules.Size = new System.Drawing.Size(1015, 145);
            this.dgvTrainerSchedules.TabIndex = 0;
            // 
            // lblTotalCommission
            // 
            this.lblTotalCommission.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalCommission.AutoSize = true;
            this.lblTotalCommission.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalCommission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblTotalCommission.Location = new System.Drawing.Point(15, 180);
            this.lblTotalCommission.Name = "lblTotalCommission";
            this.lblTotalCommission.Size = new System.Drawing.Size(320, 19);
            this.lblTotalCommission.TabIndex = 1;
            this.lblTotalCommission.Text = "💰 Tổng hoa hồng tích lũy: 0 VNĐ (0 ca thực dạy)";
            // 
            // Form_TrainerManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1084, 685);
            this.Controls.Add(this.grpScheduleDetails);
            this.Controls.Add(this.dgvTrainers);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.grpTrainerInfo);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "Form_TrainerManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý Huấn luyện viên (PT) & Tính hoa hồng ca dạy";
            this.Load += new System.EventHandler(this.Form_TrainerManagement_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpTrainerInfo.ResumeLayout(false);
            this.grpTrainerInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommissionRate)).EndInit();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainers)).EndInit();
            this.grpScheduleDetails.ResumeLayout(false);
            this.grpScheduleDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainerSchedules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblKpiTrainers;
        private System.Windows.Forms.Label lblKpiSessions;
        private System.Windows.Forms.GroupBox grpTrainerInfo;
        private System.Windows.Forms.Label lblTrainerId;
        private System.Windows.Forms.TextBox txtTrainerId;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblSpecialty;
        private System.Windows.Forms.ComboBox cboSpecialty;
        private System.Windows.Forms.Label lblCommissionRate;
        private System.Windows.Forms.NumericUpDown numCommissionRate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilterSpecialty;
        private System.Windows.Forms.ComboBox cboFilterSpecialty;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.DataGridView dgvTrainers;
        private System.Windows.Forms.GroupBox grpScheduleDetails;
        private System.Windows.Forms.DataGridView dgvTrainerSchedules;
        private System.Windows.Forms.Label lblTotalCommission;
    }
}
