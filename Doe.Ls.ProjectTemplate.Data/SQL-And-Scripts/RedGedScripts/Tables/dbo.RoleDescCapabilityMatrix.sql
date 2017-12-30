CREATE TABLE [dbo].[RoleDescCapabilityMatrix]
(
[GradeCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[Foundational_Min] [int] NOT NULL,
[Foundational_Max] [int] NOT NULL,
[Intermediate_Min] [int] NOT NULL,
[Intermediate_Max] [int] NOT NULL,
[Adept_Min] [int] NOT NULL,
[Adept_Max] [int] NOT NULL,
[Advanced_Min] [int] NOT NULL,
[Advanced_Max] [int] NOT NULL,
[HighlyAdvanced_Min] [int] NOT NULL,
[HighlyAdvanced_Max] [int] NOT NULL,
[FocusCapabilities_Min] [int] NOT NULL,
[FocusCapabilities_Max] [int] NOT NULL,
[Notes] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[RoleDescCapabilityMatrix] ADD
CONSTRAINT [FK_RoleDescCapabilityMatrix_Grade] FOREIGN KEY ([GradeCode]) REFERENCES [dbo].[Grade] ([GradeCode])
ALTER TABLE [dbo].[RoleDescCapabilityMatrix] ADD 
CONSTRAINT [PK_RoleDescCapabilityMatrix] PRIMARY KEY CLUSTERED  ([GradeCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Read only lookup used for validation of Capabilities points by grade', 'SCHEMA', N'dbo', 'TABLE', N'RoleDescCapabilityMatrix', NULL, NULL
GO
