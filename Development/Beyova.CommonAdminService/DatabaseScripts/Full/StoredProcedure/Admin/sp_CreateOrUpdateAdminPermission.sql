IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAdminPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAdminPermission]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAdminPermission](
    @Key UNIQUEIDENTIFIER,
    @PermissionIdentifier [varchar](500),
    @Description [nvarchar](max),
    @State INT
)
AS
BEGIN
    DECLARE @ExistedKey AS UNIQUEIDENTIFIER;
    DECLARE @ExistedState AS INT;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    SELECT @ExistedKey = [Key], @ExistedState = [State] FROM [dbo].[AdminPermission]
        WHERE [Key] = @Key;

    IF @State < 0
        SET @State = NULL;

    IF @ExistedKey IS NOT NULL
    BEGIN
        IF [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
            UPDATE [dbo].[AdminPermission]
                SET [PermissionIdentifier] = ISNULL(@PermissionIdentifier, [PermissionIdentifier]),
                    [Description] = ISNULL(@Description, [Description]),
                    [LastUpdatedStamp] = @NowTime,
                    [State] = ISNULL(@State, [State])
                WHERE [Key] = @Key;
        ELSE
            RAISERROR(15622, 16, 1, 'Update or delete operation is forbidden caused by state.');
    END
    ELSE
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[AdminPermission]
           ([Key]
           ,[PermissionIdentifier]
           ,[Description]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Key
           ,ISNULL(@PermissionIdentifier, @Key)
           ,ISNULL(@Description, '')
           ,@NowTime
           ,@NowTime
           ,ISNULL(@State, 0));
    END
END
GO


