IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAdminUserInfoByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetAdminUserInfoByToken]
GO

CREATE PROCEDURE [dbo].[sp_GetAdminUserInfoByToken](
    @Token [varchar](512)
)
AS
BEGIN
    SELECT TOP 1 AUI.[Key]
      ,AUI.[LoginName]
      ,AUI.[Password]
      ,AUI.[DisplayName]
      ,NULL AS [Email]
      ,NULL AS [PasswordResetToken]
      ,NULL AS [PasswordResetExpiredStamp]
      ,AUI.[CreatedStamp]
      ,AUI.[LastUpdatedStamp]
      ,AUI.[State]
    FROM [dbo].[AdminUserInfo] AS [AUI]
    JOIN [dbo].[AdminSession] AS [AS]
        ON [AS].[UserKey] = AUI.[Key] AND [AS].[ExpiredStamp] > GETUTCDATE() AND [dbo].[fn_ObjectIsWorkable]([AS].[State]) = 1
    WHERE [AS].[Token] = @Token 
        AND [dbo].[fn_ObjectIsWorkable](AUI.[State]) = 1;
END
GO


