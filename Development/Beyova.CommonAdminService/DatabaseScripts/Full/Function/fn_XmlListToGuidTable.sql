IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_XmlListToGuidTable]'))
DROP FUNCTION [dbo].[fn_XmlListToGuidTable]
GO

CREATE FUNCTION [dbo].[fn_XmlListToGuidTable](
    @Xml XML
)
RETURNS @DataTable TABLE (
    [Id] int identity(1,1),
    [Value] UNIQUEIDENTIFIER
 )
AS
BEGIN
    IF @Xml IS NOT NULL
        INSERT INTO @DataTable([Value])
            SELECT 
                Items.R.value('.','UNIQUEIDENTIFIER')
                FROM @Xml.nodes('/List/Item') AS Items(R);

    RETURN;
END
GO