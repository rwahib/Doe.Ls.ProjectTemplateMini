
DECLARE 
	@scriptNumber NVARCHAR(12) = '0160'
,	@scriptName NVARCHAR(256) = '0160-Update Capability Indicators -2 '
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN

DELETE dbo.RoleDescCapabilityMatrix
WHERE RoleDescCapabilityMatrix.GradeCode NOT IN (SELECT GradeCode FROM dbo.Grade)
;



ALTER TABLE dbo.RoleDescCapabilityMatrix ADD CONSTRAINT
	FK_RoleDescCapabilityMatrix_Grade FOREIGN KEY
	(
	GradeCode
	) REFERENCES dbo.Grade
	(
	GradeCode
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
	;




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
