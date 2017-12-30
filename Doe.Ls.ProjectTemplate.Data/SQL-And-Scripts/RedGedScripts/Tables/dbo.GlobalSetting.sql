CREATE TABLE [dbo].[GlobalSetting]
(
[SettingsKey] [int] NOT NULL,
[PropertyCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NOT NULL,
[PropertyBooleanValue] [bit] NULL,
[PropertyValue] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
[EntityContextCode] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
[EntityContextValue] [nvarchar] (120) COLLATE Latin1_General_CI_AS NULL,
[Tag] [nvarchar] (500) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GlobalSetting] ADD CONSTRAINT [PK_GlobalSettings] PRIMARY KEY CLUSTERED  ([SettingsKey]) ON [PRIMARY]
GO
EXEC sp_addextendedproperty N'MS_Description', N'To extend any entity fields like:
adding more data to any table in database

like default values to users', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', NULL, NULL
GO
EXEC sp_addextendedproperty N'MS_Description', N'Like
User
Application wide
', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'EntityContextCode'
GO
EXEC sp_addextendedproperty N'MS_Description', N'The Entity value

for example may be for Entity code = User value will be John.Smith', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'EntityContextValue'
GO
EXEC sp_addextendedproperty N'MS_Description', N'if the value just Y/N', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'PropertyBooleanValue'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Like
Default Division for a user 
Default Directorate  for a user 

and so forth', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'PropertyCode'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Any values even the numeric', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'PropertyValue'
GO
EXEC sp_addextendedproperty N'MS_Description', N'Extra data to be attached', 'SCHEMA', N'dbo', 'TABLE', N'GlobalSetting', 'COLUMN', N'Tag'
GO
