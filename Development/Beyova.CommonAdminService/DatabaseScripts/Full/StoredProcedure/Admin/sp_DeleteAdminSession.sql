IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteAdminSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteAdminSession]
GO

CREATE PROCEDURE [dbo].[sp_DeleteAdminSession](
    @UserKey UNIQUEIDENTIFIER,
    @Token [varchar](512)
)
AS
BEGIN
    UPDATE [dbo].[AdminSession]
        SET [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 0x1
        WHERE [Token] = @Token AND [UserKey] = @UserKey;
END
GO


