DECLARE 
	@scriptNumber NVARCHAR(12) = '0194'
,	@scriptName NVARCHAR(256) = '0194-InsertSysMessage4'
,	@developer NVARCHAR(256) = 'Mary Wang'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE


BEGIN
 INSERT INTO dbo.SysMessage
        ( Code ,
          MessageFormat ,
          MsgCategoryId ,
          MessageHint ,
          CreatedDate ,
          LastModifiedDate ,
          CreatedBy ,
          LastModifiedBy
        )
VALUES  ( N'ERR-ERROR-HAS-OCCURED' , -- Code - nvarchar(64)
          N'An error has occurred. ' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )



 
  INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END