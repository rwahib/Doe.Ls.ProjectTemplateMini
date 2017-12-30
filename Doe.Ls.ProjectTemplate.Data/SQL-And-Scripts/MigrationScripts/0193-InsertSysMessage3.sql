DECLARE 
	@scriptNumber NVARCHAR(12) = '0193'
,	@scriptName NVARCHAR(256) = '0193-InsertSysMessage3'
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
VALUES  ( N'ERR-SELECT-GRADE' , -- Code - nvarchar(64)
          N'Error. Please select a Public Service grade/classification.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )


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
VALUES  ( N'ERR-SELECT-TEACHING-GRADE' , -- Code - nvarchar(64)
          N'Error. Please select a Teaching Service grade/classification.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )


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
VALUES  ( N'ERR-POSITION-END-DATE' , -- Code - nvarchar(64)
          N'Position end date is required for temporary positions.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )

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
VALUES  ( N'ERR-USER-PERMIT-MODIFY-TEAM-POSITION' , -- Code - nvarchar(64)
          N'You do not have permission to modify {0} team positions.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )


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
VALUES  ( N'ERR-POSITION-NUM-EXISTS' , -- Code - nvarchar(64)
          N'The position number {0} already exists.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )

 
  INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END