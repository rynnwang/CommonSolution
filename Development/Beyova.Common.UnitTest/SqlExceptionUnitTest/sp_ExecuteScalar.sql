IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteScalar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteScalar]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteScalar]
AS
BEGIN
    SELECT GETUTCDATE();
END
GO


