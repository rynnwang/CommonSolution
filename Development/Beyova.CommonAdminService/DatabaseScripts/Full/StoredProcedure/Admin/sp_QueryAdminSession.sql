IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAdminSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAdminSession]
GO

CREATE PROCEDURE [dbo].[sp_QueryAdminSession](
    @UserKey UNIQUEIDENTIFIER,
    @Token [varchar](512),
    @IpAddress [varchar](64),
    @ActiveOnly BIT
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Token]
      ,[UserKey]
      ,[IpAddress]
      ,[UserAgent]
      ,[ExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
  FROM [dbo].[AdminSession]';

    IF @Token IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Token','=',@Token,1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserKey','=',CONVERT(VARCHAR(MAX), @UserKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('IpAddress','=',@IpAddress,1);
    END

     IF @ActiveOnly = 1
            SET @WhereStatement = @WhereStatement + '[ExpiredStamp] > GETUTCDATE() AND ';

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    EXECUTE sp_executesql @SqlStatement;
END
GO


