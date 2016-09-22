IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ThrowException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ThrowException];

GO
CREATE PROCEDURE [dbo].[sp_ThrowException](
    @Name [NVARCHAR](256),
    @Code INT,
    @Reason [NVARCHAR](256),
    @Message [NVARCHAR](512)
)
AS
BEGIN
    SELECT 
        @Name AS [SqlStoredProcedureName],
        ISNULL(@Code, 500) AS [SqlErrorCode],
        @Reason AS [SqlErrorReason],
        @Message AS [SqlErrorMessage];
END

GO