CREATE TABLE [dbo].[CapabilityGroup]
(
[CapabilityGroupId] [int] NOT NULL IDENTITY(1, 1),
[GroupName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[GroupDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DisplayOrder] [int] NULL,
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityGroup_DateCreated_1] DEFAULT (getdate()),
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_CapabilityGroup_LastUpdated_1] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityGroup_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_CapabilityGroup_LastModifiedBy] DEFAULT (N'System'),
[GroupImage] [nvarchar] (150) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[CapabilityGroup] ADD 
CONSTRAINT [PK_CapabilityGroup] PRIMARY KEY CLUSTERED  ([CapabilityGroupId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Capability Group lookup', 'SCHEMA', N'dbo', 'TABLE', N'CapabilityGroup', NULL, NULL
GO
