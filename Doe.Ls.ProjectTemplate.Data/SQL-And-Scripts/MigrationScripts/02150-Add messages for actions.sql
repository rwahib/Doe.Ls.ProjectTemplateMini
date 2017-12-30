
DECLARE 
	@scriptNumber NVARCHAR(12) = '02150'
,	@scriptName NVARCHAR(256) = '02150-Add Actions'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
   
   
INSERT [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (100, N'Bulk bring to draft', N'Draft', N'<p>Bring to draft, all attached positions that have status Imported/Approved" <strong>only</strong>, other&nbsp;positions will remain the same. if all positions&nbsp;status changed to "Draft" the related Role description or Position description will be draft</p>')
INSERT [dbo].[WfAction] ([WfActionId], [WfActionName], [WfActionStatus], [WfActionDescription]) VALUES (110, N'Bulk mark as imported', N'Imported', N'<p>Mark as imported, all attached positions that have status Draft" <strong>only</strong>, other&nbsp;positions will remain the same. if all positions&nbsp;status changed to "Imported/Approved" the related Role description or Position description will be&nbsp;Imported</p>')

INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
