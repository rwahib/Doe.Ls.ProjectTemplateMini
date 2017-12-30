CREATE TABLE [dbo].[Unit]
(
[UnitId] [int] NOT NULL,
[UnitName] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[UnitDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[BUnitId] [int] NOT NULL,
[FunctionalAreaId] [int] NOT NULL,
[ReportToUnit] [int] NOT NULL CONSTRAINT [DF_Unit_ReportToUnit] DEFAULT ((-1)),
[HierarchyId] [int] NOT NULL,
[TeamTypeId] [int] NOT NULL,
[UnitCustomClass] [nvarchar] (123) COLLATE Latin1_General_CI_AS NULL,
[StatusId] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Unit_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Unit_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Unit_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Unit_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[Unit] WITH NOCHECK ADD
CONSTRAINT [FK_Unit_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_Unit_Unit] ON [dbo].[Unit] ([ReportToUnit]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Unit_BusinessUnit] ON [dbo].[Unit] ([BUnitId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Unit_FunctionalArea] ON [dbo].[Unit] ([FunctionalAreaId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Unit_HierarchyLevel] ON [dbo].[Unit] ([HierarchyId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Unit_StatusValue] ON [dbo].[Unit] ([StatusId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Unit_TeamType] ON [dbo].[Unit] ([TeamTypeId]) ON [PRIMARY]

ALTER TABLE [dbo].[Unit] ADD 
CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED  ([UnitId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'It is the Team

we left it unit for internal effort only
', 'SCHEMA', N'dbo', 'TABLE', N'Unit', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Functional Area', 'SCHEMA', N'dbo', 'TABLE', N'Unit', 'COLUMN', N'FunctionalAreaId'
GO

ALTER TABLE [dbo].[Unit] ADD
CONSTRAINT [FK_Unit_BusinessUnit] FOREIGN KEY ([BUnitId]) REFERENCES [dbo].[BusinessUnit] ([BUnitId])
ALTER TABLE [dbo].[Unit] ADD
CONSTRAINT [FK_Unit_HierarchyLevel] FOREIGN KEY ([HierarchyId]) REFERENCES [dbo].[HierarchyLevel] ([HierarchyId])
ALTER TABLE [dbo].[Unit] ADD
CONSTRAINT [FK_Unit_TeamType] FOREIGN KEY ([TeamTypeId]) REFERENCES [dbo].[TeamType] ([TeamTypeId])
ALTER TABLE [dbo].[Unit] ADD
CONSTRAINT [FK_Unit_Unit] FOREIGN KEY ([ReportToUnit]) REFERENCES [dbo].[Unit] ([UnitId])
GO

ALTER TABLE [dbo].[Unit] WITH NOCHECK ADD CONSTRAINT [FK_Unit_FunctionalArea] FOREIGN KEY ([FunctionalAreaId]) REFERENCES [dbo].[FunctionalArea] ([FuncationalAreaId])
GO
