CREATE TABLE [dbo].[FunctionalArea]
(
[FuncationalAreaId] [int] NOT NULL,
[AreanName] [nvarchar] (512) COLLATE Latin1_General_CI_AS NOT NULL,
[AreaDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DirectorateId] [int] NOT NULL,
[AreaCustomClass] [nvarchar] (123) COLLATE Latin1_General_CI_AS NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_FunctionalArea_StatusId] DEFAULT ((10)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_FunctionalArea_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_FunctionalArea_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_FunctionalArea_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_FunctionalArea_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[FunctionalArea] WITH NOCHECK ADD
CONSTRAINT [FK_FunctionalArea_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_FunctionalArea_Directorate] ON [dbo].[FunctionalArea] ([DirectorateId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_FunctionalArea_StatusValue] ON [dbo].[FunctionalArea] ([StatusId]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[FunctionalArea] ADD CONSTRAINT [PK_FunctionalArea] PRIMARY KEY CLUSTERED  ([FuncationalAreaId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FunctionalArea] WITH NOCHECK ADD CONSTRAINT [FK_FunctionalArea_Directorate] FOREIGN KEY ([DirectorateId]) REFERENCES [dbo].[Directorate] ([DirectorateId])
GO

EXEC sp_addextendedproperty N'MS_Description', N'Function Area lookup', 'SCHEMA', N'dbo', 'TABLE', N'FunctionalArea', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Colour for Chart', 'SCHEMA', N'dbo', 'TABLE', N'FunctionalArea', 'COLUMN', N'AreaCustomClass'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Directorate', 'SCHEMA', N'dbo', 'TABLE', N'FunctionalArea', 'COLUMN', N'DirectorateId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to StatusValue', 'SCHEMA', N'dbo', 'TABLE', N'FunctionalArea', 'COLUMN', N'StatusId'
GO
