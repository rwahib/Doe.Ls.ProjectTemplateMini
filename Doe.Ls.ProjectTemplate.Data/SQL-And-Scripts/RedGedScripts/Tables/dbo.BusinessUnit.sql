CREATE TABLE [dbo].[BusinessUnit]
(
[BUnitId] [int] NOT NULL,
[DirectorateId] [int] NOT NULL,
[HierarchyId] [int] NOT NULL,
[BUnitName] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[BUnitDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[StatusId] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_BusinessUnit_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_BusinessUnit_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BusinessUnit_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_BusinessUnit_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[BusinessUnit] WITH NOCHECK ADD
CONSTRAINT [FK_BusinessUnit_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_BusinessUnit_Directorate] ON [dbo].[BusinessUnit] ([DirectorateId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_BusinessUnit_HierarchyLevel] ON [dbo].[BusinessUnit] ([HierarchyId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_BusinessUnit_StatusValue] ON [dbo].[BusinessUnit] ([StatusId]) ON [PRIMARY]


GO
ALTER TABLE [dbo].[BusinessUnit] ADD CONSTRAINT [PK_BusinessUnit] PRIMARY KEY CLUSTERED  ([BUnitId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessUnit] ADD CONSTRAINT [FK_BusinessUnit_Directorate] FOREIGN KEY ([DirectorateId]) REFERENCES [dbo].[Directorate] ([DirectorateId])
GO
ALTER TABLE [dbo].[BusinessUnit] ADD CONSTRAINT [FK_BusinessUnit_HierarchyLevel] FOREIGN KEY ([HierarchyId]) REFERENCES [dbo].[HierarchyLevel] ([HierarchyId])
GO
