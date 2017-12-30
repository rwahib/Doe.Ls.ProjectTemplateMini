CREATE TABLE [dbo].[RoleDescription]
(
[RoleDescriptionId] [int] NOT NULL,
[Cluster] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
[SeniorExecutiveWorkLevelStandards] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NULL,
[ANZSCOCode] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[PCATCode] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[AgencyOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[Agency] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
[AgencyWebsite] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
[RolePrimaryPurpose] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[KeyAccountabilities] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[KeyChallenges] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DecisionMaking] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[ReportingLine] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[DirectReports] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[BudgetExpenditure] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL,
[BudgetExpenditureValue] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
[BudgetExtraNotes] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[EssentialRequirements] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[RoleCapabilityItems] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CapabilitySummary] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[FocusCapabilities] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_RoleDescription_LastModifiedBy] DEFAULT (N'System'),
[CreatedDate] [datetime] NULL CONSTRAINT [DF_RoleDescription_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NULL CONSTRAINT [DF_RoleDescription_LastModifiedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_RoleDescription_CreatedBy] DEFAULT (N'System'),
[VersionStatus] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[OldPDFileName] [nvarchar] (150) COLLATE Latin1_General_CI_AS NULL,
[ManagerRole] [bit] NULL CONSTRAINT [DF_RoleDescription_ManagerRole] DEFAULT ((0))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[RoleDescription] ADD 
CONSTRAINT [PK_RoleDescription] PRIMARY KEY CLUSTERED  ([RoleDescriptionId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Role description details, for the positions ''PSSE'', ''PSNE''', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', NULL, NULL
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'Agency'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'AgencyOverview'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'AgencyWebsite'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Default is Nil. or   As per DoE Mo23 Financial Delegations
', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'BudgetExpenditure'
GO

EXEC sp_addextendedproperty N'MS_Description', N'If PSSE with text Budget / Expenditure
(Note, display in last line in RoleDescription.)', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'BudgetExpenditureValue'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'CapabilitySummary'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'Cluster'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'DecisionMaking'
GO


EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.
General / General + PSSE', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'EssentialRequirements'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'FocusCapabilities'
GO

EXEC sp_addextendedproperty N'MS_Description', N'max 8 bullet points, min 6', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'KeyAccountabilities'
GO

EXEC sp_addextendedproperty N'MS_Description', N'max 3 bullet points, min 2', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'KeyChallenges'
GO

EXEC sp_addextendedproperty N'MS_Description', N'This field only for driving cabability framework ( adding People Management group) section', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'ManagerRole'
GO

EXEC sp_addextendedproperty N'MS_Description', N'The text will be retrieved from table Item as static content.', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'RoleCapabilityItems'
GO

EXEC sp_addextendedproperty N'MS_Description', N'This key used as foreign key in RolePositionDescription', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'RoleDescriptionId'
GO

EXEC sp_addextendedproperty N'MS_Description', N'Free text , Must be filled for approval.
Limited as 500 charactors', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescription', 'COLUMN', N'RolePrimaryPurpose'
GO

ALTER TABLE [dbo].[RoleDescription] ADD CONSTRAINT [FK_RoleDescription_RolePositionDescription] FOREIGN KEY ([RoleDescriptionId]) REFERENCES [dbo].[RolePositionDescription] ([RolePositionDescId])
GO
