IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteNonQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteNonQuery]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteNonQuery]
AS
BEGIN
    DECLARE @Temp AS DATETIME = GETUTCDATE();
END
GO


