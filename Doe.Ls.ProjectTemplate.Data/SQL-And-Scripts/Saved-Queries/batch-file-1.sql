SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber='DOC17/721658'

SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/196845'


SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC17/721658'


SELECT * FROM dbo.Position
WHERE PositionNumber IN ('192482','192483')

AND RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC17/721658')


SELECT * FROM dbo.Position
WHERE PositionNumber NOT IN ('192482','192483')

AND RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC17/721658')

SELECT * FROM dbo.Position
WHERE RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/196845')





UPDATE dbo.Position
SET RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription WHERE DocNumber='DOC16/196845')

WHERE PositionNumber NOT IN ('192482','192483')
AND RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC17/721658')


-- Second script
UPDATE dbo.Position
SET PositionTitle=(SELECT Title FROM dbo.RolePositionDescription WHERE DocNumber='DOC16/196845')

WHERE RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/196845')