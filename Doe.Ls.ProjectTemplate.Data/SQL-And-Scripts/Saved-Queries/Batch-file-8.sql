
--GET All Live positions with non live PD/RD

--SELECT * FROM dbo.StatusValue

SELECT * FROM dbo.Position
WHERE StatusId IN (10,20)
AND RolePositionDescriptionId IN (SELECT RolePositionDescId FROM dbo.RolePositionDescription WHERE StatusId NOT IN (10,20))