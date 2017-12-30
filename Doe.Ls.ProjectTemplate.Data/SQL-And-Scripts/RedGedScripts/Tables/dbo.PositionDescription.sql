CREATE TABLE [dbo].[PositionDescription]
(
[PositionDescriptionId] [int] NOT NULL,
[BriefRoleStatement] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[StatementOfDuties] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[PositionDescription] ADD CONSTRAINT [PK_PositionDescription] PRIMARY KEY CLUSTERED  ([PositionDescriptionId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PositionDescription] ADD CONSTRAINT [FK_PositionDescription_RolePositionDescription] FOREIGN KEY ([PositionDescriptionId]) REFERENCES [dbo].[RolePositionDescription] ([RolePositionDescId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Position description - for a position of grade type is ''NSBTS''', 'SCHEMA', N'dbo', 'TABLE', N'PositionDescription', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Brief statement of the position', 'SCHEMA', N'dbo', 'TABLE', N'PositionDescription', 'COLUMN', N'BriefRoleStatement'
GO
EXEC sp_addextendedproperty N'MS_Description', N'This is used as a foreign key in RolePositionDescription.', 'SCHEMA', N'dbo', 'TABLE', N'PositionDescription', 'COLUMN', N'PositionDescriptionId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Content of Statement of Duties of this Position', 'SCHEMA', N'dbo', 'TABLE', N'PositionDescription', 'COLUMN', N'StatementOfDuties'
GO
