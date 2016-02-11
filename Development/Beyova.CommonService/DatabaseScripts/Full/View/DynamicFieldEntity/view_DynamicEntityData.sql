IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_DynamicEntityData]') AND type in (N'V'))
DROP VIEW [dbo].[view_DynamicEntityData]
GO

CREATE VIEW [dbo].[view_DynamicEntityData]
AS

SELECT DFV.[Key] AS [Key]
      ,DFV.[FieldKey]
      ,DFV.[EntityKey]
      ,DFV.[EntityPrimaryKey]
      ,DFV.[FieldValue]
      ,DFV.[CreatedStamp]
      ,DFV.[LastUpdatedStamp]
      ,DFV.[State]
      ,DS.[FieldType]
      ,DS.[EntityName]
      ,DS.[FieldName]
      ,DS.[DefaultFieldValue]
      ,DS.[Application]
  FROM [dbo].[DynamicFieldValue] AS DFV
  JOIN [dbo].[view_DynamicEntitySchema] AS DS
  ON DS.[FieldKey] = DFV.[FieldKey] AND DS.[EntityKey] = DFV.[EntityKey];

GO
