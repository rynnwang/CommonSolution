IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DualApprovalRequest]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[DualApprovalRequest];
END
GO

CREATE TABLE [dbo].[DualApprovalRequest](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [NVARCHAR](256) NOT NULL,
    [Identifier] [NVARCHAR](256) NOT NULL, -- By client to find
    [Description] [NVARCHAR](1024) NOT NULL,
    [Reference] [NVARCHAR](512) NULL,
    [CallbackUrl] [NVARCHAR](512) NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_DualApprovalRequest_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_DualApprovalRequest] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);