
DECLARE 
	@scriptNumber NVARCHAR(12) = '02140'
,	@scriptName NVARCHAR(256) = '02140-Add Message and action'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   PRINT(N'Add rows to [dbo].[SysMessage]')

INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-RENAME-PD-RD-ACTION-CONFIRM-CONTENT', N'<p>Position description / Role description title has been updated.</p>', 30, N'<p>This action {0} has been successfully performed on {1} and the status is changed to {2}</p>', '2017-07-12 12:17:35.787', '2017-07-12 12:17:35.787', N'refky.wahib', N'refky.wahib')
INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-RENAME-PD-RD-ACTION-CONFIRM-HEADER', N'<p>{0} action has been successful.</p>', 30, NULL, '2017-07-27 14:45:49.867', '2017-07-27 14:45:49.867', N'mary.wang8', N'mary.wang8')
INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-RENAME-POS-NO-ACTION-CONFIRM-CONTENT', N'<p>Position number has been updated.</p>', 30, N'<p>This action {0} has been successfully performed on {1} and the status is changed to {2}</p>', '2017-07-12 12:17:35.787', '2017-07-12 12:17:35.787', N'refky.wahib', N'refky.wahib')
INSERT INTO [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'WF-ACTION-RENAME-POS-NO-ACTION-CONFIRM-HEADER', N'<p>{0} action has been successful.</p>', 30, NULL, '2017-07-27 14:45:49.867', '2017-07-27 14:45:49.867', N'mary.wang8', N'mary.wang8')
PRINT(N'Operation applied to 4 rows out of 4')




INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
