IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteReaderTest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteReaderTest]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteReaderTest]
AS
BEGIN
    SELECT
        1 AS [Column1],
        1.2 AS [Column2],
        'ExecuteReaderTest' AS [Column3],
        GETUTCDATE() AS [Column4];
END
GO


