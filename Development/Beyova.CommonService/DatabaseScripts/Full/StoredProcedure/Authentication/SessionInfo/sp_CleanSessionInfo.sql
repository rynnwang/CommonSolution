IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CleanSessionInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CleanSessionInfo]
GO
CREATE PROCEDURE [dbo].[sp_CleanSessionInfo] (
    @Stamp DATETIME
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Stamp IS NOT NULL AND @Stamp < GETUTCDATE()
        DELETE FROM [dbo].[SessionInfo]
            WHERE [CreatedStamp] < @Stamp
                AND [ExpiredStamp] < @Stamp;
END

GO
