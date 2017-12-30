--steps
-- 1 Update all role descriptions title for all mapping records that new-doc=null && replace id=null


--UPDATE dbo.RolePositionDescription
--SET Title = (SELECT  MM.Title   FROM TrimMapping AS MM
--WHERE mm.RolePositionDescId=RolePositionDescription.RolePositionDescId AND mm.Processed=0
--)

--WHERE RolePositionDescId IN (
--SELECT        MOuter.RolePositionDescId
--FROM            TrimMapping MOuter
--WHERE        MOuter.ReplaceceID IS NULL AND MOuter.NewDocNumber IS NULL AND MOuter.Processed=0)


--2 Update all positions title as these changes

--UPDATE dbo.Position
--SET dbo.Position.PositionTitle = (SELECT  MM.Title   FROM TrimMapping AS MM
--WHERE mm.RolePositionDescId=Position.RolePositionDescriptionId AND mm.Processed=0
--)

--WHERE Position.RolePositionDescriptionId IN (
--SELECT        MOuter.RolePositionDescId
--FROM            TrimMapping MOuter
--WHERE        MOuter.ReplaceceID IS NULL AND MOuter.NewDocNumber IS NULL AND MOuter.Processed=0)



--3 Update the new doc number 


--UPDATE dbo.RolePositionDescription
--SET DocNumber = (SELECT  MM.NewDocNumber   FROM TrimMapping AS MM
--WHERE mm.RolePositionDescId=RolePositionDescription.RolePositionDescId  AND mm.Processed=0
--)

--WHERE RolePositionDescId IN (
--SELECT        MOuter.RolePositionDescId
--FROM            TrimMapping MOuter
--WHERE        NewDocNumber IS NOT NULL AND  MOuter.Processed=0

--)



--4 update position title

--UPDATE dbo.Position
--SET dbo.Position.PositionTitle = (SELECT  MM.Title   FROM TrimMapping AS MM
--WHERE mm.RolePositionDescId=Position.RolePositionDescriptionId  AND mm.Processed=0
--)

--WHERE Position.RolePositionDescriptionId IN (
--SELECT        MOuter.RolePositionDescId
--FROM            TrimMapping MOuter
--WHERE         MOuter.NewDocNumber IS NULL  AND MOuter.Processed=0)


--5 Replace RolePositionDescriptionId with the new RolePositionDescriptionId in mapping

--UPDATE dbo.Position
--SET dbo.Position.RolePositionDescriptionId = (SELECT  MM.ReplaceceID   FROM TrimMapping AS MM
--WHERE mm.RolePositionDescId=Position.RolePositionDescriptionId  AND mm.Processed=0
--)

--WHERE Position.RolePositionDescriptionId IN (
--SELECT        MOuter.RolePositionDescId
--FROM            TrimMapping MOuter
--WHERE         MOuter.ReplaceceID IS NOT NULL  AND MOuter.Processed=0)



-- 6 Delete all orphen role pos description records

/*
DELETE dbo.KeyRelationship 
WHERE KeyRelationship.RoleDescriptionId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL )





delete dbo.RoleCapability 
WHERE RoleCapability.RoleDescriptionId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL )


delete dbo.PositionFocusCriteria 
WHERE PositionFocusCriteria.PositionDescriptionId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL )



delete dbo.RoleDescription 
WHERE RoleDescription.RoleDescriptionId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL )

delete dbo.PositionDescription 
WHERE PositionDescription.PositionDescriptionId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL
)

delete dbo.RolePositionDescriptionHistory
WHERE RolePositionDescriptionHistory.RolePositionDescId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL
)


delete dbo.RolePositionDescription 
WHERE RolePositionDescription.RolePositionDescId IN ( SELECT MM.RolePositionDescId FROM dbo.TrimMapping AS MM WHERE MM.ReplaceceID IS NOT NULL
)


*/

--7 create another mapping table and check it
/*

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TrimMapping2](
	[DocNumber] [nvarchar](255) NULL,
	[Title] [nvarchar](255) NULL,
	[GradeCode] [nvarchar](255) NULL,
	[RolePositionDescId] [int] NOT NULL,
	[Action] [nvarchar](255) NULL,
	[Example] [nvarchar](255) NULL,
	[NewDocNumber] [nvarchar](255) NULL,
	[ReplaceceID] [int] NULL,
	[Processed] [bit] NULL,
 CONSTRAINT [PK_TrimMapping2] PRIMARY KEY CLUSTERED 
(
	[RolePositionDescId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


INSERT INTO [dbo].[TrimMapping2]
           ([DocNumber]
           ,[Title]
           ,[GradeCode]
           ,[RolePositionDescId]
           ,[Action]
           ,[Example]
           ,[NewDocNumber]
           ,[ReplaceceID]
           ,[Processed])
   SELECT     [DocNumber]
           ,[Title]
           ,[GradeCode]
           ,[RolePositionDescId]
           ,NULL,NULL,NULL,NULL,0
           
FROM            RolePositionDescription
WHERE        (DocNumber IN
                             (SELECT        DocNumber
                               FROM            RolePositionDescription AS RolePositionDescription_1
                               GROUP BY DocNumber
                               HAVING         (COUNT(DocNumber) > 1)))
GO

*/
