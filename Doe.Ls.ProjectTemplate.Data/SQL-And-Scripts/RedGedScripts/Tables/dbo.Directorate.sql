CREATE TABLE [dbo].[Directorate]
(
[DirectorateId] [int] NOT NULL,
[ExecutiveCod] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[DirectorateName] [nvarchar] (512) COLLATE Latin1_General_CI_AS NOT NULL,
[DirectorateDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DirectorateOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DirectorateCustomClass] [nvarchar] (123) COLLATE Latin1_General_CI_AS NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_Directorate_Deleted] DEFAULT ((10)),
[DirectorateOrder] [int] NOT NULL CONSTRAINT [DF_Directorate_DirectorateOrder] DEFAULT ((0)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Directorate_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Directorate_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Directorate_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Directorate_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[Directorate] WITH NOCHECK ADD
CONSTRAINT [FK_Directorate_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_Directorate_StatusValue] ON [dbo].[Directorate] ([StatusId]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Directorate] ADD CONSTRAINT [PK_Directorate] PRIMARY KEY CLUSTERED  ([DirectorateId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Directorate] ON [dbo].[Directorate] ([ExecutiveCod]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Directorate] ADD CONSTRAINT [FK_Directorate_Executive] FOREIGN KEY ([ExecutiveCod]) REFERENCES [dbo].[Executive] ([ExecutiveCod])
GO

EXEC sp_addextendedproperty N'MS_Description', N'Directorate lookup', 'SCHEMA', N'dbo', 'TABLE', N'Directorate', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Used in RoleDescription of a position', 'SCHEMA', N'dbo', 'TABLE', N'Directorate', 'COLUMN', N'DirectorateOverview'
GO
