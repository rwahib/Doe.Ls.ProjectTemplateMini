CREATE TABLE [dbo].[PositionLevel]
(
[PositionLevelId] [int] NOT NULL,
[PositionLevelName] [nvarchar] (240) COLLATE Latin1_General_CI_AS NOT NULL,
[CustomClass] [nvarchar] (123) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PositionLevel] ADD CONSTRAINT [PK_PositionLevel] PRIMARY KEY CLUSTERED  ([PositionLevelId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'lookups for a position, e.g Manager, Executive...', 'SCHEMA', N'dbo', 'TABLE', N'PositionLevel', NULL, NULL
GO
