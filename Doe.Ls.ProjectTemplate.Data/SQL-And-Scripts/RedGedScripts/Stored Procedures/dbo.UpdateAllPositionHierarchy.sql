
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


-- =============================================
-- Author:		<Refky wahib>
-- Create date: <2017-08-02>
-- Description:	<Full path update>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAllPositionHierarchy]

AS
BEGIN
Declare @TABLEVAR table (positionId int)

DECLARE @positionId int


INSERT INTO @TABLEVAR
select PositionId
from dbo.Position

WHILE (SELECT COUNT(*) from @TABLEVAR) > 0
Begin

select Top 1 @positionId =positionId from @TABLEVAR order by positionId asc

BEGIN TRY

EXECUTE  [dbo].[UpdatePositionHierarchy]   @positionId

DELETE @TABLEVAR WHERE positionId=@positionId

END TRY
BEGIN CATCH
select ERROR_MESSAGE()
END CATCH

END

    SELECT 1 AS ResultValue;

End

GO

