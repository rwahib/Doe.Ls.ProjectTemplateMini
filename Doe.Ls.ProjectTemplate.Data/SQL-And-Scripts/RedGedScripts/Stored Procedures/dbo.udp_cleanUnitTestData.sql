
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[udp_cleanUnitTestData]
	-- Add the parameters for the stored procedure here
	@UnitTestToken NVARCHAR(MAX)

AS
BEGIN
	 IF(@UnitTestToken=NULL) BEGIN 
	 RAISERROR(15600,-1,-1,'Parameter must have value')
	 
	 END
		-- Position and all the reletaed records

		DELETE FROM dbo.PositionNote WHERE Notes  LIKE '%'+@UnitTestToken+'%';	
		DELETE FROM dbo.CostCentreDetail WHERE   PositionId IN (SELECT PositionId FROM [dbo].[Position] WHERE [PositionTitle] LIKE '%'+@UnitTestToken+'%' OR Description LIKE '%'+@UnitTestToken+'%');
	 	DELETE FROM dbo.[PositionInformation] WHERE   PositionId IN (SELECT PositionId FROM [dbo].[Position] WHERE [PositionTitle] LIKE '%'+@UnitTestToken+'%' OR Description LIKE '%'+@UnitTestToken+'%');
	 	DELETE FROM dbo.PositionHistory WHERE   PositionId IN (SELECT PositionId FROM [dbo].[Position] WHERE [PositionTitle] LIKE '%'+@UnitTestToken+'%' OR Description LIKE '%'+@UnitTestToken+'%');
		
		DELETE dbo.Position WHERE PositionTitle  LIKE '%'+@UnitTestToken+'%' OR Description LIKE '%'+@UnitTestToken+'%';	 
		
		-- Position and all the reletaed records

		DELETE FROM dbo.[RoleDescription] WHERE   RoleDescriptionId IN (SELECT RolePositionDescId FROM  [dbo].[RolePositionDescription] WHERE [Title] LIKE '%'+@UnitTestToken+'%');
		
		DELETE FROM dbo.TrimRecord WHERE   dbo.TrimRecord.RolePositionDescId IN (SELECT RolePositionDescId FROM  [dbo].[RolePositionDescription] WHERE [Title] LIKE '%'+@UnitTestToken+'%');

		DELETE [dbo].[RoleDescription] WHERE Agency LIKE '%'+@UnitTestToken+'%';
		DELETE [dbo].[RoleDescription] WHERE Agency LIKE '%'+@UnitTestToken+'%';
		DELETE [dbo].CapabilityBehaviourIndicator WHERE IndicatorContext LIKE '%'+@UnitTestToken+'%';
		DELETE dbo.CapabilityName WHERE Name  LIKE '%'+@UnitTestToken+'%';
		DELETE dbo.CapabilityGroup WHERE GroupName  LIKE '%'+@UnitTestToken+'%';
		DELETE dbo.CapabilityLevel WHERE LevelName  LIKE '%'+@UnitTestToken+'%';

		DELETE FROM dbo.[PositionDescription] WHERE   PositionDescriptionId IN (SELECT RolePositionDescId FROM  [dbo].[RolePositionDescription] WHERE [Title] LIKE '%'+@UnitTestToken+'%');
		DELETE [dbo].[PositionDescription] WHERE BriefRoleStatement LIKE '%'+@UnitTestToken+'%';

		DELETE [dbo].RolePositionDescriptionHistory WHERE CreatedBy LIKE '%'+@UnitTestToken+'%';

		DELETE [dbo].[RolePositionDescription] WHERE [Title] LIKE '%'+@UnitTestToken+'%';
		DELETE dbo.LookupFocusGradeCriteria WHERE LastUpdatedBy LIKE '%'+@UnitTestToken+'%';
		DELETE dbo.PositionFocusCriteria WHERE LastModifiedBy LIKE '%'+@UnitTestToken+'%';
	

		DELETE dbo.Focus WHERE FocusName LIKE '%'+@UnitTestToken+'%';
			DELETE dbo.SelectionCriteria WHERE Criteria LIKE '%'+@UnitTestToken+'%';
		DELETE [dbo].KeyRelationship WHERE Who LIKE '%'+@UnitTestToken+'%';

		DELETE [dbo].[Grade] WHERE [GradeTitle] LIKE '%'+@UnitTestToken+'%';
	   DELETE [AppEntityType] WHERE [EntityTitle] LIKE '%'+@UnitTestToken+'%';

            DELETE [SysMessage] WHERE MessageFormat LIKE '%'+@UnitTestToken+'%';

			DELETE [Location] WHERE [Name] LIKE '%'+@UnitTestToken+'%' OR CreatedBy LIKE '%'+@UnitTestToken+'%' ;
            DELETE [Directorate] WHERE [DirectorateName] LIKE '%'+@UnitTestToken+'%';

			
			
            DELETE dbo.SysUserRole WHERE dbo.SysUserRole.UserId IN 
			(SELECT UserId FROM dbo.SysUser
			WHERE UserId
			 LIKE '%'+@UnitTestToken+'%');

			  
			DELETE dbo.SysUser
			WHERE UserId
			 LIKE '%'+@UnitTestToken+'%';


END

GO
