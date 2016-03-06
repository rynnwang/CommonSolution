IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ObjectIsApproved]'))
DROP FUNCTION [dbo].[fn_ObjectIsApproved]
GO

CREATE FUNCTION [dbo].[fn_ObjectIsApproved](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x110: Approved
    IF @State IS NOT NULL AND @State & 0x1F0 = 0x110
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO
