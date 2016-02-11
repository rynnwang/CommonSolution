
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryEntityData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryEntityData]
GO
----------------------------------------------
--  <Criteria>
--      <Item Name="Tag" Operator="=">Test</Item>
--      <Item Name="Date" Operator="=">2013-01-01</Item>
--      <Item Name="Name" Operator="LIKE">GB</Item>
--  </Criteria>
----------------------------------------------
CREATE PROCEDURE [dbo].[sp_QueryEntityData](
    @EntityKey UNIQUEIDENTIFIER,
    @Application NVARCHAR(max),
    @Criteria XML
)
AS
DECLARE @CriteriaTable AS TABLE(
    [_FieldName] nvarchar(max) NOT NULL,
    [_Operation] nvarchar(max) NOT NULL,
    [_Value] nvarchar(max) NOT NULL
);

CREATE TABLE #TempTable(
    [_Key] UNIQUEIDENTIFIER NOT NULL
    ,[_EntityPrimaryKey] UNIQUEIDENTIFIER NOT NULL
    ,[_FieldKey] UNIQUEIDENTIFIER NOT NULL
    ,[_FieldName] nvarchar(max) NOT NULL
    ,[_EntityKey] UNIQUEIDENTIFIER NOT NULL
    ,[_FieldValue] nvarchar(max) NOT NULL
    ,[_FieldType] int NOT NULL
    ,[_DefaultFieldValue] nvarchar(max) NOT NULL
    ,[_CreatedStamp] datetime NOT NULL
    ,[_LastUpdatedStamp] datetime NOT NULL
    ,[_State] int NOT NULL
);

BEGIN
    INSERT INTO @CriteriaTable([_FieldName], [_Operation], [_Value])
        SELECT 
        Items.R.value('(@Name)[1]','nvarchar(max)'),
        Items.R.value('(@Operator)[1]','nvarchar(max)'),
        Items.R.value('.','nvarchar(max)')
        FROM @Criteria.nodes('/Criteria/Item') AS Items(R);

    INSERT INTO #TempTable([_Key], [_EntityPrimaryKey], [_FieldKey], [_FieldName], [_EntityKey], [_FieldValue], [_FieldType], [_DefaultFieldValue], [_CreatedStamp], [_LastUpdatedStamp], [_State])
        SELECT [Key], [EntityPrimaryKey], [FieldKey], [FieldName], [EntityKey], ISNULL([FieldValue],[DefaultFieldValue]), [FieldType], [DefaultFieldValue], [CreatedStamp], [LastUpdatedStamp], [State]
            FROM [dbo].[view_DynamicEntityData] AS ODD
                WHERE [EntityKey] = @EntityKey AND [Application] = @Application
                AND NOT EXISTS
                    (
                    SELECT DD.*
                                FROM [dbo].[view_DynamicEntityData] AS DD
                        JOIN @CriteriaTable AS CT
                        ON CT.[_FieldName] = DD.[FieldName] 
                            AND (
                                    (
                                            (CT.[_Operation] = '=' AND DD.[FieldValue] <> CT.[_Value])
                                            OR
                                            --(CT.[_Operation] = '>' AND DD.[FieldValue] <= CT.[_Value])
                                            --OR
                                            --(CT.[_Operation] = '<' AND DD.[FieldValue] >= CT.[_Value])
                                            --OR
                                            --(CT.[_Operation] = '>=' AND DD.[FieldValue] < CT.[_Value])
                                            --OR
                                            --(CT.[_Operation] = '<=' AND DD.[FieldValue] > CT.[_Value])
                                            --OR
                                            (CT.[_Operation] = 'LIKE' AND DD.[FieldValue] NOT LIKE '%' + CT.[_Value] + '%')
                                    )                                  
                                )
                        WHERE ODD.[EntityPrimaryKey] = DD.[EntityPrimaryKey]
                    ); 

    DECLARE @SqlExpression  AS NVARCHAR(MAX);
    DECLARE @ColumnList  AS NVARCHAR(MAX);
    SET @ColumnList = '';

    SELECT @ColumnList = @ColumnList + '[' + [FieldName] + '],'
        FROM [dbo].[view_DynamicEntitySchema]
        WHERE [EntityKey] = @EntityKey AND [Application] = @Application
        ORDER BY [FieldName];

    IF(@ColumnList <> '')
        SET @ColumnList = SUBSTRING(@ColumnList, 0, LEN(@ColumnList));

    SET @SqlExpression = 'SELECT [_EntityPrimaryKey] AS [Key],' + @ColumnList + '
    FROM
        (SELECT [_EntityPrimaryKey],[_FieldName],[_FieldValue]
        FROM #TempTable) DataSource
        PIVOT
        (
               MAX([_FieldValue])
            FOR
                [_FieldName]
                    IN (' + @ColumnList + ')
        ) AS pvt
        ORDER BY pvt.[_EntityPrimaryKey]
    ';

    EXECUTE sp_executesql @SqlExpression;
END
GO


