-- Create the database
CREATE DATABASE EmployeeDB;
GO


USE EmployeeDB;
GO

-- Create Employees table
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    HireDate DATE
);
SELECT * FROM Employees

USE EmployeeDB;



