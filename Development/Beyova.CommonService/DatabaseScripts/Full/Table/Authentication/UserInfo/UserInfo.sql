IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserInfo]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[UserInfo];
END
GO

CREATE TABLE [dbo].[UserInfo](
    [RowId] [INT] NOT NULL IDENTITY(1,1),
    [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
    [Name] [nvarchar](64) NULL,
    [Gender] INT NOT NULL DEFAULT 0, 
    [Email] [varchar](128) NULL,
    [AvatarKey] [UNIQUEIDENTIFIER] NULL,
    [FunctionalRole] INT NOT NULL DEFAULT 0,
    [Language] VARCHAR(16) NULL,
    [TimeZone] INT NULL, --Unit: Minute   
    [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [LastUpdatedBy] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [int] NOT NULL DEFAULT 0,
CONSTRAINT [PK_UserInfo_Key] PRIMARY KEY NONCLUSTERED 
(
    [Key] ASC
),
CONSTRAINT [CIX_UserInfo] UNIQUE CLUSTERED 
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO