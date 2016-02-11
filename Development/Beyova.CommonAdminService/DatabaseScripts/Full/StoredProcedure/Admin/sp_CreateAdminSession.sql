IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateAdminSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateAdminSession]
GO

CREATE PROCEDURE [dbo].[sp_CreateAdminSession](
    @UserKey [UNIQUEIDENTIFIER],
    @IpAddress [varchar](64),
    @UserAgent [varchar](256),
    @ExpiredStamp DATETIME,
    @IsUnique BIT
)
AS
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTcDATE();

    IF @IsUnique = 1
    BEGIN
        UPDATE [dbo].[AdminSession]
            SET [LastUpdatedStamp] = @NowTime,
                [State] = 0x1
                WHERE [UserKey] = @UserKey;
    END
    
    DECLARE @Token AS VARCHAR(512) = 
            REPLACE(
                CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()), '-','');

    INSERT INTO [dbo].[AdminSession]
           ([Token]
           ,[UserKey]
           ,[IpAddress]
           ,[UserAgent]
           ,[ExpiredStamp]
           ,[CreatedStamp]
           ,[LastUpdatedStamp]
           ,[State])
     VALUES
           (@Token
           ,@UserKey
           ,@IpAddress
           ,@UserAgent
           ,ISNULL(@ExpiredStamp, DATEADD(MI,60, @NowTime))
           ,@NowTime
           ,@NowTime
           ,0);
     
    SELECT TOP 1 [Token]
      ,[UserKey]
      ,[IpAddress]
      ,[UserAgent]
      ,[ExpiredStamp]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[State]
    FROM [dbo].[AdminSession]
    WHERE [Token] = @Token;
END
GO


