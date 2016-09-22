IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DualApprovalRule]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[DualApprovalRule];
END
GO

CREATE TABLE [dbo].[DualApprovalRule](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [NVARCHAR](256) NOT NULL,
    [PermissionRequired] [NVARCHAR](256) NULL,
    [ApproveAmountRequired] [int] NOT NULL,
    [CallbackUri] NVARCHAR(512) NULL,
    [Description] NVARCHAR(512) NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_DualApprovalRule_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_DualApprovalRule] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);