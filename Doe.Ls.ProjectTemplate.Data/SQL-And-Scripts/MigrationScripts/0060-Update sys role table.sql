
DECLARE 
	@scriptNumber NVARCHAR(12) = '0060'
,	@scriptName NVARCHAR(256) = '0050-Update sys role table'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT FK_SysUserRole_OrgLevel

ALTER TABLE dbo.OrgLevel SET (LOCK_ESCALATION = TABLE)

ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT FK_SysUserRole_SysRole

ALTER TABLE dbo.SysRole SET (LOCK_ESCALATION = TABLE)
ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT FK_SysUserRole_SysUser

ALTER TABLE dbo.SysUser SET (LOCK_ESCALATION = TABLE)



ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT DF_SysUserRole_CreatedDate

ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT DF_SysUserRole_CreatedBy

ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT DF_SysUserRole_LastModifiedDate

ALTER TABLE dbo.SysUserRole
	DROP CONSTRAINT DF_SysUserRole_LastModifiedBy

CREATE TABLE dbo.Tmp_SysUserRole
	(
	UserId nvarchar(64) NOT NULL,
	RoleId int NOT NULL,
	StructureId nvarchar(64) NOT NULL,
	OrgLevelId int NOT NULL,
	ActiveFrom datetime NOT NULL,
	ActiveTo datetime NULL,
	Note nvarchar(MAX) NULL,
	CreatedDate datetime NOT NULL,
	CreatedBy nvarchar(120) NOT NULL,
	LastModifiedDate datetime NOT NULL,
	LastModifiedBy nvarchar(120) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]

ALTER TABLE dbo.Tmp_SysUserRole SET (LOCK_ESCALATION = TABLE)

ALTER TABLE dbo.Tmp_SysUserRole ADD CONSTRAINT
	DF_SysUserRole_CreatedDate DEFAULT (getdate()) FOR CreatedDate

ALTER TABLE dbo.Tmp_SysUserRole ADD CONSTRAINT
	DF_SysUserRole_CreatedBy DEFAULT (N'System') FOR CreatedBy

ALTER TABLE dbo.Tmp_SysUserRole ADD CONSTRAINT
	DF_SysUserRole_LastModifiedDate DEFAULT (getdate()) FOR LastModifiedDate

ALTER TABLE dbo.Tmp_SysUserRole ADD CONSTRAINT
	DF_SysUserRole_LastModifiedBy DEFAULT (N'System') FOR LastModifiedBy

IF EXISTS(SELECT * FROM dbo.SysUserRole)
	 EXEC('INSERT INTO dbo.Tmp_SysUserRole (UserId, RoleId, StructureId, OrgLevelId, ActiveFrom, ActiveTo, Note, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
		SELECT UserId, RoleId, CONVERT(nvarchar(64), StructureId), OrgLevelId, ActiveFrom, ActiveTo, Note, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy FROM dbo.SysUserRole WITH (HOLDLOCK TABLOCKX)')

DROP TABLE dbo.SysUserRole

EXECUTE sp_rename N'dbo.Tmp_SysUserRole', N'SysUserRole', 'OBJECT' 

ALTER TABLE dbo.SysUserRole ADD CONSTRAINT
	PK_SysUserRole_1 PRIMARY KEY CLUSTERED 
	(
	UserId,
	RoleId,
	StructureId,
	OrgLevelId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


ALTER TABLE dbo.SysUserRole ADD CONSTRAINT
	FK_SysUserRole_SysUser FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.SysUser
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.SysUserRole ADD CONSTRAINT
	FK_SysUserRole_SysRole FOREIGN KEY
	(
	RoleId
	) REFERENCES dbo.SysRole
	(
	RoleId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

ALTER TABLE dbo.SysUserRole ADD CONSTRAINT
	FK_SysUserRole_OrgLevel FOREIGN KEY
	(
	OrgLevelId
	) REFERENCES dbo.OrgLevel
	(
	OrgLevelId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	

COMMIT


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
