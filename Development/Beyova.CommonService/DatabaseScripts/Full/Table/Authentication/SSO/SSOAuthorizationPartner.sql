IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SSOAuthorizationPartner]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[SSOAuthorizationPartner];
END
GO
-- Can do both SSO and Token Exchange.

CREATE TABLE [dbo].[SSOAuthorizationPartner](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [OwnerKey] [UNIQUEIDENTIFIER] NULL,
    [Name] [NVARCHAR](256) NOT NULL,
    [Token] [NVARCHAR](512) NOT NULL,
    [CallbackUrl] [NVARCHAR](512) NULL,
    [TokenExpiration] [INT] NOT NULL DEFAULT 30, -- Unit: minute.
    [CreatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] [DATETIME] NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [INT] NOT NULL DEFAULT 0,
CONSTRAINT [PK_SSOAuthorizationPartner_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_SSOAuthorizationPartner] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO