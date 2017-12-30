CREATE TABLE [dbo].[OrgLevel]
(
[OrgLevelId] [int] NOT NULL,
[OrgLevelTitle] [nvarchar] (240) COLLATE Latin1_General_CI_AS NOT NULL,
[OrgLevelName] [nvarchar] (240) COLLATE Latin1_General_CI_AS NOT NULL,
[Description] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrgLevel] ADD CONSTRAINT [PK_OrgLevel] PRIMARY KEY CLUSTERED  ([OrgLevelId]) ON [PRIMARY]
GO
