IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QuerySSOAuthorizationPartner]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QuerySSOAuthorizationPartner]
GO

CREATE PROCEDURE [dbo].[sp_QuerySSOAuthorizationPartner](
    @Key [UNIQUEIDENTIFIER],
    @OwnerKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256),
    @Token [NVARCHAR](512),
    @CallbackUrl [NVARCHAR](512)
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
      ,[OwnerKey]
      ,[Name]
      ,[Token]
      ,[CallbackUrl]
      ,[TokenExpiration]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[SSOAuthorizationPartner]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=', CONVERT(NVARCHAR(MAX), @Key) ,1); 
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('OwnerKey','=', CONVERT(NVARCHAR(MAX), @OwnerKey) ,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Token','=', @Token,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CallbackUrl','=',@CallbackUrl,1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO


