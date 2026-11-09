using System;
using System.Collections.Generic;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class PackageRepository
    {
        /// <summary>
        /// Lấy danh sách gói tập bằng LINQ Where & OrderBy
        /// </summary>
        public List<Packages> GetAllPackages(string keyword = null, bool onlyActive = false)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = db.Packages.AsQueryable();

                if (onlyActive)
                {
                    query = query.Where(p => p.IsActive == true);
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    string kw = keyword.Trim().ToLower();
                    query = query.Where(p => p.PackageName.ToLower().Contains(kw));
                }

                return query.OrderBy(p => p.Price).ToList();
            }
        }

        /// <summary>
        /// Thêm mới gói tập
        /// </summary>
        public void AddPackage(string name, decimal price, int durationDays, bool isActive)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var pkg = new Packages
                {
                    PackageName = name,
                    Price = price,
                    DurationDays = durationDays,
                    IsActive = isActive
                };
                db.Packages.Add(pkg);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Cập nhật gói tập
        /// </summary>
        public bool UpdatePackage(int packageId, string name, decimal price, int durationDays, bool isActive)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var pkg = db.Packages.Find(packageId);
                if (pkg == null) return false;

                pkg.PackageName = name;
                pkg.Price = price;
                pkg.DurationDays = durationDays;
                pkg.IsActive = isActive;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Xóa hoặc vô hiệu hóa gói tập
        /// </summary>
        public bool DeletePackage(int packageId, out string message)
        {
            message = string.Empty;
            using (var db = new GymManagementDBEntities1())
            {
                var pkg = db.Packages.Find(packageId);
                if (pkg == null)
                {
                    message = "Không tìm thấy gói tập!";
                    return false;
                }

                if (db.MemberPackages.Any(mp => mp.PackageId == packageId) ||
                    db.InvoiceDetails.Any(id => id.PackageId == packageId))
                {
                    pkg.IsActive = false;
                    db.SaveChanges();
                    message = "Gói tập đã có hội viên đăng ký nên đã chuyển trạng thái thành Ngưng hoạt động!";
                    return true;
                }

                db.Packages.Remove(pkg);
                db.SaveChanges();
                message = "Đã xóa gói tập thành công!";
                return true;
            }
        }
    }
}
