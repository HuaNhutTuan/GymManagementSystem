; =====================================================================
; KỊCH BẢN ĐÓNG GÓI BỘ CÀI ĐẶT INNO SETUP CHO HỆ THỐNG QUẢN LÝ PHÒNG GYM
; Môn học: Lập trình trên Windows - Trường ĐH Sư Phạm TP.HCM (HCMUE)
; Phân công: Nguyễn Đăng Khoa (Thành viên 2 - Integrations, Packaging & QA/Doc)
; =====================================================================

#define MyAppName "Hệ Thống Quản Lý Phòng Gym & Fitness"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Nhóm 4 - LTTW HCMUE"
#define MyAppExeName "LTWCHARLIEKODE.exe"

[Setup]
AppId={{9F505842-DF27-4F2D-A357-3C2040AC220F}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\GymManagementSystem
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=SetupOutput
OutputBaseFilename=GymManagement_Setup_v1.0
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
; Sao chép toàn bộ các file thực thi, DLL, và config từ thư mục Release
Source: "LTWCHARLIEKODE\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Kèm theo file script SQL Server khởi tạo cơ sở dữ liệu
Source: "GymManagementDB.sql"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
