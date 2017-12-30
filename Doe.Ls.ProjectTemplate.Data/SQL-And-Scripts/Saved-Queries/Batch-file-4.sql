
SELECT * FROM dbo.RolePositionDescription
WHERE RolePositionDescId=305


SELECT * FROM dbo.Position
WHERE RolePositionDescriptionId=305


UPDATE dbo.Position
SET PositionTitle = (SELECT Title FROM dbo.RolePositionDescription WHERE RolePositionDescId=305)
WHERE RolePositionDescriptionId=305
