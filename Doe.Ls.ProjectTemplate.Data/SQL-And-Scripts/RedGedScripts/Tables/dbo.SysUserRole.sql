CREATE TABLE [dbo].[SysUserRole]
(
[UserId] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[RoleId] [int] NOT NULL,
[StructureId] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[OrgLevelId] [int] NOT NULL,
[ActiveFrom] [datetime] NOT NULL,
[ActiveTo] [datetime] NULL,
[Note] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_SysUserRole_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysUserRole_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SysUserRole_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysUserRole_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[SysUserRole] ADD 
CONSTRAINT [PK_SysUserRole_1] PRIMARY KEY CLUSTERED  ([UserId], [RoleId], [StructureId], [OrgLevelId]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SysUserRole] ADD CONSTRAINT [FK_SysUserRole_OrgLevel] FOREIGN KEY ([OrgLevelId]) REFERENCES [dbo].[OrgLevel] ([OrgLevelId])
GO
ALTER TABLE [dbo].[SysUserRole] ADD CONSTRAINT [FK_SysUserRole_SysRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[SysRole] ([RoleId])
GO
ALTER TABLE [dbo].[SysUserRole] ADD CONSTRAINT [FK_SysUserRole_SysUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[SysUser] ([UserId])
GO
