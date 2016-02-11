IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryUserInfo]
GO
CREATE PROCEDURE [dbo].[sp_QueryUserInfo] (
    @Key UNIQUEIDENTIFIER,
    @DisplayName [nvarchar](64),
    @FirstName [nvarchar](64),
    @LastName [nvarchar](64),
    @UserId [varchar](128),
    @Email [varchar](128),
    @FunctionalRole INT,
    @SchoolCode [nvarchar](64),
    @GroupKey UNIQUEIDENTIFIER,
    @MarketRegion INT,
    @CurrentBookKey [UNIQUEIDENTIFIER],
    @StartIndex INT,
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX);

    SET @WhereStatement = 'dbo.fn_ObjectIsVisible([State]) = 1 AND ';

    IF @Count IS NULL OR @Count < 1
        SET @Count = 100;

    IF @StartIndex IS NULL OR @StartIndex < 0
        SET @StartIndex = 0;

    SET @SqlStatement = 'SELECT [Key]
      ,[UserId]
      ,[DisplayName]
      ,[Gender]
      ,[EnglishFirstName]
      ,[EnglishMiddleName]
      ,[EnglishLastName]
      ,[ChineseFirstName]
      ,[ChineseLastName]
      ,[Email]
      ,[AvatarKey]
      ,[AvatarUrl]
      ,[Container]
      ,[Identifier]
      ,[FunctionalRole]
      ,[Language]
      ,[TimeZone]
      ,[GroupKey]
      ,[MarketRegion]
      ,[CurrentBookKey]
      ,[GroupId]
      ,[GroupName]
      ,[GroupCode]
      ,[SchoolCode]
      ,[ProductCode]
      ,[OdinCourseCode]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_UserInfo]';

    IF @Key IS NOT NULL
        SET @WhereStatement = '[Key] = ''' + CONVERT(NVARCHAR(MAX), @Key) + ''' AND ';
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Email','=',@Email,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserId','=',@UserId,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('DisplayName','like',@DisplayName,1);

        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('EnglishFirstName','like',@FirstName,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ChineseFirstName','like',@FirstName,1);

        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('EnglishLastName','like',@LastName,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ChineseLastName','like',@LastName,1);

        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('SchoolCode','=',@SchoolCode,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('GroupKey','=',CONVERT(NVARCHAR(MAX), @GroupKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('CurrentBookKey','=',CONVERT(NVARCHAR(MAX), @CurrentBookKey),1);
        
        IF @FunctionalRole IS NOT NULL
            SET @WhereStatement = @WhereStatement + '([FunctionalRole] & ' + CONVERT(NVARCHAR(MAX),@FunctionalRole) + ') > 0 AND ';

        IF @MarketRegion IS NOT NULL
            SET @WhereStatement = @WhereStatement + '([MarketRegion] & ' + CONVERT(NVARCHAR(MAX),@MarketRegion) + ') > 0 AND ';
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    SET  @SqlStatement = @SqlStatement +' ORDER BY [DisplayName] OFFSET (' + CONVERT(NVARCHAR(MAX), @StartIndex) + ') ROW FETCH NEXT ' + CONVERT(NVARCHAR(MAX), @Count) + ' ROWS ONLY;';

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;

END

GO
