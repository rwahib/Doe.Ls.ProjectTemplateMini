CREATE TABLE [dbo].[StatusValue]
(
[StatusId] [int] NOT NULL,
[StatusName] [nvarchar] (256) COLLATE Latin1_General_CI_AS NULL,
[StatusDescription] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StatusValue] ADD CONSTRAINT [PK_StatusHolder] PRIMARY KEY CLUSTERED  ([StatusId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Lookup for a Status', 'SCHEMA', N'dbo', 'TABLE', N'StatusValue', NULL, NULL
GO
