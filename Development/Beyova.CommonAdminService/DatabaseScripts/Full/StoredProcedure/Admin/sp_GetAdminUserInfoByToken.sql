IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAdminUserInfoByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetAdminUserInfoByToken]
GO

CREATE PROCEDURE [dbo].[sp_GetAdminUserInfoByToken](
    @Token [varchar](512)
)
AS
BEGIN
    DECLARE @UserKey AS UNIQUEIDENTIFIER;
    DECLARE @PermissionXml AS XML;

    SELECT TOP 1 @UserKey = [UserKey]
        FROM [dbo].[AdminSession] 
        WHERE [Token] = @Token  AND [ExpiredStamp] > GETUTCDATE();

    IF @UserKey IS NOT NULL
    BEGIN
        WITH temp AS (
            SELECT AR1.[Key] FROM [dbo].[AdminUserRoleBinding] AS AUR
                JOIN [dbo].[AdminRole] AS AR1
                    ON AUR.[RoleKey] = AR1.[Key] AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1 
                WHERE AUR.[UserKey] = @UserKey
            UNION ALL
            SELECT AR.[Key] FROM [dbo].[AdminRole] AR
            INNER JOIN temp T ON AR.[ParentKey] = T.[Key]
        )
        SELECT @PermissionXml =
            ISNULL((SELECT  AP.[Identifier] AS [Item] 
            FROM temp
                JOIN [dbo].[AdminRolePermissionBinding] AS ARP
                    ON ARP.[RoleKey] = temp.[Key] AND [dbo].[fn_ObjectIsWorkable](ARP.[State]) = 1 
                JOIN [dbo].[AdminPermission] AS AP
                    ON ARP.[PermissionKey] = AP.[Key] AND [dbo].[fn_ObjectIsWorkable](AP.[State]) = 1
                    FOR XML PATH('')), '');

        SELECT [Key]
          ,[LoginName]
          ,[Name]
          ,[Email]
          ,[ThirdPartyId]
          ,('<List>' + CONVERT(NVARCHAR(MAX), @PermissionXml) + '</List>') AS [Permission]
          ,[AvatarKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedBy]
          ,[State]
    FROM [dbo].[AdminUserInfo]
    WHERE [Key] = @UserKey;

    END
END
GO


