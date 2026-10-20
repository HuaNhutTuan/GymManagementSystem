using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace LTWCHARLIEKODE
{
    public class ExpiringMemberDto
    {
        public bool IsSelected { get; set; } = true;
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PackageName { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysLeft { get; set; }
        public string Status { get; set; }
    }

    public class SmtpConfig
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string SenderEmail { get; set; } = "gymcenter.fitness.hcmue@gmail.com";
        public string SenderPassword { get; set; } = ""; // Mật khẩu ứng dụng (App Password)
        public string DisplayName { get; set; } = "Trung Tâm Thể Hình Gym & Fitness";
    }

    public static class EmailReminderService
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailConfig.xml");

        public static SmtpConfig CurrentConfig { get; private set; } = new SmtpConfig();

        static EmailReminderService()
        {
            LoadConfig();
        }

        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    XDocument doc = XDocument.Load(ConfigPath);
                    var root = doc.Element("SmtpConfig");
                    if (root != null)
                    {
                        CurrentConfig.Host = root.Element("Host")?.Value ?? "smtp.gmail.com";
                        CurrentConfig.Port = int.TryParse(root.Element("Port")?.Value, out int p) ? p : 587;
                        CurrentConfig.EnableSsl = bool.TryParse(root.Element("EnableSsl")?.Value, out bool ssl) ? ssl : true;
                        CurrentConfig.SenderEmail = root.Element("SenderEmail")?.Value ?? "";
                        CurrentConfig.SenderPassword = root.Element("SenderPassword")?.Value ?? "";
                        CurrentConfig.DisplayName = root.Element("DisplayName")?.Value ?? "Trung Tâm Thể Hình Gym & Fitness";
                    }
                }
            }
            catch { }
        }

        public static void SaveConfig(SmtpConfig config)
        {
            try
            {
                CurrentConfig = config;
                XDocument doc = new XDocument(
                    new XElement("SmtpConfig",
                        new XElement("Host", config.Host),
                        new XElement("Port", config.Port),
                        new XElement("EnableSsl", config.EnableSsl),
                        new XElement("SenderEmail", config.SenderEmail),
                        new XElement("SenderPassword", config.SenderPassword),
                        new XElement("DisplayName", config.DisplayName)
                    )
                );
                doc.Save(ConfigPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể lưu cấu hình Email: " + ex.Message);
            }
        }

        public static List<ExpiringMemberDto> GetExpiringMembers(int daysThreshold = 5)
        {
            List<ExpiringMemberDto> result = new List<ExpiringMemberDto>();

            using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
            {
                DateTime today = DateTime.Today;
                DateTime limitDate = today.AddDays(daysThreshold);

                // Lấy các gói tập còn hạn hoặc vừa mới hết hạn trong 5 ngày
                DateTime expiredLimit = today.AddDays(-5);

                var query = (from mp in db.MemberPackages
                             join m in db.Members on mp.MemberId equals m.MemberId
                             join p in db.Packages on mp.PackageId equals p.PackageId into pkgJoin
                             from p in pkgJoin.DefaultIfEmpty()
                             where mp.IsActive == true && mp.EndDate <= limitDate && mp.EndDate >= expiredLimit
                             orderby mp.EndDate ascending
                             select new
                             {
                                 m.MemberId,
                                 m.FullName,
                                 m.Phone,
                                 m.Email,
                                 PackageName = p != null ? p.PackageName : "Gói tập",
                                 mp.EndDate
                             }).ToList();

                foreach (var item in query)
                {
                    int daysLeft = (int)(item.EndDate.Date - today).TotalDays;
                    string status = daysLeft < 0 ? $"Đã hết hạn {Math.Abs(daysLeft)} ngày"
                                  : (daysLeft == 0 ? "Hết hạn hôm nay" : $"Còn {daysLeft} ngày");

                    result.Add(new ExpiringMemberDto
                    {
                        IsSelected = true,
                        MemberId = item.MemberId,
                        FullName = item.FullName,
                        Phone = item.Phone,
                        Email = item.Email,
                        PackageName = item.PackageName,
                        EndDate = item.EndDate,
                        DaysLeft = daysLeft,
                        Status = status
                    });
                }
            }

            return result;
        }

        public static void SendReminderEmail(string toEmail, string memberName, string packageName, DateTime endDate, int daysLeft)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new Exception("Hội viên chưa có địa chỉ email!");
            }

            if (string.IsNullOrWhiteSpace(CurrentConfig.SenderEmail) || string.IsNullOrWhiteSpace(CurrentConfig.SenderPassword))
            {
                throw new Exception("Chưa cấu hình tài khoản Email gửi (Sender Email hoặc App Password trống)!");
            }

            string subject = daysLeft < 0
                ? $"[Gym & Fitness] Thông báo gói tập {packageName} của Quý khách đã hết hạn"
                : $"[Gym & Fitness] Nhắc nhở gói tập {packageName} của Quý khách sắp hết hạn ({daysLeft} ngày nữa)";

            string body = GenerateEmailTemplate(memberName, packageName, endDate, daysLeft);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(CurrentConfig.SenderEmail, CurrentConfig.DisplayName, System.Text.Encoding.UTF8);
                mail.To.Add(new MailAddress(toEmail.Trim()));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                using (SmtpClient smtp = new SmtpClient(CurrentConfig.Host, CurrentConfig.Port))
                {
                    smtp.EnableSsl = CurrentConfig.EnableSsl;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(CurrentConfig.SenderEmail, CurrentConfig.SenderPassword);
                    smtp.Timeout = 20000;

                    smtp.Send(mail);
                }
            }
        }

        public static void SendTestEmail(string toEmail)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new Exception("Vui lòng nhập email nhận thử nghiệm!");

            string subject = "[Gym & Fitness] Thử nghiệm cấu hình SMTP tự động thành công";
            string body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #27ae60; border-radius: 8px;'>
                    <h2 style='color: #27ae60;'>✓ Cấu hình Email hoạt động chính xác!</h2>
                    <p>Hệ thống Quản lý Phòng Gym & Fitness đã kết nối và gửi mail thử nghiệm thành công qua máy chủ <b>{CurrentConfig.Host}</b>.</p>
                    <p>Thời gian gửi: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
                </div>";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(CurrentConfig.SenderEmail, CurrentConfig.DisplayName, System.Text.Encoding.UTF8);
                mail.To.Add(new MailAddress(toEmail.Trim()));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(CurrentConfig.Host, CurrentConfig.Port))
                {
                    smtp.EnableSsl = CurrentConfig.EnableSsl;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(CurrentConfig.SenderEmail, CurrentConfig.SenderPassword);
                    smtp.Timeout = 15000;
                    smtp.Send(mail);
                }
            }
        }

        private static string GenerateEmailTemplate(string memberName, string packageName, DateTime endDate, int daysLeft)
        {
            string urgencyNotice = daysLeft < 0
                ? $"<span style='color: #c0392b; font-weight: bold;'>đã hết hạn vào ngày {endDate:dd/MM/yyyy}</span>"
                : (daysLeft == 0
                    ? "<span style='color: #e67e22; font-weight: bold;'>sẽ hết hạn vào ngày HÔM NAY</span>"
                    : $"<span style='color: #e67e22; font-weight: bold;'>sẽ hết hạn sau {daysLeft} ngày nữa ({endDate:dd/MM/yyyy})</span>");

            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: 'Segoe UI', Arial, sans-serif; background-color: #f4f6f9; margin: 0; padding: 20px; }}
                    .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
                    .header {{ background: linear-gradient(135deg, #1abc9c, #16a085); color: #ffffff; padding: 25px 20px; text-align: center; }}
                    .header h1 {{ margin: 0; font-size: 22px; }}
                    .content {{ padding: 25px 30px; color: #2c3e50; line-height: 1.6; }}
                    .card {{ background-color: #ecf0f1; border-left: 5px solid #1abc9c; padding: 15px; margin: 20px 0; border-radius: 4px; }}
                    .footer {{ background-color: #f8f9fa; color: #7f8c8d; text-align: center; padding: 15px; font-size: 13px; border-top: 1px solid #e9ecef; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>HỆ THỐNG GYM & FITNESS CENTER</h1>
                        <p style='margin: 5px 0 0 0; font-size: 14px;'>Đồng hành cùng sức khỏe và vóc dáng của bạn</p>
                    </div>
                    <div class='content'>
                        <p>Kính gửi Hội viên <b>{memberName}</b>,</p>
                        <p>Trung tâm Thể hình Gym & Fitness xin chân thành cảm ơn Quý khách đã tin tưởng và đồng hành cùng chúng tôi trong suốt thời gian qua.</p>
                        
                        <div class='card'>
                            <p style='margin: 5px 0;'><b>Gói tập hiện tại:</b> {packageName}</p>
                            <p style='margin: 5px 0;'><b>Ngày hết hạn:</b> {endDate:dd/MM/yyyy}</p>
                            <p style='margin: 5px 0;'><b>Tình trạng:</b> Gói tập của Quý khách {urgencyNotice}.</p>
                        </div>

                        <p>Để duy trì tiến trình luyện tập liên tục và nhận các ưu đãi giảm giá gia hạn đặc biệt, Quý khách vui lòng ghé quầy Lễ tân hoặc liên hệ hotline để được hỗ trợ gia hạn sớm nhất.</p>
                        <p>Chúc Quý khách luôn dồi dào sức khỏe, tràn đầy năng lượng và đạt được mục tiêu tập luyện!</p>
                        
                        <p style='margin-top: 25px;'>Trân trọng,<br/><b>Ban Quản Lý Gym & Fitness</b></p>
                    </div>
                    <div class='footer'>
                        <p>Email này được gửi tự động từ Hệ thống Quản lý Phòng Gym.<br/>Địa chỉ: TP. Hồ Chí Minh | Hotline: 1900 1234</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
