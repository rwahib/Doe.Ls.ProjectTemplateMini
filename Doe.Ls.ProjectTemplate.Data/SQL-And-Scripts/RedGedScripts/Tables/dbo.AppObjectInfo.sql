CREATE TABLE [dbo].[AppObjectInfo]
(
[ObjectName] [varchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[CounterValue] [int] NULL,
[AggregatedValueA] [float] NULL,
[AggregatedValueB] [float] NULL,
[Metadata] [xml] NULL,
[LastModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ObjectInfo_LastModifiedDate] DEFAULT (getdate()),
[lastModifiedUser] [varchar] (120) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppObjectInfo] ADD CONSTRAINT [PK_ObjectInfo] PRIMARY KEY CLUSTERED  ([ObjectName]) ON [PRIMARY]
GO
