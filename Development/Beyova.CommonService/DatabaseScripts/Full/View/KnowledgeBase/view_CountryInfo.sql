IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[view_CountryInfo]') AND type in (N'V'))
DROP VIEW [dbo].[view_CountryInfo]
GO

CREATE VIEW [dbo].[view_CountryInfo]
AS

SELECT CI.[Key]
    ,CNI.[Name]
    ,CI.[ISO2Code]
    ,CI.[ISO3Code]
    ,CI.[TelCode]
    ,CI.[TimeZone]
    ,CI.[CurrencyCode]
    ,CI.[GeographyKey]
    ,CNI.[CultureCode]
    ,CNI.[Sequence]
    ,CNI.[State]
    FROM [dbo].[CountryInfo] AS CI
        JOIN [dbo].[CountryNameInfo] AS CNI
            ON CI.[Key] = CNI.[key]
        WHERE dbo.[fn_ObjectIsWorkable](CNI.[State]) = 1;

GO
