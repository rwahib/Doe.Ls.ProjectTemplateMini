--Validating Div=>DIR=>FA=>

INSERT INTO dbo.PositionDescription
        ( PositionDescriptionId       
        )
SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE IsPositionDescription=1 and
RolePositionDescId NOT IN (SELECT PositionDescriptionId FROM dbo.PositionDescription)


SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE IsPositionDescription=1 and
RolePositionDescId NOT IN (SELECT PositionDescriptionId FROM dbo.PositionDescription)

-----------------



SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE IsPositionDescription=0 and
RolePositionDescId NOT IN (SELECT RoleDescriptionId FROM dbo.RoleDescription)


INSERT INTO dbo.RoleDescription
            ( RoleDescriptionId               )
SELECT RolePositionDescId FROM dbo.RolePositionDescription
WHERE IsPositionDescription=0 and
RolePositionDescId NOT IN (SELECT RoleDescriptionId FROM dbo.RoleDescription)
