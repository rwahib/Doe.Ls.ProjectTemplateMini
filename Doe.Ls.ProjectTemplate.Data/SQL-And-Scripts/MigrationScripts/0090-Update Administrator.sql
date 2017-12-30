
DECLARE 
	@scriptNumber NVARCHAR(12) = '0090'
,	@scriptName NVARCHAR(256) = '0090-Update Administrator'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
  UPDATE dbo.SysRole
  SET RoleApiName='Administrator',
  RoleTitle='Administrator'
  WHERE RoleApiName='Adminstrator'
	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
