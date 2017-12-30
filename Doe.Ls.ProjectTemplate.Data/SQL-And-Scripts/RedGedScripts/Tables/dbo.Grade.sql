CREATE TABLE [dbo].[Grade]
(
[GradeCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[GradeTitle] [nvarchar] (256) COLLATE Latin1_General_CI_AS NOT NULL,
[Award] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NULL,
[AwardMaxRates] [decimal] (18, 2) NULL,
[TeachingBased] [bit] NULL,
[GradeType] [nvarchar] (32) COLLATE Latin1_General_CI_AS NOT NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_Grade_StatusId] DEFAULT ((10)),
[Message] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[Grade] WITH NOCHECK ADD
CONSTRAINT [FK_Grade_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])
CREATE NONCLUSTERED INDEX [IX_Grade_StatusValue] ON [dbo].[Grade] ([StatusId]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Grade] ADD CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED  ([GradeCode]) ON [PRIMARY]
GO

EXEC sp_addextendedproperty N'MS_Description', N'Grade lookup', 'SCHEMA', N'dbo', 'TABLE', N'Grade', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'used to determine the cheif in a unit', 'SCHEMA', N'dbo', 'TABLE', N'Grade', 'COLUMN', N'AwardMaxRates'
GO
EXEC sp_addextendedproperty N'MS_Description', N'NSBTS,  
PSNE,   
PSSE,  ', 'SCHEMA', N'dbo', 'TABLE', N'Grade', 'COLUMN', N'GradeType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Foreign key to StatusValue', 'SCHEMA', N'dbo', 'TABLE', N'Grade', 'COLUMN', N'StatusId'
GO
