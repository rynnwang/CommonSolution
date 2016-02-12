IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UnbindPermissionOnRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_UnbindPermissionOnRole]
GO

CREATE PROCEDURE [dbo].[sp_UnbindPermissionOnRole](
    @OwnerKey UNIQUEIDENTIFIER,
    @PermissionKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER;

    IF @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @Key = [Key] FROM [dbo].[AdminRolePermissionBinding]
            WHERE [RoleKey] = @OwnerKey
                AND [PermissionKey] = @PermissionKey
                AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

        IF @Key IS NOT NULL
        BEGIN
            UPDATE [dbo].[AdminRolePermissionBinding]
                SET [State] = [dbo].[fn_SetObjectAsDeleted]([State]),
                        [LastUpdatedBy] = @OperatorKey,
                        [LastUpdatedStamp] = @NowTime
                    WHERE [Key] = @Key
                        AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
        END
    END
END
GO


