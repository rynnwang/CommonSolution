IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CountryInfo]') AND type in (N'U'))
DROP TABLE [dbo].[CountryInfo]
GO

CREATE TABLE [dbo].[CountryInfo](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [ISO2Code] [varchar](8) NOT NULL,
    [ISO3Code] [varchar](8) NOT NULL,
    [TelCode] [varchar](8) NOT NULL,
    [TimeZone] [int] NULL,
    [CurrencyCode] [varchar](8) NULL,
    [GeographyKey] [uniqueidentifier] NULL,
CONSTRAINT [PK_CountryInfo_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_CountryInfo] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

