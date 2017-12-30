CREATE TABLE [dbo].[SelectionCriteria]
(
[SelectionCriteriaId] [int] NOT NULL,
[Criteria] [nvarchar] (max) COLLATE Latin1_General_CI_AS NOT NULL,
[LastModifiedDate] [datetime] NOT NULL,
[LastModifiedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SelectionCriteria] ADD CONSTRAINT [PK_SelectionCriteria] PRIMARY KEY CLUSTERED  ([SelectionCriteriaId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Position Description Selction Criteria lookup', 'SCHEMA', N'dbo', 'TABLE', N'SelectionCriteria', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Free text', 'SCHEMA', N'dbo', 'TABLE', N'SelectionCriteria', 'COLUMN', N'Criteria'
GO
