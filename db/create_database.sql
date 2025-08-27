-- Script para criar o banco de dados

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'animes')
BEGIN
    CREATE DATABASE animes;
    PRINT 'The Database animes was created successfully.';
END
ELSE
BEGIN
    PRINT 'The Database animes already exists.';
END
GO

USE animes;
GO

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'admin')
BEGIN
    CREATE LOGIN admin WITH PASSWORD = 'Password123!';
    PRINT 'The Login admin was created successfully!';
END
ELSE
BEGIN
    PRINT 'The Login admin already exists.';
END
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'admin')
BEGIN
    CREATE USER admin FOR LOGIN admin;
    EXEC sp_addrolemember 'db_owner', 'admin';
    PRINT 'The User admin was created and added to the db_owner role!';
END
ELSE
BEGIN
    PRINT 'The User admin already exists in the database.';
END
GO
