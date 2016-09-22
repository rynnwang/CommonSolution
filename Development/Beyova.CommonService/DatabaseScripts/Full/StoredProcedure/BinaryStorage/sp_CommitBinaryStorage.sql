IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryStorage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitBinaryStorage]
GO

CREATE PROCEDURE [dbo].[sp_CommitBinaryStorage](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @Length INT,
    @CommitOption INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @ExistedState AS INT;
DECLARE @OwnerKey AS UNIQUEIDENTIFIER;
DECLARE @InstanceContainer AS VARCHAR(128);
DECLARE @InstanceIdentifier AS UNIQUEIDENTIFIER;
DECLARE @IsDuplicated AS BIT = 0;

DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
BEGIN
    IF @Container IS NOT NULL 
        AND @Identifier IS NOT NULL 
        AND @Hash IS NOT NULL 
        AND @Length IS NOT NULL
        AND @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @ExistedState = [State], @OwnerKey = [CreatedBy] FROM [dbo].[BinaryStorageMetaData] 
            WHERE [Identifier] = @Identifier
                AND [Container] = @Container;

        -- Only when state equals 1 (CommitPending), commit can be processed.
        IF @ExistedState = 1
        BEGIN
            IF (@OwnerKey IS NULL OR @OperatorKey = @OwnerKey)
            BEGIN
                BEGIN TRY
                BEGIN TRANSACTION
                    -- CHECK option
                    IF @CommitOption IS NULL OR @CommitOption = 0
                        SET @CommitOption = 1;

                    IF @CommitOption = 2
                    -- WHEN @CommitOption = 2, THEN need to handle duplicated items.
                    BEGIN
                        SELECT TOP 1 @InstanceContainer = [Container], @InstanceIdentifier = [Identifier]
                            FROM [dbo].[BinaryStorageMetaData] 
                            WHERE [Hash] = @Hash AND [Length] = @Length;

                        IF @InstanceContainer IS NOT NULL
                        BEGIN
                            UPDATE [dbo].[BinaryStorageMetaData]
                                SET [LastUpdatedStamp] = @NowTime,
                                    [LastUpdatedBy] = @OperatorKey,
                                    [State] = 7 --Duplicated
                                WHERE [Identifier] = @Identifier;

                            SET @IsDuplicated= 1;
                        END
                    END

                    IF @IsDuplicated = 0
                    BEGIN
                        SET @InstanceContainer = @Container;
                        SET @InstanceIdentifier = @Identifier;

                        UPDATE [dbo].[BinaryStorageMetaData]
                            SET [Mime] = ISNULL(@Mime, [Mime]),
                                [Hash] = ISNULL(@Hash, [Hash]),
                                [Length] = ISNULL(@Length, [Length]),
                                [LastUpdatedStamp] = @NowTime,
                                [LastUpdatedBy] = @OperatorKey,
                                [State] = 2 --Committed
                            WHERE [Identifier] = @Identifier;
                    END

                    --ANYWAY, NEED TO BIND USER WITH Binary.
                    IF NOT EXISTS (SELECT TOP 1 * FROM [dbo].[UserBinaryStorageMetaData]
                        WHERE [Identifier] = @InstanceIdentifier 
                            AND [OwnerKey] = @OperatorKey
                            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1 )
                    BEGIN
                    INSERT INTO [dbo].[UserBinaryStorageMetaData]
                           ([Identifier]
                           ,[OwnerKey]
                           ,[CreatedStamp]
                           ,[LastUpdatedStamp]
                           ,[CreatedBy]
                           ,[LastUpdatedBy]
                           ,[State])
                        VALUES
                           (@InstanceIdentifier
                           ,@OperatorKey
                           ,@NowTime
                           ,@NowTime
                           ,@OperatorKey
                           ,@OperatorKey
                           ,0);
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
                      ,@OperatorKey AS [OwnerKey]
                      ,[CreatedStamp]
                      ,[CreatedBy]
                      ,[LastUpdatedStamp]
                      ,[LastUpdatedBy]
                      ,[State]
                    FROM [dbo].[BinaryStorageMetaData]
                    WHERE [Identifier] = @InstanceIdentifier;

                    COMMIT TRANSACTION
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                    SET @ErrerMessage = ERROR_MESSAGE();
                    SET @ErrorSeverity = ERROR_SEVERITY();
                    SET @ErrorState = ERROR_STATE();
                    SET @ErrorCode = ERROR_NUMBER();
                    RAISERROR(@ErrorCode, @ErrorSeverity,@ErrorState, @ErrerMessage);
                END CATCH
            END
            ELSE
            BEGIN
                EXEC [dbo].[sp_ThrowException]
                    @Name = N'sp_CommitBinaryStorage',
                    @Code = 403,
                    @Reason = N'[OwnerKey]',
                    @Message = N'Owner specified is not who create this binary meta.';
                RETURN;
            END
        END
        ELSE
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CommitBinaryStorage',
                @Code = 403,
                @Reason = N'[State]',
                @Message = N'Binary object can be committed only when state is pending commit.';
            RETURN;
        END
    END
END
GO


