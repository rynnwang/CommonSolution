IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAdminPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAdminPermission]
GO

CREATE PROCEDURE [dbo].[sp_QueryAdminPermission](
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
    @Identifier NVARCHAR(256),
    @Description NVARCHAR(512),
    @UserKey UNIQUEIDENTIFIER,
    @RoleKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
      ,[Name]
      ,[Identifier]
      ,[Description]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[AdminPermission] AS AP';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Identifier','=',@Identifier,1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);

        IF @UserKey IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + ' EXISTS (
    SELECT TOP 1 1 FROM [dbo].[AdminRolePermissionBinding] AS ARP
        JOIN [dbo].[AdminUserRoleBinding] AS AUR
            ON AUR.[RoleKey] = ARP.[RoleKey]
                AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1
        WHERE ARP.[PermissionKey] = AP.[Key] 
            AND [dbo].[fn_ObjectIsWorkable](ARP.[State]) = 1
            AND AUR.[UserKey] = ''' + CONVERT(NVARCHAR(MAX), @UserKey) + '''
) AND ';
        END

        IF @RoleKey IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + ' EXISTS (
    SELECT TOP 1 * FROM [dbo].[AdminRolePermissionBinding] AS ARP
        WHERE ARP.[PermissionKey] = AP.[Key] 
            AND [dbo].[fn_ObjectIsWorkable](ARP.[State]) = 1
            AND ARP.[RoleKey] = ''' + CONVERT(NVARCHAR(MAX), @RoleKey) + '''
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


