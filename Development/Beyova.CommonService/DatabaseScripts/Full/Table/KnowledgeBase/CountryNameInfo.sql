IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryNameInfo]') AND type in (N'U'))
DROP TABLE [dbo].[CountryNameInfo]
GO

CREATE TABLE [dbo].[CountryNameInfo](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [nvarchar](128) NOT NULL,
    [CultureCode] [varchar](16) NOT NULL,
    [Sequence] [int] NOT NULL DEFAULT 0,
    [State] [int] NOT NULL DEFAULT 0
CONSTRAINT [PK_CountryNameInfo_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key],[CultureCode] ASC
),
CONSTRAINT [CIX_CountryNameInfo] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

