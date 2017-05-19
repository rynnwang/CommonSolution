IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsWorkable]'))
DROP FUNCTION [dbo].[fn_ObjectIsWorkable]
GO

CREATE FUNCTION [dbo].[fn_ObjectIsWorkable](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    -- State is neither Deleted (0x1) or Disabled (0x8)
    IF NOT ((@State & 0x1 = 0x1) OR (@State & 0x1 = 0x8))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO