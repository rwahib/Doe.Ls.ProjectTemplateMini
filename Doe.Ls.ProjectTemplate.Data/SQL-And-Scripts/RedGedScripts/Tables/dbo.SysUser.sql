CREATE TABLE [dbo].[SysUser]
(
[UserId] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[FirstName] [nvarchar] (32) COLLATE Latin1_General_CI_AS NOT NULL,
[LastName] [nvarchar] (32) COLLATE Latin1_General_CI_AS NOT NULL,
[Email] [nvarchar] (260) COLLATE Latin1_General_CI_AS NOT NULL,
[PrimaryPhone] [nvarchar] (260) COLLATE Latin1_General_CI_AS NULL,
[Note] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[Active] [bit] NOT NULL CONSTRAINT [DF_SysUser_Active] DEFAULT ((1)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_SysUser_CreatedDate_1] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysUser_CreatedBy_1] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SysUser_LastModifiedDate_1] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysUser_LastModifiedBy_1] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysUser] ADD CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED  ([UserId]) ON [PRIMARY]
GO
