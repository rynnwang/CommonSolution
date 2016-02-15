IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_QueryProvisioningObject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_QueryProvisioningObject]
GO

CREATE PROCEDURE [dbo].[sp_QueryProvisioningObject](
    @Application INT,
    @Module NVARCHAR(128),
    @Name NVARCHAR(256),
    @Value NVARCHAR(MAX),
    @OwnerKey UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER,
    @LastSynchronizedStamp DATETIME
)
AS

BEGIN
    DECLARE @SqlStatement AS NVARCHAR(MAX);
    DECLARE @WhereStatement AS NVARCHAR(MAX) = '';

    CREATE TABLE #ProvisioningObject(
        [Key] UNIQUEIDENTIFIER NOT NULL,
        [Application] [INT] NULL,
        [Module] [NVARCHAR](128) NOT NULL DEFAULT '',
        [Name] [NVARCHAR](256) NOT NULL DEFAULT '',
        [Value] [NVARCHAR](MAX) NOT NULL DEFAULT '',
        [OwnerKey] UNIQUEIDENTIFIER NULL,
        [CreatedStamp] [datetime] NOT NULL,
        [LastUpdatedStamp] [datetime] NOT NULL,
        [CreatedBy] [varchar](128) NOT NULL,
        [LastUpdatedBy] [varchar](128) NOT NULL,
        [State] [int] NOT NULL
    );

    SET @SqlStatement = 'INSERT INTO #ProvisioningObject([Key]
      ,[Key]
      ,[Application]
      ,[Module]
      ,[Name]
      ,[Value]
      ,[OwnerKey]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State])
    SELECT [Key]
      ,[Application]
      ,[Module]
      ,[Name]
      ,[Value]
      ,[OwnerKey]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
      FROM [dbo].[ProvisioningObject]';

    SET @WhereStatement = dbo.[fn_GenerateWherePattern]('OwnerKey','=',CONVERT(NVARCHAR(MAX),@OwnerKey),1);
    IF @WhereStatement = ''
        SET @WhereStatement = '[OwnerKey] IS NULL AND ';
    ELSE
        SET @WhereStatement = '([OwnerKey] IS NULL OR ' + SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3) + ') AND ';

    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Application','=',@Application,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Module','=',ISNULL(@Module,''),1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('Name','=',@Name,1);
    SET @WhereStatement = @WhereStatement + dbo.[fn_GenerateWherePattern]('LastUpdatedStamp','>=',CONVERT(NVARCHAR(MAX), @LastSynchronizedStamp, 121),1);

    IF(@WhereStatement <> '')
    BEGIN
        SET @WhereStatement = SUBSTRING(@WhereStatement, 0, LEN(@WhereStatement) - 3);
        SET @SqlStatement = @SqlStatement + ' WHERE ' + @WhereStatement ;
    END

    EXECUTE sp_executesql @SqlStatement;

    -- DELETE duplicated items if user based preference has already existed.
    DELETE PO1 
        FROM #ProvisioningObject AS PO1
        JOIN #ProvisioningObject AS PO2
        ON PO1.[Application] = PO2.[Application]
            AND PO1.[Module] = PO2.[Module]
            AND PO1.[Name] = PO2.[Name]
            WHERE PO1.[OwnerKey] IS NULL AND PO2.[OwnerKey] IS NOT NULL;

    SELECT [Key]
      ,[Application]
      ,[Module]
      ,[Name]
      ,[Value]
      ,[OwnerKey]
      ,[CreatedStamp]
      ,[LastUpdatedStamp]
      ,[CreatedBy]
      ,[LastUpdatedBy]
      ,[State]
      FROM #ProvisioningObject;

    DROP TABLE #ProvisioningObject;
END
GO


