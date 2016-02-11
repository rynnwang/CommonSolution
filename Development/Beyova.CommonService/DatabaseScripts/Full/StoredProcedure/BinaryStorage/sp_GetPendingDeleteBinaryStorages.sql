IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetPendingDeleteBinaryStorages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetPendingDeleteBinaryStorages]
GO

CREATE PROCEDURE [dbo].[sp_GetPendingDeleteBinaryStorages]
AS
SET NOCOUNT ON;
BEGIN
    SELECT TOP 50 [Identifier]
          ,[Container]
          ,[Name]
          ,[Mime]
          ,[Hash]
          ,[Length]
          ,[Height]
          ,[Width]
          ,[Duration]
          ,[OwnerKey]
          ,[CreatedStamp]
          ,[LastUpdatedStamp]
          ,[State]
      FROM [dbo].[BinaryStorageMetaData]
      WHERE [State] = 3; --DeletePending
END
GO


