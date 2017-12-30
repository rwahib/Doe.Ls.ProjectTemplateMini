
UPDATE dbo.Position
SET PositionTitle=(SELECT RPD.Title FROM dbo.RolePositionDescription RPD WHERE Position.RolePositionDescriptionId=RPD.RolePositionDescId)

SELECT        Position.PositionId, Position.ReportToPositionId, Position.PositionNumber, Position.RolePositionDescriptionId, Position.UnitId, Position.PositionTitle, 
                         Position.Description, RolePositionDescription.DocNumber, RolePositionDescription.Title, RolePositionDescription.GradeCode
FROM            Position INNER JOIN
                         RolePositionDescription ON Position.RolePositionDescriptionId = RolePositionDescription.RolePositionDescId
						 WHERE LTRIM(RTRIM(PositionTitle)) <> LTRIM(RTRIM(Title))


						 
						 