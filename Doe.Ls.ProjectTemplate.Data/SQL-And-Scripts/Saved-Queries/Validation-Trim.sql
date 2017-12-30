



 -- SELECT DocNumber
 --FROM dbo.RolePositionDescription
 --GROUP BY DocNumber,Title,GradeCode
 --having
 -- COUNT(*)>1



  SELECT *
 FROM dbo.RolePositionDescription
 WHERE DocNumber IN ( SELECT DocNumber
 FROM dbo.RolePositionDescription
 GROUP BY DocNumber,Title,GradeCode
 having
  COUNT(*)>1)

  ORDER BY DocNumber

  SELECT DocNumber,Title,GradeCode,COUNT(*)
 FROM dbo.RolePositionDescription
 GROUP BY DocNumber,Title,GradeCode
 having
  COUNT(*)>1