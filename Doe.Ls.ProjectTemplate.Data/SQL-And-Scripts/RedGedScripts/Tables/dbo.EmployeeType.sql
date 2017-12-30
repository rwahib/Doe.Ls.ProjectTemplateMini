CREATE TABLE [dbo].[EmployeeType]
(
[EmployeeTypeCode] [nvarchar] (8) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_PositionType_PositionTypeCode] DEFAULT (N'P'),
[EmployeeTypeName] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL,
[EmployeeTypeDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmployeeType] ADD CONSTRAINT [PK_PositionType_1] PRIMARY KEY CLUSTERED  ([EmployeeTypeCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Employee type look up', 'SCHEMA', N'dbo', 'TABLE', N'EmployeeType', NULL, NULL
GO
