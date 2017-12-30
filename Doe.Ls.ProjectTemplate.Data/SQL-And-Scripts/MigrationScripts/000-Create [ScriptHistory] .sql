DECLARE 
	@scriptNumber NVARCHAR(12) = '0000'
,	@scriptName NVARCHAR(256) = '000-Create [ScriptHistory] '
,	@developer NVARCHAR(256) = 'Refky Wahib'


IF EXISTS (SELECT 1 FROM sys.tables WHERE name='ScriptHistory')
	PRINT 'ScriptHistory table is already there'
ELSE
BEGIN
	

CREATE TABLE [dbo].[ScriptHistory](
	[ScriptNumber] [nvarchar](12) NOT NULL,
	[ScriptName] [nvarchar](256) NOT NULL,
	[RunDate] [datetime] NOT NULL,
	[RunBy] [nvarchar](120) NOT NULL,
	[Comments] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ScriptHistory] PRIMARY KEY CLUSTERED 
(
	[ScriptNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


END

GO