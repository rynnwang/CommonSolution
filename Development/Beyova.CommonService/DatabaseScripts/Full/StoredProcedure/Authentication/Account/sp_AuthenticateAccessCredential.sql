IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AuthenticateAccessCredential]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AuthenticateAccessCredential]
GO

CREATE PROCEDURE [dbo].[sp_AuthenticateAccessCredential](
    @AccessIdentifier VARCHAR(256),
    @Domain VARCHAR(128),
    @Token VARCHAR(512)
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 1 [Key]
      ,[UserKey]
      ,[AccessIdentifier]
      ,[Domain]
      ,[Token]
      ,[TokenExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
  FROM [dbo].[AccessCredential]
  WHERE [AccessIdentifier] = @AccessIdentifier 
    AND [Domain] = ISNULL(@Domain, '')
    AND [Token] = @Token
    AND ([TokenExpiredStamp] IS NULL OR [TokenExpiredStamp] > GETUTCDATE());
END
GO


