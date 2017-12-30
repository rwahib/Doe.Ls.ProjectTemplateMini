
DECLARE 
	@scriptNumber NVARCHAR(12) = '0190'
,	@scriptName NVARCHAR(256) = '0190-InsertSysMessage1'
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
VALUES  ( N'ERR-DOCNUM-EXISTS' , -- Code - nvarchar(64)
          N'This document number already exists.' , -- MessageFormat - nvarchar(500)
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
VALUES  ( N'ERR-NOT-FOUND' , -- Code - nvarchar(64)
          N'The {0} was not found.' , -- MessageFormat - nvarchar(500)
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
VALUES  ( N'ERR-NULL-PLEASE-ENTER' , -- Code - nvarchar(64)
          N'Please enter a {0}.' , -- MessageFormat - nvarchar(500)
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
VALUES  ( N'ERR-NULL-PLEASE-SELECT' , -- Code - nvarchar(64)
          N'Please select a {0}.' , -- MessageFormat - nvarchar(500)
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
VALUES  ( N'ERR-PDF_TEMPLATE-FOLDER-NOT-EXISTS' , -- Code - nvarchar(64)
          N'The folders for PDF templates or output didn''t exist. Please check the config file then create the folders.' , -- MessageFormat - nvarchar(500)
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
VALUES  ( N'ERR-RD-PD-MUST-EXISTS-BEFORE-POSITION' , -- Code - nvarchar(64)
          N'Error. A new position needs to be created before a role/position description can be created.' , -- MessageFormat - nvarchar(500)
          40 , -- MsgCategoryId - int
          N'' , -- MessageHint - nvarchar(max)
          GETDATE() , -- CreatedDate - datetime
          GETDATE() , -- LastModifiedDate - datetime
          N'System' , -- CreatedBy - nvarchar(120)
          N'System'  -- LastModifiedBy - nvarchar(120)
        )
  
  INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
