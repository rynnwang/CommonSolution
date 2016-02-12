IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_UserInfoWIthPermission]') AND type in (N'V'))
DROP VIEW [dbo].[view_UserInfoWIthPermission]
GO

CREATE VIEW [dbo].[view_UserInfoWIthPermission]
AS

SELECT AUI.[Key]
      ,AUI.[LoginName]
      ,AUI.[Name]
      ,AUI.[Email]
      ,AUI.[ThirdPartyId]
      ,AUI.[CreatedStamp]
      ,AUI.[LastUpdatedStamp]
      ,AUI.[CreatedBy]
      ,AUI.[LastUpdatedBy]
      ,AUI.[State]
    FROM [dbo].[AdminUserInfo] AS AUI
        LEFT JOIN [dbo].[AdminUserRoleBinding] AS AUR
            ON AUR.[UserKey] = AUI.[Key] AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1
        LEFT JOIN [dbo].[AdminUserRoleBinding] AS AUR
            ON AUR.[UserKey] = AUI.[Key] AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1

GO
