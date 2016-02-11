
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateEntityData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_UpdateEntityData]
GO
----------------------------------------------
--  <Data>
--      <Item Name="Tag">Test</Item>
--      <Item Name="Date">2013-01-01</Item>
--      <Item Name="Name">GB</Item>
--  </Data>
----------------------------------------------
CREATE PROCEDURE [dbo].[sp_UpdateEntityData](
    @EntityKey UNIQUEIDENTIFIER,
    @EntityPrimaryKey UNIQUEIDENTIFIER,
    @Application NVARCHAR(max),
    @Data XML
)
AS
DECLARE @DataTable AS TABLE(
    [_FieldName] nvarchar(max) NOT NULL,
    [_Value] nvarchar(max) NOT NULL
);

BEGIN

    IF @EntityKey IS NOT NULL
    BEGIN
        IF @EntityPrimaryKey IS NULL
            SET @EntityPrimaryKey = NEWID();

        IF @Application IS NULL
            SET @Application = '';
    
        INSERT INTO @DataTable([_FieldName], [_Value])
            SELECT 
            Items.R.value('(@Name)[1]','nvarchar(max)'),
            Items.R.value('.','nvarchar(max)')
            FROM @Data.nodes('/Data/Item') AS Items(R);

        IF EXISTS (SELECT * FROM [dbo].[DynamicField] AS DF
            JOIN [dbo].[DynamicFieldValue] AS DFV
            ON DF.[Key] = DFV.[FieldKey]
            JOIN @DataTable AS DT
            ON DT.[_FieldName] = DF.[Name]
            WHERE DFV.[EntityKey] = @EntityKey 
                AND [Application] = @Application
                AND [EntityPrimaryKey] = @EntityPrimaryKey
                AND [FieldKey] = DF.[Key])
        BEGIN
             UPDATE [dbo].[DynamicFieldValue]
                SET [FieldValue] = DT.[_Value]
                FROM [dbo].[DynamicField] AS DF
                JOIN [dbo].[DynamicFieldValue] AS DFV
                ON DF.[Key] = DFV.[FieldKey]
                JOIN @DataTable AS DT
                ON DT.[_FieldName] = DF.[Name]
                WHERE DFV.[EntityKey] = @EntityKey 
                    AND [Application] = @Application
                    AND [EntityPrimaryKey] = @EntityPrimaryKey
                    AND [FieldKey] = DF.[Key];
        END
        ELSE
        BEGIN
            INSERT INTO [dbo].[DynamicFieldValue]
        END

       


    END
 
END
GO


