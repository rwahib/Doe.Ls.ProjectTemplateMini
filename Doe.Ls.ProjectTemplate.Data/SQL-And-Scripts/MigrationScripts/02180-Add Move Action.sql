DECLARE 
	@scriptNumber NVARCHAR(12) = '02180'
,	@scriptName NVARCHAR(256) = '02180-Add Move Action'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	

INSERT INTO [dbo].[WfAction]
           ([WfActionId]
           ,[WfActionName]
           ,[WfActionStatus]
           ,[WfActionDescription])
     VALUES
           (120,
           'Move Positions',
           'PositionMoved',
           '<p>Positions will be moved from RD/PD to another RD/PD. <br/>Warning: the new title and grade will be inherited from the target RD/PD  </p>')
		   

	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
