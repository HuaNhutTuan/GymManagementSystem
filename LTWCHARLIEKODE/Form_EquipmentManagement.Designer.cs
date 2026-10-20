namespace LTWCHARLIEKODE
{
    partial class Form_EquipmentManagement
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
            this.lblKpiActive = new System.Windows.Forms.Label();
            this.lblKpiBroken = new System.Windows.Forms.Label();
            this.lblKpiMaintenance = new System.Windows.Forms.Label();
            this.grpEquipmentInfo = new System.Windows.Forms.GroupBox();
            this.lblEquipmentId = new System.Windows.Forms.Label();
            this.txtEquipmentId = new System.Windows.Forms.TextBox();
            this.lblEquipmentName = new System.Windows.Forms.Label();
            this.txtEquipmentName = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cboFilterStatus = new System.Windows.Forms.ComboBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.dgvEquipments = new System.Windows.Forms.DataGridView();
            this.grpMaintenance = new System.Windows.Forms.GroupBox();
            this.dgvMaintenance = new System.Windows.Forms.DataGridView();
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.lblAddRepair = new System.Windows.Forms.Label();
            this.txtRepairDesc = new System.Windows.Forms.TextBox();
            this.lblRepairCost = new System.Windows.Forms.Label();
            this.numRepairCost = new System.Windows.Forms.NumericUpDown();
            this.lblPerformer = new System.Windows.Forms.Label();
            this.txtPerformer = new System.Windows.Forms.TextBox();
            this.btnAddRepair = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.grpEquipmentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipments)).BeginInit();
            this.grpMaintenance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepairCost)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(43)))), ((int)(((byte)(73)))));
            this.pnlHeader.Controls.Add(this.lblKpiMaintenance);
            this.pnlHeader.Controls.Add(this.lblKpiBroken);
            this.pnlHeader.Controls.Add(this.lblKpiActive);
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
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(370, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🏋️ QUẢN LÝ TRẠNG THÁI THIẾT BỊ & MÁY TẬP";
            // 
            // lblKpiActive
            // 
            this.lblKpiActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKpiActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblKpiActive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiActive.ForeColor = System.Drawing.Color.White;
            this.lblKpiActive.Location = new System.Drawing.Point(620, 15);
            this.lblKpiActive.Name = "lblKpiActive";
            this.lblKpiActive.Size = new System.Drawing.Size(140, 30);
            this.lblKpiActive.TabIndex = 1;
            this.lblKpiActive.Text = "🟢 Hoạt động: 0";
            this.lblKpiActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKpiBroken
            // 
            this.lblKpiBroken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKpiBroken.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblKpiBroken.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiBroken.ForeColor = System.Drawing.Color.White;
            this.lblKpiBroken.Location = new System.Drawing.Point(770, 15);
            this.lblKpiBroken.Name = "lblKpiBroken";
            this.lblKpiBroken.Size = new System.Drawing.Size(130, 30);
            this.lblKpiBroken.TabIndex = 2;
            this.lblKpiBroken.Text = "🔴 Báo hỏng: 0";
            this.lblKpiBroken.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKpiMaintenance
            // 
            this.lblKpiMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKpiMaintenance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.lblKpiMaintenance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiMaintenance.ForeColor = System.Drawing.Color.White;
            this.lblKpiMaintenance.Location = new System.Drawing.Point(910, 15);
            this.lblKpiMaintenance.Name = "lblKpiMaintenance";
            this.lblKpiMaintenance.Size = new System.Drawing.Size(155, 30);
            this.lblKpiMaintenance.TabIndex = 3;
            this.lblKpiMaintenance.Text = "🟡 Đang bảo trì: 0";
            this.lblKpiMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpEquipmentInfo
            // 
            this.grpEquipmentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEquipmentInfo.Controls.Add(this.btnClear);
            this.grpEquipmentInfo.Controls.Add(this.btnDelete);
            this.grpEquipmentInfo.Controls.Add(this.btnUpdate);
            this.grpEquipmentInfo.Controls.Add(this.btnEdit);
            this.grpEquipmentInfo.Controls.Add(this.btnAdd);
            this.grpEquipmentInfo.Controls.Add(this.numQuantity);
            this.grpEquipmentInfo.Controls.Add(this.lblQuantity);
            this.grpEquipmentInfo.Controls.Add(this.cboStatus);
            this.grpEquipmentInfo.Controls.Add(this.lblStatus);
            this.grpEquipmentInfo.Controls.Add(this.txtEquipmentName);
            this.grpEquipmentInfo.Controls.Add(this.lblEquipmentName);
            this.grpEquipmentInfo.Controls.Add(this.txtEquipmentId);
            this.grpEquipmentInfo.Controls.Add(this.lblEquipmentId);
            this.grpEquipmentInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpEquipmentInfo.Location = new System.Drawing.Point(18, 70);
            this.grpEquipmentInfo.Name = "grpEquipmentInfo";
            this.grpEquipmentInfo.Size = new System.Drawing.Size(1048, 115);
            this.grpEquipmentInfo.TabIndex = 1;
            this.grpEquipmentInfo.TabStop = false;
            this.grpEquipmentInfo.Text = "Thông Tin Thiết Bị";
            // 
            // lblEquipmentId
            // 
            this.lblEquipmentId.AutoSize = true;
            this.lblEquipmentId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEquipmentId.Location = new System.Drawing.Point(15, 30);
            this.lblEquipmentId.Name = "lblEquipmentId";
            this.lblEquipmentId.Size = new System.Drawing.Size(46, 15);
            this.lblEquipmentId.TabIndex = 0;
            this.lblEquipmentId.Text = "Mã TB:";
            // 
            // txtEquipmentId
            // 
            this.txtEquipmentId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEquipmentId.Location = new System.Drawing.Point(68, 26);
            this.txtEquipmentId.Name = "txtEquipmentId";
            this.txtEquipmentId.ReadOnly = true;
            this.txtEquipmentId.Size = new System.Drawing.Size(65, 23);
            this.txtEquipmentId.TabIndex = 1;
            // 
            // lblEquipmentName
            // 
            this.lblEquipmentName.AutoSize = true;
            this.lblEquipmentName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEquipmentName.Location = new System.Drawing.Point(150, 30);
            this.lblEquipmentName.Name = "lblEquipmentName";
            this.lblEquipmentName.Size = new System.Drawing.Size(67, 15);
            this.lblEquipmentName.TabIndex = 2;
            this.lblEquipmentName.Text = "Tên thiết bị:";
            // 
            // txtEquipmentName
            // 
            this.txtEquipmentName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEquipmentName.Location = new System.Drawing.Point(225, 26);
            this.txtEquipmentName.Name = "txtEquipmentName";
            this.txtEquipmentName.Size = new System.Drawing.Size(260, 23);
            this.txtEquipmentName.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(505, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(62, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Đang hoạt động",
            "Báo hỏng",
            "Đang bảo trì"});
            this.cboStatus.Location = new System.Drawing.Point(575, 26);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(155, 23);
            this.cboStatus.TabIndex = 5;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblQuantity.Location = new System.Drawing.Point(750, 30);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(57, 15);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "Số lượng:";
            // 
            // numQuantity
            // 
            this.numQuantity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numQuantity.Location = new System.Drawing.Point(815, 26);
            this.numQuantity.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(75, 23);
            this.numQuantity.TabIndex = 7;
            this.numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
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
            this.btnAdd.TabIndex = 8;
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
            this.btnEdit.TabIndex = 9;
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
            this.btnUpdate.TabIndex = 10;
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
            this.btnDelete.TabIndex = 11;
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
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Làm mới";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearch.Controls.Add(this.btnExportExcel);
            this.grpSearch.Controls.Add(this.cboFilterStatus);
            this.grpSearch.Controls.Add(this.lblFilterStatus);
            this.grpSearch.Controls.Add(this.txtSearch);
            this.grpSearch.Controls.Add(this.lblSearch);
            this.grpSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.grpSearch.Location = new System.Drawing.Point(18, 195);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1048, 60);
            this.grpSearch.TabIndex = 2;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm kiếm tức thì & Lọc trạng thái";
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
            this.txtSearch.Size = new System.Drawing.Size(250, 24);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(360, 25);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(70, 17);
            this.lblFilterStatus.TabIndex = 2;
            this.lblFilterStatus.Text = "Trạng thái:";
            // 
            // cboFilterStatus
            // 
            this.cboFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilterStatus.FormattingEnabled = true;
            this.cboFilterStatus.Items.AddRange(new object[] {
            "Tất cả",
            "Đang hoạt động",
            "Báo hỏng",
            "Đang bảo trì"});
            this.cboFilterStatus.Location = new System.Drawing.Point(436, 22);
            this.cboFilterStatus.Name = "cboFilterStatus";
            this.cboFilterStatus.Size = new System.Drawing.Size(160, 24);
            this.cboFilterStatus.TabIndex = 3;
            this.cboFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cboFilterStatus_SelectedIndexChanged);
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
            // dgvEquipments
            // 
            this.dgvEquipments.AllowUserToAddRows = false;
            this.dgvEquipments.AllowUserToDeleteRows = false;
            this.dgvEquipments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEquipments.BackgroundColor = System.Drawing.Color.White;
            this.dgvEquipments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipments.Location = new System.Drawing.Point(18, 265);
            this.dgvEquipments.MultiSelect = false;
            this.dgvEquipments.Name = "dgvEquipments";
            this.dgvEquipments.ReadOnly = true;
            this.dgvEquipments.RowHeadersWidth = 35;
            this.dgvEquipments.RowTemplate.Height = 30;
            this.dgvEquipments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipments.Size = new System.Drawing.Size(1048, 185);
            this.dgvEquipments.TabIndex = 3;
            this.dgvEquipments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEquipments_CellClick);
            // 
            // grpMaintenance
            // 
            this.grpMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMaintenance.Controls.Add(this.btnAddRepair);
            this.grpMaintenance.Controls.Add(this.txtPerformer);
            this.grpMaintenance.Controls.Add(this.lblPerformer);
            this.grpMaintenance.Controls.Add(this.numRepairCost);
            this.grpMaintenance.Controls.Add(this.lblRepairCost);
            this.grpMaintenance.Controls.Add(this.txtRepairDesc);
            this.grpMaintenance.Controls.Add(this.lblAddRepair);
            this.grpMaintenance.Controls.Add(this.lblTotalCost);
            this.grpMaintenance.Controls.Add(this.dgvMaintenance);
            this.grpMaintenance.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.grpMaintenance.Location = new System.Drawing.Point(18, 460);
            this.grpMaintenance.Name = "grpMaintenance";
            this.grpMaintenance.Size = new System.Drawing.Size(1048, 210);
            this.grpMaintenance.TabIndex = 4;
            this.grpMaintenance.TabStop = false;
            this.grpMaintenance.Text = "Lịch Sử Sửa Chữa & Chi Phí Bảo Trì Thiết Bị (Chọn máy tập phía trên để xem)";
            // 
            // dgvMaintenance
            // 
            this.dgvMaintenance.AllowUserToAddRows = false;
            this.dgvMaintenance.AllowUserToDeleteRows = false;
            this.dgvMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMaintenance.BackgroundColor = System.Drawing.Color.White;
            this.dgvMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaintenance.Location = new System.Drawing.Point(15, 25);
            this.dgvMaintenance.MultiSelect = false;
            this.dgvMaintenance.Name = "dgvMaintenance";
            this.dgvMaintenance.ReadOnly = true;
            this.dgvMaintenance.RowHeadersWidth = 30;
            this.dgvMaintenance.RowTemplate.Height = 28;
            this.dgvMaintenance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMaintenance.Size = new System.Drawing.Size(640, 145);
            this.dgvMaintenance.TabIndex = 0;
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTotalCost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTotalCost.Location = new System.Drawing.Point(15, 180);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(195, 17);
            this.lblTotalCost.TabIndex = 1;
            this.lblTotalCost.Text = "Tổng chi phí sửa chữa: 0 VNĐ";
            // 
            // lblAddRepair
            // 
            this.lblAddRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAddRepair.AutoSize = true;
            this.lblAddRepair.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAddRepair.Location = new System.Drawing.Point(670, 25);
            this.lblAddRepair.Name = "lblAddRepair";
            this.lblAddRepair.Size = new System.Drawing.Size(95, 15);
            this.lblAddRepair.TabIndex = 2;
            this.lblAddRepair.Text = "Mô tả hỏng hóc:";
            // 
            // txtRepairDesc
            // 
            this.txtRepairDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRepairDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRepairDesc.Location = new System.Drawing.Point(670, 45);
            this.txtRepairDesc.Multiline = true;
            this.txtRepairDesc.Name = "txtRepairDesc";
            this.txtRepairDesc.Size = new System.Drawing.Size(360, 45);
            this.txtRepairDesc.TabIndex = 3;
            // 
            // lblRepairCost
            // 
            this.lblRepairCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRepairCost.AutoSize = true;
            this.lblRepairCost.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRepairCost.Location = new System.Drawing.Point(670, 100);
            this.lblRepairCost.Name = "lblRepairCost";
            this.lblRepairCost.Size = new System.Drawing.Size(81, 15);
            this.lblRepairCost.TabIndex = 4;
            this.lblRepairCost.Text = "Chi phí (VNĐ):";
            // 
            // numRepairCost
            // 
            this.numRepairCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRepairCost.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numRepairCost.Increment = new decimal(new int[] { 50000, 0, 0, 0 });
            this.numRepairCost.Location = new System.Drawing.Point(670, 120);
            this.numRepairCost.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            this.numRepairCost.Name = "numRepairCost";
            this.numRepairCost.Size = new System.Drawing.Size(160, 23);
            this.numRepairCost.TabIndex = 5;
            this.numRepairCost.ThousandsSeparator = true;
            // 
            // lblPerformer
            // 
            this.lblPerformer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPerformer.AutoSize = true;
            this.lblPerformer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPerformer.Location = new System.Drawing.Point(850, 100);
            this.lblPerformer.Name = "lblPerformer";
            this.lblPerformer.Size = new System.Drawing.Size(95, 15);
            this.lblPerformer.TabIndex = 6;
            this.lblPerformer.Text = "Đơn vị sửa chữa:";
            // 
            // txtPerformer
            // 
            this.txtPerformer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPerformer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPerformer.Location = new System.Drawing.Point(850, 120);
            this.txtPerformer.Name = "txtPerformer";
            this.txtPerformer.Size = new System.Drawing.Size(180, 23);
            this.txtPerformer.TabIndex = 7;
            // 
            // btnAddRepair
            // 
            this.btnAddRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRepair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnAddRepair.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddRepair.FlatAppearance.BorderSize = 0;
            this.btnAddRepair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRepair.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddRepair.ForeColor = System.Drawing.Color.White;
            this.btnAddRepair.Location = new System.Drawing.Point(670, 155);
            this.btnAddRepair.Name = "btnAddRepair";
            this.btnAddRepair.Size = new System.Drawing.Size(360, 32);
            this.btnAddRepair.TabIndex = 8;
            this.btnAddRepair.Text = "🔧 Ghi nhận sửa chữa & Đổi trạng thái thiết bị";
            this.btnAddRepair.UseVisualStyleBackColor = false;
            this.btnAddRepair.Click += new System.EventHandler(this.btnAddRepair_Click);
            // 
            // Form_EquipmentManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1084, 685);
            this.Controls.Add(this.grpMaintenance);
            this.Controls.Add(this.dgvEquipments);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.grpEquipmentInfo);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "Form_EquipmentManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý trạng thái thiết bị & Lịch sử sửa chữa";
            this.Load += new System.EventHandler(this.Form_EquipmentManagement_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpEquipmentInfo.ResumeLayout(false);
            this.grpEquipmentInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipments)).EndInit();
            this.grpMaintenance.ResumeLayout(false);
            this.grpMaintenance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaintenance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepairCost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblKpiActive;
        private System.Windows.Forms.Label lblKpiBroken;
        private System.Windows.Forms.Label lblKpiMaintenance;
        private System.Windows.Forms.GroupBox grpEquipmentInfo;
        private System.Windows.Forms.Label lblEquipmentId;
        private System.Windows.Forms.TextBox txtEquipmentId;
        private System.Windows.Forms.Label lblEquipmentName;
        private System.Windows.Forms.TextBox txtEquipmentName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cboFilterStatus;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.DataGridView dgvEquipments;
        private System.Windows.Forms.GroupBox grpMaintenance;
        private System.Windows.Forms.DataGridView dgvMaintenance;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.Label lblAddRepair;
        private System.Windows.Forms.TextBox txtRepairDesc;
        private System.Windows.Forms.Label lblRepairCost;
        private System.Windows.Forms.NumericUpDown numRepairCost;
        private System.Windows.Forms.Label lblPerformer;
        private System.Windows.Forms.TextBox txtPerformer;
        private System.Windows.Forms.Button btnAddRepair;
    }
}
