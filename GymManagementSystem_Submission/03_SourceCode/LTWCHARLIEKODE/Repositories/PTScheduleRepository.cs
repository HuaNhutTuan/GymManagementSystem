using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LTWCHARLIEKODE.Repositories
{
    public class BookedSlotInfo
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string MemberName { get; set; }
    }

    public class PTScheduleRepository
    {
        /// <summary>
        /// Nạp danh sách lịch tập hiển thị trên DataGridView bằng LINQ Join & Where
        /// </summary>
        public DataTable GetSchedulesDataTable(int? filterTrainerId = null, DateTime? filterDate = null)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var query = from s in db.PTSchedules
                            join m in db.Members on s.MemberId equals m.MemberId
                            join t in db.Trainers on s.TrainerId equals t.TrainerId
                            select new
                            {
                                s.ScheduleId,
                                MemberName = m.FullName,
                                MemberPhone = m.Phone,
                                TrainerId = s.TrainerId,
                                TrainerName = t.FullName,
                                t.Specialty,
                                s.ScheduleDate,
                                s.StartTime,
                                s.EndTime,
                                s.Status,
                                s.Notes
                            };

                if (filterTrainerId.HasValue && filterTrainerId.Value > 0)
                {
                    query = query.Where(x => x.TrainerId == filterTrainerId.Value);
                }

                if (filterDate.HasValue)
                {
                    DateTime dateOnly = filterDate.Value.Date;
                    query = query.Where(x => x.ScheduleDate == dateOnly);
                }

                var list = query.OrderByDescending(x => x.ScheduleDate)
                                .ThenBy(x => x.StartTime)
                                .ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã Lịch", typeof(int));
                dt.Columns.Add("Hội Viên", typeof(string));
                dt.Columns.Add("Số ĐT", typeof(string));
                dt.Columns.Add("Huấn Luyện Viên", typeof(string));
                dt.Columns.Add("Chuyên Môn", typeof(string));
                dt.Columns.Add("Ngày Tập", typeof(DateTime));
                dt.Columns.Add("Bắt Đầu", typeof(string));
                dt.Columns.Add("Kết Thúc", typeof(string));
                dt.Columns.Add("Trạng Thái", typeof(string));
                dt.Columns.Add("Ghi Chú", typeof(string));

                foreach (var item in list)
                {
                    dt.Rows.Add(
                        item.ScheduleId,
                        item.MemberName,
                        item.MemberPhone ?? "",
                        item.TrainerName,
                        item.Specialty ?? "Thể hình Gym",
                        item.ScheduleDate,
                        item.StartTime.ToString(@"hh\:mm"),
                        item.EndTime.ToString(@"hh\:mm"),
                        item.Status,
                        item.Notes ?? ""
                    );
                }

                return dt;
            }
        }

        /// <summary>
        /// Kiểm tra trùng lịch tập HLV và Hội viên bằng LINQ Where & FirstOrDefault
        /// </summary>
        public bool CheckScheduleConflict(int? currentScheduleId, int trainerId, int memberId, DateTime scheduleDate, TimeSpan startTime, TimeSpan endTime, out string conflictMessage)
        {
            conflictMessage = string.Empty;
            DateTime dateOnly = scheduleDate.Date;

            using (var db = new GymManagementDBEntities1())
            {
                // 1. Kiểm tra Huấn luyện viên có bị trùng lịch không
                var trainerConflict = (from s in db.PTSchedules
                                       join t in db.Trainers on s.TrainerId equals t.TrainerId
                                       join m in db.Members on s.MemberId equals m.MemberId
                                       where s.TrainerId == trainerId
                                          && s.ScheduleDate == dateOnly
                                          && s.Status != "Đã hủy"
                                          && (!currentScheduleId.HasValue || s.ScheduleId != currentScheduleId.Value)
                                          && s.StartTime < endTime && s.EndTime > startTime
                                       select new
                                       {
                                           s.ScheduleId,
                                           TrainerName = t.FullName,
                                           MemberName = m.FullName,
                                           s.StartTime,
                                           s.EndTime
                                       }).FirstOrDefault();

                if (trainerConflict != null)
                {
                    conflictMessage = $"TRÙNG LỊCH HUẤN LUYỆN VIÊN!\n\n" +
                                      $"Huấn luyện viên [{trainerConflict.TrainerName}] đã có lịch tập với Hội viên [{trainerConflict.MemberName}] " +
                                      $"vào khung giờ {trainerConflict.StartTime:hh\\:mm} - {trainerConflict.EndTime:hh\\:mm} ngày {scheduleDate:dd/MM/yyyy}.\n\n" +
                                      $"Vui lòng chọn khung giờ khác hoặc đổi Huấn luyện viên!";
                    return true;
                }

                // 2. Kiểm tra Hội viên có bị trùng lịch của chính mình không
                var memberConflict = (from s in db.PTSchedules
                                      join t in db.Trainers on s.TrainerId equals t.TrainerId
                                      join m in db.Members on s.MemberId equals m.MemberId
                                      where s.MemberId == memberId
                                         && s.ScheduleDate == dateOnly
                                         && s.Status != "Đã hủy"
                                         && (!currentScheduleId.HasValue || s.ScheduleId != currentScheduleId.Value)
                                         && s.StartTime < endTime && s.EndTime > startTime
                                      select new
                                      {
                                          s.ScheduleId,
                                          MemberName = m.FullName,
                                          TrainerName = t.FullName,
                                          s.StartTime,
                                          s.EndTime
                                      }).FirstOrDefault();

                if (memberConflict != null)
                {
                    conflictMessage = $"TRÙNG LỊCH HỘI VIÊN!\n\n" +
                                      $"Hội viên [{memberConflict.MemberName}] đã có lịch tập với HLV [{memberConflict.TrainerName}] " +
                                      $"vào khung giờ {memberConflict.StartTime:hh\\:mm} - {memberConflict.EndTime:hh\\:mm} ngày {scheduleDate:dd/MM/yyyy}.\n\n" +
                                      $"Một hội viên không thể tập 2 buổi cùng lúc!";
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Thêm lịch tập mới qua Entity Framework
        /// </summary>
        public void AddSchedule(int memberId, int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, string status, string notes)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var s = new PTSchedules
                {
                    MemberId = memberId,
                    TrainerId = trainerId,
                    ScheduleDate = date.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = status,
                    Notes = notes
                };
                db.PTSchedules.Add(s);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Cập nhật lịch tập qua Entity Framework
        /// </summary>
        public void UpdateSchedule(int scheduleId, int memberId, int trainerId, DateTime date, TimeSpan startTime, TimeSpan endTime, string status, string notes)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var s = db.PTSchedules.Find(scheduleId);
                if (s != null)
                {
                    s.MemberId = memberId;
                    s.TrainerId = trainerId;
                    s.ScheduleDate = date.Date;
                    s.StartTime = startTime;
                    s.EndTime = endTime;
                    s.Status = status;
                    s.Notes = notes;
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Xóa lịch tập qua Entity Framework
        /// </summary>
        public void DeleteSchedule(int scheduleId)
        {
            using (var db = new GymManagementDBEntities1())
            {
                var s = db.PTSchedules.Find(scheduleId);
                if (s != null)
                {
                    db.PTSchedules.Remove(s);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Lấy danh sách ca đã đặt của HLV trong ngày bằng LINQ
        /// </summary>
        public List<BookedSlotInfo> GetBookedSlots(int trainerId, DateTime date)
        {
            DateTime dateOnly = date.Date;
            using (var db = new GymManagementDBEntities1())
            {
                return (from s in db.PTSchedules
                        join m in db.Members on s.MemberId equals m.MemberId
                        where s.TrainerId == trainerId
                           && s.ScheduleDate == dateOnly
                           && s.Status != "Đã hủy"
                        orderby s.StartTime
                        select new BookedSlotInfo
                        {
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            MemberName = m.FullName
                        }).ToList();
            }
        }
    }
}
