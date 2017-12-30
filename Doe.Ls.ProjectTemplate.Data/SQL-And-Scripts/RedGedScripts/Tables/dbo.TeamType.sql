CREATE TABLE [dbo].[TeamType]
(
[TeamTypeId] [int] NOT NULL,
[TeamTypeName] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[TeamTypeDescription] [nvarchar] (240) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TeamType] ADD CONSTRAINT [PK_TeamType] PRIMARY KEY CLUSTERED  ([TeamTypeId]) ON [PRIMARY]
GO
