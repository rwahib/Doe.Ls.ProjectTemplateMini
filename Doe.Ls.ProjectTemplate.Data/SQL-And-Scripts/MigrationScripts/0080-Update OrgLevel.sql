
DECLARE 
	@scriptNumber NVARCHAR(12) = '0080'
,	@scriptName NVARCHAR(256) = '0070-Update org level'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	UPDATE dbo.SysUserRole
SET OrgLevelId=20
WHERE OrgLevelId=10


DELETE dbo.OrgLevel
WHERE OrgLevelId=10;



	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
