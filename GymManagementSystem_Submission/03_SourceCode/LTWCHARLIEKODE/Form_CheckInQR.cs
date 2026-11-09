using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace LTWCHARLIEKODE
{
    public partial class Form_CheckInQR : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Timer scanTimer;
        private Bitmap currentFrame;
        private readonly object frameLock = new object();
        private DateTime lastScanTime = DateTime.MinValue;
        private string lastScannedCode = "";

        public Form_CheckInQR()
        {
            InitializeComponent();
        }

        private void Form_CheckInQR_Load(object sender, EventArgs e)
        {
            InitCameraDevices();

            // Khởi tạo timer quét mã QR từ camera mỗi 350ms
            scanTimer = new Timer();
            scanTimer.Interval = 350;
            scanTimer.Tick += ScanTimer_Tick;

            // Khởi tạo cột cho DataGridView lịch sử check-in
            InitHistoryGrid();
        }

        private void InitCameraDevices()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                cboCamera.Items.Clear();

                if (videoDevices.Count > 0)
                {
                    foreach (FilterInfo device in videoDevices)
                    {
                        cboCamera.Items.Add(device.Name);
                    }
                    cboCamera.SelectedIndex = 0;
                    btnStartCamera.Enabled = true;
                    lblCameraStatus.Text = $"Đã tìm thấy {videoDevices.Count} thiết bị camera.";
                    lblCameraStatus.ForeColor = Color.DarkGreen;
                }
                else
                {
                    cboCamera.Items.Add("Không tìm thấy Webcam");
                    cboCamera.SelectedIndex = 0;
                    btnStartCamera.Enabled = false;
                    lblCameraStatus.Text = "Không có Webcam kết nối. Bạn có thể chọn ảnh QR từ máy để quét!";
                    lblCameraStatus.ForeColor = Color.DarkOrange;
                }
            }
            catch (Exception ex)
            {
                lblCameraStatus.Text = "Lỗi khởi tạo camera: " + ex.Message;
                lblCameraStatus.ForeColor = Color.Red;
            }
        }

        private void InitHistoryGrid()
        {
            dgvCheckInHistory.Columns.Clear();
            dgvCheckInHistory.Columns.Add("ColTime", "Thời gian");
            dgvCheckInHistory.Columns.Add("ColMemberId", "Mã HV");
            dgvCheckInHistory.Columns.Add("ColMemberName", "Họ tên");
            dgvCheckInHistory.Columns.Add("ColPackage", "Gói tập");
            dgvCheckInHistory.Columns.Add("ColStatus", "Trạng thái");
            dgvCheckInHistory.Columns.Add("ColDaysLeft", "Hạn còn");

            dgvCheckInHistory.Columns["ColTime"].Width = 85;
            dgvCheckInHistory.Columns["ColMemberId"].Width = 70;
            dgvCheckInHistory.Columns["ColMemberName"].Width = 140;
            dgvCheckInHistory.Columns["ColPackage"].Width = 150;
            dgvCheckInHistory.Columns["ColStatus"].Width = 110;
            dgvCheckInHistory.Columns["ColDaysLeft"].Width = 80;
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            if (videoDevices == null || videoDevices.Count == 0 || cboCamera.SelectedIndex < 0) return;

            try
            {
                StopCamera();

                videoSource = new VideoCaptureDevice(videoDevices[cboCamera.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();

                scanTimer.Start();
                btnStartCamera.Enabled = false;
                btnStopCamera.Enabled = true;
                cboCamera.Enabled = false;
                lblCameraStatus.Text = "Camera đang hoạt động. Hãy đưa mã QR vào khung hình...";
                lblCameraStatus.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể bật camera: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();
                lock (frameLock)
                {
                    currentFrame?.Dispose();
                    currentFrame = (Bitmap)frame.Clone();
                }

                picWebcam.Image?.Dispose();
                picWebcam.Image = frame;
            }
            catch { }
        }

        private void ScanTimer_Tick(object sender, EventArgs e)
        {
            if (currentFrame == null) return;

            Bitmap bitmapToDecode = null;
            lock (frameLock)
            {
                if (currentFrame != null)
                {
                    bitmapToDecode = (Bitmap)currentFrame.Clone();
                }
            }

            if (bitmapToDecode == null) return;

            try
            {
                BarcodeReader reader = new BarcodeReader
                {
                    AutoRotate = true,
                    Options = new ZXing.Common.DecodingOptions { TryInverted = true }
                };

                var result = reader.Decode(bitmapToDecode);
                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    string scannedText = result.Text.Trim();

                    // Tránh quét liên tục cùng 1 mã trong 3 giây
                    if (scannedText == lastScannedCode && (DateTime.Now - lastScanTime).TotalSeconds < 3)
                    {
                        return;
                    }

                    lastScannedCode = scannedText;
                    lastScanTime = DateTime.Now;

                    this.Invoke(new Action(() =>
                    {
                        txtManualCode.Text = scannedText;
                        ProcessCheckIn(scannedText);
                    }));
                }
            }
            catch { }
            finally
            {
                bitmapToDecode.Dispose();
            }
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void StopCamera()
        {
            scanTimer?.Stop();

            if (videoSource != null)
            {
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }
                videoSource.NewFrame -= VideoSource_NewFrame;
                videoSource = null;
            }

            btnStartCamera.Enabled = (videoDevices != null && videoDevices.Count > 0);
            btnStopCamera.Enabled = false;
            cboCamera.Enabled = true;
            lblCameraStatus.Text = "Camera đã tắt.";
            lblCameraStatus.ForeColor = Color.Gray;
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh mã QR của Hội viên";
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(ofd.FileName);
                        picWebcam.Image?.Dispose();
                        picWebcam.Image = (Bitmap)bmp.Clone();

                        BarcodeReader reader = new BarcodeReader
                        {
                            AutoRotate = true,
                            Options = new ZXing.Common.DecodingOptions { TryInverted = true }
                        };
                        var result = reader.Decode(bmp);
                        bmp.Dispose();

                        if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                        {
                            txtManualCode.Text = result.Text.Trim();
                            ProcessCheckIn(result.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã QR trong bức ảnh vừa chọn!\nVui lòng chọn ảnh chụp rõ nét hơn.",
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi đọc file ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnManualCheckIn_Click(object sender, EventArgs e)
        {
            string code = txtManualCode.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập Mã QR hoặc Mã hội viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtManualCode.Focus();
                return;
            }

            ProcessCheckIn(code);
        }

        private void ProcessCheckIn(string qrCodeOrId)
        {
            try
            {
                using (GymManagementDBEntities1 db = new GymManagementDBEntities1())
                {
                    // 1. Tìm hội viên theo QRCode hoặc MemberId (nếu nhập số)
                    int memberId = 0;
                    int.TryParse(qrCodeOrId, out memberId);

                    var member = db.Members.FirstOrDefault(m => m.QRCode == qrCodeOrId || (memberId > 0 && m.MemberId == memberId));

                    if (member == null)
                    {
                        // Thử tìm theo Phone nếu quét/nhập số điện thoại
                        member = db.Members.FirstOrDefault(m => m.Phone == qrCodeOrId);
                    }

                    if (member == null)
                    {
                        DisplayCheckInResult(false, "KHÔNG TÌM THẤY", "Mã không hợp lệ hoặc không có Hội viên nào trong hệ thống!", null, null, 0);
                        System.Media.SystemSounds.Hand.Play();
                        return;
                    }

                    // 2. Tra cứu gói tập của hội viên
                    var memberPackages = db.MemberPackages
                        .Where(mp => mp.MemberId == member.MemberId)
                        .OrderByDescending(mp => mp.EndDate)
                        .ToList();

                    var activePackage = memberPackages.FirstOrDefault(mp => mp.IsActive == true && mp.EndDate >= DateTime.Today);

                    string packageName = "Chưa có";
                    int daysLeft = 0;
                    DateTime? endDate = null;

                    if (activePackage != null)
                    {
                        var pkg = db.Packages.FirstOrDefault(p => p.PackageId == activePackage.PackageId);
                        packageName = pkg != null ? pkg.PackageName : $"Gói #{activePackage.PackageId}";
                        daysLeft = (int)(activePackage.EndDate.Date - DateTime.Today).TotalDays;
                        endDate = activePackage.EndDate;

                        // Check-in thành công
                        DisplayCheckInResult(true, "HỢP LỆ - VÀO TẬP", $"Chào mừng {member.FullName} vào tập!", member, packageName, daysLeft, endDate);
                        System.Media.SystemSounds.Asterisk.Play();

                        AddHistoryRow(DateTime.Now.ToString("HH:mm:ss"), member.MemberId.ToString(), member.FullName, packageName, "Thành công", $"{daysLeft} ngày");
                    }
                    else if (memberPackages.Count > 0)
                    {
                        // Đã từng có gói tập nhưng đã hết hạn
                        var latestPkg = memberPackages.First();
                        var pkg = db.Packages.FirstOrDefault(p => p.PackageId == latestPkg.PackageId);
                        packageName = pkg != null ? pkg.PackageName : $"Gói #{latestPkg.PackageId}";
                        daysLeft = (int)(latestPkg.EndDate.Date - DateTime.Today).TotalDays;
                        endDate = latestPkg.EndDate;

                        DisplayCheckInResult(false, "GÓI HẾT HẠN", $"Gói tập của {member.FullName} đã hết hạn từ ngày {latestPkg.EndDate:dd/MM/yyyy}! Vui lòng gia hạn.", member, packageName, daysLeft, endDate);
                        System.Media.SystemSounds.Exclamation.Play();

                        AddHistoryRow(DateTime.Now.ToString("HH:mm:ss"), member.MemberId.ToString(), member.FullName, packageName, "Hết hạn", $"{daysLeft} ngày");
                    }
                    else
                    {
                        // Chưa đăng ký gói tập nào
                        DisplayCheckInResult(false, "CHƯA ĐĂNG KÝ GÓI", $"Hội viên {member.FullName} chưa đăng ký gói tập nào trong hệ thống!", member, "Không có", 0, null);
                        System.Media.SystemSounds.Exclamation.Play();

                        AddHistoryRow(DateTime.Now.ToString("HH:mm:ss"), member.MemberId.ToString(), member.FullName, "Chưa đăng ký", "Không hợp lệ", "0");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý Check-in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCheckInResult(bool isValid, string statusText, string message, Members member, string packageName, int daysLeft, DateTime? endDate = null)
        {
            lblResultStatus.Text = statusText;
            lblResultMessage.Text = message;

            if (isValid)
            {
                pnlResultHeader.BackColor = Color.FromArgb(39, 174, 96); // Green
                lblResultStatus.ForeColor = Color.White;
            }
            else if (statusText == "GÓI HẾT HẠN")
            {
                pnlResultHeader.BackColor = Color.FromArgb(231, 76, 60); // Red
                lblResultStatus.ForeColor = Color.White;
            }
            else
            {
                pnlResultHeader.BackColor = Color.FromArgb(243, 156, 18); // Orange
                lblResultStatus.ForeColor = Color.White;
            }

            if (member != null)
            {
                lblInfoMemberId.Text = $"Mã HV: #{member.MemberId}";
                lblInfoFullName.Text = $"Họ tên: {member.FullName}";
                lblInfoPhone.Text = $"SĐT: {member.Phone ?? "Chưa có"}";
                lblInfoEmail.Text = $"Email: {member.Email ?? "Chưa có"}";
                lblInfoPackage.Text = $"Gói tập: {packageName ?? "Chưa đăng ký"}";
                lblInfoExpiry.Text = endDate.HasValue ? $"Hạn dùng: {endDate.Value:dd/MM/yyyy} (còn {daysLeft} ngày)" : "Hạn dùng: --";
            }
            else
            {
                lblInfoMemberId.Text = "Mã HV: --";
                lblInfoFullName.Text = "Họ tên: Không xác định";
                lblInfoPhone.Text = "SĐT: --";
                lblInfoEmail.Text = "Email: --";
                lblInfoPackage.Text = "Gói tập: --";
                lblInfoExpiry.Text = "Hạn dùng: --";
            }
        }

        private void AddHistoryRow(string time, string id, string name, string pkg, string status, string daysLeft)
        {
            int index = dgvCheckInHistory.Rows.Add(time, id, name, pkg, status, daysLeft);
            if (status == "Thành công")
            {
                dgvCheckInHistory.Rows[index].DefaultCellStyle.ForeColor = Color.DarkGreen;
            }
            else if (status == "Hết hạn")
            {
                dgvCheckInHistory.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
            }
            else
            {
                dgvCheckInHistory.Rows[index].DefaultCellStyle.ForeColor = Color.DarkOrange;
            }

            // Tự động cuộn tới dòng mới nhất
            dgvCheckInHistory.FirstDisplayedScrollingRowIndex = index;
            lblTotalCheckInToday.Text = $"Tổng lượt quét phiên này: {dgvCheckInHistory.Rows.Count}";
        }

        private void Form_CheckInQR_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }
    }
}
