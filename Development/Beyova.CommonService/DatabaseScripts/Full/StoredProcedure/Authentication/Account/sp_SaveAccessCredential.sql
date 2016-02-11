IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveAccessCredential]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SaveAccessCredential]
GO

CREATE PROCEDURE [dbo].[sp_SaveAccessCredential](
    @UserKey uniqueidentifier,
    @AccessIdentifier varchar(256),
    @Domain varchar(128),
    @Token varchar(512),
    @TokenExpiredStamp datetime,
    @OperatorKey uniqueidentifier
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @Key AS UNIQUEIDENTIFIER;
    DECLARE @ExistedUserKey AS UNIQUEIDENTIFIER;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @AccessIdentifier IS NOT NULL AND @UserKey IS NOT NULL
    BEGIN

        SET @Domain = ISNULL(@Domain, '');

        SELECT TOP 1 @ExistedUserKey = [UserKey], @Key = [Key]
            FROM [dbo].[AccessCredential]
            WHERE [AccessIdentifier] = @AccessIdentifier;
        
        IF @ExistedUserKey IS NULL
        BEGIN
            SET @Key = NEWID();

            INSERT INTO [dbo].[AccessCredential]
               ([Key]
               ,[UserKey]
               ,[AccessIdentifier]
               ,[Domain]
               ,[Token]
               ,[TokenExpiredStamp]
               ,[CreatedStamp]
               ,[LastUpdatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedBy]
               ,[State])
            VALUES
                (@Key
                ,@UserKey
                ,@AccessIdentifier
                ,@Domain
                ,@Token
                ,@TokenExpiredStamp
                ,@NowTime
                ,@NowTime
                ,@OperatorKey
                ,@OperatorKey
                ,0);
        END 
        ELSE IF @ExistedUserKey = @UserKey
        BEGIN
            UPDATE [dbo].[AccessCredential]
                SET [Token] = @Token,
                    [TokenExpiredStamp] = @TokenExpiredStamp,
                    [LastUpdatedStamp] = @NowTime,
                    [LastUpdatedBy] = @OperatorKey
                WHERE [Key] = @Key;
        END
        ELSE
            RAISERROR(60403, 16, 1, 'Create or update operation is forbidden caused by ownership confliction.');
    END
END
GO


