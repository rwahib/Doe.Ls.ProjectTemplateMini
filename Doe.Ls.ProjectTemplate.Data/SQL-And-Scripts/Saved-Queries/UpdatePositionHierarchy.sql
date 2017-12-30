/****** Object:  StoredProcedure [dbo].[UpdatePositionHierarchy]    Script Date: 2/08/2017 5:25:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Refky wahib>
-- Create date: <2017-08-02>
-- Description:	<Full path update>
-- =============================================
ALTER PROCEDURE [dbo].[UpdatePositionHierarchy]

	@PosId INT

AS
BEGIN

-- OLD WAY	

--UPDATE dbo.Position 
--SET dbo.Position.positionpath =CONCAT('/',@PosId, o.PositionPath )
  
--FROM 
--    dbo.Position o 
--  JOIN 
--    dbo.Position  ON dbo.Position.ReportToPositionId = o.PositionId
--WHERE 
--    dbo.Position.PositionId = @PosId


Declare @TABLEVAR table (id int ,parentid int)
DECLARE @path NVARCHAR(500)

;with name_tree as 
(
   select Pos.PositionId, Pos.ReportToPositionId
   from dbo.Position AS Pos
   where Pos.PositionId = @PosId 
   union all
   select Child.PositionId, Child.ReportToPositionId
    from dbo.Position AS Child
   join name_tree p on Child.PositionId = P.ReportToPositionId-- this is the recursion
   
    AND Child.PositionId<>Child.ReportToPositionId
) 
-- Here you can insert directly to a temp table without CREATE TABLE synthax
INSERT INTO @TABLEVAR
select *
from name_tree

DECLARE @FULLPATH VARCHAR(8000) 
SELECT @FULLPATH = COALESCE(@FULLPATH + '/', '') + CAST(parentid AS NVARCHAR(500))
FROM @TABLEVAR

SET @FULLPATH= CAST(@PosId AS NVARCHAR(500))+'/'+@FULLPATH

UPDATE dbo.Position
SET PositionPath=@FULLPATH
WHERE PositionId=@PosId

SELECT @PosId
SELECT @FULLPATH
END



--  BATCH
Declare @TABLEVAR table (positionId int)
 Declare @positionId int


INSERT INTO @TABLEVAR
select TOP 10 PositionId
from dbo.Position

SELECT * FROM @TABLEVAR

WHILE (SELECT COUNT(*) from @TABLEVAR) > 0
Begin

select Top 1 @positionId =positionId from @TABLEVAR order by positionId asc

BEGIN TRY

SELECT @positionId
EXECUTE  [dbo].[UpdatePositionHierarchy]   @positionId

DELETE @TABLEVAR WHERE positionId=@positionId

END TRY
BEGIN CATCH
select ERROR_MESSAGE()
END CATCH
End

Go