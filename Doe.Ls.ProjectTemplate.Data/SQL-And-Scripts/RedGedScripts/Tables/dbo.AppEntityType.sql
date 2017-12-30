CREATE TABLE [dbo].[AppEntityType]
(
[EntityTypeId] [int] NOT NULL,
[EntityApiName] [nvarchar] (240) COLLATE Latin1_General_CI_AS NOT NULL,
[EntityTitle] [nvarchar] (240) COLLATE Latin1_General_CI_AS NOT NULL,
[EntityDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[SysAdminDashboard] [bit] NOT NULL CONSTRAINT [DF_EntityType_SysAdminDashboard] DEFAULT ((0)),
[PowerUserDashboard] [bit] NOT NULL CONSTRAINT [DF_EntityType_PowerUserDashboard] DEFAULT ((0)),
[HighPriority] [bit] NOT NULL CONSTRAINT [DF_EntityType_HighPriority] DEFAULT ((0))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppEntityType] ADD CONSTRAINT [PK_EntityType] PRIMARY KEY CLUSTERED  ([EntityTypeId]) ON [PRIMARY]
GO
