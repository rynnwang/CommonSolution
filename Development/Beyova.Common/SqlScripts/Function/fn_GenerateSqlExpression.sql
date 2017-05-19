IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GenerateSqlExpression]'))
DROP FUNCTION [dbo].[fn_GenerateSqlExpression]
GO
/*
Sample: 
	dbo.fn_GenerateSqlExpression('Name','Like','%W', 1)
	RETURNS: [Name] Like '%W'
Sample: 
	dbo.fn_GenerateSqlExpression('CreatedStamp','>','2010-01-02', 1)
	RETURNS: [CreatedStamp] > '2010-01-02'
Sample: 
	dbo.fn_GenerateSqlExpression('FileSize','<=','3600', 0)
	RETURNS: [FileSize] <= 3600
*/
CREATE FUNCTION [dbo].[fn_GenerateSqlExpression](
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

    IF ISNULL(@ColumnName,'') <> '' AND ISNULL(@Operator, '') <> ''
    BEGIN
        IF(@Value IS NULL)
            SET @Result = '['+ @ColumnName + '] IS NULL';
        ELSE
            SET @Result = '['+ @ColumnName + '] ' + @Operator + ' '
                + (CASE WHEN @IsStringType = 1 THEN 'N''' ELSE '' END)
                + @Value
                + (CASE WHEN @IsStringType = 1 THEN '''' ELSE '' END);

    END

    RETURN @Result;
END
GO