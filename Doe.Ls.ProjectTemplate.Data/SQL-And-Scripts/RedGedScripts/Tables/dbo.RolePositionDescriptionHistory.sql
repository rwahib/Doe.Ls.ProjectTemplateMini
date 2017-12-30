CREATE TABLE [dbo].[RolePositionDescriptionHistory]
(
[RolePositionDescriptionHistoryId] [int] NOT NULL IDENTITY(1, 1),
[RolePositionDescId] [int] NOT NULL,
[Action] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
[StatusFrom] [nvarchar] (24) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RolePositionDescriptionHistory_StatusFrom] DEFAULT (getdate()),
[StatusTo] [nvarchar] (24) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RolePositionDescriptionHistory_StatusTo] DEFAULT (getdate()),
[AdditionalInfo] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_RolePositionDescriptionHistory_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RolePositionDescriptionHistory_CreatedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[RolePositionDescriptionHistory] WITH NOCHECK ADD
CONSTRAINT [FK_RolePositionDescriptionHistory_RolePositionDescription] FOREIGN KEY ([RolePositionDescId]) REFERENCES [dbo].[RolePositionDescription] ([RolePositionDescId])
GO
ALTER TABLE [dbo].[RolePositionDescriptionHistory] ADD CONSTRAINT [PK_RolePositionDescriptionHistory] PRIMARY KEY CLUSTERED  ([RolePositionDescriptionHistoryId]) ON [PRIMARY]
GO

EXEC sp_addextendedproperty N'MS_Description', N'Used as foreign key in Position. Also this key exists in either RoleDescription or PositionDescription', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescriptionHistory', 'COLUMN', N'RolePositionDescId'
GO
