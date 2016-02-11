IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CommitBinaryStorageDeletion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CommitBinaryStorageDeletion]
GO

/* -----------------------------------
@Xml XML SAMPLE:
<Storage>
    <Item Container="Container"></Item>
</Storage>
----------------------------------- */
CREATE PROCEDURE [dbo].[sp_CommitBinaryStorageDeletion](
    @Xml XML
)
AS
SET NOCOUNT ON;
BEGIN
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Xml IS NOT NULL
    BEGIN
        CREATE TABLE #BlobIdentifiers(
            [Container] [varchar](128) NOT NULL,
            [Identifier] UNIQUEIDENTIFIER NOT NULL
        );

        INSERT INTO #BlobIdentifiers([Container],[Identifier])
            SELECT 
            Items.R.value('(@Container)[1]','[varchar](128)'),
            Items.R.value('.','UNIQUEIDENTIFIER')
            FROM @Xml.nodes('/Storage/Item') AS Items(R);

        UPDATE [dbo].[BinaryStorageMetaData]
            SET [State] = 4, --Deleted
                [LastUpdatedStamp] = @NowTime
            FROM [dbo].[BinaryStorageMetaData] AS BSM
                JOIN #BlobIdentifiers AS B
                    ON B.[Container] = BSM.[Container] 
                        AND B.[Identifier] = BSM.[Identifier] 
                        AND BSM.[State] = 3; --DeletePending
    END
END
GO


