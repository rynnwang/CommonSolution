IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteNonQueryWithException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteNonQueryWithException]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteNonQueryWithException]
AS
BEGIN
        EXEC [dbo].[sp_ThrowException]
        @Name = N'sp_ExecuteNonQueryWithException',
        @Code = 500,
        @Reason = 'Intend to throw',
        @Message = N'Tester ask to do so...again'
END
GO


