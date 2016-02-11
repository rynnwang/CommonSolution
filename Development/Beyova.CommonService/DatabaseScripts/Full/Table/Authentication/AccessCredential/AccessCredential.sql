IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccessCredential]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[AccessCredential];
END
GO

CREATE TABLE [dbo].[AccessCredential](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [UserKey] [UNIQUEIDENTIFIER] NOT NULL,
    [AccessIdentifier] [varchar](256) NOT NULL,
    [Domain] [varchar](128) NOT NULL DEFAULT '', 
    [Token] [varchar](512) NOT NULL DEFAULT '', --Token for oauth or hash for self accounts.
    [TokenExpiredStamp] [datetime] NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [INT] NOT NULL DEFAULT 0,
CONSTRAINT [PK_AccessCredential_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_AccessCredential] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO