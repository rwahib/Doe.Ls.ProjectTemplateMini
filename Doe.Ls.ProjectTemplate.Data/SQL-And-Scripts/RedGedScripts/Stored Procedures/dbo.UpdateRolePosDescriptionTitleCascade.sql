
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



-- =============================================
-- Author:		<Refky wahib>
-- Create date: <2017-08-02>
-- Description:	<Rename the Role/Pos description and all positions>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateRolePosDescriptionTitleCascade]
@DocNumber nvarchar(150),
@Title nvarchar(250)
AS
BEGIN

BEGIN TRANSACTION
UPDATE dbo.RolePositionDescription
SET Title=@Title
WHERE DocNumber=@DocNumber

UPDATE dbo.Position
SET PositionTitle=@Title
WHERE RolePositionDescriptionId IN (SELECT INRPD.RolePositionDescId FROM dbo.RolePositionDescription AS INRPD
WHERE DocNumber=@DocNumber)

COMMIT




SELECT @DocNumber
END


GO

