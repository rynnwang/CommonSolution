IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteReaderTestWithException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteReaderTestWithException]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteReaderTestWithException]
AS
BEGIN
    EXEC [dbo].[sp_ThrowException]
        @Name = N'sp_ExecuteReaderTestWithException',
        @Code = 500,
        @Reason = 'Intend to throw',
        @Message = N'Tester ask to do so...'
END
GO


