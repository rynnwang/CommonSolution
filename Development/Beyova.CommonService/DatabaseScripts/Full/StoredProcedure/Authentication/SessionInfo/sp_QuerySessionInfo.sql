IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QuerySessionInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QuerySessionInfo]
GO

CREATE PROCEDURE [dbo].[sp_QuerySessionInfo](
    @UserKey UNIQUEIDENTIFIER,
    @Token VARCHAR(512),
    @UserAgent VARCHAR(512),
    @IpAddress VARCHAR(64),
    @Platform INT,
    @DeviceType INT,
    @IsExpired BIT,
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    IF @Count IS NULL OR @Count < 1
        SET @Count = 50;

    SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(MAX),@Count) + ' [Token]
      ,[UserKey]
      ,[UserAgent]
      ,[Platform]
      ,[DeviceType]
      ,[IpAddress]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[ExpiredStamp]
    FROM [dbo].[SessionInfo]';

    IF @Token IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Token','=',@Token,1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserKey','=',CONVERT(NVARCHAR(MAX), @UserKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserAgent','=',@UserAgent,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('IpAddress','=',@IpAddress,1);

        IF @Platform IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + '([Platform] & ' + CONVERT(NVARCHAR(MAX), @Platform) + ') > 0 AND ';
        END

        IF @DeviceType IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + '([DeviceType] & ' + CONVERT(NVARCHAR(MAX), @DeviceType) + ') > 0 AND ';
        END

        IF @IsExpired IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + '[ExpiredStamp] ' + (CASE @IsExpired WHEN 1 THEN '<' WHEN 0 THEN '>' END) + '''' + CONVERT(NVARCHAR(MAX), GETUTCDATE(), 121) + ''' AND ';
        END
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        SET @SqlStatement = @SqlStatement + ' ORDER BY [CreatedStamp] DESC';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO


