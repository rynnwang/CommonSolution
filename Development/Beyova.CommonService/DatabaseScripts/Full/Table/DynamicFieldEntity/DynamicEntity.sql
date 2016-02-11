IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DynamicEntity]') AND type in (N'U'))
DROP TABLE [dbo].[DynamicEntity]
GO

CREATE TABLE [dbo].[DynamicEntity](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [NVARCHAR](MAX) NOT NULL,
    [Application] [NVARCHAR](MAX) NOT NULL DEFAULT '',
    [Tag] [NVARCHAR](MAX) NOT NULL DEFAULT '', -- For management only, not used for calculation.
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_DynamicEntity_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_DynamicEntity] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

