using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_RevenueReport : Form
    {
        private decimal _totalRevenue = 0;
        private int _totalInvoices = 0;
        private int _totalPackages = 0;
        private decimal _avgInvoice = 0;

        private DataTable _dtInvoices = new DataTable();
        private DataTable _dtPackages = new DataTable();
        private DataTable _dtMonthly = new DataTable();

        public Form_RevenueReport()
        {
            InitializeComponent();
        }

        private void Form_RevenueReport_Load(object sender, EventArgs e)
        {
            cboPeriod.SelectedIndex = 0; // "Tháng này"
            LoadReport();
        }

        #region 1. XỬ LÝ KỲ THỜI GIAN

        private void cboPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            switch (cboPeriod.SelectedIndex)
            {
                case 0: // Tháng này
                    dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
                    dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    break;

                case 1: // Tháng trước
                    DateTime lastMonth = now.AddMonths(-1);
                    dtpFromDate.Value = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                    dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    break;

                case 2: // Năm nay
                    dtpFromDate.Value = new DateTime(now.Year, 1, 1);
                    dtpToDate.Value = new DateTime(now.Year, 12, 31);
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    break;

                case 3: // Toàn thời gian
                    dtpFromDate.Value = new DateTime(2020, 1, 1);
                    dtpToDate.Value = now.Date.AddDays(1);
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    break;

                case 4: // Tùy chọn...
                default:
                    dtpFromDate.Enabled = true;
                    dtpToDate.Enabled = true;
                    break;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        #endregion

        #region 2. TÍNH TOÁN DỮ LIỆU & NẠP VÀO BÁO CÁO

        private void LoadReport()
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1);

            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    // 1. Dữ liệu Hóa đơn trong kỳ
                    var invoiceQuery = from inv in db.Invoices
                                       where inv.CreatedDate >= fromDate && inv.CreatedDate < toDate
                                       join m in db.Members on inv.MemberId equals m.MemberId
                                       join u in db.Users on inv.CreatedByUserId equals u.UserId into uJoin
                                       from usr in uJoin.DefaultIfEmpty()
                                       select new
                                       {
                                           inv.InvoiceId,
                                           inv.CreatedDate,
                                           MemberName = m.FullName,
                                           MemberPhone = m.Phone,
                                           inv.TotalAmount,
                                           CashierName = usr != null ? usr.FullName : "Admin"
                                       };

                    var invoiceList = invoiceQuery.OrderByDescending(x => x.CreatedDate).ToList();

                    // 2. Dữ liệu Chi tiết Hóa đơn trong kỳ
                    var detailQuery = from d in db.InvoiceDetails
                                      join inv in db.Invoices on d.InvoiceId equals inv.InvoiceId
                                      where inv.CreatedDate >= fromDate && inv.CreatedDate < toDate
                                      join p in db.Packages on d.PackageId equals p.PackageId
                                      select new
                                      {
                                          d.InvoiceId,
                                          p.PackageId,
                                          p.PackageName,
                                          d.Price,
                                          Quantity = d.Quantity ?? 1,
                                          LineTotal = d.Price * (d.Quantity ?? 1)
                                      };

                    var detailList = detailQuery.ToList();

                    // 3. Tính toán các thẻ chỉ số KPI
                    _totalInvoices = invoiceList.Count;
                    _totalRevenue = invoiceList.Sum(x => x.TotalAmount);
                    _totalPackages = detailList.Sum(x => x.Quantity);
                    _avgInvoice = _totalInvoices > 0 ? (_totalRevenue / _totalInvoices) : 0;

                    // Hiển thị KPI Cards
                    lblTotalRevenue.Text = $"{_totalRevenue:N0} đ";
                    lblTotalInvoices.Text = $"{_totalInvoices:N0}";
                    lblTotalPackages.Text = $"{_totalPackages:N0}";
                    lblAverageInvoice.Text = $"{_avgInvoice:N0} đ";

                    // 4. Tab 1: Chi tiết từng hóa đơn
                    _dtInvoices = new DataTable();
                    _dtInvoices.Columns.Add("Mã HĐ", typeof(string));
                    _dtInvoices.Columns.Add("Ngày Lập", typeof(DateTime));
                    _dtInvoices.Columns.Add("Hội Viên", typeof(string));
                    _dtInvoices.Columns.Add("Số ĐT", typeof(string));
                    _dtInvoices.Columns.Add("Doanh Thu (đ)", typeof(decimal));
                    _dtInvoices.Columns.Add("Thu Ngân Lập", typeof(string));

                    foreach (var inv in invoiceList)
                    {
                        _dtInvoices.Rows.Add(
                            $"HD-{inv.InvoiceId:D5}",
                            inv.CreatedDate,
                            inv.MemberName,
                            inv.MemberPhone ?? "",
                            inv.TotalAmount,
                            inv.CashierName
                        );
                    }

                    dgvInvoices.DataSource = _dtInvoices;
                    if (dgvInvoices.Columns["Ngày Lập"] != null)
                        dgvInvoices.Columns["Ngày Lập"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    if (dgvInvoices.Columns["Doanh Thu (đ)"] != null)
                        dgvInvoices.Columns["Doanh Thu (đ)"].DefaultCellStyle.Format = "N0";

                    // 5. Tab 2: Thống kê theo Gói tập
                    var packageGrouping = detailList
                        .GroupBy(x => x.PackageName)
                        .Select(g => new
                        {
                            PackageName = g.Key,
                            QuantitySold = g.Sum(x => x.Quantity),
                            Revenue = g.Sum(x => x.LineTotal),
                            Percentage = _totalRevenue > 0 ? (g.Sum(x => x.LineTotal) / _totalRevenue * 100) : 0
                        })
                        .OrderByDescending(x => x.Revenue)
                        .ToList();

                    _dtPackages = new DataTable();
                    _dtPackages.Columns.Add("Tên Gói Tập", typeof(string));
                    _dtPackages.Columns.Add("Số Lượng Bán", typeof(int));
                    _dtPackages.Columns.Add("Doanh Thu (đ)", typeof(decimal));
                    _dtPackages.Columns.Add("Tỷ Trọng (%)", typeof(string));

                    foreach (var p in packageGrouping)
                    {
                        _dtPackages.Rows.Add(p.PackageName, p.QuantitySold, p.Revenue, $"{p.Percentage:F1}%");
                    }

                    dgvPackages.DataSource = _dtPackages;
                    if (dgvPackages.Columns["Doanh Thu (đ)"] != null)
                        dgvPackages.Columns["Doanh Thu (đ)"].DefaultCellStyle.Format = "N0";

                    // 6. Tab 3: Thống kê theo Tháng
                    var monthlyGrouping = invoiceList
                        .GroupBy(x => new { Year = (x.CreatedDate ?? DateTime.Now).Year, Month = (x.CreatedDate ?? DateTime.Now).Month })
                        .Select(g => new
                        {
                            MonthYear = $"Tháng {g.Key.Month:D2}/{g.Key.Year}",
                            InvoiceCount = g.Count(),
                            TotalAmount = g.Sum(x => x.TotalAmount)
                        })
                        .OrderByDescending(x => x.MonthYear)
                        .ToList();

                    _dtMonthly = new DataTable();
                    _dtMonthly.Columns.Add("Kỳ Tháng / Năm", typeof(string));
                    _dtMonthly.Columns.Add("Số Hóa Đơn", typeof(int));
                    _dtMonthly.Columns.Add("Doanh Thu (đ)", typeof(decimal));

                    foreach (var m in monthlyGrouping)
                    {
                        _dtMonthly.Rows.Add(m.MonthYear, m.InvoiceCount, m.TotalAmount);
                    }

                    dgvMonthly.DataSource = _dtMonthly;
                    if (dgvMonthly.Columns["Doanh Thu (đ)"] != null)
                        dgvMonthly.Columns["Doanh Thu (đ)"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo báo cáo doanh thu: {ex.Message}", "Lỗi Báo Cáo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 3. IN BÁO CÁO TÀI CHÍNH

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DocumentName = $"BaoCaoDoanhThu_{dtpFromDate.Value:yyyyMMdd}_{dtpToDate.Value:yyyyMMdd}";
                pd.PrintPage += new PrintPageEventHandler(PrintReportPage);

                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.WindowState = FormWindowState.Maximized;
                ppd.Text = "Xem trước Báo Cáo Doanh Thu";
                ppd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xem in báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReportPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontTitle = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fontHeader = new Font("Segoe UI", 12, FontStyle.Bold);
            Font fontBold = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fontNormal = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontItalic = new Font("Segoe UI", 9, FontStyle.Italic);

            Brush brush = Brushes.Black;
            Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1);
            Pen penThick = new Pen(Color.FromArgb(24, 43, 73), 2);

            int x = 50;
            int y = 40;
            int width = e.PageBounds.Width - 100;

            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };

            // 1. Header Trung tâm
            g.DrawString("TRUNG TÂM THỂ HÌNH GYM & FITNESS", fontBold, new SolidBrush(Color.FromArgb(24, 43, 73)), x, y);
            y += 20;
            g.DrawString("Bộ phận: Quản lý Tài chính & Doanh thu", fontItalic, brush, x, y);
            y += 25;
            g.DrawLine(penThick, x, y, x + width, y);
            y += 20;

            // 2. Tiêu đề Báo Cáo
            g.DrawString("BÁO CÁO TỔNG HỢP DOANH THU & KẾT QUẢ KINH DOANH", fontTitle, brush, new RectangleF(x, y, width, 30), centerFormat);
            y += 30;
            g.DrawString($"Giai đoạn: Từ ngày {dtpFromDate.Value:dd/MM/yyyy} đến ngày {dtpToDate.Value:dd/MM/yyyy}", fontBold, brush, new RectangleF(x, y, width, 20), centerFormat);
            y += 20;
            g.DrawString($"(Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm} - Người xuất: {UserSession.FullName ?? "Quản trị viên"})", fontItalic, brush, new RectangleF(x, y, width, 20), centerFormat);
            y += 35;

            // 3. Tóm tắt các chỉ số chính (KPI Table)
            g.DrawString("I. TỔNG HỢP CÁC CHỈ SỐ KINH DOANH CHÍNH", fontHeader, new SolidBrush(Color.FromArgb(24, 43, 73)), x, y);
            y += 25;

            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 242, 245)), x, y, width, 28);
            g.DrawRectangle(pen, x, y, width, 28);
            g.DrawString("Chỉ Số Đánh Giá", fontBold, brush, x + 15, y + 5);
            g.DrawString("Kết Quả Đạt Được", fontBold, brush, x + 400, y + 5);
            y += 28;

            string[,] kpis = {
                { "1. Tổng doanh thu thực thu", $"{_totalRevenue:N0} VNĐ" },
                { "2. Tổng số hóa đơn đã phát hành", $"{_totalInvoices:N0} hóa đơn" },
                { "3. Tổng số lượt gói tập bán ra", $"{_totalPackages:N0} gói" },
                { "4. Giá trị trung bình trên mỗi hóa đơn", $"{_avgInvoice:N0} VNĐ / hóa đơn" }
            };

            for (int i = 0; i < 4; i++)
            {
                g.DrawRectangle(pen, x, y, width, 25);
                g.DrawString(kpis[i, 0], fontNormal, brush, x + 15, y + 4);
                g.DrawString(kpis[i, 1], fontBold, brush, x + 400, y + 4);
                y += 25;
            }

            y += 25;

            // 4. Bảng doanh số theo gói tập
            g.DrawString("II. CƠ CẤU DOANH SỐ THEO GÓI TẬP", fontHeader, new SolidBrush(Color.FromArgb(24, 43, 73)), x, y);
            y += 25;

            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 242, 245)), x, y, width, 28);
            g.DrawRectangle(pen, x, y, width, 28);
            g.DrawString("Tên Gói Tập Dịch Vụ", fontBold, brush, x + 15, y + 5);
            g.DrawString("Số Lượng Bán", fontBold, brush, x + 350, y + 5);
            g.DrawString("Doanh Thu (VNĐ)", fontBold, brush, x + 470, y + 5);
            g.DrawString("Tỷ Trọng (%)", fontBold, brush, x + 630, y + 5);
            y += 28;

            foreach (DataRow row in _dtPackages.Rows)
            {
                g.DrawRectangle(pen, x, y, width, 25);
                g.DrawString(row["Tên Gói Tập"].ToString(), fontNormal, brush, x + 15, y + 4);
                g.DrawString(row["Số Lượng Bán"].ToString(), fontNormal, brush, x + 370, y + 4);
                g.DrawString($"{Convert.ToDecimal(row["Doanh Thu (đ)"]):N0}", fontBold, brush, x + 470, y + 4);
                g.DrawString(row["Tỷ Trọng (%)"].ToString(), fontNormal, brush, x + 640, y + 4);
                y += 25;
            }

            y += 40;

            // 5. Chữ ký xác nhận
            g.DrawString("Người Lập Báo Cáo", fontBold, brush, x + 60, y);
            g.DrawString("Kế Toán Trưởng", fontBold, brush, x + 330, y);
            g.DrawString("Giám Đốc Trung Tâm", fontBold, brush, x + 550, y);
            y += 20;
            g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brush, x + 55, y);
            g.DrawString("(Ký, ghi rõ họ tên)", fontItalic, brush, x + 325, y);
            g.DrawString("(Ký, đóng dấu)", fontItalic, brush, x + 565, y);
            y += 70;

            g.DrawString(UserSession.FullName ?? "Người lập", fontBold, brush, x + 60, y);
        }

        #endregion

        #region 4. XUẤT FILE CSV / EXCEL

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (_dtInvoices == null || _dtInvoices.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV File (*.csv)|*.csv";
            sfd.FileName = $"BaoCaoDoanhThu_{dtpFromDate.Value:yyyyMMdd}_{dtpToDate.Value:yyyyMMdd}.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // Header UTF-8 BOM
                    sb.AppendLine("BÁO CÁO DOANH THU HỆ THỐNG PHÒNG GYM");
                    sb.AppendLine($"Giai đoạn: Từ {dtpFromDate.Value:dd/MM/yyyy} đến {dtpToDate.Value:dd/MM/yyyy}");
                    sb.AppendLine($"Tổng doanh thu:;{_totalRevenue:N0} VNĐ");
                    sb.AppendLine($"Tổng số hóa đơn:;{_totalInvoices}");
                    sb.AppendLine($"Số gói tập bán ra:;{_totalPackages}");
                    sb.AppendLine();

                    // Tiêu đề cột
                    string[] colNames = _dtInvoices.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                    sb.AppendLine(string.Join(";", colNames));

                    // Dòng dữ liệu
                    foreach (DataRow row in _dtInvoices.Rows)
                    {
                        var fields = row.ItemArray.Select(field =>
                        {
                            if (field is DateTime dt) return dt.ToString("dd/MM/yyyy HH:mm");
                            if (field is decimal dec) return dec.ToString("N0");
                            return field?.ToString().Replace(";", ",") ?? "";
                        });
                        sb.AppendLine(string.Join(";", fields));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất báo cáo sang file CSV thành công!\nBạn có thể mở trực tiếp bằng Microsoft Excel.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
