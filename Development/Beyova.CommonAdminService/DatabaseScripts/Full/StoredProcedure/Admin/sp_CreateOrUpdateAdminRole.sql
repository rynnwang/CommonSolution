IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAdminRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAdminRole]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAdminRole](
    @Key UNIQUEIDENTIFIER,
    @Name [NVARCHAR](128),
    @ParentKey [UNIQUEIDENTIFIER],
    @Description [NVARCHAR](512),
    @State INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @ExistedKey AS UNIQUEIDENTIFIER;
    DECLARE @ExistedState AS INT;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    SELECT @ExistedKey = [Key], @ExistedState = [State] FROM [dbo].[AdminRole]
        WHERE [Key] = @Key;

    IF @State < 0
        SET @State = NULL;

    IF @ExistedKey IS NOT NULL AND [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
    BEGIN
        UPDATE [dbo].[AdminRole]
            SET [Name] = ISNULL(@Name, [Name]),
                [ParentKey] = @ParentKey,
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

        INSERT INTO [dbo].[AdminRole]
           ([Key]
           ,[Name]
           ,[ParentKey]
           ,[Description]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     VALUES
           (@Key
           ,@Name
           ,@ParentKey
           ,@Description
           ,@NowTime
           ,@NowTime
           ,@OperatorKey
           ,@OperatorKey
           ,ISNULL(@State, 0));
    END
END
GO


