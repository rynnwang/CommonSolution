IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BinaryStorageMetaData]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[BinaryStorageMetaData];
END
GO

CREATE TABLE [dbo].[BinaryStorageMetaData](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Identifier] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Container] [varchar](128) NOT NULL,
    [Name] [nvarchar](512) NOT NULL,
    [Mime] [varchar](64) NOT NULL,
    [Hash] [varchar](256) NULL,
    [Length] [int] NULL,
    [Height] [int] NULL,
    [Width] [int] NULL,
    [Duration] [int] NULL,
    [OwnerKey] UNIQUEIDENTIFIER NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [varchar](128) NOT NULL,
    [LastUpdatedBy] [varchar](128) NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_BinaryStorageMetaData_Key] PRIMARY KEY NONCLUSTERED 
(
    [Identifier] ASC
),
CONSTRAINT [CIX_BinaryStorageMetaData] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO