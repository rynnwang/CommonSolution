IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdminSession]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[AdminSession];
END
GO

CREATE TABLE [dbo].[AdminSession](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Token] [varchar](512) NOT NULL DEFAULT '',
    [UserKey] [UNIQUEIDENTIFIER] NOT NULL,
    [IpAddress] [varchar](64) NULL,
    [UserAgent] [varchar](256) NULL,
    [ExpiredStamp] DATETIME NOT NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_AdminSession_Token] PRIMARY KEY NONCLUSTERED 
(
    [Token] ASC
),
CONSTRAINT [CIX_AdminSession] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);