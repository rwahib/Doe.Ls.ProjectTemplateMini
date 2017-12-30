
DECLARE 
	@scriptNumber NVARCHAR(12) = '0140'
,	@scriptName NVARCHAR(256) = '0140-Update Capability Indicators -2 '
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN
INSERT INTO [dbo].[CapabilityBehaviourIndicator]
           ([CapabilityNameId]
           ,[CapabilityLevelId]
           ,[IndicatorContext]
           ,[DateCreated]
           ,[LastUpdated]
           ,[CreatedBy]
           ,[LastModifiedBy])
     VALUES
           (11,3,'<ul>
<li>Undertake objective, critical analysis to draw accurate conclusions that recognise and manage contextual issues</li>

<li>Work through issues, weigh up alternatives and identify the most effective solutions</li>

<li>Take account of the wider business context when considering options to resolve issues</li>

<li>Explore a range of possibilities and creative alternatives to contribute to systems, process and business improvements</li>

<li>Implement systems and processes that underpin high quality research and analysis</li>
</ul>',GETDATE(),GETDATE(),'System','System')




	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
