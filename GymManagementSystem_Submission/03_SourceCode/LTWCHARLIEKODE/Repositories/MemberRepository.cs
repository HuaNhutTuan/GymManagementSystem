using System;
using System.Collections.Generic;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class MemberRepository
    {
        /// <summary>
        /// Tìm kiếm và lọc hội viên sử dụng LINQ Where, OrderByDescending
        /// </summary>
        public List<Members> GetMembers(string keyword = null)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = db.Members.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.Trim().ToLower();
                    query = query.Where(m => m.FullName.ToLower().Contains(kw) ||
                                             (m.Phone != null && m.Phone.Contains(kw)) ||
                                             (m.Email != null && m.Email.ToLower().Contains(kw)));
                }

                return query.OrderByDescending(m => m.MemberId).ToList();
            }
        }

        /// <summary>
        /// Lấy hội viên theo mã QR bằng LINQ FirstOrDefault & Where
        /// </summary>
        public Members GetMemberByQRCode(string qrCode)
        {
            if (string.IsNullOrWhiteSpace(qrCode)) return null;

            using (var db = new GymManagementDBEntities1())
            {
                return db.Members.FirstOrDefault(m => m.QRCode == qrCode.Trim());
            }
        }

        /// <summary>
        /// Thêm mới hội viên qua Entity Framework
        /// </summary>
        public Members AddMember(string fullName, string phone, string email, string qrCode = null)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var m = new Members
                {
                    FullName = fullName,
                    Phone = phone,
                    Email = email,
                    QRCode = qrCode,
                    CreatedDate = DateTime.Now
                };
                db.Members.Add(m);
                db.SaveChanges();
                return m;
            }
        }

        /// <summary>
        /// Cập nhật thông tin hội viên qua Entity Framework
        /// </summary>
        public bool UpdateMember(int memberId, string fullName, string phone, string email)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var m = db.Members.Find(memberId);
                if (m == null) return false;

                m.FullName = fullName;
                m.Phone = phone;
                m.Email = email;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Xóa hội viên nếu chưa phát sinh dữ liệu liên quan
        /// </summary>
        public bool DeleteMember(int memberId, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (var db = new GymManagementDBEntities1())
            {
                var m = db.Members.Find(memberId);
                if (m == null)
                {
                    errorMessage = "Không tìm thấy hội viên cần xóa!";
                    return false;
                }

                if (db.Invoices.Any(i => i.MemberId == memberId) ||
                    db.MemberPackages.Any(mp => mp.MemberId == memberId) ||
                    db.PTSchedules.Any(s => s.MemberId == memberId))
                {
                    errorMessage = "Hội viên này đã có dữ liệu hóa đơn, gói tập hoặc lịch PT, không thể xóa!";
                    return false;
                }

                db.Members.Remove(m);
                db.SaveChanges();
                return true;
            }
        }
    }
}
