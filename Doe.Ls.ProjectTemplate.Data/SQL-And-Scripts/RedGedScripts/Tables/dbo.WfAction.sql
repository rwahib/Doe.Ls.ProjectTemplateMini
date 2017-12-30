CREATE TABLE [dbo].[WfAction]
(
[WfActionId] [int] NOT NULL,
[WfActionName] [nvarchar] (128) COLLATE Latin1_General_CI_AS NOT NULL,
[WfActionStatus] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
[WfActionDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
ALTER TABLE [dbo].[WfAction] ADD 
CONSTRAINT [PK_WfAction] PRIMARY KEY CLUSTERED  ([WfActionId]) ON [PRIMARY]
GO
