CREATE TABLE [dbo].[SysRole]
(
[RoleId] [int] NOT NULL,
[RoleTitle] [nvarchar] (128) COLLATE Latin1_General_CI_AS NOT NULL,
[RoleApiName] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
[RoleDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_SysRole_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysRole_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SysRole_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysRole_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysRole] ADD CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED  ([RoleId]) ON [PRIMARY]
GO
