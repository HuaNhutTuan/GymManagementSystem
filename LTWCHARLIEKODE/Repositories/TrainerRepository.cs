using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class TrainerRepository
    {
        /// <summary>
        /// Đếm số ca dạy đã hoàn thành của HLV bằng LINQ Count & Where
        /// </summary>
        public int GetCompletedSessionsCount(int trainerId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                return db.PTSchedules.Count(s => s.TrainerId == trainerId && s.Status == "Đã hoàn thành");
            }
        }

        /// <summary>
        /// Đếm số hội viên đang kèm trực tiếp bằng LINQ Count & Where
        /// </summary>
        public int GetActiveMembersCount(int trainerId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                return db.MemberPackages.Count(mp => mp.TrainerId == trainerId && mp.IsActive == true);
            }
        }

        /// <summary>
        /// Lấy chi tiết lịch tập của Huấn luyện viên bằng LINQ Join & Where & OrderByDescending
        /// </summary>
        public DataTable GetTrainerScheduleDetails(int trainerId, decimal commissionRate, out decimal totalCommission, out int completedSessions)
        {
            totalCommission = 0;
            completedSessions = 0;

            using (var db = new GymManagementDBEntities1())
            {
                var list = (from s in db.PTSchedules
                            join m in db.Members on s.MemberId equals m.MemberId
                            where s.TrainerId == trainerId
                            orderby s.ScheduleDate descending, s.StartTime descending
                            select new
                            {
                                s.ScheduleId,
                                s.ScheduleDate,
                                s.StartTime,
                                s.EndTime,
                                MemberName = m.FullName,
                                MemberPhone = m.Phone,
                                s.Status,
                                s.Notes
                            }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã Lịch", typeof(int));
                dt.Columns.Add("Ngày Tập", typeof(DateTime));
                dt.Columns.Add("Khung Giờ", typeof(string));
                dt.Columns.Add("Hội Viên", typeof(string));
                dt.Columns.Add("SĐT Hội Viên", typeof(string));
                dt.Columns.Add("Trạng Thái", typeof(string));
                dt.Columns.Add("Ghi Chú", typeof(string));
                dt.Columns.Add("Hoa Hồng Ca (VNĐ)", typeof(decimal));

                foreach (var item in list)
                {
                    decimal comm = 0;
                    if (item.Status == "Đã hoàn thành")
                    {
                        comm = commissionRate;
                        totalCommission += comm;
                        completedSessions++;
                    }

                    string timeRange = item.StartTime.ToString(@"hh\:mm") + " - " + item.EndTime.ToString(@"hh\:mm");
                    dt.Rows.Add(
                        item.ScheduleId,
                        item.ScheduleDate,
                        timeRange,
                        item.MemberName,
                        item.MemberPhone ?? "",
                        item.Status,
                        item.Notes ?? "",
                        comm
                    );
                }

                return dt;
            }
        }
    }
}
