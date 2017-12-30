DECLARE 
	@scriptNumber NVARCHAR(12) = '0030'
,	@scriptName NVARCHAR(256) = '0030-Update rolecapabilities field.sql'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN


EXECUTE sp_rename N'dbo.RoleDescription.RoleCapabilities', N'Tmp_RoleCapabilityItems_2', 'COLUMN' 

EXECUTE sp_rename N'dbo.RoleDescription.Tmp_RoleCapabilityItems_2', N'RoleCapabilityItems', 'COLUMN' 


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
