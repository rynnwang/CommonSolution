IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateUserPreference]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateUserPreference]
GO
/* -----------------------------------
@Xml XML SAMPLE:
<Preference>
    <Item Realm="Realm" Identifier="Identifier">Value</Item>
</Preference>
----------------------------------- */
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateUserPreference](
    @OwnerKey UNIQUEIDENTIFIER,
    @Xml XML
)
AS
BEGIN    
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

     IF @Xml IS NOT NULL
    BEGIN
        CREATE TABLE #Preference(
            [Realm] [varchar](128) NOT NULL DEFAULT '',
            [Identifier] [varchar](128) NOT NULL DEFAULT '',
            [Value] [NVARCHAR](max) NOT NULL DEFAULT '',
            [IsUpdate] [bit] NOT NULL DEFAULT 0
        );

        INSERT INTO #Preference([Realm], [Identifier], [Value])
            SELECT 
            ISNULL(Items.R.value('(@Realm)[1]','[varchar](128)'), ''),
            Items.R.value('(@Identifier)[1]','[varchar](128)'),
            Items.R.value('.','[NVARCHAR](max)')
            FROM @Xml.nodes('/Preference/Item') AS Items(R);

        UPDATE #Preference
            SET [IsUpdate] = 1
            FROM #Preference AS P
                JOIN [dbo].[UserPreference] AS UP
                    ON P.[Realm] = UP.[Realm] AND P.[Identifier] = UP.[Identifier] AND [dbo].[fn_ObjectIsWorkable](UP.[State]) = 1
            WHERE (@OwnerKey IS NULL OR UP.[OwnerKey] = @OwnerKey);

        UPDATE [dbo].[UserPreference]
            SET [Value] = P.[Value],
                [LastUpdatedStamp] = @NowTime
            FROM [dbo].[UserPreference] AS UP
                JOIN #Preference AS P
                ON P.[Realm] = UP.[Realm] AND P.[Identifier] = UP.[Identifier] AND P.[IsUpdate] = 1
            WHERE (@OwnerKey IS NULL OR UP.[OwnerKey] = @OwnerKey);

        INSERT INTO [dbo].[UserPreference]
               ([OwnerKey]
               ,[Realm]
               ,[Identifier]
               ,[Value]
               ,[CreatedStamp]
               ,[LastUpdatedStamp]
               ,[State])
         SELECT
            @OwnerKey
            ,[Realm]
            ,[Identifier]
            ,[Value]
            ,@NowTime
            ,@NowTime
            ,0
            FROM #Preference
                WHERE [IsUpdate] = 0;
    END

END
GO

