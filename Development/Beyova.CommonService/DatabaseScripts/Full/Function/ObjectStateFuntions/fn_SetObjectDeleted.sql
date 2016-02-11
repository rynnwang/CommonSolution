IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_SetObjectDeleted]'))
DROP FUNCTION [dbo].[fn_SetObjectDeleted]
GO

CREATE FUNCTION [dbo].[fn_SetObjectDeleted](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    -- 0x1: Deleted
    IF @State IS NOT NULL
        SET @ReturnValue = (@State | 0x1);
    ELSE
        SET @ReturnValue = 0x1;

        RETURN @ReturnValue;
END
GO