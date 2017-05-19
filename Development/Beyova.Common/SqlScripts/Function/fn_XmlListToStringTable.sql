IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_XmlListToStringTable]'))
DROP FUNCTION [dbo].[fn_XmlListToStringTable]
GO

CREATE FUNCTION [dbo].[fn_XmlListToStringTable](
    @Xml XML
)
RETURNS @DataTable TABLE (
    [Id] int identity(1,1),
    [Value] NVARCHAR(MAX)
 )
AS
BEGIN
    IF @Xml IS NOT NULL
        INSERT INTO @DataTable([Value])
            SELECT 
                Items.R.value('.','NVARCHAR(MAX)')
                FROM @Xml.nodes('/List/Item') AS Items(R);
    RETURN;
END
GO