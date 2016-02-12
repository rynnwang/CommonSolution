IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BindRoleOnUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_BindRoleOnUser]
GO

CREATE PROCEDURE [dbo].[sp_BindRoleOnUser](
    @OwnerKey UNIQUEIDENTIFIER,
    @RoleKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER;

    IF @OwnerKey IS NOT NULL
        AND @RoleKey IS NOT NULL
        AND @OperatorKey IS NOT NULL
    BEGIN
        SELECT TOP 1 @Key = [Key] FROM [dbo].[AdminUserRoleBinding]
            WHERE [UserKey] = @OwnerKey
                AND [RoleKey] = @RoleKey
                AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

        IF @Key IS NULL
        BEGIN
            SET @Key = NEWID();

            INSERT INTO [dbo].[AdminUserRoleBinding]
                   ([Key]
                   ,[UserKey]
                   ,[RoleKey]
                   ,[CreatedStamp]
                   ,[LastUpdatedStamp]
                   ,[CreatedBy]
                   ,[LastUpdatedBy]
                   ,[State])
             VALUES
                   (@Key
                   ,@OwnerKey
                   ,@RoleKey
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


