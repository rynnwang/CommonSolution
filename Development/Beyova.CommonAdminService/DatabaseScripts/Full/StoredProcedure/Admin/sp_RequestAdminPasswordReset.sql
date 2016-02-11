IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RequestAdminPasswordReset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_RequestAdminPasswordReset]
GO
CREATE PROCEDURE [dbo].[sp_RequestAdminPasswordReset](
    @LoginName [varchar](64),
    @Expiration [int] -- Hour
)
AS
BEGIN
    DECLARE @UserKey AS UNIQUEIDENTIFIER;
    DECLARE @Token [VARCHAR](512);
    DECLARE @NowTime DATETIME = GETUTCDATE();

    IF @Expiration IS NULL OR @Expiration < 0
        SET @Expiration = 24;

    SELECT @UserKey = [Key]
        FROM [dbo].[AdminUserInfo]
            WHERE [LoginName] = @LoginName 
                AND [dbo].[fn_objectIsWorkable]([State]) = 1;

    IF @UserKey IS NOT NULL
    BEGIN
        SET @Token = REPLACE(
            (CONVERT(VARCHAR(MAX), NEWID()) + CONVERT(VARCHAR(MAX), NEWID()) + CONVERT(VARCHAR(MAX), NEWID()) + CONVERT(VARCHAR(MAX), NEWID())),
            '-',
            '');

        UPDATE [dbo].[AdminUserInfo]
            SET [PasswordResetToken] = @Token,
                [PasswordResetExpiredStamp] = DATEADD(hh, @Expiration, @NowTime)
            WHERE [Key] = @UserKey;

        SELECT @Token AS [Token];
    END
END
GO