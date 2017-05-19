IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExportTableValueToJSON]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ExportTableValueToJSON]
GO

CREATE PROCEDURE [dbo].[sp_ExportTableValueToJSON] (
    @TableName NVARCHAR(400),
    @Environment NVARCHAR(256),
    @AuthoringIdentifier NVARCHAR(1024)
)
AS
BEGIN
DECLARE @hasEnvironment BIT;
DECLARE @hasAuthoringIdentifier BIT;
DECLARE @columnName NVARCHAR(MAX);
DECLARE @columnType NVARCHAR(MAX);
DECLARE @max_Length INT;
DECLARE @tableColumns NVARCHAR(MAX)='';
DECLARE @SqlStatement NVARCHAR(MAX);
DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

DECLARE Column_Cursor CURSOR
                    LOCAL
                    FORWARD_ONLY
                    STATIC
                    READ_ONLY 
         FOR
                SELECT COLUMN_NAME, CHARACTER_MAXIMUM_LENGTH
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = @TableName
                        AND COLUMN_NAME <> 'Identity';

OPEN Column_Cursor;

FETCH NEXT FROM Column_Cursor INTO @columnName,@max_Length;

WHILE @@FETCH_STATUS=0

BEGIN  
     
    IF(@columnName = 'Environment')
    BEGIN
        SET @hasEnvironment = 1;
    END

    IF(@columnName = 'AuthoringIdentifier')
    BEGIN
        SET @hasAuthoringIdentifier = 1;
    END

    SET @tableColumns=@tableColumns+CASE WHEN  @max_Length=-1 THEN 'JSON_QUERY(['+@columnName+']) ['+@columnName+']' ELSE '['+@columnName+']' END +',';

    FETCH NEXT FROM Column_Cursor INTO @columnName,@max_Length;

END
CLOSE Column_Cursor;
DEALLOCATE Column_Cursor;
    IF @tableColumns<>''
    BEGIN

        SET @tableColumns=SUBSTRING(@tableColumns,0,LEN(@tableColumns)); 
        SET @SqlStatement=N'SELECT ' + @tableColumns + ' FROM [dbo].['+@TableName + ']';

        IF  @Environment IS NOT NULL AND @hasEnvironment=1
        BEGIN
            SET @WhereStatement=@WhereStatement+dbo.[fn_GenerateWherePattern]('[Environment]','=',@Environment,1);
        END

        IF @AuthoringIdentifier IS NOT NULL AND @hasAuthoringIdentifier=1
        BEGIN
            SET @WhereStatement=@WhereStatement+dbo.[fn_GenerateWherePattern]('[AuthoringIdentifier]','=',@AuthoringIdentifier,1);
        END


        IF(@WhereStatement <> '')
        BEGIN
            SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
            SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
        END

        SET @SqlStatement=@SqlStatement +N' FOR JSON PATH,INCLUDE_NULL_VALUES ';
        EXEC SP_EXECUTESQL  @SqlStatement;
    END

END

GO