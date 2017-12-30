IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'position')
CREATE LOGIN [position] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [position] FOR LOGIN [position]
GO
