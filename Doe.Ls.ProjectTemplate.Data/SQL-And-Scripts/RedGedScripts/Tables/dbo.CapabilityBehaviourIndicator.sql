CREATE TABLE [dbo].[CapabilityBehaviourIndicator]
(
[CapabilityNameId] [int] NOT NULL,
[CapabilityLevelId] [int] NOT NULL,
[IndicatorContext] [nvarchar] (max) COLLATE Latin1_General_CI_AS NOT NULL,
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityBehaviourIndicator_DateCreated] DEFAULT (getdate()),
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityBehaviourIndicator_LastUpdated] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityBehaviourIndicator_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityBehaviourIndicator_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CapabilityBehaviourIndicator] ADD CONSTRAINT [PK_CapabilityBehaviourIndicator_1] PRIMARY KEY CLUSTERED  ([CapabilityNameId], [CapabilityLevelId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CapabilityBehaviourIndicator] ADD CONSTRAINT [FK_CapabilityBehaviourIndicator_CapabilityLevel1] FOREIGN KEY ([CapabilityLevelId]) REFERENCES [dbo].[CapabilityLevel] ([CapabilityLevelId])
GO
ALTER TABLE [dbo].[CapabilityBehaviourIndicator] ADD CONSTRAINT [FK_CapabilityBehaviourIndicator_CapabilityName1] FOREIGN KEY ([CapabilityNameId]) REFERENCES [dbo].[CapabilityName] ([CapabilityNameId])
GO
EXEC sp_addextendedproperty N'MS_Description', N'Holds the relationship among Capability Group, Capability Name and their levels', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityBehaviourIndicator', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Capability Level', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityBehaviourIndicator', 'COLUMN', N'CapabilityLevelId'
GO
