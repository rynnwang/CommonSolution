IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExchangeToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExchangeToken]
GO

CREATE PROCEDURE [dbo].[sp_ExchangeToken](
    @ClientRequestId [NVARCHAR](512),
    @AuthorizationToken [NVARCHAR](512)
)
AS
DECLARE @Key AS UNIQUEIDENTIFIER;
DECLARE @UserKey AS UNIQUEIDENTIFIER;
DECLARE @TokenExpiration AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();

DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
BEGIN
    SELECT TOP 1 @Key = A.[Key], @UserKey = A.[UserKey], @TokenExpiration = AP.[TokenExpiration]
        FROM [dbo].[SSOAuthorization] AS A
            JOIN [dbo].[SSOAuthorizationPartner] AS AP
                ON AP.[Key] = A.[PartnerKey] AND [dbo].[fn_ObjectIsWorkable](AP.[State]) = 1
            WHERE A.[ClientRequestId] = @ClientRequestId
                AND A.[AuthorizationToken] = @AuthorizationToken
                AND A.[UserKey] IS NOT NULL --ensure record is for OAuth, not for token exchange.
                AND A.[ExpiredStamp] > @NowTime
                AND A.[UsedStamp] IS NULL;

    IF @UserKey IS NOT NULL
    BEGIN
        BEGIN TRY
        BEGIN TRANSACTION
            DECLARE @Token AS VARCHAR(512) = 
                REPLACE(
                    CONVERT(VARCHAR(64), NEWID()) 
                    + CONVERT(VARCHAR(64), NEWID()) 
                    + CONVERT(VARCHAR(64), NEWID()) 
                    + CONVERT(VARCHAR(64), NEWID()), '-','');

            INSERT INTO [dbo].[SessionInfo]
                   ([Token]
                   ,[UserKey]
                   ,[UserAgent]
                   ,[Platform]
                   ,[DeviceType]
                   ,[IpAddress]
                   ,[CreatedStamp]
                   ,[LastUpdatedStamp]
                   ,[ExpiredStamp])
             VALUES
                   (@Token
                   ,@UserKey
                   ,NULL
                   ,0
                   ,0
                   ,NULL
                   ,@NowTime
                   ,@NowTime
                   ,DATEADD(MI,ISNULL(@TokenExpiration, 30), @NowTime)
                   );

            UPDATE [dbo].[SSOAuthorization]
                SET [UsedStamp] = @NowTime,
                    [LastUpdatedStamp] = @NowTime
                WHERE [Key] = @Key;

            SELECT TOP 1 [Token]
                ,[UserKey]
                ,[UserAgent]
                ,[Platform]
                ,[DeviceType]
                ,[IpAddress]
                ,[CreatedStamp]
                ,[LastUpdatedStamp]
                ,[ExpiredStamp]
            FROM [dbo].[SessionInfo]
            WHERE [Token] = @Token;

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
            @Name = N'sp_ExchangeToken',
            @Code = 403,
            @Reason = N'@ClientRequestId/@AuthorizationToken',
            @Message = N'Invalid AuthorizationToken or/and ClientRequestId  for SSO Token Exchange.';
        RETURN;
    END
END
GO


