IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateSSOAuthorizationPartner]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateSSOAuthorizationPartner]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateSSOAuthorizationPartner](
    @Key [UNIQUEIDENTIFIER],
    @OwnerKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256),
    @Token [NVARCHAR](512),
    @CallbackUrl [NVARCHAR](512),
    @TokenExpiration INT,
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @ExistedState AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    SELECT TOP 1 @ExistedState = [State] FROM [dbo].[SSOAuthorizationPartner] WHERE [Key] = @Key;

    IF @ExistedState IS NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[SSOAuthorizationPartner]
           ([Key]
           ,[OwnerKey]
           ,[Name]
           ,[Token]
           ,[CallbackUrl]
           ,[TokenExpiration]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     VALUES
           (@Key
           ,@OwnerKey
           ,@Name
           ,@Token
           ,@CallbackUrl
           ,CASE WHEN @TokenExpiration IS NULL OR @TokenExpiration < 1 THEN 30 ELSE @TokenExpiration END
           ,@NowTime
           ,@NowTime
           ,@OperatorKey
           ,@OperatorKey
           ,0);
    END
    ELSE IF [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
    BEGIN
        UPDATE [dbo].[SSOAuthorizationPartner]
           SET [Name] = ISNULL(@Name, [Name])
           ,[CallbackUrl] = ISNULL(@CallbackUrl, [CallbackUrl])
           ,[TokenExpiration] = ISNULL(@TokenExpiration, [TokenExpiration])
           ,[Token] = ISNULL(@Token, [Token])    
           ,[LastUpdatedStamp] = @NowTime
           ,[LastUpdatedBy] = @OperatorKey
        WHERE [Key] = @Key;
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_CreateOrUpdateSSOAuthorizationPartner',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Update operation is forbidden caused by state.';
        RETURN;
    END

    SELECT @Key;
   
END
GO


