IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RequestTokenExchange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_RequestTokenExchange]
GO

CREATE PROCEDURE [dbo].[sp_RequestTokenExchange](
    @PartnerKey [UNIQUEIDENTIFIER],
    @ClientRequestId [NVARCHAR](512),
    @UserKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @Key AS [UNIQUEIDENTIFIER];
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @AuthorizationToken AS VARCHAR(512);
BEGIN
    IF @ClientRequestId IS NULL OR LTRIM(RTRIM(@ClientRequestId)) = '' OR EXISTS (SELECT TOP 1 * FROM [dbo].[SSOAuthorization]
        WHERE [ClientRequestId] = @ClientRequestId)
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestTokenExchange',
            @Code = 403,
            @Reason = N'@ClientRequestId',
            @Message = N'Unique ClientRequestId is required for SSO Token Exchange request.';
        RETURN;
    END

    IF @UserKey IS NULL OR NOT EXISTS (SELECT TOP 1 * FROM [dbo].[UserInfo]
        WHERE [Key] = @UserKey AND [dbo].[fn_ObjectIsWorkable]([State]) = 1)
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestTokenExchange',
            @Code = 403,
            @Reason = N'@UserKey',
            @Message = N'Valid userKey is required for SSO Token Exchange request.';
        RETURN;
    END

    IF EXISTS (SELECT TOP 1 * FROM [dbo].[SSOAuthorizationPartner] WHERE [Key] = @PartnerKey AND [dbo].[fn_ObjectIsWorkable]([State]) = 1)
    BEGIN
        SET @Key = NEWID();

        SET @AuthorizationToken = 
            REPLACE(
                CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()), '-','');

        INSERT INTO [dbo].[SSOAuthorization]
           ([Key]
           ,[PartnerKey]
           ,[ClientRequestId]
           ,[AuthorizationToken]
           ,[UserKey]
           ,[ExpiredStamp]
           ,[UsedStamp]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Key
           ,@PartnerKey
           ,@ClientRequestId
           ,@AuthorizationToken
           ,@UserKey
           ,DATEADD(MI,15, @NowTime)
           ,NULL
           ,@NowTime
           ,@NowTime
           ,0);
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestTokenExchange',
            @Code = 403,
            @Reason = N'[Partner]',
            @Message = N'No partner is found for specific request.';
        RETURN;
    END

    SELECT TOP 1 [Key]
      ,[PartnerKey]
      ,[ClientRequestId]
      ,[AuthorizationToken]
      ,[UserKey]
      ,[ExpiredStamp]
      ,[UsedStamp]
      ,[CallbackUrl]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[view_SSOAuthorization]
    WHERE [Key] = @Key;
END
GO


