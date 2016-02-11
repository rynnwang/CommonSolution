IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdminUserInfo]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[AdminUserInfo];
END
GO

CREATE TABLE [dbo].[AdminUserInfo](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [LoginName] [NVARCHAR](64) NOT NULL,
    [Password] [NVARCHAR](256) NOT NULL DEFAULT '',
    [DisplayName] [NVARCHAR](64) NOT NULL DEFAULT '',
    [Email] [NVARCHAR](64) NOT NULL,
    [ThirdPartyId] [NVARCHAR](256) NULL, -- Can be name or identifier (int, guid, etc.) from AD, SSO, or any other 3rd party
    [PasswordResetToken] [NVARCHAR](512) NULL,
    [PasswordResetExpiredStamp] [datetime] NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_AdminUserInfo_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_AdminUserInfo] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);