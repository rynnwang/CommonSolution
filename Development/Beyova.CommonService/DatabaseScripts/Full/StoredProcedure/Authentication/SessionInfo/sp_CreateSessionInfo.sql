IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateSessionInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateSessionInfo]
GO

CREATE PROCEDURE [dbo].[sp_CreateSessionInfo](
    @UserKey UNIQUEIDENTIFIER,
    @UserAgent VARCHAR(512),
    @Platform INT,
    @DeviceType INT,
    @IpAddress VARCHAR(64),
    @ExpiredStamp DATETIME
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    IF @UserKey IS NOT NULL
    BEGIN
        IF @ExpiredStamp IS NULL
            SET @ExpiredStamp = DATEADD(MI,43200, @NowTime);

        DECLARE @Token AS VARCHAR(512) = 
            REPLACE(
                CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()) 
                + CONVERT(VARCHAR(64), NEWID()), '-','');

        INSERT INTO [dbo].[SessionInfo]
               ([Token]
               ,[UserKey]
               ,[UserAgent]
               ,[Platform]
               ,[DeviceType]
               ,[IpAddress]
               ,[CreatedStamp]
               ,[LastUpdatedStamp]
               ,[ExpiredStamp])
         VALUES
               (@Token
               ,@UserKey
               ,@UserAgent
               ,ISNULL(@Platform, 0)
               ,ISNULL(@DeviceType, 0)
               ,@IpAddress
               ,@NowTime
               ,@NowTime
               ,@ExpiredStamp);

        SELECT TOP 1 [Token]
            ,[UserKey]
            ,[UserAgent]
            ,[Platform]
            ,[DeviceType]
            ,[IpAddress]
            ,[CreatedStamp]
            ,[LastUpdatedStamp]
            ,[ExpiredStamp]
            FROM [dbo].[SessionInfo]
            WHERE [Token] = @Token;
    END
END
GO


