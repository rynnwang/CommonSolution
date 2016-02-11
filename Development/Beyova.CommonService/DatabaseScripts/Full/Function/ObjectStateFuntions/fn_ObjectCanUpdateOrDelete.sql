IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectCanUpdateOrDelete]'))
DROP FUNCTION [dbo].[fn_ObjectCanUpdateOrDelete]
GO

CREATE FUNCTION [dbo].[fn_ObjectCanUpdateOrDelete](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x1: Deleted, 0x4: readonly
    IF @State IS NOT NULL AND NOT ((@State & 0x1 = 0x1) OR (@State & 0x4 = 0x4))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO