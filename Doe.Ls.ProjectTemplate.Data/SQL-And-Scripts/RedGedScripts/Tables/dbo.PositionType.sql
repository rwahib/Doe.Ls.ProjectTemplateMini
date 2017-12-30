CREATE TABLE [dbo].[PositionType]
(
[PositionTypeCode] [nvarchar] (8) COLLATE Latin1_General_CI_AS NOT NULL,
[PositionTypeName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[PositionTypeDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[PositionType] ADD CONSTRAINT [PK_PositionType] PRIMARY KEY CLUSTERED  ([PositionTypeCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'lookups for a type of position, e.g. Ongoing...', 'SCHEMA', N'dbo', 'TABLE', N'PositionType', NULL, NULL
GO
