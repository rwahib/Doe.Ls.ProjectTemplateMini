CREATE TABLE [dbo].[HierarchyLevel]
(
[HierarchyId] [int] NOT NULL,
[HierarchyName] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[HierarchyDescription] [nvarchar] (240) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HierarchyLevel] ADD CONSTRAINT [PK_HierarchyLevel] PRIMARY KEY CLUSTERED  ([HierarchyId]) ON [PRIMARY]
GO
