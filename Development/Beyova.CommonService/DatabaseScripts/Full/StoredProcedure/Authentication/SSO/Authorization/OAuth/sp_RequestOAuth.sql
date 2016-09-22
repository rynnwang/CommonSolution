IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RequestOAuth]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_RequestOAuth]
GO

CREATE PROCEDURE [dbo].[sp_RequestOAuth](
    @PartnerKey [UNIQUEIDENTIFIER],
    @ClientRequestId [NVARCHAR](512),
    @CallbackUrl [NVARCHAR](512)
)
AS
DECLARE @Key AS [UNIQUEIDENTIFIER];
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @AuthorizationToken AS VARCHAR(512);
BEGIN
    IF @ClientRequestId IS NULL OR @ClientRequestId = ''
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestOAuth',
            @Code = 403,
            @Reason = N'@ClientRequestId',
            @Message = N'Unique ClientRequestId is required for OAuth request.';
        RETURN;
    END

    IF EXISTS (SELECT TOP 1 * FROM [dbo].[SSOAuthorizationPartner]
        WHERE [Key] = @Key
            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1
            AND [CallbackUrl] = @CallbackUrl)
    BEGIN
        SET @Key = NEWID();

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
           ,NULL
           ,NULL
           ,DATEADD(MI,15, @NowTime)
           ,NULL
           ,@NowTime
           ,@NowTime
           ,0);
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_RequestOAuth',
            @Code = 403,
            @Reason = N'[Partner]',
            @Message = N'No Partner is found for specific request.';
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


