
DECLARE 
	@scriptNumber NVARCHAR(12) = '02130'
,	@scriptName NVARCHAR(256) = '02130-Add Message and action'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   
INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-INVALID-POS-NO', N'Invalid position number value', 40, NULL, '2017-09-01 12:38:57.847', '2017-09-01 12:38:57.847', N'refky.wahib', N'refky.wahib')
INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-POS-NO-EXISTS', N'position number is already exists', 40, NULL, '2017-09-01 12:38:57.847', '2017-09-01 12:38:57.847', N'refky.wahib', N'refky.wahib')

INSERT INTO [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (90, N'Update position  number', N'Position number updated', N'<p>Updating position number to match the value from the HR system</p>')



INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
