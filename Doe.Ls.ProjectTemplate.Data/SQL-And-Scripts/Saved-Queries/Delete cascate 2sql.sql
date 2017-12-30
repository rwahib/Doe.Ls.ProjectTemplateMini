


DELETE FROM dbo.RoleCapability WHERE   dbo.RoleCapability.RoleDescriptionId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));

DELETE FROM dbo.PositionFocusCriteria WHERE   dbo.PositionFocusCriteria.PositionDescriptionId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));

DELETE FROM dbo.KeyRelationship WHERE   dbo.KeyRelationship.RoleDescriptionId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));


DELETE FROM dbo.RoleDescription WHERE   dbo.RoleDescription.RoleDescriptionId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));
DELETE FROM dbo.PositionDescription WHERE   dbo.PositionDescription.PositionDescriptionId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));


DELETE FROM dbo.RolePositionDescriptionHistory WHERE   dbo.RolePositionDescriptionHistory.RolePositionDescId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));

DELETE FROM dbo.TrimRecord WHERE   dbo.TrimRecord.RolePositionDescId IN (select RD.RolePositionDescId from RolePositionDescription as RD where RD.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS));

delete RolePositionDescription where RolePositionDescription.RolePositionDescId not in ( select POS.RolePositionDescriptionId from Position as POS)
