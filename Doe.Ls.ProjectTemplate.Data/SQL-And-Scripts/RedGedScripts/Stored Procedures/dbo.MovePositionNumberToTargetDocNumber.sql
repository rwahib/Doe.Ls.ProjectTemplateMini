SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE	PROC [dbo].[MovePositionNumberToTargetDocNumber]
@sourcePositionNumber NVARCHAR(10),
@targetDocNumber NVARCHAR(150)
AS
BEGIN


PRINT 'Source POSITIONS'
SELECT * FROM dbo.Position
WHERE PositionNumber =@sourcePositionNumber


PRINT 'TARGET DOC NUMBER'
SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber=@targetDocNumber;

PRINT 'TARGET POSITIONS'
SELECT * FROM dbo.Position
where RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber=@targetDocNumber);




UPDATE dbo.Position
SET RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription WHERE DocNumber=@targetDocNumber)
WHERE PositionNumber =@sourcePositionNumber



PRINT 'Source POSITIONS'
SELECT * FROM dbo.Position
WHERE PositionNumber =@sourcePositionNumber


PRINT 'TARGET DOC NUMBER'
SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber=@targetDocNumber;

PRINT 'TARGET POSITIONS'
SELECT * FROM dbo.Position
where RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber=@targetDocNumber);


END

GO
