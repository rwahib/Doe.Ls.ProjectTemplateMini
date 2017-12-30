SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
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
GO
