



delete PositionNote  where PositionNote  .PositionId in (select positionid from position  where position  .UnitId in (select UnitId from Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )))



delete PositionHistory  where PositionHistory  .PositionId in (select positionid from position  where position  .UnitId in (select UnitId from Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )))



delete CostCentreDetail  where CostCentreDetail  .PositionId in (select positionid from position  where position  .UnitId in (select UnitId from Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )))


delete PositionInformation  where PositionInformation  .PositionId in (select positionid from position  where position  .UnitId in (select UnitId from Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )))



delete position  where position  .UnitId in (select UnitId from Unit
where Unit.BUnitId in (select BUnitId from BusinessUnit  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') ))


delete position  where position  .UnitId in (select UnitId from Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') ))


delete Unit
where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )

delete Unit
where Unit.BUnitId in (select BUnitId from BusinessUnit  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )


delete FunctionalArea
where FunctionalArea.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')


delete Location
where Location.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')

delete BusinessUnit
where BusinessUnit.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')


delete Directorate
where  Directorate.ExecutiveCod='ES'

delete Executive
where ExecutiveCod='ES'



--select * from Executive
--where ExecutiveCod='ES'


--select * from position  where position  .UnitId in (select UnitId from Unit
--where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') ))

--select * from Unit
--where Unit.FunctionalAreaId in (select FuncationalAreaId from FunctionalArea  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )

--select * from Unit
--where Unit.BUnitId in (select BUnitId from BusinessUnit  where DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES') )


--select * from FunctionalArea
--where FunctionalArea.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')


--select * from Location
--where Location.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')

--select * from BusinessUnit
--where BusinessUnit.DirectorateId in (  select DirectorateId from Directorate where  Directorate.ExecutiveCod='ES')


--select * from Directorate
--where  Directorate.ExecutiveCod='ES'

--select * from Executive
--where ExecutiveCod='ES'

