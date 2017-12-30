CREATE TABLE [dbo].[CapabilityName]
(
[CapabilityNameId] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[CapabilityDescription] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
[CapabilityGroupId] [int] NOT NULL,
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityName_DateCreated] DEFAULT (getdate()),
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityName_LastUpdated] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityName_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityName_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_CapabilityName_CapabilityGroup] ON [dbo].[CapabilityName] ([CapabilityGroupId]) ON [PRIMARY]

ALTER TABLE [dbo].[CapabilityName] ADD 
CONSTRAINT [PK_CapabilityName] PRIMARY KEY CLUSTERED  ([CapabilityNameId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Capability Name lookup', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityName', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Capabiility Group', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityName', 'COLUMN', N'CapabilityGroupId'
GO

ALTER TABLE [dbo].[CapabilityName] ADD CONSTRAINT [FK_CapabilityName_CapabilityGroup] FOREIGN KEY ([CapabilityGroupId]) REFERENCES [dbo].[CapabilityGroup] ([CapabilityGroupId])
GO
