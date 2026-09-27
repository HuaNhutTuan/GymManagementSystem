-- ==========================================================
-- SCRIPT TẠO DATABASE VÀ DỮ LIỆU MẪU CHO GYMMANAGEMENTDB
-- Hệ thống Quản lý Phòng Gym (Gym Management System)
-- ==========================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GymManagementDB')
BEGIN
    CREATE DATABASE GymManagementDB;
END
GO

USE GymManagementDB;
GO

-- 1. Bảng Roles (Vai trò người dùng)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE Roles (
        RoleId INT IDENTITY(1,1) PRIMARY KEY,
        RoleName NVARCHAR(50) NOT NULL,
        Description NVARCHAR(250) NULL
    );
END
GO

-- 2. Bảng Users (Tài khoản người dùng)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserId INT IDENTITY(1,1) PRIMARY KEY,
        Username VARCHAR(50) NOT NULL UNIQUE,
        PasswordHash VARCHAR(256) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        RoleId INT NOT NULL,
        IsActive BIT DEFAULT 1,
        CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
    );
END
GO

-- 3. Bảng Permissions (Quyền hạn)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permissions')
BEGIN
    CREATE TABLE Permissions (
        PermissionId INT IDENTITY(1,1) PRIMARY KEY,
        PermissionName NVARCHAR(100) NOT NULL,
        ControlName VARCHAR(100) NOT NULL
    );
END
GO

-- 4. Bảng RolePermissions (Phân quyền cho vai trò)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolePermissions')
BEGIN
    CREATE TABLE RolePermissions (
        RoleId INT NOT NULL,
        PermissionId INT NOT NULL,
        PRIMARY KEY (RoleId, PermissionId),
        CONSTRAINT FK_RP_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
        CONSTRAINT FK_RP_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId)
    );
END
GO

-- 5. Bảng Members (Hội viên)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Members')
BEGIN
    CREATE TABLE Members (
        MemberId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Phone VARCHAR(15) NULL,
        Email VARCHAR(100) NULL,
        QRCode VARCHAR(100) NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- 6. Bảng Packages (Gói tập)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Packages')
BEGIN
    CREATE TABLE Packages (
        PackageId INT IDENTITY(1,1) PRIMARY KEY,
        PackageName NVARCHAR(100) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        DurationDays INT NOT NULL,
        IsActive BIT DEFAULT 1
    );
END
GO

-- 7. Bảng Trainers (Huấn luyện viên)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Trainers')
BEGIN
    CREATE TABLE Trainers (
        TrainerId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Phone VARCHAR(15) NULL,
        Specialty NVARCHAR(100) NULL
    );
END
GO

-- 8. Bảng MemberPackages (Gói tập của hội viên)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MemberPackages')
BEGIN
    CREATE TABLE MemberPackages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MemberId INT NOT NULL,
        PackageId INT NOT NULL,
        TrainerId INT NULL,
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        IsActive BIT DEFAULT 1,
        CONSTRAINT FK_MP_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
        CONSTRAINT FK_MP_Packages FOREIGN KEY (PackageId) REFERENCES Packages(PackageId),
        CONSTRAINT FK_MP_Trainers FOREIGN KEY (TrainerId) REFERENCES Trainers(TrainerId)
    );
END
GO

-- 9. Bảng Equipments (Thiết bị)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Equipments')
BEGIN
    CREATE TABLE Equipments (
        EquipmentId INT IDENTITY(1,1) PRIMARY KEY,
        EquipmentName NVARCHAR(100) NOT NULL,
        Status NVARCHAR(50) NULL,
        Quantity INT DEFAULT 0
    );
END
GO

-- 10. Bảng Invoices (Hóa đơn)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Invoices')
BEGIN
    CREATE TABLE Invoices (
        InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
        MemberId INT NOT NULL,
        CreatedByUserId INT NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL,
        CreatedDate DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Invoices_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId),
        CONSTRAINT FK_Invoices_Users FOREIGN KEY (CreatedByUserId) REFERENCES Users(UserId)
    );
END
GO

-- 11. Bảng InvoiceDetails (Chi tiết hóa đơn)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'InvoiceDetails')
BEGIN
    CREATE TABLE InvoiceDetails (
        DetailId INT IDENTITY(1,1) PRIMARY KEY,
        InvoiceId INT NOT NULL,
        PackageId INT NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Quantity INT DEFAULT 1,
        CONSTRAINT FK_ID_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
        CONSTRAINT FK_ID_Packages FOREIGN KEY (PackageId) REFERENCES Packages(PackageId)
    );
END
GO

-- ==========================================================
-- DỮ LIỆU KHỞI TẠO BAN ĐẦU (SEED DATA)
-- ==========================================================

-- Thêm Roles mặc định
IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleId = 1)
BEGIN
    SET IDENTITY_INSERT Roles ON;
    INSERT INTO Roles (RoleId, RoleName, Description) VALUES
    (1, N'Admin', N'Quản trị viên toàn quyền hệ thống'),
    (2, N'Lễ Tân', N'Nhân viên lễ tân tiếp đón và đăng ký'),
    (3, N'PT', N'Huấn luyện viên thể hình');
    SET IDENTITY_INSERT Roles OFF;
END
GO

-- Thêm Users mặc định (Mật khẩu mặc định là: 123456 -> SHA256: 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92)
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, PasswordHash, FullName, RoleId, IsActive) VALUES
    ('admin', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Quản Trị Viên', 1, 1),
    ('tuan', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Hứa Nhựt Tuấn', 1, 1),
    ('tuanhua', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Hứa Nhựt Tuấn', 1, 1),
    ('letan', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Nguyễn Văn Lễ Tân', 2, 1),
    ('pt_tuan', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Hứa Nhựt Tuấn (PT)', 3, 1);
END
GO

-- Thêm một số Gói tập mẫu nếu chưa có
IF NOT EXISTS (SELECT 1 FROM Packages)
BEGIN
    INSERT INTO Packages (PackageName, Price, DurationDays, IsActive) VALUES
    (N'Gói 1 Tháng Thường', 300000, 30, 1),
    (N'Gói 3 Tháng Tiết Kiệm', 800000, 90, 1),
    (N'Gói 1 Năm VIP (Có PT kèm)', 3500000, 365, 1);
END
GO
