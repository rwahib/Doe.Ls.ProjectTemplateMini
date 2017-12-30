
DECLARE 
	@scriptNumber NVARCHAR(12) = '0130'
,	@scriptName NVARCHAR(256) = '0130 0130-Update Capability Indicators'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN


/****** Object:  Table [dbo].[SysMsgCatery]    Script Date: 28/06/2017 10:05:57 AM ******/
/****** Script for SelectTopNRows command from SSMS  ******/





UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font color=black>','')

UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'</font>','')


UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font size=2 color=black>','')



UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font face="Times New Roman" size=3 color=black>','')

UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'face="Times New Roman" size=3 color=black','')



UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font'+CHAR(13)+CHAR(10)+'>','')



UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font'+CHAR(10)+CHAR(13)+'>','')






UPDATE [CapabilityBehaviourIndicator]
SET IndicatorContext=REPLACE(IndicatorContext,'<font
     >','')


SELECT TOP 1000 [CapabilityNameId]
      ,[CapabilityLevelId]
      ,[IndicatorContext]
      ,[DateCreated]
      ,[LastUpdated]
      ,[CreatedBy]
      ,[LastModifiedBy]
  FROM [CapabilityBehaviourIndicator]
  




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
