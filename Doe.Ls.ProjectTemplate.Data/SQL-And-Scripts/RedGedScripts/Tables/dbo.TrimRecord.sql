CREATE TABLE [dbo].[TrimRecord]
(
[RolePositionDescId] [int] NOT NULL,
[Uri] [bigint] NULL,
[Token] [nvarchar] (64) COLLATE Latin1_General_CI_AS NULL,
[LastPublishedDate] [datetime] NULL CONSTRAINT [DF_TrimRecord_LastModifiedDate] DEFAULT (getdate()),
[LastRevisionNumber] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TrimRecord] ADD CONSTRAINT [PK_TrimRecord] PRIMARY KEY CLUSTERED  ([RolePositionDescId]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TrimRecord] ON [dbo].[TrimRecord] ([Uri]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TrimRecord] ADD CONSTRAINT [FK_TrimRecord_RolePositionDescription] FOREIGN KEY ([RolePositionDescId]) REFERENCES [dbo].[RolePositionDescription] ([RolePositionDescId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Used as foreign key in Position. Also this key exists in either RoleDescription or PositionDescription', 'SCHEMA', N'dbo', 'TABLE', N'TrimRecord', 'COLUMN', N'RolePositionDescId'
GO
