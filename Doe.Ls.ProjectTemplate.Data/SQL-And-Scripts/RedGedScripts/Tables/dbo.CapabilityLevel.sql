CREATE TABLE [dbo].[CapabilityLevel]
(
[CapabilityLevelId] [int] NOT NULL IDENTITY(1, 1),
[LevelName] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[DisplayOrder] [int] NOT NULL,
[LevelOrder] [int] NOT NULL,
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityLevel_DateCreated] DEFAULT (getdate()),
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityLevel_LastUpdated] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityLevel_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityLevel_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY]
ALTER TABLE [dbo].[CapabilityLevel] ADD 
CONSTRAINT [PK_CapabilityLevel] PRIMARY KEY CLUSTERED  ([CapabilityLevelId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Capability Level lookup', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityLevel', NULL, NULL
GO
