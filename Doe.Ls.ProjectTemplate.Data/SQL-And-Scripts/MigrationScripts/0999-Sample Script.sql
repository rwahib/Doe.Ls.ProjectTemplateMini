DECLARE 
	@scriptNumber NVARCHAR(12) = '0999'
,	@scriptName NVARCHAR(256) = '0999-Sample Script.sql'
,	@developer NVARCHAR(256) = 'Mary Wang'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	
	/*
	your script will be here

	*/


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
