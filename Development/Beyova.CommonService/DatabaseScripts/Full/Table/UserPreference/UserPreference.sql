IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPreference]') AND type in (N'U'))
    DROP TABLE [dbo].[UserPreference]
GO

CREATE TABLE [dbo].[UserPreference](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [OwnerKey] [uniqueidentifier] NULL, -- IF IS NULL, then it is default value.
    [Realm] [nvarchar](256) NOT NULL DEFAULT '',
    [Category] [nvarchar](256) NOT NULL DEFAULT '',
    [Identifier] [varchar](256) NOT NULL DEFAULT '',
    [Value] [NVARCHAR](max) NOT NULL DEFAULT '',
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [LastUpdatedBy] UNIQUEIDENTIFIER NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_UserPreference_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_UserPreference] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO

