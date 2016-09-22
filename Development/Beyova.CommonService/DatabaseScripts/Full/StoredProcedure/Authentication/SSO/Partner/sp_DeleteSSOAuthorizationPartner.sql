IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteSSOAuthorizationPartner]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteSSOAuthorizationPartner]
GO

CREATE PROCEDURE [dbo].[sp_DeleteSSOAuthorizationPartner](
    @Key [UNIQUEIDENTIFIER],
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @ExistedState AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    SELECT TOP 1 @ExistedState = [State] FROM [dbo].[SSOAuthorizationPartner] WHERE [Key] = @Key;

    IF [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
    BEGIN
        UPDATE [dbo].[SSOAuthorizationPartner]
           SET [State] = [dbo].[fn_SetObjectAsDeleted]([State])
           ,[LastUpdatedStamp] = @NowTime
           ,[LastUpdatedBy] = @OperatorKey
        WHERE [Key] = @Key;
    END
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_DeleteSSOAuthorizationPartner',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Delete operation is forbidden caused by state.';
        RETURN;
    END
END
GO


