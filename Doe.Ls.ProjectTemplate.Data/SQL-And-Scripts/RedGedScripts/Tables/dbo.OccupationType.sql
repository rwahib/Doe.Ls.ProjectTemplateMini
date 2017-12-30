CREATE TABLE [dbo].[OccupationType]
(
[OccupationTypeCode] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[OccupationTypeName] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OccupationType] ADD CONSTRAINT [PK_OccupationType] PRIMARY KEY CLUSTERED  ([OccupationTypeCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Lookups for an Occupation of a position', 'SCHEMA', N'dbo', 'TABLE', N'OccupationType', NULL, NULL
GO
