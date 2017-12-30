
DECLARE 
	@scriptNumber NVARCHAR(12) = '0100'
,	@scriptName NVARCHAR(256) = '0100-Update NA with TBC'
,	@developer NVARCHAR(256) = 'Refky Wahib'



IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN

UPDATE dbo.BusinessUnit
SET BUnitName='TBC', BUnitDescription='TBC'
WHERE BUnitId=-1



UPDATE dbo.CapabilityGroup
SET GroupName='TBC', GroupDescription='TBC'
WHERE CapabilityGroupId=-1



UPDATE dbo.Directorate
SET DirectorateName='TBC', DirectorateDescription='TBC'
WHERE DirectorateId=-1


UPDATE dbo.Executive
SET ExecutiveTitle='TBC', ExecutiveDescription='TBC'
WHERE ExecutiveCod='-1'


UPDATE dbo.FunctionalArea
SET AreanName='TBC', AreaDescription='TBC'
WHERE FuncationalAreaId='-1'



UPDATE dbo.Unit
SET UnitName='TBC', UnitDescription='TBC'
WHERE UnitId='-1'


UPDATE dbo.EmployeeType
SET EmployeeTypeName='TBC', EmployeeTypeDescription='TBC'
WHERE EmployeeTypeCode='-1'

UPDATE dbo.Focus
SET FocusName='TBC'
WHERE FocusId=-1

UPDATE dbo.Grade
SET GradeTitle='TBC',GradeType='TBC'
WHERE GradeCode='-1'


UPDATE dbo.Location
SET Name='TBC'
WHERE LocationId=-1


UPDATE dbo.OccupationType
SET OccupationTypeName='TBC'
WHERE OccupationTypeCode='-1'


UPDATE dbo.Position
SET PositionTitle='TBC',Description='TBC'
WHERE PositionId=-1


UPDATE dbo.PositionLevel
SET PositionLevelName='TBC',CustomClass='TBC'
WHERE PositionLevelId=-1


UPDATE dbo.PositionType
SET PositionTypeName='TBC',PositionTypeDescription='TBC'
WHERE PositionTypeCode='-1'


UPDATE dbo.PositionStatusValue
SET PosStatusTitle='TBC'
WHERE PosStatusCode='-1'


UPDATE dbo.RolePositionDescription
SET Title='TBC'
WHERE RolePositionDescId=-1



	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
