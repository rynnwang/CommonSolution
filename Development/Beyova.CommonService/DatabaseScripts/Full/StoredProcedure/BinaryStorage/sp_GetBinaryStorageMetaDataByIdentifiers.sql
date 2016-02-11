IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetBinaryStorageMetaDataByIdentifiers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_GetBinaryStorageMetaDataByIdentifiers]
GO
/* -----------------------------------
@@Xml XML SAMPLE:
<Identifier>
    <Item Container="Container">Blob Identifier</Item>
</Identifier>
----------------------------------- */
CREATE PROCEDURE [dbo].[sp_GetBinaryStorageMetaDataByIdentifiers](
    @Xml XML
)
AS
SET NOCOUNT ON;
BEGIN
    IF @Xml IS NOT NULL
    BEGIN
        CREATE TABLE #Identifier(
            [Identifier] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
            [Container] [varchar](128) NOT NULL
        );

        INSERT INTO #Identifier([Container], [Identifier])
            SELECT 
            Items.R.value('(@Container)[1]','[varchar](128)'),
            Items.R.value('.','UNIQUEIDENTIFIER')
            FROM @Xml.nodes('/Identifier/Item') AS Items(R);

        SELECT BSMD.[Identifier]
              ,BSMD.[Container]
              ,BSMD.[Name]
              ,BSMD.[Mime]
              ,BSMD.[Hash]
              ,BSMD.[Length]
              ,BSMD.[Height]
              ,BSMD.[Width]
              ,BSMD.[Duration]
              ,BSMD.[OwnerKey]
              ,BSMD.[CreatedStamp]
              ,BSMD.[LastUpdatedStamp]
              ,BSMD.[State]
            FROM [dbo].[BinaryStorageMetaData] AS BSMD
                JOIN #Identifier AS IDTABLE
                    ON BSMD.[Identifier] = IDTABLE.[Identifier] AND BSMD.[Container] = IDTABLE.[Container]
                WHERE [dbo].[fn_ObjectIsWorkable](BSMD.[State]) = 1;

    END
END
GO


