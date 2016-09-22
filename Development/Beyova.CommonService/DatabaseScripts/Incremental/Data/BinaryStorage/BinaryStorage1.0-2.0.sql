IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'OwnerKey' AND Object_ID = Object_ID(N'BinaryStorageMetaData'))
BEGIN
    -- CREATE USER TABLE
    CREATE TABLE [dbo].[UserBinaryStorageMetaData](
        [RowId] INT NOT NULL IDENTITY(1,1),
        [Identifier] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
        [OwnerKey] NVARCHAR(128) NOT NULL,
        [CreatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [LastUpdatedStamp] DATETIME NOT NULL DEFAULT GETUTCDATE(),
        [CreatedBy] UNIQUEIDENTIFIER NULL,
        [LastUpdatedBy] UNIQUEIDENTIFIER NULL,
        [State] [int] NOT NULL DEFAULT 0,
    CONSTRAINT [PK_UserBinaryStorageMetaData_Key] PRIMARY KEY NONCLUSTERED 
    (
        [Identifier] ASC
    ),
    CONSTRAINT [CIX_UserBinaryStorageMetaData] UNIQUE CLUSTERED 
    (
        [RowId] ASC
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    );

    -- MIGRATE DATA
    INSERT INTO [dbo].[UserBinaryStorageMetaData]
           ([OwnerKey]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     SELECT
           [OwnerKey]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,0
           FROM [dbo].[BinaryStorageMetaData];

    -- DELETE COLUMN
    ALTER TABLE [dbo].[BinaryStorageMetaData]
    DROP COLUMN [OwnerKey];

END