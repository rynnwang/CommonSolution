IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryUserPreference]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryUserPreference]
GO

CREATE PROCEDURE [dbo].[sp_QueryUserPreference](
    @OwnerKey UNIQUEIDENTIFIER,
    @Realm NVARCHAR(256),
    @Category NVARCHAR(256),
    @Identifier NVARCHAR(256)
)
AS

BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    CREATE TABLE #UserPreference(
        [Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWID(),
        [OwnerKey] [uniqueidentifier] NULL, -- IF IS NULL, then it is default value.
        [AppId] [varchar](128) NOT NULL DEFAULT '',
        [Identifier] [varchar](128) NOT NULL DEFAULT '',
        [Value] [NVARCHAR](max) NOT NULL DEFAULT '',
        [CreatedStamp] [datetime] NOT NULL DEFAULT GETUTCDATE(),
        [LastUpdatedStamp] [datetime] NOT NULL DEFAULT GETUTCDATE(),
        [State] [int] NOT NULL DEFAULT 0,
    );

    SET @SqlStatement = 'INSERT INTO #UserPreference([Key]
      ,[OwnerKey]
      ,[AppId]
      ,[Identifier]
      ,[Value]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State])
    SELECT [Key]
      ,[OwnerKey]
      ,[AppId]
      ,[Identifier]
      ,[Value]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
      FROM [dbo].[UserPreference]';

    SET @WhereStatement = dbo.[fn_GenerateWherePattern]('OwnerKey','=',CONVERT(NVARCHAR(MAX),@OwnerKey),1);
    IF @WhereStatement = ''
        SET @WhereStatement = '[OwnerKey] IS NULL AND ';
    ELSE
        SET @WhereStatement = '([OwnerKey] IS NULL OR ' + SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3) + ') AND ';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('AppId','=',@AppId,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Identifier','=',@Identifier,1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement ;
    END

    EXECUTE sp_executesql @SqlStatement;

    -- DELETE duplicated items if user based preference has already existed.
    DELETE UP1 
        FROM #UserPreference AS UP1
        JOIN #UserPreference AS UP2
        ON UP1.[AppId] = UP2.[AppId]
            AND UP1.[Identifier] = UP2.[Identifier]
            WHERE UP1.[OwnerKey] IS NULL AND UP2.[OwnerKey] IS NOT NULL;

    SELECT [Key]
      ,[OwnerKey]
      ,[AppId]
      ,[Identifier]
      ,[Value]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
      FROM #UserPreference;

    DROP TABLE #UserPreference;
END
GO


