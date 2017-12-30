CREATE TABLE [dbo].[SysMessage]
(
[Code] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[MessageFormat] [nvarchar] (500) COLLATE Latin1_General_CI_AS NOT NULL,
[MsgCategoryId] [int] NOT NULL CONSTRAINT [DF_SysMessage_MessageCate;ry] DEFAULT (N'Information'),
[MessageHint] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_SysMessage_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SysMessage_LastModifiedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysMessage_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_SysMessage_LastModifiedBy] DEFAULT (N'System')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysMessage] ADD CONSTRAINT [PK_SysMessage] PRIMARY KEY CLUSTERED  ([Code]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysMessage] ADD CONSTRAINT [FK_SysMessage_SysMsgCategory] FOREIGN KEY ([MsgCategoryId]) REFERENCES [dbo].[SysMsgCategory] ([MsgCategoryId])
GO
