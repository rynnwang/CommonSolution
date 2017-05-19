IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsVisible]'))
DROP FUNCTION [dbo].[fn_ObjectIsVisible]
GO

CREATE FUNCTION [dbo].[fn_ObjectIsVisible](
	@State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x1: Deleted, 0x2: Invisible
    IF @State IS NOT NULL AND NOT ((@State & 0x1  = 0x1) OR (@State & 0x2 = 0x2))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;
    RETURN @ReturnValue;
END
GO