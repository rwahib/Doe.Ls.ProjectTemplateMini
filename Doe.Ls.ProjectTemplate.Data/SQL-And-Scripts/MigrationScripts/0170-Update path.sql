
DECLARE 
	@scriptNumber NVARCHAR(12) = '0170'
,	@scriptName NVARCHAR(256) = '0170-Update position path '
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN

DECLARE @posIdToRepair TABLE
(
id int
)

declare @currentId int;

insert into @posIdToRepair
select PositionId FROM dbo.Position
order by PositionId

WHILE (SELECT COUNT(*) from @posIdToRepair) > 0
Begin

select Top 1 @currentId=id from @posIdToRepair order by id asc


delete from @posIdToRepair where id= @currentId

BEGIN TRY


PRINT  CAST(@currentId AS NVARCHAR(120)) +' now be updated'
EXEC	[dbo].[UpdatePositionHierarchy]		@PosId = @currentId

END TRY
BEGIN CATCH
select ERROR_MESSAGE()
END CATCH
End


UPDATE dbo.Position
SET PositionPath=REPLACE(PositionPath,'-1/','') 
WHERE PositionId<>-1 AND  ReportToPositionId<>-1


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
