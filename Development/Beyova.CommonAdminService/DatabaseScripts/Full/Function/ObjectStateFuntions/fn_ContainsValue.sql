IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ContainsValue]'))
DROP FUNCTION [dbo].[fn_ContainsValue]
GO

CREATE FUNCTION [dbo].[fn_ContainsValue](
    @Value INT,
    @BitValue INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    IF @Value IS NOT NULL AND @BitValue IS NOT NULL
        SELECT @ReturnValue = 1 WHERE (@Value & @BitValue) = @BitValue;
    ELSE
        SET @ReturnValue =  0;

    RETURN @ReturnValue;
END
GO