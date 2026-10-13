namespace LTWCHARLIEKODE
{
    partial class Form_PackageManagement
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRoleBadge = new System.Windows.Forms.Label();
            this.grpInfo = new System.Windows.Forms.GroupBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtRegisteredCount = new System.Windows.Forms.TextBox();
            this.lblRegisteredCount = new System.Windows.Forms.Label();
            this.cboPackageType = new System.Windows.Forms.ComboBox();
            this.lblPackageType = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtDurationDays = new System.Windows.Forms.TextBox();
            this.lblDurationDays = new System.Windows.Forms.Label();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.txtPackageId = new System.Windows.Forms.TextBox();
            this.lblPackageId = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cboTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.dgvPackages = new System.Windows.Forms.DataGridView();
            this.grpInfo.SuspendLayout();
            this.grpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(90)))), ((int)(((byte)(157)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(306, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DANH MỤC GÓI TẬP";
            // 
            // lblRoleBadge
            // 
            this.lblRoleBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRoleBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.lblRoleBadge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRoleBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoleBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(71)))), ((int)(((byte)(161)))));
            this.lblRoleBadge.Location = new System.Drawing.Point(520, 16);
            this.lblRoleBadge.Name = "lblRoleBadge";
            this.lblRoleBadge.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.lblRoleBadge.Size = new System.Drawing.Size(520, 36);
            this.lblRoleBadge.TabIndex = 1;
            this.lblRoleBadge.Text = "Vai trò: Đang xác thực...";
            this.lblRoleBadge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpInfo
            // 
            this.grpInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInfo.Controls.Add(this.chkIsActive);
            this.grpInfo.Controls.Add(this.txtRegisteredCount);
            this.grpInfo.Controls.Add(this.lblRegisteredCount);
            this.grpInfo.Controls.Add(this.cboPackageType);
            this.grpInfo.Controls.Add(this.lblPackageType);
            this.grpInfo.Controls.Add(this.txtPrice);
            this.grpInfo.Controls.Add(this.lblPrice);
            this.grpInfo.Controls.Add(this.txtDurationDays);
            this.grpInfo.Controls.Add(this.lblDurationDays);
            this.grpInfo.Controls.Add(this.txtPackageName);
            this.grpInfo.Controls.Add(this.lblPackageName);
            this.grpInfo.Controls.Add(this.txtPackageId);
            this.grpInfo.Controls.Add(this.lblPackageId);
            this.grpInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpInfo.Location = new System.Drawing.Point(25, 58);
            this.grpInfo.Name = "grpInfo";
            this.grpInfo.Size = new System.Drawing.Size(1015, 125);
            this.grpInfo.TabIndex = 2;
            this.grpInfo.TabStop = false;
            this.grpInfo.Text = "Thông tin gói tập";
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.ForeColor = System.Drawing.Color.Green;
            this.chkIsActive.Location = new System.Drawing.Point(745, 76);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(178, 25);
            this.chkIsActive.TabIndex = 6;
            this.chkIsActive.Text = "Đang áp dụng gói";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtRegisteredCount
            // 
            this.txtRegisteredCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtRegisteredCount.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegisteredCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.txtRegisteredCount.Location = new System.Drawing.Point(860, 28);
            this.txtRegisteredCount.Name = "txtRegisteredCount";
            this.txtRegisteredCount.ReadOnly = true;
            this.txtRegisteredCount.Size = new System.Drawing.Size(130, 29);
            this.txtRegisteredCount.TabIndex = 5;
            this.txtRegisteredCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblRegisteredCount
            // 
            this.lblRegisteredCount.AutoSize = true;
            this.lblRegisteredCount.Location = new System.Drawing.Point(710, 32);
            this.lblRegisteredCount.Name = "lblRegisteredCount";
            this.lblRegisteredCount.Size = new System.Drawing.Size(144, 21);
            this.lblRegisteredCount.TabIndex = 10;
            this.lblRegisteredCount.Text = "Số người đăng ký:";
            // 
            // cboPackageType
            // 
            this.cboPackageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPackageType.FormattingEnabled = true;
            this.cboPackageType.Items.AddRange(new object[] {
            "Có PT",
            "Không có PT"});
            this.cboPackageType.Location = new System.Drawing.Point(490, 74);
            this.cboPackageType.Name = "cboPackageType";
            this.cboPackageType.Size = new System.Drawing.Size(190, 29);
            this.cboPackageType.TabIndex = 4;
            // 
            // lblPackageType
            // 
            this.lblPackageType.AutoSize = true;
            this.lblPackageType.Location = new System.Drawing.Point(380, 77);
            this.lblPackageType.Name = "lblPackageType";
            this.lblPackageType.Size = new System.Drawing.Size(103, 21);
            this.lblPackageType.TabIndex = 8;
            this.lblPackageType.Text = "Phân loại gói:";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(490, 28);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(190, 29);
            this.txtPrice.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(380, 32);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(95, 21);
            this.lblPrice.TabIndex = 6;
            this.lblPrice.Text = "Đơn giá (VNĐ):";
            // 
            // txtDurationDays
            // 
            this.txtDurationDays.Location = new System.Drawing.Point(245, 28);
            this.txtDurationDays.Name = "txtDurationDays";
            this.txtDurationDays.Size = new System.Drawing.Size(100, 29);
            this.txtDurationDays.TabIndex = 2;
            // 
            // lblDurationDays
            // 
            this.lblDurationDays.AutoSize = true;
            this.lblDurationDays.Location = new System.Drawing.Point(175, 32);
            this.lblDurationDays.Name = "lblDurationDays";
            this.lblDurationDays.Size = new System.Drawing.Size(68, 21);
            this.lblDurationDays.TabIndex = 4;
            this.lblDurationDays.Text = "Số ngày:";
            // 
            // txtPackageName
            // 
            this.txtPackageName.Location = new System.Drawing.Point(110, 74);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(235, 29);
            this.txtPackageName.TabIndex = 1;
            // 
            // lblPackageName
            // 
            this.lblPackageName.AutoSize = true;
            this.lblPackageName.Location = new System.Drawing.Point(18, 77);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(89, 21);
            this.lblPackageName.TabIndex = 2;
            this.lblPackageName.Text = "Tên gói tập:";
            // 
            // txtPackageId
            // 
            this.txtPackageId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPackageId.Location = new System.Drawing.Point(110, 28);
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.ReadOnly = true;
            this.txtPackageId.Size = new System.Drawing.Size(55, 29);
            this.txtPackageId.TabIndex = 0;
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.Location = new System.Drawing.Point(18, 32);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(65, 21);
            this.lblPackageId.TabIndex = 0;
            this.lblPackageId.Text = "Mã gói:";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(25, 195);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(115, 38);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(150, 195);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 38);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Chỉnh sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(270, 195);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 38);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(390, 195);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 38);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(490, 195);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 38);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Nhập lại";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnReload
            // 
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Location = new System.Drawing.Point(600, 195);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(105, 38);
            this.btnReload.TabIndex = 8;
            this.btnReload.Text = "Làm mới";
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // grpSearch
            // 
            this.grpSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.txtSearch);
            this.grpSearch.Controls.Add(this.lblSearch);
            this.grpSearch.Controls.Add(this.cboTypeFilter);
            this.grpSearch.Controls.Add(this.lblFilter);
            this.grpSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSearch.Location = new System.Drawing.Point(25, 245);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(1015, 65);
            this.grpSearch.TabIndex = 9;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Tìm kiếm & Lọc";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(390, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(95, 31);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(145, 24);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(235, 29);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(18, 28);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(121, 21);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Từ khóa tra cứu:";
            // 
            // cboTypeFilter
            // 
            this.cboTypeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeFilter.FormattingEnabled = true;
            this.cboTypeFilter.Items.AddRange(new object[] {
            "-- Tất cả gói tập --",
            "Có PT",
            "Không có PT"});
            this.cboTypeFilter.Location = new System.Drawing.Point(775, 24);
            this.cboTypeFilter.Name = "cboTypeFilter";
            this.cboTypeFilter.Size = new System.Drawing.Size(225, 29);
            this.cboTypeFilter.TabIndex = 4;
            this.cboTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cboTypeFilter_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(645, 28);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(125, 21);
            this.lblFilter.TabIndex = 3;
            this.lblFilter.Text = "Lọc loại gói tập:";
            // 
            // dgvPackages
            // 
            this.dgvPackages.AllowUserToAddRows = false;
            this.dgvPackages.AllowUserToDeleteRows = false;
            this.dgvPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPackages.BackgroundColor = System.Drawing.Color.White;
            this.dgvPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPackages.Location = new System.Drawing.Point(25, 320);
            this.dgvPackages.MultiSelect = false;
            this.dgvPackages.Name = "dgvPackages";
            this.dgvPackages.ReadOnly = true;
            this.dgvPackages.RowHeadersWidth = 35;
            this.dgvPackages.RowTemplate.Height = 32;
            this.dgvPackages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackages.Size = new System.Drawing.Size(1015, 355);
            this.dgvPackages.TabIndex = 10;
            this.dgvPackages.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPackages_CellClick);
            // 
            // Form_PackageManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1065, 700);
            this.Controls.Add(this.dgvPackages);
            this.Controls.Add(this.grpSearch);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpInfo);
            this.Controls.Add(this.lblRoleBadge);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(1050, 680);
            this.Name = "Form_PackageManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh mục gói tập";
            this.Load += new System.EventHandler(this.Form_PackageManagement_Load);
            this.grpInfo.ResumeLayout(false);
            this.grpInfo.PerformLayout();
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRoleBadge;
        private System.Windows.Forms.GroupBox grpInfo;
        private System.Windows.Forms.TextBox txtPackageId;
        private System.Windows.Forms.Label lblPackageId;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.Label lblPackageName;
        private System.Windows.Forms.TextBox txtDurationDays;
        private System.Windows.Forms.Label lblDurationDays;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.ComboBox cboPackageType;
        private System.Windows.Forms.Label lblPackageType;
        private System.Windows.Forms.TextBox txtRegisteredCount;
        private System.Windows.Forms.Label lblRegisteredCount;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cboTypeFilter;
        private System.Windows.Forms.DataGridView dgvPackages;
    }
}
