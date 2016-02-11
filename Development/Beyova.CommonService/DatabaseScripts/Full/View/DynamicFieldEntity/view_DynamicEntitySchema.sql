IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_DynamicEntitySchema]') AND type in (N'V'))
DROP VIEW [dbo].[view_DynamicEntitySchema]
GO

CREATE VIEW [dbo].[view_DynamicEntitySchema]
AS

SELECT DE.[Key] AS [EntityKey]
      ,DE.[Name] AS [EntityName]
      ,DE.[Application]
      ,DF.[Key] AS [FieldKey]
      ,DF.[Name] AS [FieldName]
      ,DF.[FieldType]
      ,DF.[DefaultFieldValue]
    FROM [dbo].[DynamicEntity] AS DE
        JOIN [dbo].[DynamicEntityMapping] AS DEM
            ON DEM.[EntityKey] =  DE.[Key] AND [dbo].[fn_ObjectIsWorkable](DEM.[State]) = 1
        JOIN [dbo].[DynamicField] AS DF
            ON DEM.[FieldKey] =  DF.[Key] AND [dbo].[fn_ObjectIsWorkable](DF.[State]) = 1
        WHERE [dbo].[fn_ObjectIsWorkable](DE.[State]) = 1
            AND DE.[Application] = DF.[Application];

GO
