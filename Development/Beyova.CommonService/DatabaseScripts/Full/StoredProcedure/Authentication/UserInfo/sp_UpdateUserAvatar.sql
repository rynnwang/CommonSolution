IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateUserAvatar]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_UpdateUserAvatar]
GO
CREATE PROCEDURE [dbo].[sp_UpdateUserAvatar] (
    @UserKey [UNIQUEIDENTIFIER],
    @AvatarKey [UNIQUEIDENTIFIER],
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    UPDATE [dbo].[UserInfo]
        SET
            [AvatarKey] = ISNULL(@AvatarKey, [AvatarKey]),
            [LastUpdatedStamp] = @NowTime,
            [LastUpdatedBy] = @OperatorKey
        WHERE [Key] = @UserKey
            AND[dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
END

GO


