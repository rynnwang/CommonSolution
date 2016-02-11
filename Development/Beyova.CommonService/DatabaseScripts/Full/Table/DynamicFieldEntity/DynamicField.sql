IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DynamicField]') AND type in (N'U'))
DROP TABLE [dbo].[DynamicField]
GO

CREATE TABLE [dbo].[DynamicField](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [nvarchar](max) NOT NULL,
    [Application] [nvarchar](max) NOT NULL DEFAULT '',
    [Tag] [nvarchar](max) NOT NULL DEFAULT '', -- For management only, not used for calculation.
    [FieldType] [int] NOT NULL DEFAULT 0, --0 : string, 1: long, 2: decimal
    [DefaultFieldValue] [nvarchar](max) NOT NULL DEFAULT '',
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_DynamicField_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_DynamicField] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

