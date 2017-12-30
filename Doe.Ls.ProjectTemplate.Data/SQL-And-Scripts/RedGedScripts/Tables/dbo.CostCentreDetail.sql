CREATE TABLE [dbo].[CostCentreDetail]
(
[PositionId] [int] NOT NULL,
[CostCentre] [nvarchar] (150) COLLATE Latin1_General_CI_AS NULL,
[Fund] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[PayrollWBS] [nvarchar] (150) COLLATE Latin1_General_CI_AS NULL,
[RCCJDEPayrollCode] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[GLAccount] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL,
[LastUpdatedDate] [datetime] NOT NULL,
[LastUpdatedBy] [nvarchar] (250) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[CostCentreDetail] ADD 
CONSTRAINT [PK_CostCentreDetail] PRIMARY KEY CLUSTERED  ([PositionId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Cost centre details for a position.', 'SCHEMA', N'dbo', 'TABLE', N'CostCentreDetail', NULL, NULL
GO

ALTER TABLE [dbo].[CostCentreDetail] WITH NOCHECK ADD CONSTRAINT [FK_CostCentreDetail_Position] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position] ([PositionId])
GO
