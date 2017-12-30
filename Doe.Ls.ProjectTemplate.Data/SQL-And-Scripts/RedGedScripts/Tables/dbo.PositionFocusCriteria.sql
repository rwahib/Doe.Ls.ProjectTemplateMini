CREATE TABLE [dbo].[PositionFocusCriteria]
(
[PositionDescriptionId] [int] NOT NULL,
[LookupId] [int] NOT NULL,
[LookupCustomContent] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL,
[LastModifiedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[LastModifiedDate] [datetime] NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[PositionFocusCriteria] ADD 
CONSTRAINT [PK_PositionFocusCriteria_1] PRIMARY KEY CLUSTERED  ([PositionDescriptionId], [LookupId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Holds the relations of PositionDesc with focused selection criteria of a given grade', 'SCHEMA', N'dbo', 'TABLE', N'PositionFocusCriteria', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to lkupFocusGradeCriteria', 'SCHEMA', N'dbo', 'TABLE', N'PositionFocusCriteria', 'COLUMN', N'LookupId'
GO

ALTER TABLE [dbo].[PositionFocusCriteria] WITH NOCHECK ADD CONSTRAINT [FK_PositionFocusCriteria_LkupTRIMFocusGradeSelectionCriteria] FOREIGN KEY ([LookupId]) REFERENCES [dbo].[LookupFocusGradeCriteria] ([LookupId])
GO
ALTER TABLE [dbo].[PositionFocusCriteria] ADD CONSTRAINT [FK_PositionFocusCriteria_PositionDescription] FOREIGN KEY ([PositionDescriptionId]) REFERENCES [dbo].[PositionDescription] ([PositionDescriptionId])
GO
