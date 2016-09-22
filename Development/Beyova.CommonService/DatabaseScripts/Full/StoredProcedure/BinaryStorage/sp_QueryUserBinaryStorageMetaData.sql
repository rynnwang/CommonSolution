IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryUserBinaryStorageMetaData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryUserBinaryStorageMetaData]
GO

CREATE PROCEDURE [dbo].[sp_QueryUserBinaryStorageMetaData](
    @Container NVARCHAR(128),
    @Identifier UNIQUEIDENTIFIER,
    @Name [NVARCHAR](512),
    @Mime VARCHAR(64),
    @Hash VARCHAR(128),
    @MinLength INT,
    @MaxLength INT,
    @MinHeight INT,
    @MaxHeight INT,
    @MinWidth INT,
    @MaxWidth INT,
    @MinDuration INT,
    @MaxDuration INT,
    @FromStamp DATETIME,
    @ToStamp DATETIME,
    @OwnerKey UNIQUEIDENTIFIER,
    @Count INT
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Container IS NOT NULL
    BEGIN
        DECLARE @SqlStatement AS NVARCHAR(MAX);
        -- In View [view_UserBinaryStorageMetaData], it already filter for State= 2 only.
        DECLARE @WhereStatement AS NVARCHAR(MAX) = '[Container] = ''' + @Container + ''' AND ';

        IF @Count IS NULL OR @Count < 1
            SET @Count = 50;

        SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(MAX), @Count) + ' [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[OwnerKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[CreatedBy]
          ,[LastUpdatedBy]
          ,[State]
      FROM [dbo].[view_UserBinaryStorageMetaData]';

        IF @Identifier IS NOT NULL
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Identifier','=',CONVERT(NVARCHAR(MAX), @Identifier),1);
        ELSE
        BEGIN
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Mime','=',@Mime,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Hash','=',@Hash,1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('OwnerKey','=',CONVERT(NVARCHAR(MAX), @OwnerKey),1);

            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','>=',CONVERT(NVARCHAR(MAX), @MinLength),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Length','<',CONVERT(NVARCHAR(MAX), @MaxLength),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','>=',CONVERT(NVARCHAR(MAX), @MinHeight),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Height','<',CONVERT(NVARCHAR(MAX), @MaxHeight),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','>=',CONVERT(NVARCHAR(MAX), @MinWidth),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Width','<',CONVERT(NVARCHAR(MAX), @MaxWidth),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','>=',CONVERT(NVARCHAR(MAX), @MinDuration),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Duration','<',CONVERT(NVARCHAR(MAX), @MaxDuration),0);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','>=',CONVERT(NVARCHAR(MAX), @FromStamp, 121),1);
            SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','<',CONVERT(NVARCHAR(MAX), @ToStamp, 121),1);

        END

        IF(@WhereStatement <> '')
        BEGIN
            SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
            SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
            SET @SqlStatement = @SqlStatement + ' ORDER BY [Container],[Name] ';
        END

        PRINT @SqlStatement;
        EXECUTE sp_executesql @SqlStatement;
    END
END
GO


