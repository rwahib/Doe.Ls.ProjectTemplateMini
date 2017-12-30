
DECLARE 
	@scriptNumber NVARCHAR(12) = '0120'
,	@scriptName NVARCHAR(256) = '0120 Update Oveview'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN


/****** Object:  Table [dbo].[SysMsgCatery]    Script Date: 28/06/2017 10:05:57 AM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET NUMERIC_ROUNDABORT OFF

SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON

SET XACT_ABORT ON

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

BEGIN TRANSACTION

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Dropping extended properties'

EXEC sp_dropextendedproperty N'MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'DivisionOverview'

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Altering [dbo].[RoleDescription]'

IF @@ERROR <> 0 SET NOEXEC ON

ALTER TABLE [dbo].[RoleDescription] DROP
COLUMN [DivisionOverview]

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Altering [dbo].[Position]'

IF @@ERROR <> 0 SET NOEXEC ON

ALTER TABLE [dbo].[Position] ADD
[DivisionOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Altering [dbo].[Executive]'

IF @@ERROR <> 0 SET NOEXEC ON

ALTER TABLE [dbo].[Executive] ADD
[DefaultExecutiveOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL

IF @@ERROR <> 0 SET NOEXEC ON

PRINT N'Refreshing [dbo].[udv_positions]'

EXEC sp_refreshview N'[dbo].[udv_positions]'

IF @@ERROR <> 0 SET NOEXEC ON

COMMIT TRANSACTION

IF @@ERROR <> 0 SET NOEXEC ON

DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END


UPDATE dbo.Position
SET DivisionOverview = (SELECT         Executive.DefaultExecutiveOverview
FROM            Position AS POS INNER JOIN
                         Unit ON POS .UnitId = Unit.UnitId INNER JOIN
                         BusinessUnit ON Unit.BUnitId = BusinessUnit.BUnitId INNER JOIN
                         Directorate ON BusinessUnit.DirectorateId = Directorate.DirectorateId INNER JOIN
                         Executive ON Directorate.ExecutiveCod = Executive.ExecutiveCod
						 WHERE POS.PositionId=Position.PositionId
						 )




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
