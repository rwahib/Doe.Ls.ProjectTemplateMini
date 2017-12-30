		




		DECLARE 
	@scriptNumber NVARCHAR(12) = '02190'
,	@scriptName NVARCHAR(256) = '02190-Trim Integration'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
DECLARE @sql nvarchar(max);	
set @sql = 'CREATE TABLE [dbo].[TrimRecord]
(
[RolePositionDescId] [int] NOT NULL,
[Uri] [bigint] NULL,
[Token] [nvarchar] (64) COLLATE Latin1_General_CI_AS NULL,
[LastPublishedDate] [datetime] NULL CONSTRAINT [DF_TrimRecord_LastModifiedDate] DEFAULT (getdate()),
[LastRevisionNumber] [int] NULL
)';




BEGIN TRY

EXECUTE (@sql); 

END try

BEGIN CATCH
PRINT ERROR_MESSAGE()
END CATCH



INSERT INTO [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (130, N'Sync Pd Or Rd attachment to Trim', N'TrimSynced', N'<p> Published the approved or Imported Position or role description pdf files to TRIM if it is not published, if the files are changed, it will re published with new revisions in TRIM</p>')
INSERT INTO [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (140, N'Sync ALL Pd or Rd attachment to Trim', N'TrimSynced', N'<p> Published the approved or Imported Position or role description pdf files to TRIM if it is not published, if the files are changed, it will re published with new revisions in TRIM</p>')




INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
