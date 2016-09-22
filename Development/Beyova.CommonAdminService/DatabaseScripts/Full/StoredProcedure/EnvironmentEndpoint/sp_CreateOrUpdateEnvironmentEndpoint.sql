IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateEnvironmentEndpoint]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateEnvironmentEndpoint]
GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateEnvironmentEndpoint] (
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
    @Code NVARCHAR(64),
    @Environment NVARCHAR(64),
    @Protocol NVARCHAR(32),
    @Host NVARCHAR(256),
    @Port INT,
    @Version NVARCHAR(64),
    @Account NVARCHAR(256),
    @Token NVARCHAR(512),
    @SecondaryToken NVARCHAR(256),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @State AS INT;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Key IS NOT NULL
    BEGIN
        SELECT TOP 1 @State = [State]
            FROM [dbo].[EnvironmentEndpoint]
            WHERE [Key] = @Key;

        IF @State IS NOT NULL AND [dbo].[fn_ObjectCanUpdateOrDelete](@State) = 1
        BEGIN
            UPDATE [dbo].[EnvironmentEndpoint]
                SET [Name] = ISNULL(@Name, [Name]),
                    [Code] = ISNULL(@Code, [Code]),
                    [Environment] = ISNULL(@Environment, [Environment]),
                    [Protocol] = ISNULL(@Protocol, [Protocol]),
                    [Host] = ISNULL(@Host, [Host]),
                    [Port] = ISNULL(@Port, [Port]),
                    [Version] = ISNULL(@Version, [Version]),
                    [Account] = ISNULL(@Account, [Account]),
                    [Token] = ISNULL(@Token, [Token]),
                    [SecondaryToken] = ISNULL(@SecondaryToken, [SecondaryToken]),
                    [LastUpdatedStamp] = @NowTime,
                    [LastUpdatedBy] = @OperatorKey
                WHERE [Key] = @Key;

            SELECT @Key
        END
    END
    ELSE
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[EnvironmentEndpoint]
           ([Key]
           ,[Protocol]
           ,[Host]
           ,[Port]
           ,[Version]
           ,[Account]
           ,[Token]
           ,[SecondaryToken]
           ,[Name]
           ,[Code]
           ,[Environment]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[CreatedBy]
           ,[LastUpdatedBy]
           ,[State])
     VALUES
           (@Key
           ,@Protocol
           ,@Host
           ,@Port
           ,@Version
           ,@Account
           ,@Token
           ,@SecondaryToken
           ,@Name
           ,@Code
           ,@Environment
           ,@NowTime
           ,@NowTime
           ,@OperatorKey
           ,@OperatorKey
           ,0);

        SELECT @Key
    END

END

GO
