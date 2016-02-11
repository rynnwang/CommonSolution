IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProvisioningObject]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[ProvisioningObject];
END
GO

CREATE TABLE [dbo].[ProvisioningObject](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Application] [INT] NULL,
    [Module] [NVARCHAR](128) NOT NULL DEFAULT '',
    [Name] [NVARCHAR](256) NOT NULL DEFAULT '',
    [Value] [NVARCHAR](MAX) NOT NULL DEFAULT '',
    [OwnerKey] UNIQUEIDENTIFIER NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [varchar](128) NOT NULL,
    [LastUpdatedBy] [varchar](128) NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_ProvisioningObject_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_ProvisioningObject] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO
