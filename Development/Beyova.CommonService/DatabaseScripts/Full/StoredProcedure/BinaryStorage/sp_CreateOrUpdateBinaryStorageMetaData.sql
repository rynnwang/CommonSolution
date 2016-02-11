IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateBinaryStorageMetaData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateBinaryStorageMetaData]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateBinaryStorageMetaData](
    @Container VARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name [NVARCHAR](512),
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @Length INT,
    @Height INT,
    @Width INT,
    @Duration INT,
    @OwnerKey UNIQUEIDENTIFIER,
    @State INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @ExistedState AS INT;
DECLARE @ExistedOwnerKey AS UNIQUEIDENTIFIER;
BEGIN
    IF @Container IS NOT NULL
    BEGIN
        IF @State IS NULL OR @State < 0
            SET @State = 0;

        IF @Identifier IS NULL OR NOT EXISTS (SELECT TOP 1 * FROM [dbo].[BinaryStorageMetaData] WHERE [Identifier] = @Identifier)
        BEGIN
            SET @Identifier = ISNULL(@Identifier, NEWID());

            INSERT INTO [dbo].[BinaryStorageMetaData]
               ([Identifier]
               ,[Container]
               ,[Name]
               ,[Mime]
               ,[Hash]
               ,[Length]
               ,[Height]
               ,[Width]
               ,[Duration]
               ,[OwnerKey]
               ,[CreatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedStamp]
               ,[LastUpdatedBy]
               ,[State])
         VALUES
               (@Identifier
               ,@Container
               ,ISNULL(@Name, CONVERT(VARCHAR(512),@Identifier))
               ,ISNULL(@Mime, 'application/octet-stream')
               ,@Hash
               ,@Length
               ,@Height
               ,@Width
               ,@Duration
               ,@OwnerKey
               ,@NowTime
               ,@OperatorKey
               ,@NowTime
               ,@OperatorKey
               ,1 -- Commit pending: 1
               );
        END
        ELSE 
        BEGIN
            SELECT TOP 1 @ExistedState = [State], @ExistedOwnerKey = [OwnerKey] FROM [dbo].[BinaryStorageMetaData] WHERE [Identifier] = @Identifier;

            IF @ExistedState > 2 OR (@ExistedOwnerKey IS NOT NULL AND @OwnerKey <> @ExistedOwnerKey)
                RAISERROR(60403, 16, 1, 'Update or delete operation is forbidden caused by state.');
            ELSE
            UPDATE [dbo].[BinaryStorageMetaData]
                SET [Mime] = ISNULL(@Mime, [Mime]),
                    [Hash] = ISNULL(@Hash, [Hash]),
                    [Length] = ISNULL(@Length, [Length]),
                    [Width] = ISNULL(@Width, [Width]),
                    [Height] = ISNULL(@Height, [Height]),
                    [Duration] = ISNULL(@Duration, [Duration]),
                    [LastUpdatedStamp] = @NowTime,
                    [LastUpdatedBy] = @OperatorKey,
                    [State] = ISNULL(@State, [State])
                WHERE [Container] = @Container AND [Identifier] = @Identifier;
        END

        SELECT TOP 1 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[OwnerKey]
          ,[CreatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedStamp]
          ,[LastUpdatedBy]
          ,[State]
        FROM [dbo].[BinaryStorageMetaData]
        WHERE [Identifier] = @Identifier;
    END
END
GO


