DECLARE 
	@scriptNumber NVARCHAR(12) = '02170'
,	@scriptName NVARCHAR(256) = '02170-STP Bulk Import-BulkDraft'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
Exec(N'	


-- =============================================
-- Author:		Refky Wahib
-- Create date: 04/09/2017
-- Description:	<Bulk Bring to Draft>
-- =============================================
CREATE PROCEDURE [dbo].[BulkBringToDraftPositions]
@DocNumber NVARCHAR(150) 
	
AS
BEGIN
	
	/*

StatusId	StatusName	StatusDescription
10	Approved	For any database records
20	Imported	For any database records
30	Draft	For any database workflow records

*/

Update dbo.Position
SET StatusId=30
WHERE StatusId IN (10,20) 
AND RolePositionDescriptionId = (SELECT rpd.RolePositionDescId FROM dbo.RolePositionDescription AS RPD WHERE rpd.DocNumber=@DocNumber)



UPDATE dbo.RolePositionDescription
SET StatusId=30
WHERE NOT EXISTS
(
   SELECT 1 FROM Position POS
   WHERE POS.RolePositionDescriptionId = RolePositionDescription.RolePositionDescId AND POS.StatusId<>-1 
   AND POS.StatusId NOT IN (30)
   )
AND RolePositionDescription.DocNumber=@DocNumber


    
	SELECT 1 AS ResultValue;

END


');




Exec(N'


-- =============================================
-- Author:		Refky Wahib
-- Create date: 04/09/2017
-- Description:	<Bulk Mark as Import >
-- =============================================
CREATE PROCEDURE [dbo].[BulkMarkAsImportPositions]
@DocNumber NVARCHAR(150) 
	
AS
BEGIN
	
	/*

StatusId	StatusName	StatusDescription
10	Approved	For any database records
20	Imported	For any database records
30	Draft	For any database workflow records

*/

Update dbo.Position
SET StatusId=20
WHERE StatusId IN (30) 
AND RolePositionDescriptionId = (SELECT rpd.RolePositionDescId FROM dbo.RolePositionDescription AS RPD WHERE rpd.DocNumber=@DocNumber)



UPDATE dbo.RolePositionDescription
SET StatusId=20
WHERE NOT EXISTS 

(SELECT * FROM dbo.Position AS POS
WHERE RolePositionDescId=POS.RolePositionDescriptionId AND POS.StatusId<>-1 
AND POS.StatusId NOT IN (20,10)
)

AND RolePositionDescription.DocNumber=@DocNumber


    
	SELECT 1 AS ResultValue;

END


');
	



	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
