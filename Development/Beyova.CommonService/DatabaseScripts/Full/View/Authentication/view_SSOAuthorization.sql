IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_SSOAuthorization]') AND type in (N'V'))
DROP VIEW [dbo].[view_SSOAuthorization]
GO

CREATE VIEW [dbo].[view_SSOAuthorization]
AS

SELECT A.[Key]
      ,A.[PartnerKey]
      ,A.[ClientRequestId]
      ,A.[AuthorizationToken]
      ,A.[UserKey]
      ,A.[ExpiredStamp]
      ,A.[UsedStamp]
      ,AP.[CallbackUrl]
      ,A.[CreatedStamp]
      ,A.[LastUpdatedStamp]
      ,A.[State]
    FROM [dbo].[SSOAuthorization] AS A
        JOIN [dbo].[SSOAuthorizationPartner] AS AP
            ON AP.[Key] = A.[PartnerKey]
                AND [dbo].[fn_ObjectIsWorkable](AP.[State]) = 1
    WHERE [dbo].[fn_ObjectIsWorkable](A.[State]) = 1


GO
