-- Script para criar o banco de dados

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$(DB_NAME)')
BEGIN
    CREATE DATABASE $(DB_NAME);
    PRINT 'The Database $(DB_NAME) was created successfully.';
END
ELSE
BEGIN
    PRINT 'The Database $(DB_NAME) already exists.';
END
GO

USE $(DB_NAME);
GO

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = '$(DB_USER)')
BEGIN
    CREATE LOGIN $(DB_USER) WITH PASSWORD = '$(DB_PASSWORD)';
    PRINT 'The Login $(DB_USER) was created successfully!';
END
ELSE
BEGIN
    PRINT 'The Login $(DB_USER) already exists.';
END
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = '$(DB_USER)')
BEGIN
    CREATE USER $(DB_USER) FOR LOGIN $(DB_USER);
    EXEC sp_addrolemember 'db_owner', '$(DB_USER)';
    PRINT 'The User $(DB_USER) was created and added to the db_owner role!';
END
ELSE
BEGIN
    PRINT 'The User $(DB_USER) already exists in the database.';
END
GO
