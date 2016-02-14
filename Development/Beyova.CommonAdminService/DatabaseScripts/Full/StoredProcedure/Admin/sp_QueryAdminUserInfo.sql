IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAdminUserInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAdminUserInfo]
GO

CREATE PROCEDURE [dbo].[sp_QueryAdminUserInfo](
    @Key UNIQUEIDENTIFIER,
    @LoginName [NVARCHAR](128),
    @Name [NVARCHAR](128),
    @Email [NVARCHAR](128),
    @ThirdPartyId [NVARCHAR](128),
    @RoleKey UNIQUEIDENTIFIER,
    @Count INT
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    IF @Count IS NULL OR @Count <= 0
        SET @Count = 50;

    SET @SqlStatement = 'SELECT TOP ' + CONVERT(NVARCHAR(5), @Count) + ' [Key]
      ,[LoginName]
      ,[Name]
      ,[Email]
      ,[ThirdPartyId]
      ,NULL AS [Permission]
      ,[AvatarKey]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[AdminUserInfo] AS AUI';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LoginName','=',@LoginName,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Email','=',@Email,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ThirdPartyId','=',@ThirdPartyId,1);

        IF @RoleKey IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + ' EXISTS (
    SELECT TOP 1 * FROM [dbo].[AdminUserRoleBinding] AS AUR
        WHERE AUR.[UserKey] = AUI.[Key] 
            AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1
            AND AUR.[RoleKey] = ''' + CONVERT(NVARCHAR(MAX), @RoleKey) + '''
) AND ';
        END
    END

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement;
    END

    EXECUTE sp_executesql @SqlStatement;
END
GO


