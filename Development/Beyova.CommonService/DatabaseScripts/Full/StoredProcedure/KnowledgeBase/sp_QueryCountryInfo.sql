IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryCountryInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryCountryInfo]
GO

CREATE PROCEDURE [dbo].[sp_QueryCountryInfo](
    @Key UNIQUEIDENTIFIER,
    @Code [VARCHAR](8),
    @Name [NVARCHAR](128),
    @CultureCode [nvarchar](16)
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX);

    SET @WhereStatement = 'dbo.fn_ObjectIsVisible([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
      ,[Name]
      ,[ISO2Code]
      ,[ISO3Code]
      ,[TelCode]
      ,[TimeZone]
      ,[CurrencyCode]
      ,[GeographyKey]
      ,[CultureCode]
      ,[Sequence]
      ,[State]
    FROM [dbo].[view_CountryInfo]';

    SET @Code = REPLACE(@Code, '''', '''''');
    SET @Code = REPLACE(@Code, '--', '');
    SET @Code = REPLACE(@Code, '/*', '');
    SET @Code = REPLACE(@Code, '*/', '');

    IF @Key IS NOT NULL
        SET @WhereStatement = '[Key] = ''' + CONVERT(NVARCHAR(MAX), @Key) + ''' AND ';
    ELSE
    BEGIN
        IF(@Code IS NOT NULL AND @Code <> '')
            SET @WhereStatement = @WhereStatement + '([ISO2Code] = ''' + @Code + ''' OR [ISO3Code] = ''' + @Code + ''' OR [TelCode] = ''' + @Code + ''') AND ';
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','like',@Name,1);
    END

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CultureCode','=',@CultureCode,1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement +' ORDER BY [Sequence] DESC, [Name]';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO


