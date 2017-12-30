
DECLARE 
	@scriptNumber NVARCHAR(12) = '0050'
,	@scriptName NVARCHAR(256) = '0050-Update organisational table'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN

update OrgLevel
set OrgLevelName='BusinessUnit',OrgLevelTitle='Business Unit',Description='Bussiness unit level'
where OrgLevel.OrgLevelId=50;


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
