CREATE TABLE [dbo].[PositionNote]
(
[PositionNoteId] [int] NOT NULL IDENTITY(1, 1),
[PositionId] [int] NOT NULL,
[Notes] [nvarchar] (max) COLLATE Latin1_General_CI_AS NOT NULL,
[CreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_PositionNote_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_PositionNote_LastModifiedBy] DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_PositionNote_PositionInformation] ON [dbo].[PositionNote] ([PositionId]) ON [PRIMARY]

ALTER TABLE [dbo].[PositionNote] ADD 
CONSTRAINT [PK_PositionNotes] PRIMARY KEY CLUSTERED  ([PositionNoteId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Only hold Position Notes with hisotry', 'SCHEMA', N'dbo', 'TABLE', N'PositionNote', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to position', 'SCHEMA', N'dbo', 'TABLE', N'PositionNote', 'COLUMN', N'PositionId'
GO

ALTER TABLE [dbo].[PositionNote] WITH NOCHECK ADD CONSTRAINT [FK_PositionNote_PositionInformation] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[PositionInformation] ([PositionId])
GO
