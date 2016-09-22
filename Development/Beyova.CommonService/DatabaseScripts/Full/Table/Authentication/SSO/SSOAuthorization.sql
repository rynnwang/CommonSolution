IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SSOAuthorization]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[SSOAuthorization];
END
GO

CREATE TABLE [dbo].[SSOAuthorization](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [PartnerKey] [UNIQUEIDENTIFIER] NOT NULL,
    [ClientRequestId] [NVARCHAR](512) NOT NULL,
    [AuthorizationToken] [NVARCHAR](512) NULL,
    [UserKey] [UNIQUEIDENTIFIER] NULL, -- This field is for token exchange only.
    [ExpiredStamp] [DATETIME] NOT NULL,
    [UsedStamp] [DATETIME] NULL,
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [State] [INT] NOT NULL DEFAULT 0,
CONSTRAINT [PK_SSOAuthorization_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_SSOAuthorization] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO