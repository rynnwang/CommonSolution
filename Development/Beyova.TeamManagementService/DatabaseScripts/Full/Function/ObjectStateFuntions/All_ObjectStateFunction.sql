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
    IF @State IS NOT NULL AND NOT ((@State & 0x1 = 0x1) OR (@State & 0x1 = 0x8))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO

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
    IF @State IS NOT NULL AND @State & 0xFF0 = 0x110
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO


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
