IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAdminUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAdminUserInfo]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAdminUserInfo](
    @Key UNIQUEIDENTIFIER,
    @LoginName [varchar](50),
    @Password [varchar](200),
    @DisplayName [nvarchar](50),
    @Email [varchar](64),
    @State INT
)
AS
BEGIN
    DECLARE @ExistedKey AS UNIQUEIDENTIFIER;
    DECLARE @ExistedState AS INT;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    SELECT @ExistedKey = [Key], @ExistedState = [State] FROM [dbo].[AdminUserInfo]
        WHERE [Key] = @Key;

    IF @State < 0
        SET @State = NULL;

    IF @ExistedKey IS NOT NULL
    BEGIN
        IF [dbo].[fn_ObjectCanUpdateOrDelete](@ExistedState) = 1
            UPDATE [dbo].[AdminUserInfo]
                SET [Password] = ISNULL(@Password, [Password]),
                    [DisplayName] = ISNULL(@DisplayName, [DisplayName]),
                    [Email] = ISNULL(@Email, ''),
                    [LastUpdatedStamp] = @NowTime,
                    [State] = ISNULL(@State, [State])
                WHERE [Key] = @Key;
        ELSE
            RAISERROR(15622, 16, 1, 'Update or delete operation is forbidden caused by state.');
    END        
    ELSE
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[AdminUserInfo]
           ([Key]
           ,[LoginName]
           ,[Password]
           ,[DisplayName]
           ,[Email]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Key
           ,ISNULL(@LoginName, @Key)
           ,ISNULL(@Password, '')
           ,ISNULL(@DisplayName, @Key)
           ,ISNULL(@Email , '')
           ,@NowTime
           ,@NowTime
           ,ISNULL(@State, 0));
    END
END
GO


