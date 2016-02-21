IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ExecuteScalarWithException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ExecuteScalarWithException]
GO

CREATE PROCEDURE [dbo].[sp_ExecuteScalarWithException]
AS
BEGIN
    EXEC [dbo].[sp_ThrowException]
        @Name = N'sp_ExecuteScalarWithException',
        @Code = 403,
        @Reason = 'Intend to throw with forbidden',
        @Message = N'No idea how it happened'
END
GO


