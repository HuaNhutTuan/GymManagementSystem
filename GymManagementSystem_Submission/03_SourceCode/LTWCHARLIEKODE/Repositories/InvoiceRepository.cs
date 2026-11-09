using System;
using System.Collections.Generic;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class InvoiceDisplayItem
    {
        public int InvoiceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string CashierName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceDetailDisplayItem
    {
        public int DetailId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int? DurationDays { get; set; }
        public decimal Price { get; set; }
        public int? Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class InvoiceRepository
    {
        /// <summary>
        /// Lấy danh sách hóa đơn kết hợp LINQ Join & Where & OrderByDescending
        /// </summary>
        public List<InvoiceDisplayItem> GetInvoices(DateTime? fromDate = null, DateTime? toDate = null, string keyword = null)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = from inv in db.Invoices
                            join m in db.Members on inv.MemberId equals m.MemberId
                            join u in db.Users on inv.CreatedByUserId equals u.UserId into userJoin
                            from usr in userJoin.DefaultIfEmpty()
                            select new
                            {
                                inv.InvoiceId,
                                inv.CreatedDate,
                                inv.MemberId,
                                MemberName = m.FullName,
                                CashierName = usr != null ? usr.FullName : "Hệ thống",
                                inv.TotalAmount
                            };

                if (fromDate.HasValue)
                {
                    DateTime f = fromDate.Value.Date;
                    query = query.Where(x => x.CreatedDate >= f);
                }

                if (toDate.HasValue)
                {
                    DateTime t = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(x => x.CreatedDate <= t);
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.Trim().ToLower();
                    query = query.Where(x => x.MemberName.ToLower().Contains(kw) ||
                                             x.InvoiceId.ToString().Contains(kw));
                }

                return query.OrderByDescending(x => x.CreatedDate)
                            .Select(x => new InvoiceDisplayItem
                            {
                                InvoiceId = x.InvoiceId,
                                CreatedDate = x.CreatedDate,
                                MemberId = x.MemberId,
                                MemberName = x.MemberName,
                                CashierName = x.CashierName,
                                TotalAmount = x.TotalAmount
                            }).ToList();
            }
        }

        /// <summary>
        /// Lấy chi tiết hóa đơn bằng LINQ Join & Where
        /// </summary>
        public List<InvoiceDetailDisplayItem> GetInvoiceDetails(int invoiceId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = from d in db.InvoiceDetails
                            join p in db.Packages on d.PackageId equals p.PackageId
                            where d.InvoiceId == invoiceId
                            select new InvoiceDetailDisplayItem
                            {
                                DetailId = d.DetailId,
                                PackageId = d.PackageId,
                                PackageName = p.PackageName,
                                DurationDays = p.DurationDays,
                                Price = d.Price,
                                Quantity = d.Quantity ?? 1,
                                SubTotal = d.Price * (d.Quantity ?? 1)
                            };

                return query.ToList();
            }
        }

        /// <summary>
        /// Tính tổng doanh thu bằng LINQ Sum kết hợp Where
        /// </summary>
        public decimal GetTotalRevenue(DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = db.Invoices.AsQueryable();

                if (fromDate.HasValue)
                {
                    DateTime f = fromDate.Value.Date;
                    query = query.Where(i => i.CreatedDate >= f);
                }

                if (toDate.HasValue)
                {
                    DateTime t = toDate.Value.Date.AddDays(1).AddTicks(-1);
                    query = query.Where(i => i.CreatedDate <= t);
                }

                return query.Sum(i => (decimal?)i.TotalAmount) ?? 0;
            }
        }
    }
}
