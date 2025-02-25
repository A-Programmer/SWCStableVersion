SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module_Commers_Items](
	[CMID] [int] IDENTITY(1,1) NOT NULL,
	[CmTitle] [nvarchar](500) NOT NULL,
	[CMDescription] [nvarchar](1500) NOT NULL,
	[CMDetails] [ntext] NOT NULL,
	[CMSendDate] [datetime] NOT NULL,
	[CMDay] [nvarchar](50) NULL,
	[CMMonth] [nvarchar](50) NULL,
	[CMPrice] [int] NULL,
	[CMTags] [ntext] NOT NULL,
	[CMKind] [nvarchar](150) NOT NULL,
	[CMKindID] [int] NOT NULL,
 CONSTRAINT [PK_Module_Commers_Items] PRIMARY KEY CLUSTERED 
(
	[CMID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO