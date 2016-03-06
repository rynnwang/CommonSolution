IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_SplitStringToList]'))
DROP FUNCTION [dbo].[fn_SplitStringToList]
GO

CREATE FUNCTION [dbo].[fn_SplitStringToList](
    @DataString [nvarchar](max),
    @Separator [nvarchar](max)
)
RETURNS @DataStringsTable TABLE (
    [Id] int identity(1,1),
    [Value] nvarchar(max)
 )
AS
BEGIN
     DECLARE @CurrentIndex int;
     DECLARE @NextIndex int;
     DECLARE @ReturnText nvarchar(max);
     SELECT @CurrentIndex=1;
     WHILE(@CurrentIndex<=len(@DataString))
         BEGIN
             SELECT @NextIndex=charindex(@Separator,@DataString,@CurrentIndex);
             IF(@NextIndex=0 OR @NextIndex IS NULL)
                 SELECT @NextIndex=len(@DataString)+1;
                 SELECT @ReturnText=substring(@DataString,@CurrentIndex,@NextIndex-@CurrentIndex);
                 INSERT INTO @DataStringsTable([Value]) VALUES(@ReturnText);
                 SELECT @CurrentIndex=@NextIndex+1;
             END
     RETURN;
END
GO