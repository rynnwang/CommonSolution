IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetWorkflowValue]'))
DROP FUNCTION [dbo].[fn_GetWorkflowValue]
GO

CREATE FUNCTION [dbo].[fn_GetWorkflowValue](
    @State INT
)
RETURNS INT
AS
BEGIN
        RETURN @State & 0x1F0;
END
GO
