CREATE TABLE [dbo].[RelationshipScope]
(
[ScopeId] [int] NOT NULL,
[ScopeTitle] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RelationshipScope] ADD CONSTRAINT [PK_RelationshipScope] PRIMARY KEY CLUSTERED  ([ScopeId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Scope lookup for key relationship, e.g. Internal, external.', 'SCHEMA', N'dbo', 'TABLE', N'RelationshipScope', NULL, NULL
GO
