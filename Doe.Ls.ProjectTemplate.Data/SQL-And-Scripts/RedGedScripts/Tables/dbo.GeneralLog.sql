CREATE TABLE [dbo].[GeneralLog]
(
[LogId] [int] NOT NULL IDENTITY(1, 1),
[Action] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
[Context] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreationDate] [datetime] NULL,
[Username] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
[RoleId] [int] NOT NULL,
[Note] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[GeneralLog] ADD
CONSTRAINT [FK_GeneralLog_SysRole] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[SysRole] ([RoleId])
GO
ALTER TABLE [dbo].[GeneralLog] ADD CONSTRAINT [PK_GeneralLog] PRIMARY KEY CLUSTERED  ([LogId]) ON [PRIMARY]
GO
