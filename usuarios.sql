IF DB_ID('PruebaTecnicaDB') IS NULL
BEGIN
	CREATE DATABASE PruebaTecnicaDB;
END
GO

USE PruebaTecnicaDB;
GO

IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.Usuarios
	(
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		Email NVARCHAR(255) NOT NULL,
		[Password] NVARCHAR(255) NOT NULL
	);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Email = 'admin@prueba.com')
BEGIN
	INSERT INTO dbo.Usuarios (Email, [Password])
	VALUES ('admin@prueba.com', 'Admin123*');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Email = 'usuario@prueba.com')
BEGIN
	INSERT INTO dbo.Usuarios (Email, [Password])
	VALUES ('usuario@prueba.com', 'User123*');
END
GO
