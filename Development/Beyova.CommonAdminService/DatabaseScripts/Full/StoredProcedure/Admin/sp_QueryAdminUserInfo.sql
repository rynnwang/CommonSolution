IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAdminUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAdminUserInfo]
GO

CREATE PROCEDURE [dbo].[sp_QueryAdminUserInfo](
    @Key UNIQUEIDENTIFIER,
    @LoginName [varchar](64),
    @DisplayName [nvarchar](64)
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
    ,[LoginName]
    ,NULL AS [Password]
    ,[DisplayName]
    ,[CreatedStamp]
    ,[LastUpdatedStamp]
    ,[State]
FROM [dbo].[AdminUserInfo]';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LoginName','=',@LoginName,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('DisplayName','=',@DisplayName,1);
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    EXECUTE sp_executesql @SqlStatement;
END
GO


