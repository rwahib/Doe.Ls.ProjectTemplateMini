CREATE TABLE [dbo].[Executive]
(
[ExecutiveCod] [nvarchar] (64) COLLATE Latin1_General_CI_AS NOT NULL,
[ExecutiveTitle] [nvarchar] (512) COLLATE Latin1_General_CI_AS NOT NULL,
[ExecutiveDescription] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NULL,
[CustomClass] [nvarchar] (123) COLLATE Latin1_General_CI_AS NULL,
[StatusId] [int] NOT NULL CONSTRAINT [DF_Executive_StatusId] DEFAULT ((10)),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Executive_CreatedDate] DEFAULT (getdate()),
[CreatedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Executive_CreatedBy] DEFAULT (N'System'),
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Executive_LastModifiedDate] DEFAULT (getdate()),
[LastModifiedBy] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Executive_LastModifiedBy] DEFAULT (N'System'),
[DefaultExecutiveOverview] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[Executive] ADD 
CONSTRAINT [PK_Executive] PRIMARY KEY CLUSTERED  ([ExecutiveCod]) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Executive_StatusValue] ON [dbo].[Executive] ([StatusId]) ON [PRIMARY]

GO
EXEC sp_addextendedproperty N'MS_Description', N'Division??', 'SCHEMA', N'dbo', 'TABLE', N'Executive', NULL, NULL
GO


ALTER TABLE [dbo].[Executive] WITH NOCHECK ADD
CONSTRAINT [FK_Executive_StatusValue] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[StatusValue] ([StatusId])


GO
