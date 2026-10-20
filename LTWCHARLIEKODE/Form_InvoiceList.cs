using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace LTWCHARLIEKODE
{
    public partial class Form_InvoiceList : Form
    {
        private int? _initialInvoiceId = null;
        private Invoices _currentSelectedInvoice = null;
        private List<InvoiceDetailViewItem> _currentInvoiceDetails = new List<InvoiceDetailViewItem>();

        public Form_InvoiceList()
        {
            InitializeComponent();
        }

        public Form_InvoiceList(int selectInvoiceId) : this()
        {
            _initialInvoiceId = selectInvoiceId;
        }

        private void Form_InvoiceList_Load(object sender, EventArgs e)
        {
            // Mặc định khoảng ngày: 30 ngày gần nhất
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
            dtpToDate.Value = DateTime.Today.AddDays(1);

            LoadInvoices();

            txtKeyword.TextChanged += (senderObj, eventArgs) => LoadInvoices(txtKeyword.Text, dtpFromDate.Value, dtpToDate.Value);

            if (_initialInvoiceId.HasValue)
            {
                SelectInvoiceById(_initialInvoiceId.Value);
            }
        }

        #region 1. NẠP VÀ LỌC DANH SÁCH HÓA ĐƠN

        private void LoadInvoices(string keyword = "", DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    var query = from inv in db.Invoices
                                join m in db.Members on inv.MemberId equals m.MemberId
                                join u in db.Users on inv.CreatedByUserId equals u.UserId into userJoin
                                from usr in userJoin.DefaultIfEmpty()
                                select new
                                {
                                    MaHoaDon = inv.InvoiceId,
                                    HoiVien = m.FullName,
                                    SoDienThoai = m.Phone,
                                    NhanVienLap = usr != null ? usr.FullName : "Admin",
                                    TongTien = inv.TotalAmount,
                                    NgayLap = inv.CreatedDate
                                };

                    if (fromDate.HasValue)
                    {
                        DateTime f = fromDate.Value.Date;
                        query = query.Where(x => x.NgayLap >= f);
                    }

                    if (toDate.HasValue)
                    {
                        DateTime t = toDate.Value.Date.AddDays(1);
                        query = query.Where(x => x.NgayLap < t);
                    }

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        string kw = keyword.Trim().ToLower();
                        query = query.Where(x => x.HoiVien.ToLower().Contains(kw) || (x.SoDienThoai != null && x.SoDienThoai.Contains(kw)));
                    }

                    var list = query.OrderByDescending(x => x.MaHoaDon).ToList();
                    dgvInvoices.DataSource = list;

                    if (dgvInvoices.Columns["MaHoaDon"] != null) dgvInvoices.Columns["MaHoaDon"].HeaderText = "Mã HĐ";
                    if (dgvInvoices.Columns["HoiVien"] != null) dgvInvoices.Columns["HoiVien"].HeaderText = "Hội Viên";
                    if (dgvInvoices.Columns["SoDienThoai"] != null) dgvInvoices.Columns["SoDienThoai"].HeaderText = "Số ĐT";
                    if (dgvInvoices.Columns["NhanVienLap"] != null) dgvInvoices.Columns["NhanVienLap"].HeaderText = "Người Lập";
                    if (dgvInvoices.Columns["TongTien"] != null)
                    {
                        dgvInvoices.Columns["TongTien"].HeaderText = "Tổng Tiền";
                        dgvInvoices.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvInvoices.Columns["NgayLap"] != null)
                    {
                        dgvInvoices.Columns["NgayLap"].HeaderText = "Ngày Lập";
                        dgvInvoices.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectInvoiceById(int invoiceId)
        {
            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.Cells["MaHoaDon"] != null && Convert.ToInt32(row.Cells["MaHoaDon"].Value) == invoiceId)
                {
                    row.Selected = true;
                    dgvInvoices.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadInvoices(txtKeyword.Text, dtpFromDate.Value, dtpToDate.Value);
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtKeyword.Text = string.Empty;
            dtpFromDate.Value = DateTime.Today.AddDays(-365);
            dtpToDate.Value = DateTime.Today.AddDays(1);
            LoadInvoices();
        }

        #endregion

        #region 2. CHI TIẾT HÓA ĐƠN

        private void dgvInvoices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInvoices.CurrentRow == null || dgvInvoices.CurrentRow.Index < 0)
            {
                _currentSelectedInvoice = null;
                _currentInvoiceDetails.Clear();
                dgvInvoiceDetails.DataSource = null;
                lblDetailInvoiceInfo.Text = "Chưa chọn hóa đơn";
                lblDetailTotalAmount.Text = "0 đ";
                return;
            }

            int invoiceId = Convert.ToInt32(dgvInvoices.CurrentRow.Cells["MaHoaDon"].Value);
            LoadInvoiceDetails(invoiceId);
        }

        private void LoadInvoiceDetails(int invoiceId)
        {
            try
            {
                using (var db = new GymManagementDBEntities1())
                {
                    _currentSelectedInvoice = db.Invoices.Include("Members").Include("Users")
                                                .FirstOrDefault(i => i.InvoiceId == invoiceId);

                    if (_currentSelectedInvoice == null) return;

                    var details = (from d in db.InvoiceDetails
                                   join p in db.Packages on d.PackageId equals p.PackageId
                                   where d.InvoiceId == invoiceId
                                   select new InvoiceDetailViewItem
                                   {
                                       DetailId = d.DetailId,
                                       PackageName = p.PackageName,
                                       Price = d.Price,
                                       Quantity = d.Quantity ?? 1,
                                       Total = d.Price * (d.Quantity ?? 1)
                                   }).ToList();

                    _currentInvoiceDetails = details;
                    dgvInvoiceDetails.DataSource = details;

                    if (dgvInvoiceDetails.Columns["DetailId"] != null) dgvInvoiceDetails.Columns["DetailId"].HeaderText = "STT";
                    if (dgvInvoiceDetails.Columns["PackageName"] != null) dgvInvoiceDetails.Columns["PackageName"].HeaderText = "Gói Tập";
                    if (dgvInvoiceDetails.Columns["Price"] != null)
                    {
                        dgvInvoiceDetails.Columns["Price"].HeaderText = "Đơn Giá";
                        dgvInvoiceDetails.Columns["Price"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvInvoiceDetails.Columns["Quantity"] != null) dgvInvoiceDetails.Columns["Quantity"].HeaderText = "Số Lượng";
                    if (dgvInvoiceDetails.Columns["Total"] != null)
                    {
                        dgvInvoiceDetails.Columns["Total"].HeaderText = "Thành Tiền";
                        dgvInvoiceDetails.Columns["Total"].DefaultCellStyle.Format = "N0";
                    }

                    string memberName = _currentSelectedInvoice.Members != null ? _currentSelectedInvoice.Members.FullName : "Khách";
                    lblDetailInvoiceInfo.Text = $"Hóa đơn: HD-{invoiceId:D5} | Hội viên: {memberName} | Ngày: {_currentSelectedInvoice.CreatedDate:dd/MM/yyyy HH:mm}";
                    lblDetailTotalAmount.Text = $"{_currentSelectedInvoice.TotalAmount:N0} đ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 3. IN ẤN HÓA ĐƠN DẠNG PHIẾU THU CHUYÊN NGHIỆP

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (_currentSelectedInvoice == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn cần in từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DocumentName = $"HoaDon_HD{_currentSelectedInvoice.InvoiceId:D5}";
                pd.PrintPage += new PrintPageEventHandler(PrintInvoicePage);

                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ppd.Document = pd;
                ppd.WindowState = FormWindowState.Maximized;
                ppd.Text = $"Xem trước Hóa đơn #{_currentSelectedInvoice.InvoiceId}";
                ppd.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở giao diện in: {ex.Message}", "Lỗi In", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintInvoicePage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font fontHeader = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fontSub = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontBold = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fontNormal = new Font("Segoe UI", 10, FontStyle.Regular);
            Font fontSmallItalic = new Font("Segoe UI", 9, FontStyle.Italic);

            Brush brush = Brushes.Black;
            Pen pen = new Pen(Color.FromArgb(80, 80, 80), 1);
            Pen penThick = new Pen(Color.FromArgb(24, 43, 73), 2);

            int x = 50;
            int y = 50;
            int width = e.PageBounds.Width - 100;

            // 1. Header Trung tâm
            g.DrawString("TRUNG TÂM THỂ HÌNH GYM & FITNESS", fontHeader, new SolidBrush(Color.FromArgb(24, 43, 73)), x, y);
            y += 30;
            g.DrawString("Địa chỉ: 123 Đường Thể Thao, Quận 1, TP. Hồ Chí Minh", fontSub, brush, x, y);
            y += 20;
            g.DrawString("Hotline: 1900 6868 | Email: contact@gymfitness.com", fontSub, brush, x, y);
            y += 30;
            g.DrawLine(penThick, x, y, x + width, y);
            y += 20;

            // 2. Tiêu đề Phiếu Thu
            StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center };
            g.DrawString("PHIẾU THU TIỀN DỊCH VỤ / HÓA ĐƠN", new Font("Segoe UI", 14, FontStyle.Bold), brush, new RectangleF(x, y, width, 25), centerFormat);
            y += 25;
            g.DrawString($"Mã HĐ: HD-{_currentSelectedInvoice.InvoiceId:D5}", fontBold, brush, new RectangleF(x, y, width, 20), centerFormat);
            y += 35;

            // 3. Thông tin Hội viên & Nhân viên lập
            string memberName = _currentSelectedInvoice.Members != null ? _currentSelectedInvoice.Members.FullName : "Khách vãng lai";
            string phone = _currentSelectedInvoice.Members != null ? _currentSelectedInvoice.Members.Phone : "";
            string email = _currentSelectedInvoice.Members != null ? _currentSelectedInvoice.Members.Email : "";
            string cashierName = _currentSelectedInvoice.Users != null ? _currentSelectedInvoice.Users.FullName : "Thu ngân";

            g.DrawString($"Khách hàng (Hội viên): {memberName}", fontBold, brush, x, y);
            g.DrawString($"Ngày lập: {_currentSelectedInvoice.CreatedDate:dd/MM/yyyy HH:mm}", fontNormal, brush, x + 400, y);
            y += 25;
            g.DrawString($"Số điện thoại: {phone}", fontNormal, brush, x, y);
            g.DrawString($"Thu ngân lập phiếu: {cashierName}", fontNormal, brush, x + 400, y);
            y += 25;
            g.DrawString($"Email: {email}", fontNormal, brush, x, y);
            y += 35;

            // 4. Bảng chi tiết hóa đơn
            int colStt = 50;
            int colName = 350;
            int colQty = 100;
            int colPrice = 120;

            // Tiêu đề bảng
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 242, 245)), x, y, width, 30);
            g.DrawRectangle(pen, x, y, width, 30);

            g.DrawString("STT", fontBold, brush, x + 10, y + 6);
            g.DrawString("Tên Gói Tập Dịch Vụ", fontBold, brush, x + colStt + 10, y + 6);
            g.DrawString("Số Lượng", fontBold, brush, x + colStt + colName, y + 6);
            g.DrawString("Đơn Giá (đ)", fontBold, brush, x + colStt + colName + colQty, y + 6);
            g.DrawString("Thành Tiền (đ)", fontBold, brush, x + colStt + colName + colQty + colPrice, y + 6);
            y += 30;

            // Danh sách dòng
            int stt = 1;
            foreach (var item in _currentInvoiceDetails)
            {
                g.DrawRectangle(pen, x, y, width, 30);
                g.DrawString(stt.ToString(), fontNormal, brush, x + 10, y + 6);
                g.DrawString(item.PackageName, fontNormal, brush, x + colStt + 10, y + 6);
                g.DrawString(item.Quantity.ToString(), fontNormal, brush, x + colStt + colName + 20, y + 6);
                g.DrawString($"{item.Price:N0}", fontNormal, brush, x + colStt + colName + colQty, y + 6);
                g.DrawString($"{item.Total:N0}", fontBold, brush, x + colStt + colName + colQty + colPrice, y + 6);
                y += 30;
                stt++;
            }

            y += 10;
            // 5. Tổng thanh toán
            g.DrawString($"Tổng tiền thanh toán:", fontHeader, brush, x + 350, y);
            g.DrawString($"{_currentSelectedInvoice.TotalAmount:N0} VNĐ", new Font("Segoe UI", 16, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 0, 0)), x + 560, y);
            y += 35;
            g.DrawString($"(Số tiền viết bằng chữ: {NumberToWords((long)_currentSelectedInvoice.TotalAmount)} đồng)", fontSmallItalic, brush, x, y);
            y += 50;

            // 6. Chữ ký xác nhận
            g.DrawString("Người nộp tiền", fontBold, brush, x + 80, y);
            g.DrawString("Thu ngân / Người lập phiếu", fontBold, brush, x + 480, y);
            y += 20;
            g.DrawString("(Ký, ghi rõ họ tên)", fontSmallItalic, brush, x + 70, y);
            g.DrawString("(Ký, ghi rõ họ tên)", fontSmallItalic, brush, x + 500, y);
            y += 80;

            g.DrawString(memberName, fontBold, brush, x + 80, y);
            g.DrawString(cashierName, fontBold, brush, x + 500, y);
            y += 40;

            // Footer lời cảm ơn
            g.DrawString("Cảm ơn Quý khách đã tin tưởng và đồng hành cùng Gym & Fitness! Chúc Quý khách có những buổi tập hiệu quả!", fontSmallItalic, brush, new RectangleF(x, y, width, 30), centerFormat);
        }

        private static string NumberToWords(long number)
        {
            if (number == 0) return "Không";
            if (number < 0) return "Âm " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " tỷ ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " triệu ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " nghìn ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " trăm ";
                number %= 100;
            }

            if (number > 0)
            {
                string[] unitsMap = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười", "mười một", "mười hai", "mười ba", "mười bốn", "mười lăm", "mười sáu", "mười bảy", "mười tám", "mười chín" };
                string[] tensMap = { "không", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            words = words.Trim();
            if (!string.IsNullOrEmpty(words))
            {
                words = char.ToUpper(words[0]) + words.Substring(1);
            }
            return words;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelExportHelper.ExportDataGridViewWithDialog(dgvInvoices, "Danh_Sach_Hoa_Don", "HoaDon", "DANH SÁCH HÓA ĐƠN & DOANH THU PHÒNG GYM");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }

    public class InvoiceDetailViewItem
    {
        public int DetailId { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
