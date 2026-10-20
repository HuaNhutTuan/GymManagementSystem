using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_EquipmentManagement : Form
    {
        private int? selectedEquipmentId = null;

        public Form_EquipmentManagement()
        {
            InitializeComponent();
        }

        private void Form_EquipmentManagement_Load(object sender, EventArgs e)
        {
            cboStatus.SelectedIndex = 0; // "Đang hoạt động"
            cboFilterStatus.SelectedIndex = 0; // "Tất cả"
            ResetForm();
            LoadEquipments();
        }

        #region 1. NẠP DỮ LIỆU & THỐNG KÊ KPI

        private void LoadEquipments(string keyword = "", string statusFilter = "Tất cả")
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var query = db.Equipments.AsQueryable();

                    // 1. Tìm kiếm theo tên
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        query = query.Where(eq => eq.EquipmentName.ToLower().Contains(kw));
                    }

                    // 2. Lọc theo trạng thái
                    if (statusFilter != "Tất cả")
                    {
                        query = query.Where(eq => eq.Status == statusFilter);
                    }

                    var list = query.OrderBy(eq => eq.EquipmentId).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã TB", typeof(int));
                    dt.Columns.Add("Tên Thiết Bị / Máy Tập", typeof(string));
                    dt.Columns.Add("Trạng Thái", typeof(string));
                    dt.Columns.Add("Số Lượng", typeof(int));

                    foreach (var item in list)
                    {
                        dt.Rows.Add(item.EquipmentId, item.EquipmentName, item.Status ?? "Đang hoạt động", item.Quantity ?? 0);
                    }

                    dgvEquipments.DataSource = dt;
                    FormatEquipmentsGrid();

                    // Cập nhật số liệu KPI từ toàn bộ cơ sở dữ liệu
                    int activeCount = db.Equipments.Count(x => x.Status == "Đang hoạt động");
                    int brokenCount = db.Equipments.Count(x => x.Status == "Báo hỏng");
                    int maintenanceCount = db.Equipments.Count(x => x.Status == "Đang bảo trì");

                    lblKpiActive.Text = $"🟢 Hoạt động: {activeCount}";
                    lblKpiBroken.Text = $"🔴 Báo hỏng: {brokenCount}";
                    lblKpiMaintenance.Text = $"🟡 Đang bảo trì: {maintenanceCount}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp danh sách thiết bị: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatEquipmentsGrid()
        {
            if (dgvEquipments.Columns["Mã TB"] != null)
            {
                dgvEquipments.Columns["Mã TB"].Width = 80;
                dgvEquipments.Columns["Mã TB"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvEquipments.Columns["Tên Thiết Bị / Máy Tập"] != null)
            {
                dgvEquipments.Columns["Tên Thiết Bị / Máy Tập"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvEquipments.Columns["Tên Thiết Bị / Máy Tập"].MinimumWidth = 250;
            }
            if (dgvEquipments.Columns["Trạng Thái"] != null)
            {
                dgvEquipments.Columns["Trạng Thái"].Width = 160;
                dgvEquipments.Columns["Trạng Thái"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvEquipments.Columns["Số Lượng"] != null)
            {
                dgvEquipments.Columns["Số Lượng"].Width = 100;
                dgvEquipments.Columns["Số Lượng"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Tô màu trực quan cho từng dòng trạng thái
            foreach (DataGridViewRow row in dgvEquipments.Rows)
            {
                string status = row.Cells["Trạng Thái"].Value?.ToString() ?? "";
                if (status == "Đang hoạt động")
                {
                    row.Cells["Trạng Thái"].Style.BackColor = Color.FromArgb(212, 237, 218);
                    row.Cells["Trạng Thái"].Style.ForeColor = Color.FromArgb(21, 87, 36);
                    row.Cells["Trạng Thái"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (status == "Báo hỏng")
                {
                    row.Cells["Trạng Thái"].Style.BackColor = Color.FromArgb(248, 215, 218);
                    row.Cells["Trạng Thái"].Style.ForeColor = Color.FromArgb(114, 28, 36);
                    row.Cells["Trạng Thái"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (status == "Đang bảo trì")
                {
                    row.Cells["Trạng Thái"].Style.BackColor = Color.FromArgb(255, 243, 205);
                    row.Cells["Trạng Thái"].Style.ForeColor = Color.FromArgb(133, 100, 4);
                    row.Cells["Trạng Thái"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
        }

        #endregion

        #region 2. XỬ LÝ CLICK CHỌN & LỊCH SỬ SỬA CHỮA (MAINTENANCE)

        private void dgvEquipments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvEquipments.Rows.Count) return;

            DataGridViewRow row = dgvEquipments.Rows[e.RowIndex];
            selectedEquipmentId = Convert.ToInt32(row.Cells["Mã TB"].Value);

            txtEquipmentId.Text = selectedEquipmentId.ToString();
            txtEquipmentName.Text = row.Cells["Tên Thiết Bị / Máy Tập"].Value?.ToString() ?? "";
            string status = row.Cells["Trạng Thái"].Value?.ToString() ?? "Đang hoạt động";
            cboStatus.SelectedItem = status;
            if (int.TryParse(row.Cells["Số Lượng"].Value?.ToString(), out int qty))
            {
                numQuantity.Value = Math.Max(numQuantity.Minimum, Math.Min(numQuantity.Maximum, qty));
            }

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = true;

            // Nạp lịch sử sửa chữa của thiết bị này
            LoadMaintenanceHistory(selectedEquipmentId.Value);
        }

        private void LoadMaintenanceHistory(int equipmentId)
        {
            try
            {
                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["GymManagementDBEntities1"]?.ConnectionString;
                if (connStr != null && connStr.Contains("connection string=\""))
                {
                    int start = connStr.IndexOf("connection string=\"") + 19;
                    int end = connStr.IndexOf("\"", start);
                    connStr = connStr.Substring(start, end - start);
                }
                else
                {
                    connStr = "Server=.\\SQLEXPRESS;Database=GymManagementDB;Integrated Security=True;";
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"
                        SELECT MaintenanceId AS [Mã Phiếu], 
                               MaintenanceDate AS [Ngày Sửa], 
                               Description AS [Mô Tả Hỏng Hóc / Sửa Chữa], 
                               Cost AS [Chi Phí (VNĐ)], 
                               Status AS [Trạng Thái], 
                               PerformedBy AS [Đơn Vị Thực Hiện]
                        FROM EquipmentMaintenance
                        WHERE EquipmentId = @EquipmentId
                        ORDER BY MaintenanceDate DESC";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@EquipmentId", equipmentId);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvMaintenance.DataSource = dt;

                        if (dgvMaintenance.Columns["Ngày Sửa"] != null)
                        {
                            dgvMaintenance.Columns["Ngày Sửa"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }
                        if (dgvMaintenance.Columns["Chi Phí (VNĐ)"] != null)
                        {
                            dgvMaintenance.Columns["Chi Phí (VNĐ)"].DefaultCellStyle.Format = "N0";
                            dgvMaintenance.Columns["Chi Phí (VNĐ)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }

                        decimal totalCost = 0;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (r["Chi Phí (VNĐ)"] != DBNull.Value)
                                totalCost += Convert.ToDecimal(r["Chi Phí (VNĐ)"]);
                        }

                        lblTotalCost.Text = $"💰 Tổng chi phí sửa chữa thiết bị này: {totalCost:N0} VNĐ";
                    }
                }
            }
            catch (Exception ex)
            {
                lblTotalCost.Text = "Tổng chi phí sửa chữa: 0 VNĐ";
                System.Diagnostics.Debug.WriteLine($"Lỗi lịch sử sửa chữa: {ex.Message}");
            }
        }

        private void btnAddRepair_Click(object sender, EventArgs e)
        {
            if (!selectedEquipmentId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn một thiết bị trên bảng trước khi ghi nhận sửa chữa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string desc = txtRepairDesc.Text.Trim();
            if (string.IsNullOrWhiteSpace(desc))
            {
                MessageBox.Show("Vui lòng nhập mô tả lỗi hỏng hóc hoặc công việc bảo trì!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRepairDesc.Focus();
                return;
            }

            decimal cost = numRepairCost.Value;
            string performer = txtPerformer.Text.Trim();

            try
            {
                string connStr = "Server=.\\SQLEXPRESS;Database=GymManagementDB;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string insertSql = @"
                        INSERT INTO EquipmentMaintenance (EquipmentId, MaintenanceDate, Description, Cost, Status, PerformedBy)
                        VALUES (@EquipmentId, GETDATE(), @Desc, @Cost, N'Đang bảo trì', @Performer)";

                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EquipmentId", selectedEquipmentId.Value);
                        cmd.Parameters.AddWithValue("@Desc", desc);
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@Performer", string.IsNullOrWhiteSpace(performer) ? "Kỹ thuật viên phòng gym" : performer);
                        cmd.ExecuteNonQuery();
                    }

                    // Tự động cập nhật trạng thái thiết bị sang "Đang bảo trì" nếu chưa chuyển
                    using (var db = new GymManagementDBEntities1())
                    {
                        var eq = db.Equipments.Find(selectedEquipmentId.Value);
                        if (eq != null && eq.Status != "Đang bảo trì")
                        {
                            eq.Status = "Đang bảo trì";
                            db.SaveChanges();
                        }
                    }
                }

                MessageBox.Show("Đã lưu phiếu sửa chữa và cập nhật trạng thái thiết bị sang 'Đang bảo trì' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRepairDesc.Clear();
                numRepairCost.Value = 0;
                txtPerformer.Clear();

                LoadEquipments(txtSearch.Text, cboFilterStatus.SelectedItem?.ToString() ?? "Tất cả");
                LoadMaintenanceHistory(selectedEquipmentId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu phiếu sửa chữa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 3. THAO TÁC CRUD THIẾT BỊ

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtEquipmentName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập tên thiết bị / máy tập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEquipmentName.Focus();
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var eq = new Equipments
                    {
                        EquipmentName = name,
                        Status = cboStatus.SelectedItem?.ToString() ?? "Đang hoạt động",
                        Quantity = (int)numQuantity.Value
                    };
                    db.Equipments.Add(eq);
                    db.SaveChanges();
                }

                MessageBox.Show("Thêm thiết bị mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadEquipments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm thiết bị: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!selectedEquipmentId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn thiết bị cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEdit.Enabled = false;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            txtEquipmentName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!selectedEquipmentId.HasValue) return;

            string name = txtEquipmentName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên thiết bị không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var eq = db.Equipments.Find(selectedEquipmentId.Value);
                    if (eq != null)
                    {
                        eq.EquipmentName = name;
                        eq.Status = cboStatus.SelectedItem?.ToString() ?? "Đang hoạt động";
                        eq.Quantity = (int)numQuantity.Value;
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Cập nhật thông tin thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadEquipments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật thiết bị: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!selectedEquipmentId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn thiết bị cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa thiết bị '{txtEquipmentName.Text}' không?",
                                          "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GymManagementDBEntities1())
                    {
                        var eq = db.Equipments.Find(selectedEquipmentId.Value);
                        if (eq != null)
                        {
                            db.Equipments.Remove(eq);
                            db.SaveChanges();
                        }
                    }

                    MessageBox.Show("Xóa thiết bị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    LoadEquipments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa thiết bị: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            selectedEquipmentId = null;
            txtEquipmentId.Clear();
            txtEquipmentName.Clear();
            cboStatus.SelectedIndex = 0;
            numQuantity.Value = 1;

            btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dgvMaintenance.DataSource = null;
            lblTotalCost.Text = "Tổng chi phí sửa chữa: 0 VNĐ";
            txtRepairDesc.Clear();
            numRepairCost.Value = 0;
            txtPerformer.Clear();
        }

        #endregion

        #region 4. TÌM KIẾM TỨC THÌ & XUẤT EXCEL EPPLUS

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadEquipments(txtSearch.Text, cboFilterStatus.SelectedItem?.ToString() ?? "Tất cả");
        }

        private void cboFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEquipments(txtSearch.Text, cboFilterStatus.SelectedItem?.ToString() ?? "Tất cả");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEquipments.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu thiết bị để xuất file Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = $"DanhSachThietBi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mã TB", typeof(int));
                    dt.Columns.Add("Tên Thiết Bị / Máy Tập", typeof(string));
                    dt.Columns.Add("Trạng Thái Hoạt Động", typeof(string));
                    dt.Columns.Add("Số Lượng", typeof(int));

                    foreach (DataGridViewRow row in dgvEquipments.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(
                            Convert.ToInt32(row.Cells["Mã TB"].Value ?? 0),
                            row.Cells["Tên Thiết Bị / Máy Tập"].Value?.ToString() ?? "",
                            row.Cells["Trạng Thái"].Value?.ToString() ?? "",
                            Convert.ToInt32(row.Cells["Số Lượng"].Value ?? 0)
                        );
                    }

                    ExcelExportHelper.ExportToExcel(dt, sfd.FileName, "ThietBi", "DANH SÁCH & TRẠNG THÁI TRANG THIẾT BỊ PHÒNG GYM");

                    var result = MessageBox.Show("Xuất file Excel thành công bằng EPPlus!\nBạn có muốn mở file ngay không?",
                                                 "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
