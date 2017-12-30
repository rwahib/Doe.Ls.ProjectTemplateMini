CREATE TABLE [dbo].[SysMsgCategory]
(
[MsgCategoryId] [int] NOT NULL,
[MsgCategoryName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysMsgCategory] ADD CONSTRAINT [PK_SysMsgCategory] PRIMARY KEY CLUSTERED  ([MsgCategoryId]) ON [PRIMARY]
GO
