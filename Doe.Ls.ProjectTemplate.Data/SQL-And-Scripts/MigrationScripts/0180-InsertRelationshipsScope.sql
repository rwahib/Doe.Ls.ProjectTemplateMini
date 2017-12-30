
DECLARE 
	@scriptNumber NVARCHAR(12) = '0180'
,	@scriptName NVARCHAR(256) = '0180-Insert new relationship scope '
,	@developer NVARCHAR(256) = 'Mary Wang'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
  insert into RelationshipScope values(30,'Ministerial')

	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
