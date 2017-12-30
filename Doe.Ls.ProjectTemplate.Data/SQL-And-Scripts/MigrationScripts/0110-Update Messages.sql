
DECLARE 
	@scriptNumber NVARCHAR(12) = '0110'
,	@scriptName NVARCHAR(256) = '0110-0110-Update Messages'
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM dbo.ScriptHistory WHERE ScriptNumber = @scriptNumber)
	PRINT 'INFO: Script run already'
ELSE
BEGIN




/****** Object:  Table [dbo].[SysMsgCategory]    Script Date: 28/06/2017 10:05:57 AM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON


CREATE TABLE [dbo].[SysMsgCategory](
	[MsgCategoryId] [int] NOT NULL,
	[MsgCategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SysMsgCategory] PRIMARY KEY CLUSTERED 
(
	[MsgCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

;



INSERT INTO dbo.SysMsgCategory
       
VALUES  ( 10, 'Information'   )

INSERT INTO dbo.SysMsgCategory
       
VALUES  ( 20, 'Warning'   )


INSERT INTO dbo.SysMsgCategory
       
VALUES  ( 30, 'Confirmation'   )

INSERT INTO dbo.SysMsgCategory
       
VALUES  ( 40, 'Error'   )




CREATE TABLE [dbo].[SysMessage](
	[Code] [nvarchar](64) NOT NULL,
	[MessageFormat] [nvarchar](500) NOT NULL,
	[MsgCategoryId] [int] NOT NULL,
	[MessageHint] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](120) NOT NULL,
	[LastModifiedBy] [nvarchar](120) NOT NULL,
 CONSTRAINT [PK_SysMessage] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

;

ALTER TABLE [dbo].[SysMessage] ADD  CONSTRAINT [DF_SysMessage_MessageCate;ry]  DEFAULT (N'Information') FOR [MsgCategoryId]
;

ALTER TABLE [dbo].[SysMessage] ADD  CONSTRAINT [DF_SysMessage_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
;

ALTER TABLE [dbo].[SysMessage] ADD  CONSTRAINT [DF_SysMessage_LastModifiedDate]  DEFAULT (getdate()) FOR [LastModifiedDate]
;

ALTER TABLE [dbo].[SysMessage] ADD  CONSTRAINT [DF_SysMessage_CreatedBy]  DEFAULT (N'System') FOR [CreatedBy]
;

ALTER TABLE [dbo].[SysMessage] ADD  CONSTRAINT [DF_SysMessage_LastModifiedBy]  DEFAULT (N'System') FOR [LastModifiedBy]
;

ALTER TABLE [dbo].[SysMessage]  WITH CHECK ADD  CONSTRAINT [FK_SysMessage_SysMsgCategory] FOREIGN KEY([MsgCategoryId])
REFERENCES [dbo].[SysMsgCategory] ([MsgCategoryId])
;

ALTER TABLE [dbo].[SysMessage] CHECK CONSTRAINT [FK_SysMessage_SysMsgCategory]
;


;
INSERT [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'PD-TASKS-CONFIRMATION-1', N'The position description {0} all required tasks has been completed!', 10, NULL, CAST(N'2017-06-28 10:09:59.890' AS DateTime), CAST(N'2017-06-28 10:09:59.890' AS DateTime), N'System', N'System')
;
INSERT [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'PRDPD-TASKS-WARNING-1', N'This {0} tasks is not completed, please complete the missing tasks!', 10, NULL, CAST(N'2017-06-28 10:09:59.890' AS DateTime), CAST(N'2017-06-28 10:09:59.890' AS DateTime), N'System', N'System')
;
INSERT [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'PS-TASKS-CONFIRMATION-1', N'The position {0} all required tasks has been completed!', 10, NULL, CAST(N'2017-06-28 10:09:59.890' AS DateTime), CAST(N'2017-06-28 10:09:59.890' AS DateTime), N'System', N'System')
;
INSERT [dbo].[SysMessage] ([Code], [MessageFormat], [MsgCategoryId], [MessageHint], [CreatedDate], [LastModifiedDate], [CreatedBy], [LastModifiedBy]) VALUES (N'RD-TASKS-CONFIRMATION-1', N'The role description {0} all required tasks has been completed!', 10, NULL, CAST(N'2017-06-28 10:09:59.890' AS DateTime), CAST(N'2017-06-28 10:09:59.890' AS DateTime), N'System', N'System')
;


	INSERT INTO dbo.ScriptHistory (ScriptNumber, scriptname, rundate, runby) VALUES (@scriptNumber, @scriptName, GETDATE(), @developer)

END
