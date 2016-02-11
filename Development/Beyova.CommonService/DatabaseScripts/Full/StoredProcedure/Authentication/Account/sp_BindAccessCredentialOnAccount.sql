IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BindAccessCredentialOnAccount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_BindAccessCredentialOnAccount]
GO

CREATE PROCEDURE [dbo].[sp_BindAccessCredentialOnAccount](
    @AccessIdentifier VARCHAR(256),
    @Domain VARCHAR(128),
    @Token VARCHAR(512),
    @TokenExpiredStamp DATETIME,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @Key AS UNIQUEIDENTIFIER;
    DECLARE @ExistedOwnerKey AS UNIQUEIDENTIFIER;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    SET @Domain = ISNULL(@Domain, '');

    SELECT TOP 1 @Key = [Key], @ExistedOwnerKey = [UserKey]
        FROM [dbo].[AccessCredential]
        WHERE [AccessIdentifier] = @AccessIdentifier 
            AND [Domain] = @Domain;

    IF @ExistedOwnerKey IS NOT NULL AND @ExistedOwnerKey <> @OperatorKey
        RAISERROR(60403, 16, 1, 'Action is forbidden by different owner.');

    IF @Key IS NOT NULL
    BEGIN
        UPDATE [dbo].[AccessCredential]
            SET [Token] = @Token,
                [TokenExpiredStamp] = @TokenExpiredStamp,
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy] = @OperatorKey
            WHERE [Key] = @Key;
    END
    ELSE
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
            ,@OperatorKey
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

    SELECT @Key;
END
GO
