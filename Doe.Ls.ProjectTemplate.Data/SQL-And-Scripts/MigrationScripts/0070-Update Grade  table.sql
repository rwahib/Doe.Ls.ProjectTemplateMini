
DECLARE 
	@scriptNumber NVARCHAR(12) = '0070'
,	@scriptName NVARCHAR(256) = '0070-Update Grade  table'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	UPDATE dbo.Grade
		SET StatusId=1000
		WHERE StatusId=10;

		
		UPDATE dbo.Grade
		SET GradeTitle=GradeCode
		WHERE GradeCode in ('SNO1','SNO2','SNO3','SPEP3');




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
