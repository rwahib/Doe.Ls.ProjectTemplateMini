DECLARE 
	@scriptNumber NVARCHAR(12) = '0040'
,	@scriptName NVARCHAR(256) = '0040-Add default value for unit'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/

SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON



ALTER TABLE dbo.Unit ADD CONSTRAINT
	DF_Unit_ReportToUnit DEFAULT -1 FOR ReportToUnit

ALTER TABLE dbo.Unit SET (LOCK_ESCALATION = TABLE)




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
