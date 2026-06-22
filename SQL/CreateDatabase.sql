-- =============================================
-- Product Management Database Setup Script
-- Run this in SSMS if using a full SQL Server
-- instance instead of LocalDB
-- =============================================

-- Create Database (skip if already created by EF migrations)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProductManagementDb')
BEGIN
    CREATE DATABASE ProductManagementDb;
END
GO

USE ProductManagementDb;
GO

-- Create Products Table (skip if using EF migrations)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Name        NVARCHAR(100)     NOT NULL,
        Description NVARCHAR(500)     NULL,
        Price       DECIMAL(18,2)     NOT NULL,
        Stock       INT               NOT NULL DEFAULT 0,
        Category    NVARCHAR(50)      NOT NULL,
        CreatedAt   DATETIME2         NOT NULL DEFAULT GETDATE(),
        UpdatedAt   DATETIME2         NULL,
        IsActive    BIT               NOT NULL DEFAULT 1
    );
END
GO

-- Insert seed data
IF NOT EXISTS (SELECT 1 FROM Products)
BEGIN
    INSERT INTO Products (Name, Description, Price, Stock, Category, CreatedAt, IsActive)
    VALUES
        ('Laptop Pro 15',   'High-performance laptop with 16GB RAM and 512GB SSD', 1299.99, 25,  'Electronics', GETDATE(), 1),
        ('Wireless Mouse',  'Ergonomic wireless mouse with 2.4GHz connectivity',     29.99, 150, 'Accessories', GETDATE(), 1),
        ('USB-C Hub',       '7-in-1 USB-C hub with HDMI, USB 3.0, and SD card',      49.99,  80, 'Accessories', GETDATE(), 1);
END
GO

-- Useful queries for verification
SELECT * FROM Products;
GO
