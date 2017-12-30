DECLARE 
	@scriptNumber NVARCHAR(12) = '0010'
,	@scriptName NVARCHAR(256) = '0010-Updating-Active-InActive status.sql'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
	
	
IF EXISTS (SELECT 1 FROM [StatusValue] WHERE [StatusId] = 1000)
BEGIN
	PRINT 'STATUS is already exists'
	END
	ELSE
	BEGIN
INSERT INTO [dbo].[StatusValue]
           ([StatusId]
           ,[StatusName]
           ,[StatusDescription])
     VALUES
           (1000,'Active','Active');
		   END
		   
	IF EXISTS (SELECT 1 FROM [StatusValue] WHERE [StatusId] = 1010)	
BEGIN
	PRINT 'STATUS is already exists'
	END
	ELSE
	BEGIN
INSERT INTO [dbo].[StatusValue]
           ([StatusId]
           ,[StatusName]
           ,[StatusDescription])
     VALUES
           (1010,'InActive','InActive');

  END


	update Executive
	set StatusId=1000
	where Executive.StatusId=10;
	

	update Directorate
	set StatusId=1000
	where Directorate.StatusId=10;

		update Directorate
	set StatusId=1000
	where Directorate.StatusId=10;


	update FunctionalArea
	set StatusId=1000
	where FunctionalArea.StatusId=10;


	update BusinessUnit
	set StatusId=1000
	where BusinessUnit.StatusId=10;

					update Unit
	set StatusId=1000
	where Unit.StatusId=10;

	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)
END
