IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ConvertPendingCommitToPendingDelete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ConvertPendingCommitToPendingDelete]
GO

CREATE PROCEDURE [dbo].[sp_ConvertPendingCommitToPendingDelete](
    @Stamp DATETIME
)
AS
SET NOCOUNT OFF;
BEGIN
    UPDATE [dbo].[BinaryStorageMetaData]
        SET 
            [LastUpdatedStamp] = GETUTCDATE(),
            [State] = 3 -- DeletePending
        WHERE 
            [CreatedStamp] < @Stamp AND
            [State] = 1; --CommitPending
END
GO


