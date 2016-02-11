IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAccessCredential]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAccessCredential]
GO

CREATE PROCEDURE [dbo].[sp_QueryAccessCredential](
    @UserKey UNIQUEIDENTIFIER,
    @AccessIdentifier VARCHAR(256),
    @Domain VARCHAR(128),
    @Token VARCHAR(512),
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Count IS NULL OR @Count < 1
        SET @Count = 50;

    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(MAX), @Count) + ' [Key]
      ,[UserKey]
      ,[AccessIdentifier]
      ,[Domain]
      ,[Token]
      ,[TokenExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
  FROM [dbo].[AccessCredential]';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('UserKey','=',CONVERT(NVARCHAR(MAX), @UserKey),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('AccessIdentifier','like',@AccessIdentifier,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Domain','=',@Domain,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Token','=',@Token,1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement +' ORDER BY [Domain],[AccessIdentifier]';
    END

    PRINT @SqlStatement;
    EXECUTE sp_executesql @SqlStatement;
END
GO


