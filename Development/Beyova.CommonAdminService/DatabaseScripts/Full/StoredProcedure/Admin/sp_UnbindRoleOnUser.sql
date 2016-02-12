IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UnbindRoleOnUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_UnbindRoleOnUser]
GO

CREATE PROCEDURE [dbo].[sp_UnbindRoleOnUser](
    @OwnerKey UNIQUEIDENTIFIER,
    @RoleKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER;

    IF @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @Key = [Key] FROM [dbo].[AdminUserRoleBinding]
            WHERE [UserKey] = @OwnerKey
                AND [RoleKey] = @RoleKey
                AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

        IF @Key IS NOT NULL
        BEGIN
            UPDATE [dbo].[AdminUserRoleBinding]
                SET [State] = [dbo].[fn_SetObjectAsDeleted]([State]),
                        [LastUpdatedBy] = @OperatorKey,
                        [LastUpdatedStamp] = @NowTime
                    WHERE [Key] = @Key
                        AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
        END
    END
END
GO


