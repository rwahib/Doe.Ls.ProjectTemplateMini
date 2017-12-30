
DECLARE 
	@scriptNumber NVARCHAR(12) = '02120'
,	@scriptName NVARCHAR(256) = '02120-Add OtherOverView'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   
ALTER TABLE PositionInformation
ADD OtherOverview nvarchar(max);


INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
