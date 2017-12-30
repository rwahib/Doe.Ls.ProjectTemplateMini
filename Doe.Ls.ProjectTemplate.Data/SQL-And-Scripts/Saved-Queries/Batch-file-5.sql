UPDATE dbo.RolePositionDescription
SET DocNumber='DOC17/343314'
WHERE RolePositionDescId IN ( SELECT RolePositionDescriptionId FROM dbo.Position WHERE PositionNumber='198562')