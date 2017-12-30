CREATE TABLE [dbo].[PositionStatusValue]
(
[PosStatusCode] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[PosStatusTitle] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PositionStatusValue] ADD CONSTRAINT [PK_PositionStatusValue] PRIMARY KEY CLUSTERED  ([PosStatusCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Lookups for Position status, e.g. filled, vacant', 'SCHEMA', N'dbo', 'TABLE', N'PositionStatusValue', NULL, NULL
GO
