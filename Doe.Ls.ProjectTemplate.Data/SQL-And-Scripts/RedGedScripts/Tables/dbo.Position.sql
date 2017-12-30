CREATE TABLE [dbo].[Position]
(
[PositionId] [int] NOT NULL IDENTITY(1, 1),
[ReportToPositionId] [int] NOT NULL,
[PositionNumber] [nvarchar] (10) COLLATE Latin1_General_CI_AS NOT NULL,
[RolePositionDescriptionId] [int] NOT NULL,
[UnitId] [int] NOT NULL,
[PositionTitle] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
[Description] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[PositionLevelId] [int] NOT NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_Position_Deleted] DEFAULT ((10)),
[PositionPath] [nvarchar] (500) COLLATE Latin1_General_CI_AS NOT NULL,
[LocationId] [int] NOT NULL CONSTRAINT [DF_Position_LocationId] DEFAULT ((-1)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Position_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Position_LastModifiedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Position_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Position_LastModifiedBy] DEFAULT (N'System'),
[DivisionOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[Position] WITH NOCHECK ADD
CONSTRAINT [FK_Position_PositionLevel] FOREIGN KEY ([PositionLevelId]) REFERENCES [dbo].[PositionLevel] ([PositionLevelId])
ALTER TABLE [dbo].[Position] WITH NOCHECK ADD
CONSTRAINT [FK_Position_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_Position_Location] ON [dbo].[Position] ([LocationId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Position_PositionLevel] ON [dbo].[Position] ([PositionLevelId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Position_Position] ON [dbo].[Position] ([ReportToPositionId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Position_RolePositionDescription] ON [dbo].[Position] ([RolePositionDescriptionId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Position_StatusValue] ON [dbo].[Position] ([StatusId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Position_Unit] ON [dbo].[Position] ([UnitId]) ON [PRIMARY]

ALTER TABLE [dbo].[Position] ADD
CONSTRAINT [FK_Position_RolePositionDescription] FOREIGN KEY ([RolePositionDescriptionId]) REFERENCES [dbo].[RolePositionDescription] ([RolePositionDescId])
GO
ALTER TABLE [dbo].[Position] ADD CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED  ([PositionId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PositionNumberUnique] ON [dbo].[Position] ([PositionNumber]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Position] ADD CONSTRAINT [FK_Position_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId])
GO

ALTER TABLE [dbo].[Position] ADD CONSTRAINT [FK_Position_Position] FOREIGN KEY ([ReportToPositionId]) REFERENCES [dbo].[Position] ([PositionId])
GO

ALTER TABLE [dbo].[Position] ADD CONSTRAINT [FK_Position_Unit] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[Unit] ([UnitId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Position details', 'SCHEMA', N'dbo', 'TABLE', N'Position', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Location, used in display position location in Role/position Description', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'LocationId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Primary identity key', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'PositionId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to PositionLevel', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'PositionLevelId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Unique position number', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'PositionNumber'
GO
EXEC sp_addextendedproperty N'MS_Description', N'
Position hierarchy, delimited with ''/''. Used to display Position Chart, as well as DirectReports and ReportingLine.


Example:Has the trace up to the top level like :

10/150/135/345

which 10 is the top
and 345 is the current position', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'PositionPath'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Carried from table RolePositionDescription', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'PositionTitle'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Id of Report to Position', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'ReportToPositionId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Id of either RoleDesc or PositionDesc which linked to this position, foreign key', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'RolePositionDescriptionId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to StatusValue', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'StatusId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Unit.', 'SCHEMA', N'dbo', 'TABLE', N'Position', 'COLUMN', N'UnitId'
GO
