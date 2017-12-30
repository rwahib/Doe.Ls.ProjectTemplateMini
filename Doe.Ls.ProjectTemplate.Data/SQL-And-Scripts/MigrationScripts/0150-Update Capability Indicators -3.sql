
DECLARE 
	@scriptNumber NVARCHAR(12) = '0150'
,	@scriptName NVARCHAR(256) = '0150-Update Capability Indicators -3 '
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
           (19,5,'<ul>
<li>Ensure that organisational architecture is aligned to the organisation’s goals and responds to changes over time</li>

<li>Engage in strategic workforce planning, and strategic resource utilisation to ensure achievement of both the organisation’s aims and goals and government’s objectives</li>

<li>Align workforce resources and talent with organisational priorities</li>
</ul>',GETDATE(),GETDATE(),'System','System')



INSERT INTO [dbo].[CapabilityBehaviourIndicator]
           ([CapabilityNameId]
           ,[CapabilityLevelId]
           ,[IndicatorContext]
           ,[DateCreated]
           ,[LastUpdated]
           ,[CreatedBy]
           ,[LastModifiedBy])
     VALUES
           (13,5,'<ul>
<li>Apply strategic management of financial and budgetary compliance and governance responsibilities within the organisation</li>

<li>Define organisational directions and set priorities and business plans with reference to key financial indicators</li>

<li>Anticipate operational and capital needs, and identify the most appropriate financing and funding strategies to meet them, through direct provision or purchase of services</li>

<li>Ensure that the organisation informs strategic decisions with appropriate advice from finance professionals</li>

<li>Establish effective governance to ensure the ethical and honest use of financial resources across the organisation</li>

<li>Actively pursue financial risk minimisation strategies, plans and outcomes for the organisation</li>
</ul>',GETDATE(),GETDATE(),'System','System')


INSERT INTO [dbo].[CapabilityBehaviourIndicator]
           ([CapabilityNameId]
           ,[CapabilityLevelId]
           ,[IndicatorContext]
           ,[DateCreated]
           ,[LastUpdated]
           ,[CreatedBy]
           ,[LastModifiedBy])
     VALUES
           (10,5,'<ul>
<li>Establish broad organisational objectives, ensure that these are the focus for all planning activities and communicate to staff</li>

<li>Understand the organisation''''s current and potential future role within government and the community, and plan appropriately</li>

<li>Ensure effective governance frameworks and guidance enable high quality strategic, corporate, business and operational planning</li>

<li>Consider emerging trends, identify long-term opportunities and align organisational requirements with desired whole-of-government outcomes</li>

<li>Drive initiatives in an environment of ongoing, widespread change, including whole-of-government policy directions</li>
</ul>',GETDATE(),GETDATE(),'System','System')



INSERT INTO [dbo].[CapabilityBehaviourIndicator]
           ([CapabilityNameId]
           ,[CapabilityLevelId]
           ,[IndicatorContext]
           ,[DateCreated]
           ,[LastUpdated]
           ,[CreatedBy]
           ,[LastModifiedBy])
     VALUES
           (3,5,'<ul>
<li>Promote and model the value of self-improvement and be proactive in seeking opportunities for growth</li>

<li>Actively seek, reflect and integrate feedback to enhance own performance, showing a strong capacity and willingness to modify own behaviours</li>

<li>Manage challenging, ambiguous and complex issues calmly and logically</li>

<li>Model initiative and decisiveness</li>
</ul>',GETDATE(),GETDATE(),'System','System')



	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
