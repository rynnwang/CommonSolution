IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryAdminRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryAdminRole]
GO

CREATE PROCEDURE [dbo].[sp_QueryAdminRole](
    @Key UNIQUEIDENTIFIER,
    @Name [NVARCHAR](128),
    @ParentKey UNIQUEIDENTIFIER,
    @UserKey UNIQUEIDENTIFIER,
    @PermissionKey UNIQUEIDENTIFIER
)
AS
BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '[dbo].[fn_ObjectIsWorkable]([State]) = 1 AND ';

    SET @SqlStatement = 'SELECT [Key]
      ,[Name]
      ,[ParentKey]
      ,[Description]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
    FROM [dbo].[AdminRole] AS AR';

    IF @Key IS NOT NULL
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Key','=',CONVERT(NVARCHAR(MAX), @Key),1);
    ELSE
    BEGIN
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('ParentKey','=',CONVERT(NVARCHAR(MAX), @ParentKey),1);
        SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','LIKE',@Name,1);

        IF @UserKey IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + ' EXISTS (
    SELECT TOP 1 * FROM [dbo].[AdminUserRoleBinding] AS AUR
        WHERE AUR.[RoleKey] = AR.[Key] 
            AND [dbo].[fn_ObjectIsWorkable](AUR.[State]) = 1
            AND AUR.[UserKey] = ''' + CONVERT(NVARCHAR(MAX), @UserKey) + '''
) AND ';
        END

        IF @PermissionKey IS NOT NULL
        BEGIN
            SET @WhereStatement = @WhereStatement + ' EXISTS (
    SELECT TOP 1 * FROM [dbo].[AdminRolePermissionBinding] AS ARP
        WHERE ARP.[RoleKey] = AR.[Key] 
            AND [dbo].[fn_ObjectIsWorkable](ARP.[State]) = 1
            AND ARP.[PermissionKey] = ''' + CONVERT(NVARCHAR(MAX), @PermissionKey) + '''
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


