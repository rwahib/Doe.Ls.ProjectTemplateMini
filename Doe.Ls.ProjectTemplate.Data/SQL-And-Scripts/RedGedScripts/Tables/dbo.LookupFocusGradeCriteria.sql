CREATE TABLE [dbo].[LookupFocusGradeCriteria]
(
[LookupId] [int] NOT NULL IDENTITY(1, 1),
[FocusId] [int] NOT NULL,
[GradeCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[SelectionCriteriaId] [int] NOT NULL,
[LastUpdatedDate] [datetime] NOT NULL,
[LastUpdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[IsMandatory] [bit] NOT NULL CONSTRAINT [DF_LookupFocusGradeCriteria_IsMandatory] DEFAULT ((0))
) ON [PRIMARY]
ALTER TABLE [dbo].[LookupFocusGradeCriteria] WITH NOCHECK ADD
CONSTRAINT [FK_LkupTRIMFocusGradeSelectionCriteria_Grade] FOREIGN KEY ([GradeCode]) REFERENCES [dbo].[Grade] ([GradeCode])
ALTER TABLE [dbo].[LookupFocusGradeCriteria] ADD 
CONSTRAINT [PK_LkupTRIMFocusGradeSelectionCriteria] PRIMARY KEY CLUSTERED  ([LookupId]) ON [PRIMARY]
CREATE UNIQUE NONCLUSTERED INDEX [IX_FocusGradeSelectionCriteria] ON [dbo].[LookupFocusGradeCriteria] ([FocusId], [GradeCode], [SelectionCriteriaId]) ON [PRIMARY]

GO
EXEC sp_addextendedproperty N'MS_Description', N'Lookups for holding the relationships among Grade, Focus, and Selectioin Criteria (Focused selection criteria for a grade)', 'SCHEMA', N'dbo', 'TABLE', N'LookupFocusGradeCriteria', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Focus', 'SCHEMA', N'dbo', 'TABLE', N'LookupFocusGradeCriteria', 'COLUMN', N'FocusId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to Grade', 'SCHEMA', N'dbo', 'TABLE', N'LookupFocusGradeCriteria', 'COLUMN', N'GradeCode'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to SelectionCriteria', 'SCHEMA', N'dbo', 'TABLE', N'LookupFocusGradeCriteria', 'COLUMN', N'SelectionCriteriaId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'X', 'SCHEMA', N'dbo', 'TABLE', N'LookupFocusGradeCriteria', 'INDEX', N'IX_FocusGradeSelectionCriteria'
GO

ALTER TABLE [dbo].[LookupFocusGradeCriteria] WITH NOCHECK ADD CONSTRAINT [FK_LkupTRIMFocusGradeSelectionCriteria_Focus] FOREIGN KEY ([FocusId]) REFERENCES [dbo].[Focus] ([FocusId])
GO

ALTER TABLE [dbo].[LookupFocusGradeCriteria] WITH NOCHECK ADD CONSTRAINT [FK_LkupTRIMFocusGradeSelectionCriteria_SelectionCriteria] FOREIGN KEY ([SelectionCriteriaId]) REFERENCES [dbo].[SelectionCriteria] ([SelectionCriteriaId])
GO
