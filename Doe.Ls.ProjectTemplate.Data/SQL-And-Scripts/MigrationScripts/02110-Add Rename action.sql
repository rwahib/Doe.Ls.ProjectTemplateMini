
DECLARE 
	@scriptNumber NVARCHAR(12) = '02110'
,	@scriptName NVARCHAR(256) = '02110-Add Rename action'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   
   
BEGIN TRANSACTION
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

PRINT(N'Add row to [dbo].[WfAction]')

INSERT INTO [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (80, N'Rename', N'Rename', N'<p>Rename the role description (or position description) and cascade update <strong>for all attached positions</strong> even the deleted.</p>')
COMMIT TRANSACTION

  INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
