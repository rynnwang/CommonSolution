IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAdminUserInfoByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetAdminUserInfoByToken]
GO
CREATE PROCEDURE [dbo].[sp_GetAdminUserInfoByToken] (
    @Token VARCHAR(512)
)
AS
BEGIN
    SELECT TOP 1 UI.[Key]
      ,UI.[LoginName]
      ,UI.[Password]
      ,UI.[DisplayName]
      ,UI.[CreatedStamp]
      ,UI.[LastUpdatedStamp]
      ,UI.[State]
    FROM [dbo].[SessionInfo] AS SI
        JOIN [dbo].[AdminUserInfo] AS UI
            ON UI.[Key] = SI.[UserKey] AND [dbo].[fn_ObjectIsWorkable]([State]) = 1
        WHERE SI.[Token] = @Token AND SI.[ExpiredStamp] > GETUTCDATE()
END

GO
