DECLARE 
	@scriptNumber NVARCHAR(12) = '0020'
,	@scriptName NVARCHAR(256) = '0020-Add HR dataEntry Role.sql'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	
	
	INSERT INTO [dbo].[SysRole]
           ([RoleId]
           ,[RoleTitle]
           ,[RoleApiName]
           ,[RoleDescription]
           )
     VALUES
           (70,'HR DataEntry','HRDataEntry','HR DataEntry')
  


INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
