SELECT        Position.PositionNumber, Position.ReportToPositionId, Position.PositionTitle, Position.StatusId, RolePositionDescription.Title, RolePositionDescription.DocNumber, 
                         RolePositionDescription.GradeCode
FROM            Position INNER JOIN
                         RolePositionDescription ON Position.RolePositionDescriptionId = RolePositionDescription.RolePositionDescId
WHERE DocNumber IN  (


  SELECT DocNumber
 FROM dbo.RolePositionDescription
 GROUP BY DocNumber
 having
  COUNT(*)>1)

  ORDER BY DocNumber




SELECT        DocNumber, Title, GradeCode, RolePositionDescId, StatusId, Version, DateOfApproval, IsPositionDescription, CreatedDate, LastModifiedDate, CreatedBy

FROM dbo.RolePositionDescription
WHERE DocNumber IN  (

  SELECT DocNumber
 FROM dbo.RolePositionDescription
 GROUP BY DocNumber
 having
  COUNT(*)>1)