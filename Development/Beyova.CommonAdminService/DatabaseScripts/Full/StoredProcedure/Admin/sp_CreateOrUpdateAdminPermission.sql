IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAdminPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAdminPermission]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAdminPermission](
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
    @Identifier NVARCHAR(256),
    @Description NVARCHAR(512),
    @State INT,
    @OperatorKey UNIQUEIDENTIFIER
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

    IF @ExistedKey IS NOT NULL AND [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
    BEGIN
        UPDATE [dbo].[AdminPermission]
            SET [Name] = ISNULL(@Name, [Name]),
                [Identifier] = @Identifier,
                [Description] = ISNULL(@Description, [Description]),
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy] = @OperatorKey,
                [State] = ISNULL(@State, [State])
            WHERE [Key] = @Key;

        SELECT @Key;
    END
    ELSE
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[AdminPermission]
           ([Key]
           ,[Name]
           ,[Identifier]
           ,[Description]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     VALUES
           (@Key
           ,@Name
           ,@Identifier
           ,@Description
           ,@NowTime
           ,@NowTime
           ,@OperatorKey
           ,@OperatorKey
           ,ISNULL(@State, 0));
    END
END
GO


