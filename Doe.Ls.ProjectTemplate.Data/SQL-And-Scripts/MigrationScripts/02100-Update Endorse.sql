
DECLARE 
	@scriptNumber NVARCHAR(12) = '02100'
,	@scriptName NVARCHAR(256) = '02100-Update Endorse'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   UPDATE dbo.StatusValue
SET StatusName='Endorsed'
WHERE StatusId=50
  INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
