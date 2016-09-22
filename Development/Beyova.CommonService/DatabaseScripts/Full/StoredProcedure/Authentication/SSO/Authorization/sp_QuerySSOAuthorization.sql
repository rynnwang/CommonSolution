IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QuerySSOAuthorization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QuerySSOAuthorization]
GO

CREATE PROCEDURE [dbo].[sp_QuerySSOAuthorization](
    @Key [UNIQUEIDENTIFIER],
    @PartnerKey [UNIQUEIDENTIFIER],
    @UserKey [UNIQUEIDENTIFIER],
    @AuthorizationToken [NVARCHAR](512),
    @IsUsed BIT,
    @StartIndex INT,
    @Count INT
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX);

   SET @WhereStatement = '[dbo].[fn_ObjectIsVisible]([State]) = 1 AND ';

    IF @Count IS NULL OR @Count < 1
        SET @Count = 100;

    IF @StartIndex IS NULL OR @StartIndex < 0
        SET @StartIndex = 0;

    SET @SqlStatement = 'SELECT TOP ' +  CONVERT(NVARCHAR(MAX), @Count) + ' [Key]
      ,[PartnerKey]
      ,[ClientRequestId]
      ,[AuthorizationToken]
      ,[UserKey]
      ,[ExpiredStamp]
      ,[UsedStamp]
      ,[CallbackUrl]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[view_SSOAuthorization]';

    IF @Key IS NOT NULL
        SET @WhereStatement = '[Key] = ''' + CONVERT(NVARCHAR(MAX), @Key) + ''' AND ';
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('PartnerKey','=',CONVERT(NVARCHAR(MAX), @PartnerKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserKey','=',CONVERT(NVARCHAR(MAX), @UserKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('AuthorizationToken','=',@AuthorizationToken,1);

        IF @IsUsed IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + '[UsedStamp] IS ' + (CASE WHEN @IsUsed = 1 THEN 'NOT ' ELSE '' END) + 'NULL AND ';
        END
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    SET  @SqlStatement = @SqlStatement +' ORDER BY [CreatedStamp] DESC OFFSET (' + CONVERT(NVARCHAR(MAX), @StartIndex) + ') ROW FETCH NEXT ' + CONVERT(NVARCHAR(MAX), @Count) + ' ROWS ONLY;';

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
   
END
GO


