SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072733'

SELECT * FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072723'


SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072733'


SELECT * FROM dbo.Position
where RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072733')



SELECT * FROM dbo.Position
WHERE RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072723')





UPDATE dbo.Position
SET RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription WHERE DocNumber='DOC16/1072723')

WHERE RolePositionDescriptionId=(SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE DocNumber='DOC16/1072733')


UPDATE dbo.RolePositionDescription
SET StatusId=-1
WHERE DocNumber='DOC16/1072733'
