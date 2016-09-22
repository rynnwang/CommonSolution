IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryEnvironmentEndpoint]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryEnvironmentEndpoint]
GO
CREATE PROCEDURE [dbo].[sp_QueryEnvironmentEndpoint] (
    @Key UNIQUEIDENTIFIER,
    @Code NVARCHAR(64),
    @Environment NVARCHAR(64)   
)
AS
SET NOCOUNT ON;
BEGIN
    SELECT [Key]
      ,[Protocol]
      ,[Host]
      ,[Port]
      ,[Version]
      ,[Account]
      ,[Token]
      ,[SecondaryToken]
      ,[Name]
      ,[Code]
      ,[Environment]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[EnvironmentEndpoint]
    WHERE ((@Key IS NULL OR [Key] = @Key)
        OR (@Code IS NULL OR [Code] = @Code)
        OR (@Environment IS NULL OR [Environment] = @Environment))
        AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;
END

GO
