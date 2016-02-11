IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateUserInfo]
GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateUserInfo] (
    @Key [UNIQUEIDENTIFIER],
    @UserId [varchar](128),
    @DisplayName [nvarchar](64),
    @Gender INT,
    @EnglishFirstName [nvarchar](64),
    @EnglishMiddleName [nvarchar](64),
    @EnglishLastName [nvarchar](64),
    @ChineseFirstName [nvarchar](64),
    @ChineseLastName [nvarchar](64),
    @Email [varchar](128),
    @AvatarKey [UNIQUEIDENTIFIER],
    @AvatarUrl [NVARCHAR](512),
    @FunctionalRole INT,
    @Language VARCHAR(16),
    @TimeZone INT,
    @GroupKey [UNIQUEIDENTIFIER],
    @MarketRegion [int],
    @CurrentBookKey [UNIQUEIDENTIFIER],
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Key IS NULL AND @UserId IS NOT NULL
    BEGIN
        SELECT TOP 1 @Key = [Key]
            FROM [dbo].[UserInfo]
            WHERE [UserId] = @UserId;
    END

    IF @Key IS NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[UserInfo]
            ([Key]
            ,[UserId]
            ,[DisplayName]
            ,[Gender]
            ,[EnglishFirstName]
            ,[EnglishMiddleName]
            ,[EnglishLastName]
            ,[ChineseFirstName]
            ,[ChineseLastName]
            ,[Email]
            ,[AvatarKey]
            ,[AvatarUrl]
            ,[FunctionalRole]
            ,[Language]
            ,[TimeZone]
            ,[GroupKey]
            ,[MarketRegion]
            ,[CurrentBookKey]
            ,[CreatedStamp]
            ,[LastUpdatedStamp]
            ,[CreatedBy]
            ,[LastUpdatedBy]
            ,[State])
        VALUES
            (@Key
            ,@UserId
            ,@DisplayName
            ,ISNULL(@Gender, 0)
            ,@EnglishFirstName
            ,@EnglishMiddleName
            ,@EnglishLastName
            ,@ChineseFirstName
            ,@ChineseLastName
            ,@Email
            ,@AvatarKey
            ,@AvatarUrl
            ,ISNULL(@FunctionalRole, 0)
            ,@Language
            ,@TimeZone
            ,@GroupKey
            ,ISNULL(@MarketRegion, 0)
            ,@CurrentBookKey
            ,@NowTime
            ,@NowTime
            ,@OperatorKey
            ,@OperatorKey
            ,0)
    END
    ELSE
    BEGIN
        UPDATE [dbo].[UserInfo]
            SET
                [DisplayName] = ISNULL(@DisplayName, [DisplayName]),
                [Gender] = ISNULL(@Gender, [Gender]),
                [EnglishFirstName] = ISNULL(@EnglishFirstName, [EnglishFirstName]),
                [EnglishMiddleName] = ISNULL(@EnglishMiddleName, [EnglishMiddleName]),
                [EnglishLastName] = ISNULL(@EnglishLastName, [EnglishLastName]),
                [ChineseFirstName] = ISNULL(@ChineseFirstName, [ChineseFirstName]),
                [ChineseLastName] = ISNULL(@ChineseLastName, [ChineseLastName]),
                [Email] = ISNULL(@Email, [Email]),
                [AvatarKey] = ISNULL(@AvatarKey, [AvatarKey]),
                [AvatarUrl] = ISNULL(@AvatarUrl, [AvatarUrl]),
                [FunctionalRole] = ISNULL(@FunctionalRole, [FunctionalRole]),
                [Language] = ISNULL(@Language, [Language]),
                [TimeZone] = ISNULL(@TimeZone, [TimeZone]),
                [GroupKey] = ISNULL(@GroupKey, [GroupKey]),
                [MarketRegion] = ISNULL(@MarketRegion, [MarketRegion]),
                [CurrentBookKey] = ISNULL(@CurrentBookKey, [CurrentBookKey]),
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy] = @OperatorKey
            WHERE [Key] = @Key
                AND[dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
    END

    SELECT TOP 1 [Key]
      ,[UserId]
      ,[DisplayName]
      ,[Gender]
      ,[EnglishFirstName]
      ,[EnglishMiddleName]
      ,[EnglishLastName]
      ,[ChineseFirstName]
      ,[ChineseLastName]
      ,[Email]
      ,[AvatarKey]
      ,[AvatarUrl]
      ,[Container]
      ,[Identifier]
      ,[FunctionalRole]
      ,[Language]
      ,[TimeZone]
      ,[GroupKey]
      ,[MarketRegion]
      ,[CurrentBookKey]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[view_UserInfo]
    WHERE [Key] = @Key;

END

GO


