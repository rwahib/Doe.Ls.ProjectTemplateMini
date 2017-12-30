CREATE TABLE [dbo].[Employee]
(
[EmployeeId] [int] NOT NULL,
[FirstName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[LastName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[CostCentreNumber] [numeric] (10, 0) NULL,
[CreatedDate] [datetime] NULL CONSTRAINT [DF_Employee_CreatedDate] DEFAULT (getdate()),
[LastModifiedDate] [datetime] NULL CONSTRAINT [DF_Employee_LastModifiedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Employee_CreatedBy] DEFAULT (N'System'),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL CONSTRAINT [DF_Employee_LastModifiedBy] DEFAULT (N'System'),
[StatusId] [int] NOT NULL CONSTRAINT [DF_Employee_Deleted] DEFAULT ((0))
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED  ([EmployeeId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'TODO', 'SCHEMA', N'dbo', 'TABLE', N'Employee', NULL, NULL
GO
