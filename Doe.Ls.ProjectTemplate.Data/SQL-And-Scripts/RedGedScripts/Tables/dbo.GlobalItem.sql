CREATE TABLE [dbo].[GlobalItem]
(
[ItemCode] [nvarchar] (12) COLLATE Latin1_General_CI_AS NOT NULL,
[ItemName] [nvarchar] (124) COLLATE Latin1_General_CI_AS NOT NULL,
[ItemDescription] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[ItemContent] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL,
[CreatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Item_CreatedBy] DEFAULT (N'System'),
[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Item_CreatedDate] DEFAULT (getdate()),
[LastupdatedBy] [nvarchar] (50) COLLATE Latin1_General_CI_AS NOT NULL CONSTRAINT [DF_Item_LastupdatedBy] DEFAULT (N'System'),
[LastupdatedDate] [datetime] NOT NULL CONSTRAINT [DF_Item_LastupdatedDate] DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[GlobalItem] ADD CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED  ([ItemCode]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'Static text for RoleDesciption, Position Description. e.g Department Info.', 'SCHEMA', N'dbo', 'TABLE', N'GlobalItem', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Lookup key', 'SCHEMA', N'dbo', 'TABLE', N'GlobalItem', 'COLUMN', N'ItemCode'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Actual Content which used for static content', 'SCHEMA', N'dbo', 'TABLE', N'GlobalItem', 'COLUMN', N'ItemContent'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Description of the item', 'SCHEMA', N'dbo', 'TABLE', N'GlobalItem', 'COLUMN', N'ItemDescription'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Field name', 'SCHEMA', N'dbo', 'TABLE', N'GlobalItem', 'COLUMN', N'ItemName'
GO
