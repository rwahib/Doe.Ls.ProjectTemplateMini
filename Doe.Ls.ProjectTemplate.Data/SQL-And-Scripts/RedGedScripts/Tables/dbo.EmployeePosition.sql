CREATE TABLE [dbo].[EmployeePosition]
(
[EmployeeId] [int] NOT NULL,
[PositionId] [int] NOT NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_EmployeePosition_StatusId] DEFAULT ((0)),
[DisplayInOrgChart] [bit] NOT NULL,
[Reason] [nvarchar] (250) COLLATE Latin1_General_CI_AS NULL,
[FromDate] [date] NULL,
[ToDate] [date] NULL,
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_EmployeePosition_LastModifiedBy] DEFAULT (N'System'),
[CreatedDate] [datetime] NULL CONSTRAINT [DF_EmployeePosition_CreatedDate_1] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NULL CONSTRAINT [DF_EmployeePosition_LastModifiedDate_1] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_EmployeePosition_CreatedBy_1] DEFAULT (N'System')
) ON [PRIMARY]
ALTER TABLE [dbo].[EmployeePosition] WITH NOCHECK ADD
CONSTRAINT [FK_EmployeePosition_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId])
ALTER TABLE [dbo].[EmployeePosition] WITH NOCHECK ADD
CONSTRAINT [FK_EmployeePosition_Position] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position] ([PositionId])
GO
ALTER TABLE [dbo].[EmployeePosition] ADD CONSTRAINT [PK_EmployeePosition] PRIMARY KEY CLUSTERED  ([EmployeeId], [PositionId], [StatusId]) ON [PRIMARY]
GO

EXEC sp_addextendedproperty N'MS_Description', N'TODO', 'SCHEMA', N'dbo', 'TABLE', N'EmployeePosition', NULL, NULL
GO
