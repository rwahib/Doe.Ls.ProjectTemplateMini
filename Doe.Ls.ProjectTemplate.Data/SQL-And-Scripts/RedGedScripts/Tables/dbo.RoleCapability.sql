CREATE TABLE [dbo].[RoleCapability]
(
[RoleDescriptionId] [int] NOT NULL,
[CapabilityNameId] [int] NOT NULL,
[CapabilityLevelId] [int] NOT NULL,
[Highlighted] [bit] NOT NULL CONSTRAINT [DF_RoleCapability_Highlighted] DEFAULT ((0)),
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_RoleCapability_DateCreated] DEFAULT (getdate()),
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_RoleCapability_LastUpdated] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RoleCapability_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_RoleCapability_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RoleCapability] ADD CONSTRAINT [PK_RoleCapability] PRIMARY KEY CLUSTERED  ([RoleDescriptionId], [CapabilityNameId], [CapabilityLevelId], [Highlighted]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RoleCapability] WITH NOCHECK ADD CONSTRAINT [FK_RoleCapability_CapabilityLevel] FOREIGN KEY ([CapabilityLevelId]) REFERENCES [dbo].[CapabilityLevel] ([CapabilityLevelId])
GO
ALTER TABLE [dbo].[RoleCapability] WITH NOCHECK ADD CONSTRAINT [FK_RoleCapability_CapabilityName] FOREIGN KEY ([CapabilityNameId]) REFERENCES [dbo].[CapabilityName] ([CapabilityNameId])
GO
ALTER TABLE [dbo].[RoleCapability] WITH NOCHECK ADD CONSTRAINT [FK_RoleCapability_RoleDescription] FOREIGN KEY ([RoleDescriptionId]) REFERENCES [dbo].[RoleDescription] ([RoleDescriptionId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Indexing table holds Role Description with its capability names, groups, levels and highlighted capabilities', 'SCHEMA', N'dbo', 'TABLE', N'RoleCapability', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Capability level', 'SCHEMA', N'dbo', 'TABLE', N'RoleCapability', 'COLUMN', N'CapabilityLevelId'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Capability name', 'SCHEMA', N'dbo', 'TABLE', N'RoleCapability', 'COLUMN', N'CapabilityNameId'
GO
