CREATE TABLE [dbo].[RolePositionDescription]
(
[RolePositionDescId] [int] NOT NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_RolePositionDescription_StatusId] DEFAULT ((40)),
[Version] [int] NOT NULL CONSTRAINT [DF_RolePositionDescription_Version] DEFAULT ((1)),
[Title] [nvarchar] (250) COLLATE Latin1_General_CI_AS NOT NULL,
[DocNumber] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[GradeCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[DateOfApproval] [datetime] NULL,
[IsPositionDescription] [bit] NOT NULL CONSTRAINT [DF_RolePositionDescription_IsPositionDescription] DEFAULT ((0)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_RolePositionDescription_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_RolePositionDescription_LastModifiedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RolePositionDescription_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RolePositionDescription_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [IX_DocNumberVersion] ON [dbo].[RolePositionDescription] ([Version], [DocNumber]) ON [PRIMARY]

ALTER TABLE [dbo].[RolePositionDescription] WITH NOCHECK ADD
CONSTRAINT [FK_RolePositionDescription_Grade] FOREIGN KEY ([GradeCode]) REFERENCES [dbo].[Grade] ([GradeCode])
ALTER TABLE [dbo].[RolePositionDescription] WITH NOCHECK ADD
CONSTRAINT [FK_RolePositionDescription_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_RolePositionDescription_Grade] ON [dbo].[RolePositionDescription] ([GradeCode]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_RolePositionDescription_StatusValue] ON [dbo].[RolePositionDescription] ([StatusId]) ON [PRIMARY]

ALTER TABLE [dbo].[RolePositionDescription] ADD 
CONSTRAINT [PK_RolePositionDescription] PRIMARY KEY CLUSTERED  ([RolePositionDescId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Holder of Role Description and Position description, with common fields of DocNumber, GradeCode, Title', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'TRIM Document number', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'DocNumber'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Grade', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'GradeCode'
GO

EXEC sp_addextendedproperty N'MS_Description', N'This key indicates whether a RoleDesc or PositionDesc.
If gradeType =''PSSE'' or ''PSNE'', then it is true. Otherwise gradeType=''NSBTS'' then is false ', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'IsPositionDescription'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Used as foreign key in Position. Also this key exists in either RoleDescription or PositionDescription', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'RolePositionDescId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign Key to StatusValue', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'StatusId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Title is the same as Position Title', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'Title'
GO

EXEC sp_addextendedproperty N'MS_Description', N'? version + Doc number is unique', 'SCHEMA', N'dbo', 'TABLE', N'RolePositionDescription', 'COLUMN', N'Version'
GO
