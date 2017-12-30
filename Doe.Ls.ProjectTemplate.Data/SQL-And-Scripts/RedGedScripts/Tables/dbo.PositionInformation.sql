CREATE TABLE [dbo].[PositionInformation]
(
[PositionId] [int] NOT NULL,
[OlderPositionNumber3] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
[OlderPositionNumber1] [nvarchar] (10) COLLATE Latin1_General_CI_AS NULL,
[OlderPositionNumber2] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
[SchNumber] [nvarchar] (20) COLLATE Latin1_General_CI_AS NULL,
[PositionTypeCode] [nvarchar] (8) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_PositionInformation_PositionTypeCode] DEFAULT (N'P'),
[EmployeeTypeCode] [nvarchar] (8) COLLATE Latin1_General_CI_AS NOT NULL,
[PositionNoteId] [int] NULL,
[TrimLink] [nvarchar] (350) COLLATE Latin1_General_CI_AS NULL,
[PositionEndDate] [datetime] NULL,
[PositionFTE] [float] NOT NULL,
[PositionStatusCode] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[OccupationTypeCode] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[OtherOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[PositionInformation] WITH NOCHECK ADD
CONSTRAINT [FK_PositionInformation_OccupationType] FOREIGN KEY ([OccupationTypeCode]) REFERENCES [dbo].[OccupationType] ([OccupationTypeCode])
ALTER TABLE [dbo].[PositionInformation] WITH NOCHECK ADD
CONSTRAINT [FK_PositionInformation_PositionStatusValue] FOREIGN KEY ([PositionStatusCode]) REFERENCES [dbo].[PositionStatusValue] ([PosStatusCode])
ALTER TABLE [dbo].[PositionInformation] WITH NOCHECK ADD
CONSTRAINT [FK_PositionInformation_PositionType] FOREIGN KEY ([PositionTypeCode]) REFERENCES [dbo].[PositionType] ([PositionTypeCode])
CREATE NONCLUSTERED INDEX [IX_PositionInformation_EmployeeType] ON [dbo].[PositionInformation] ([EmployeeTypeCode]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_PositionInformation_OccupationType] ON [dbo].[PositionInformation] ([OccupationTypeCode]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_PositionInformation_PositionStatusValue] ON [dbo].[PositionInformation] ([PositionStatusCode]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_PositionInformation_PositionType] ON [dbo].[PositionInformation] ([PositionTypeCode]) ON [PRIMARY]

ALTER TABLE [dbo].[PositionInformation] ADD 
CONSTRAINT [PK_PositionInformation] PRIMARY KEY CLUSTERED  ([PositionId]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'More infomation of a position', 'SCHEMA', N'dbo', 'TABLE', N'PositionInformation', NULL, NULL
GO


ALTER TABLE [dbo].[PositionInformation] ADD CONSTRAINT [FK_PositionInformation_EmployeeType1] FOREIGN KEY ([EmployeeTypeCode]) REFERENCES [dbo].[EmployeeType] ([EmployeeTypeCode])
GO
ALTER TABLE [dbo].[PositionInformation] WITH NOCHECK ADD CONSTRAINT [FK_PositionInformation_Position] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position] ([PositionId])
GO
