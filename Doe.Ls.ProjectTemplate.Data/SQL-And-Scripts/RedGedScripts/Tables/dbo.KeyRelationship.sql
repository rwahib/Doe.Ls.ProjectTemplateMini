CREATE TABLE [dbo].[KeyRelationship]
(
[KeyRelationshipId] [int] NOT NULL IDENTITY(1, 1),
[RoleDescriptionId] [int] NOT NULL,
[ScopeId] [int] NULL,
[OrderNumber] [int] NULL,
[Who] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[Why] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_KeyRelationships_DateCreated] DEFAULT (getdate()),
[ModifiedUserName] [nvarchar] (250) COLLATE Latin1_General_CI_AS NOT NULL,
[LastUpdated] [datetime] NOT NULL CONSTRAINT [DF_KeyRelationships_LastUpdated] DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[KeyRelationship] ADD 
CONSTRAINT [PK_KeyRelationships] PRIMARY KEY CLUSTERED  ([KeyRelationshipId]) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_KeyRelationship_RoleDescription] ON [dbo].[KeyRelationship] ([RoleDescriptionId]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_KeyRelationship_RelationshipScope] ON [dbo].[KeyRelationship] ([ScopeId]) ON [PRIMARY]

GO
EXEC sp_addextendedproperty N'MS_Description', N'For capability framework key relationship builder with the scope (Internal & external)', 'SCHEMA', N'dbo', 'TABLE', N'KeyRelationship', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to RoleDescription', 'SCHEMA', N'dbo', 'TABLE', N'KeyRelationship', 'COLUMN', N'RoleDescriptionId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Scope', 'SCHEMA', N'dbo', 'TABLE', N'KeyRelationship', 'COLUMN', N'ScopeId'
GO

ALTER TABLE [dbo].[KeyRelationship] WITH NOCHECK ADD CONSTRAINT [FK_KeyRelationship_RoleDescription] FOREIGN KEY ([RoleDescriptionId]) REFERENCES [dbo].[RoleDescription] ([RoleDescriptionId])
GO
ALTER TABLE [dbo].[KeyRelationship] ADD CONSTRAINT [FK_KeyRelationship_RelationshipScope] FOREIGN KEY ([ScopeId]) REFERENCES [dbo].[RelationshipScope] ([ScopeId])
GO
