IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_UserBinaryStorageMetaData]') AND type in (N'V'))
DROP VIEW [dbo].[view_UserBinaryStorageMetaData]
GO

CREATE VIEW [dbo].[view_UserBinaryStorageMetaData]
AS

SELECT BSM.[RowId]
      ,BSM.[Identifier]
      ,BSM.[Container]
      ,BSM.[Name]
      ,BSM.[Mime]
      ,BSM.[Hash]
      ,BSM.[Length]
      ,BSM.[Height]
      ,BSM.[Width]
      ,BSM.[Duration]
      ,UBSM.[OwnerKey]
      ,BSM.[CreatedStamp]
      ,BSM.[LastUpdatedStamp]
      ,BSM.[CreatedBy]
      ,BSM.[LastUpdatedBy]
      ,BSM.[State]
    FROM [dbo].[BinaryStorageMetaData] AS BSM
        JOIN [dbo].[UserBinaryStorageMetaData] AS UBSM
            ON BSM.[Identifier] = UBSM.[Identifier]
                AND [dbo].[fn_ObjectIsWorkable]([UBSM].[State]) = 1
                AND BSM.[State] = 2; --Committed

GO
