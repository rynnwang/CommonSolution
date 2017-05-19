IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'fn_GenerateWherePattern'))
DROP FUNCTION [fn_GenerateWherePattern]
GO

CREATE FUNCTION [fn_GenerateWherePattern](
    @ColumnName NVARCHAR(MAX),
    @Operator NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX);
    SET @Result = '';

    IF @Value IS NOT NULL AND @ColumnName IS NOT NULL AND @Operator IS NOT NULL
    BEGIN
        IF LOWER(@Operator) = 'like'
        BEGIN
            SET @Value = '%'+@Value+'%';
            SET @IsStringType = 1;
        END
        SET @Result =  dbo.fn_GenerateSqlExpression(@ColumnName,@Operator,@Value, @IsStringType) + ' AND ';
    END

    RETURN @Result;
END
GO


