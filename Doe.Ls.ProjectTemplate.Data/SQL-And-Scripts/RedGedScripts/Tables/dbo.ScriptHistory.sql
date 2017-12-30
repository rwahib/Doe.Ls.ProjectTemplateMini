CREATE TABLE [dbo].[ScriptHistory]
(
[ScriptNumber] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[ScriptName] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
[RunDate] [datetime] NOT NULL,
[RunBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[Comments] [nvarchar] (1000) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ScriptHistory] ADD CONSTRAINT [PK_ScriptHistory] PRIMARY KEY CLUSTERED  ([ScriptNumber]) ON [PRIMARY]
GO
