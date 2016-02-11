IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateProvisioningObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateProvisioningObject]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateProvisioningObject](
    @Application INT,
    @Module NVARCHAR(128),
    @Name NVARCHAR(256),
    @Value NVARCHAR(MAX),
    @OwnerKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    
    DECLARE @ErrerMessage AS NVARCHAR(MAX);
    DECLARE @ErrorSeverity AS INT;
    DECLARE @ErrorState AS INT;
    DECLARE @ErrorCode AS INT;

    DECLARE @Key AS UNIQUEIDENTIFIER;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Module IS NOT NULL
    BEGIN
        SET @Module = ISNULL(@Module, '');

        BEGIN TRY
        BEGIN TRANSACTION
            SELECT @Key = [Key] FROM [dbo].[ProvisioningObject]
                WHERE ([OwnerKey] = @OwnerKey OR ([OwnerKey] IS NULL AND @OwnerKey IS NULL))
                AND ([Application] = @Application OR ([Application] IS NULL AND @Application IS NULL))
                AND ([Module] = @Module OR ([Module] IS NULL AND @Module IS NULL))
                AND [Name] = @Name
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;

            IF @Key IS NOT NULL
                UPDATE [dbo].[ProvisioningObject]
                    SET [State] = [dbo].[fn_SetObjectDeleted]([State])
                    WHERE [Key] = @Key
                        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

            IF @Value IS NOT NULL
            BEGIN
                SET @Key = NEWID();
                INSERT INTO [dbo].[ProvisioningObject]
                    ([Key]
                    ,[Application]
                    ,[Module]
                    ,[Name]
                    ,[Value]
                    ,[OwnerKey]
                    ,[CreatedStamp]
                    ,[LastUpdatedStamp]
                    ,[CreatedBy]
                    ,[LastUpdatedBy]
                    )
                VALUES
                    (@Key
                    ,@Application
                    ,@Module
                    ,@Name
                    ,@Value
                    ,@OwnerKey
                    ,@NowTime
                    ,@NowTime
                    ,@OperatorKey
                    ,@OperatorKey
                    );
            END           

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

   
END
GO


