IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ImportTableValueUsingJSON]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_ImportTableValueUsingJSON]
GO

CREATE PROCEDURE [dbo].[sp_ImportTableValueUsingJSON] (
@TableName NVARCHAR(400),
@Json NVARCHAR(MAX)
)
AS
BEGIN

    IF(@TableName IS NULL OR ISJSON(@Json) = 0)
    BEGIN
          EXEC [dbo].[sp_ThrowException]
               @Name = N'sp_ImportTableValueUsingJSON',
               @Code = 403,
               @Reason = N'Parameter is invalid',
               @Message = N'We need @TableName and valid @Json.';
          RETURN;
    END

    DECLARE @columnName NVARCHAR(MAX);
    DECLARE @columnType NVARCHAR(MAX);
    DECLARE @max_Length INT;
    DECLARE @tableColumns NVARCHAR(MAX)='';
    DECLARE @openJsonWithSql NVARCHAR(MAX)='';
    DECLARE @updateColumnList NVARCHAR(MAX)='';
    DECLARE @usingSql NVARCHAR(MAX)='';
    DECLARE @matchedUpdateSql NVARCHAR(MAX)=' ';
    DECLARE @notMatchedInsertSql NVARCHAR(MAX)=' ';
    DECLARE @sourceTableColumn NVARCHAR(MAX)='';
    DECLARE @execSql NVARCHAR(MAX)='MERGE INTO [dbo].['+@TableName+'] AS T';

    DECLARE Column_Cursor CURSOR
                        LOCAL
                        FORWARD_ONLY
                        STATIC
                        READ_ONLY 
         FOR
            SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME=@TableName
                    AND COLUMN_NAME <> 'Identity';

OPEN Column_Cursor;

FETCH NEXT FROM Column_Cursor INTO @columnName,@columnType,@max_Length;

WHILE @@FETCH_STATUS=0

BEGIN

        SET @tableColumns=@tableColumns+'['+@columnName+'],';
        SET @sourceTableColumn=@sourceTableColumn+'SOURCE.['+@columnName+'],';
        SET @openJsonWithSql=@openJsonWithSql+'['+@columnName+']'+@columnType+CASE WHEN  @max_Length>0  THEN '('+CAST(@max_Length AS NVARCHAR(40))+')'+',' WHEN @max_Length=-1 THEN '(MAX) AS JSON,' ELSE ',' END;
        SET @updateColumnList=@updateColumnList+'['+@columnName+']='+'ISNULL(SOURCE.['+@columnName++'],T.['+@columnName+'])'+',';  

FETCH NEXT FROM Column_Cursor INTO @columnName,@columnType,@max_Length;

END
CLOSE Column_Cursor;
DEALLOCATE Column_Cursor;

SET @tableColumns=SUBSTRING(@tableColumns,0,LEN(@tableColumns));
SET @sourceTableColumn=SUBSTRING(@sourceTableColumn,0,LEN(@sourceTableColumn));
SET @openJsonWithSql=SUBSTRING(@openJsonWithSql,0,LEN(@openJsonWithSql));
SET @updateColumnList=SUBSTRING(@updateColumnList,0,LEN(@updateColumnList));

SET @usingSql=@usingSql+' USING(SELECT * FROM OPENJSON('''+@Json+''') WITH('+@openJsonWithSql+'))SOURCE';
SET @matchedUpdateSql=@matchedUpdateSql+'  ON T.[Key]=SOURCE.[Key] WHEN MATCHED THEN UPDATE SET '+@updateColumnList;
SET @notMatchedInsertSql=@notMatchedInsertSql+' WHEN NOT MATCHED THEN INSERT ('+@tableColumns+')VALUES('+@sourceTableColumn+');';

SET @execSql=@execSql+@usingSql+@matchedUpdateSql+@notMatchedInsertSql;

--SELECT @execSql;
EXEC SP_EXECUTESQL @execSql;

END


GO
