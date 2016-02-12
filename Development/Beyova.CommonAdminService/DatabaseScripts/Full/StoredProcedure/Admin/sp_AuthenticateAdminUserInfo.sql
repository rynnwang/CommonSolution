IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AuthenticateAdminUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AuthenticateAdminUserInfo]
GO

CREATE PROCEDURE [dbo].[sp_AuthenticateAdminUserInfo](
    @LoginName [NVARCHAR](64),
    @Password [NVARCHAR](256)
)
AS
BEGIN
    SELECT TOP 1 [Key]
      ,[LoginName]
      ,[Password]
      ,[DisplayName]
      ,[Email]
      ,NULL AS [PasswordResetToken]
      ,NULL AS [PasswordResetExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
  FROM [dbo].[AdminUserInfo]
    WHERE [LoginName] = @LoginName 
        AND [Password] = @Password
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;
END
GO


