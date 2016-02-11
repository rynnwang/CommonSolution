IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUserByToken]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetUserByToken]
GO

CREATE PROCEDURE [dbo].[sp_GetUserByToken](
    @Token VARCHAR(512)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 U.[Key]
      ,U.[UserId]
      ,U.[DisplayName]
      ,U.[Gender]
      ,U.[EnglishFirstName]
      ,U.[EnglishMiddleName]
      ,U.[EnglishLastName]
      ,U.[ChineseFirstName]
      ,U.[ChineseLastName]
      ,U.[Email]
      ,U.[AvatarKey]
      ,U.[AvatarUrl]
      ,U.[Container]
      ,U.[Identifier]
      ,U.[FunctionalRole]
      ,U.[Language]
      ,U.[TimeZone]
      ,U.[GroupKey]
      ,U.[MarketRegion]
      ,U.[CurrentBookKey]
      ,U.[CreatedStamp]
      ,U.[LastUpdatedStamp]
      ,U.[CreatedBy]
      ,U.[LastUpdatedBy]
      ,U.[State]
    FROM [dbo].[view_UserInfo] AS U
    JOIN [dbo].[SessionInfo] AS S
        ON S.[UserKey] = U.[Key] AND [ExpiredStamp] > GETUTCDATE()
    WHERE S.[Token] = @Token AND [dbo].[fn_ObjectIsWorkable](U.[State]) = 1;
END
GO


