using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class EquipmentRepository
    {
        /// <summary>
        /// Lấy lịch sử sửa chữa thiết bị bằng LINQ Where & OrderByDescending
        /// </summary>
        public DataTable GetMaintenanceHistory(int equipmentId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var list = db.EquipmentMaintenance
                             .Where(m => m.EquipmentId == equipmentId)
                             .OrderByDescending(m => m.MaintenanceDate)
                             .ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã Phiếu", typeof(int));
                dt.Columns.Add("Ngày Sửa", typeof(DateTime));
                dt.Columns.Add("Mô Tả Hỏng Hóc / Sửa Chữa", typeof(string));
                dt.Columns.Add("Chi Phí (VNĐ)", typeof(decimal));
                dt.Columns.Add("Trạng Thái", typeof(string));
                dt.Columns.Add("Đơn Vị Thực Hiện", typeof(string));

                foreach (var item in list)
                {
                    dt.Rows.Add(
                        item.MaintenanceId,
                        item.MaintenanceDate,
                        item.Description ?? "",
                        item.Cost,
                        item.Status ?? "",
                        item.PerformedBy ?? ""
                    );
                }

                return dt;
            }
        }

        /// <summary>
        /// Tính tổng chi phí sửa chữa bằng hàm LINQ Sum kết hợp Where
        /// </summary>
        public decimal GetTotalMaintenanceCost(int equipmentId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = db.EquipmentMaintenance.Where(m => m.EquipmentId == equipmentId);
                if (!query.Any())
                {
                    return 0;
                }
                return query.Sum(m => (decimal?)m.Cost) ?? 0;
            }
        }

        /// <summary>
        /// Ghi nhận phiếu sửa chữa thiết bị qua Entity Framework
        /// </summary>
        public void AddMaintenanceRecord(int equipmentId, string description, decimal cost, string performer)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var record = new EquipmentMaintenance
                {
                    EquipmentId = equipmentId,
                    MaintenanceDate = DateTime.Now,
                    Description = description,
                    Cost = cost,
                    Status = "Đang bảo trì",
                    PerformedBy = string.IsNullOrWhiteSpace(performer) ? "Kỹ thuật viên phòng gym" : performer
                };
                db.EquipmentMaintenance.Add(record);

                // Cập nhật trạng thái thiết bị sang "Đang bảo trì"
                var eq = db.Equipments.Find(equipmentId);
                if (eq != null && eq.Status != "Đang bảo trì")
                {
                    eq.Status = "Đang bảo trì";
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Cập nhật trạng thái thiết bị
        /// </summary>
        public void UpdateEquipmentStatus(int equipmentId, string status)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var eq = db.Equipments.Find(equipmentId);
                if (eq != null)
                {
                    eq.Status = status;
                    db.SaveChanges();
                }
            }
        }
    }
}
