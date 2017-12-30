CREATE TABLE [dbo].[Focus]
(
[FocusId] [int] NOT NULL IDENTITY(1, 1),
[FocusName] [nvarchar] (150) COLLATE Latin1_General_CI_AS NOT NULL,
[OrderList] [int] NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[Focus] ADD 
CONSTRAINT [PK_Focus] PRIMARY KEY CLUSTERED  ([FocusId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'PositionDescription Focus lookup', 'SCHEMA', N'dbo', 'TABLE', N'Focus', NULL, NULL
GO
