
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Refky wahib>
-- Create date: <2017-08-02>
-- Description:	<Full path update>
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePositionHierarchy]
	@PosId INT
AS
BEGIN

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
WHERE PositionId=@PosId ;

END
GO
