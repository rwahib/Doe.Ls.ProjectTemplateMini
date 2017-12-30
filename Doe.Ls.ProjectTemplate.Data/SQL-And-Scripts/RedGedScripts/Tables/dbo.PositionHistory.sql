CREATE TABLE [dbo].[PositionHistory]
(
[PositionHistoryId] [int] NOT NULL IDENTITY(1, 1),
[PositionId] [int] NOT NULL,
[Action] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
[StatusFrom] [nvarchar] (24) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_PositionHistory_CreatedDate2] DEFAULT (getdate()),
[StatusTo] [nvarchar] (24) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_PositionHistory_StatusFrom1] DEFAULT (getdate()),
[AdditionalInfo] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_PositionHistory_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_PositionHistory_CreatedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[PositionHistory] WITH NOCHECK ADD
CONSTRAINT [FK_PositionHistory_Position] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position] ([PositionId])
GO
ALTER TABLE [dbo].[PositionHistory] ADD CONSTRAINT [PK_PositionHistory] PRIMARY KEY CLUSTERED  ([PositionHistoryId]) ON [PRIMARY]
GO

EXEC sp_addextendedproperty N'MS_Description', N'Primary identity key', 'SCHEMA', N'dbo', 'TABLE', N'PositionHistory', 'COLUMN', N'PositionId'
GO
