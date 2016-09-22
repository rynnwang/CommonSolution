IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteBinaryStorage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteBinaryStorage]
GO

CREATE PROCEDURE [dbo].[sp_DeleteBinaryStorage](
    @Identifier UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
DECLARE @NowTime AS DATETIME = GETUTCDATE();

BEGIN
    IF @Identifier IS NOT NULL 
        AND @OperatorKey IS NOT NULL 
    BEGIN
        UPDATE [dbo].[UserBinaryStorageMetaData]
            SET
                [LastUpdatedStamp] = @NowTime,
                [LastUpdatedBy]  = @OperatorKey,
                [State] = [dbo].[fn_SetObjectDeleted]([State])
            WHERE [Identifier] = @Identifier
                AND [OwnerKey] = @OperatorKey
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1;
    END
END
GO


