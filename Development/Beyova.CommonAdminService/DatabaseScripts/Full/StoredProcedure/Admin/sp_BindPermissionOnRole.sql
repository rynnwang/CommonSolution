IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BindPermissionOnRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_BindPermissionOnRole]
GO

CREATE PROCEDURE [dbo].[sp_BindPermissionOnRole](
    @OwnerKey UNIQUEIDENTIFIER,
    @PermissionKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER;

    IF @OwnerKey IS NOT NULL
        AND @PermissionKey IS NOT NULL
        AND @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @Key = [Key] FROM [dbo].[AdminRolePermissionBinding]
            WHERE [RoleKey] = @OwnerKey
                AND [PermissionKey] = @PermissionKey
                AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

        IF @Key IS NULL
        BEGIN
            SET @Key = NEWID();

            INSERT INTO [dbo].[AdminRolePermissionBinding]
                   ([Key]
                   ,[RoleKey]
                   ,[PermissionKey]
                   ,[CreatedStamp]
                   ,[LastUpdatedStamp]
                   ,[CreatedBy]
                   ,[LastUpdatedBy]
                   ,[State])
             VALUES
                   (@Key
                   ,@OwnerKey
                   ,@PermissionKey
                   ,@NowTime
                   ,@NowTime
                   ,@OperatorKey
                   ,@OperatorKey
                   ,0);
        END

        SELECT @Key;
    END
END
GO


