IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAdminUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAdminUserInfo]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAdminUserInfo](
    @Key UNIQUEIDENTIFIER,
    @LoginName [NVARCHAR](128),
    @Password [NVARCHAR](256),
    @Name [NVARCHAR](128),
    @Email [NVARCHAR](128),
    @ThirdPartyId [NVARCHAR](256),
    @State INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @ExistedKey AS UNIQUEIDENTIFIER;
    DECLARE @ExistedState AS INT;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @State < 0
        SET @State = NULL;

    SELECT @ExistedKey = [Key], @ExistedState = [State] FROM [dbo].[AdminUserInfo]
        WHERE [Key] = @Key;

    IF @ExistedKey IS NOT NULL AND [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
    BEGIN
        UPDATE [dbo].[AdminUserInfo]
            SET [Password] = ISNULL(@Password, [Password]),
                [Name] = ISNULL(@Name, [Name]),
                [Email] = ISNULL(@Email, ''),
                [ThirdPartyId] = ISNULL(@ThirdPartyId, [ThirdPartyId]),
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy] = @OperatorKey,
                [State] = ISNULL(@State, [State])
            WHERE [Key] = @Key;

        SELECT @Key;
    END
    ELSE
    BEGIN
        SET @Key = NEWID();

        SET @LoginName = ISNULL(@LoginName, 'User' + CONVERT(NVARCHAR(MAX), @Key));
        SET @Name = ISNULL(@Name, @LoginName);

        INSERT INTO [dbo].[AdminUserInfo]
            ([Key]
           ,[LoginName]
           ,[Password]
           ,[Name]
           ,[Email]
           ,[ThirdPartyId]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     VALUES
           (@Key
           ,@LoginName
           ,ISNULL(@Password, '')
           ,@Name
           ,ISNULL(@Email , '')
           ,@ThirdPartyId
           ,@NowTime
           ,@NowTime
           ,@OperatorKey
           ,@OperatorKey
           ,ISNULL(@State, 0));

        SELECT @Key;
    END
END
GO


