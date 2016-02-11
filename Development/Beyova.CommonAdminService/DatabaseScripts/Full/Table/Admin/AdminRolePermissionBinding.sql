IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AdminRolePermissionBinding]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[AdminRolePermissionBinding];
END
GO

CREATE TABLE [dbo].[AdminRolePermissionBinding](
    [RowId] INT NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),    
    [RoleKey] [UNIQUEIDENTIFIER] NOT NULL, 
    [PermissionKey] [UNIQUEIDENTIFIER] NOT NULL,
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_AdminRolePermissionBinding_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_AdminRolePermissionBinding] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);