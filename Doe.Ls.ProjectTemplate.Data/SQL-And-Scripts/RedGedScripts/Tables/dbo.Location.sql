CREATE TABLE [dbo].[Location]
(
[LocationId] [int] NOT NULL,
[Name] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[DirectorateId] [int] NOT NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Location_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Location_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Location_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Location_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Location_Directorate] ON [dbo].[Location] ([DirectorateId]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Location] ADD CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED  ([LocationId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Location] WITH NOCHECK ADD CONSTRAINT [FK_Location_Directorate] FOREIGN KEY ([DirectorateId]) REFERENCES [dbo].[Directorate] ([DirectorateId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Position location lookup', 'SCHEMA', N'dbo', 'TABLE', N'Location', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key for directorate', 'SCHEMA', N'dbo', 'TABLE', N'Location', 'COLUMN', N'DirectorateId'
GO
