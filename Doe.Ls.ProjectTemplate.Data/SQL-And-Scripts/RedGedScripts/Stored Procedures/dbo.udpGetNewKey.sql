SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE procedure [dbo].[udpGetNewKey](@objectName VARCHAR(50),@increment INT = 1)
AS 
BEGIN
	DECLARE @ret INT = -1;

	--print @objectName
	SELECT @ret = MAX(ISNULL(OI.CounterValue,0)) + @increment 
	FROM dbo.AppObjectInfo AS OI
	WHERE OI.ObjectName=@objectName 

	IF (@ret IS NULL OR @ret=-1)
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM sys.tables AS TBL WHERE TBL.name =@objectName)
		BEGIN
			DECLARE @message VARCHAR(120);
			SET @message=@objectName + ' is not defined';
			  RAISERROR (@message, 
					   10, -- Severity.
					   1 -- State.
					   );
			RETURN -1;
		END

		INSERT INTO AppObjectInfo 
		(ObjectName,CounterValue,LastModifiedDate,lastModifiedUser)
		VALUES (@objectName,@increment,GETDATE(),'System')
	
		SET @ret = @increment
	END
	ELSE
		UPDATE AppObjectInfo 
		SET CounterValue=@ret
		WHERE AppObjectInfo.ObjectName=@objectName 

    SELECT @ret AS ResultValue;
	
END;

GO
