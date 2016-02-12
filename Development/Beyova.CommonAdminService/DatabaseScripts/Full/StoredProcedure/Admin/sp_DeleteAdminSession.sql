IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteAdminSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteAdminSession]
GO

CREATE PROCEDURE [dbo].[sp_DeleteAdminSession](
    @Token [varchar](512)
)
AS
BEGIN
    UPDATE [dbo].[AdminSession]
        SET [LastUpdatedStamp] = GETUTCDATE(),
            [State] = [dbo].[fn_SetObjectDeleted]([State])
        WHERE [Token] = @Token
            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;
END
GO


