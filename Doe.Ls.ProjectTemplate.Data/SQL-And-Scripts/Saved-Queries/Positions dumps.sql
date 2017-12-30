SELECT        RolePositionDescription.RolePositionDescId, RolePositionDescription.DocNumber, RolePositionDescription.Title, RolePositionDescription.GradeCode, 
                         Position.PositionId, Position.PositionNumber, Position.PositionTitle, RolePositionDescription.DateOfApproval, RolePositionDescription.IsPositionDescription, 
                         RolePositionDescription.CreatedDate, RolePositionDescription.LastModifiedDate, RolePositionDescription.CreatedBy, RolePositionDescription.LastModifiedBy, 
                         Unit.UnitId, Unit.UnitName, BusinessUnit.BUnitName, Directorate.DirectorateId, Directorate.DirectorateName, Executive.ExecutiveCod, 
                         Executive.ExecutiveTitle
FROM            Position INNER JOIN
                         RolePositionDescription ON Position.RolePositionDescriptionId = RolePositionDescription.RolePositionDescId INNER JOIN
                         Unit ON Position.UnitId = Unit.UnitId INNER JOIN
                         BusinessUnit ON Unit.BUnitId = BusinessUnit.BUnitId INNER JOIN
                         Directorate ON BusinessUnit.DirectorateId = Directorate.DirectorateId INNER JOIN
                         Executive ON Directorate.ExecutiveCod = Executive.ExecutiveCod
ORDER BY RolePositionDescription.DocNumber,Directorate.ExecutiveCod,BusinessUnit.DirectorateId,BusinessUnit.BUnitId,Position.UnitId