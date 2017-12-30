--  step 1 Import xImport sheet
-- clear bad data

delete
  FROM [PositionDescription].[dbo].[xImport]
  WHERE Status <> 'current' OR PositionNumber IS null

---


UPDATE [PositionDescription].[dbo].[xImport]
SET cc =SUBSTRING(cc,5,LEN(cc)-3)


-- add  id colum to be the primary key



-- Adding position id to imported records
UPDATE dbo.xImport SET
Id = (SELECT  POS.PositionId FROM dbo.Position AS POS WHERE POS.PositionNumber=dbo.xImport.PositionNumber AND pos.StatusId!=-1)
WHERE EXISTS
(SELECT * FROM dbo.Position WHERE PositionNumber=dbo.xImport.PositionNumber AND Position.StatusId!=-1)


  --UPDATE FTE
  UPDATE dbo.PositionInformation
  SET PositionFTE =  (SELECT  TEMP.FTE FROM dbo.xImport AS TEMP WHERE temp.Id=dbo.PositionInformation.PositionId)

  WHERE EXISTS
  (SELECT * FROM dbo.xImport WHERE dbo.xImport.id = PositionInformation.PositionId)



--CostCentreDetail
  UPDATE dbo.CostCentreDetail
  SET CostCentre =  (SELECT  CAST( CONVERT(BIGINT, CONVERT(DOUBLE PRECISION, TEMP.CC)) AS NVARCHAR(120)) FROM dbo.xImport AS TEMP WHERE temp.Id=dbo.CostCentreDetail.PositionId),
   Fund =  (SELECT  TEMP.Fund FROM dbo.xImport AS TEMP WHERE temp.Id=dbo.CostCentreDetail.PositionId),
   PayrollWBS=  (SELECT  TEMP.WBS FROM dbo.xImport AS TEMP WHERE temp.Id=dbo.CostCentreDetail.PositionId),
   RCCJDEPayrollCode=(SELECT  TEMP.RCC FROM dbo.xImport AS TEMP WHERE temp.Id=dbo.CostCentreDetail.PositionId)

  WHERE EXISTS
  (SELECT * FROM dbo.xImport WHERE dbo.xImport.id = CostCentreDetail.PositionId)





  






/*  Just to generate reports for him


  SELECT        Executive.ExecutiveCod, Executive.ExecutiveTitle, Directorate.DirectorateName, Directorate.DirectorateId, BusinessUnit.BUnitId, BusinessUnit.BUnitName, 
                         Unit.UnitId, Unit.UnitName, Position.PositionNumber, Position.PositionTitle, CostCentreDetail.CostCentre, CostCentreDetail.Fund, CostCentreDetail.PayrollWBS, 
                         CostCentreDetail.RCCJDEPayrollCode, CostCentreDetail.GLAccount, xtmp_establishment_data.PositionNumber AS Sheet_PositionNumber, 
                         xtmp_establishment_data.Status AS Sheet_Status, xtmp_establishment_data.Division AS Sheet_Division, 
                         xtmp_establishment_data.PositionTitle AS Sheet_PositionTitle, xtmp_establishment_data.Grade AS Sheet_Grade, xtmp_establishment_data.FTE AS Sheet_FTE, 
                         xtmp_establishment_data.CostCentre AS Sheet_Cost_center, xtmp_establishment_data.Fund AS Sheet_Fund, xtmp_establishment_data.WBS AS Sheet_WBS
FROM            CostCentreDetail INNER JOIN
                         Position ON CostCentreDetail.PositionId = Position.PositionId INNER JOIN
                         Unit ON Position.UnitId = Unit.UnitId INNER JOIN
                         BusinessUnit ON Unit.BUnitId = BusinessUnit.BUnitId INNER JOIN
                         Directorate ON BusinessUnit.DirectorateId = Directorate.DirectorateId INNER JOIN
                         Executive ON Directorate.ExecutiveCod = Executive.ExecutiveCod LEFT OUTER JOIN
                         xtmp_establishment_data ON Position.PositionNumber = xtmp_establishment_data.PositionNumber
WHERE        (Position.StatusId IN (10, 20))
ORDER BY Executive.ExecutiveCod, Directorate.DirectorateId, BusinessUnit.BUnitId, Unit.UnitId, Sheet_Grade

SELECT      COUNT(*) AS Total FROM           
                         Position 
                         
WHERE  (Position.StatusId IN (10, 20)) 


SELECT      COUNT(*) AS HasValues
FROM            CostCentreDetail INNER JOIN
                         Position ON CostCentreDetail.PositionId = Position.PositionId LEFT OUTER JOIN
                         xtmp_establishment_data ON Position.PositionNumber = xtmp_establishment_data.PositionNumber
WHERE  (Position.StatusId IN (10, 20)) AND  xtmp_establishment_data.Division IS NOT NULL

SELECT      COUNT(*) AS HasNoValues
FROM            CostCentreDetail INNER JOIN
                         Position ON CostCentreDetail.PositionId = Position.PositionId LEFT OUTER JOIN
                         xtmp_establishment_data ON Position.PositionNumber = xtmp_establishment_data.PositionNumber
WHERE (Position.StatusId IN (10, 20)) AND   xtmp_establishment_data.Division IS  NULL


*/