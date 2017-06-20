using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    ///
    /// </summary>
    public class SqlScriptGenerator
    {
        private const string defaultDbSchema = "dbo";

        /// <summary>
        /// Gets or sets the destination directory.
        /// </summary>
        /// <value>
        /// The destination directory.
        /// </value>
        public DirectoryInfo DestinationDirectory { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the database schema.
        /// </summary>
        /// <value>
        /// The name of the database schema.
        /// </value>
        public string DbSchemaName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptGenerator" /> class.
        /// </summary>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="dbSchema">The database schema.</param>
        public SqlScriptGenerator(string destinationDirectory, string dbSchema = null)
            : this(new DirectoryInfo(destinationDirectory), dbSchema)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlScriptGenerator" /> class.
        /// </summary>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="dbSchema">The database schema.</param>
        public SqlScriptGenerator(DirectoryInfo destinationDirectory, string dbSchema = null)
        {
            this.DestinationDirectory = destinationDirectory;
            this.DbSchemaName = dbSchema.SafeToString(defaultDbSchema);
        }

        /// <summary>
        /// Generates the type of the by exact.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="scriptOutputFilter">The script output filter.</param>
        /// <param name="specificRules">The specific rules.</param>
        public void GenerateByExactType<T>(string fileName, SqlScriptType scriptOutputFilter = SqlScriptType.Table | SqlScriptType.StoredProcedure, Dictionary<string, SqlTableScriptGenerationRule> specificRules = null)
            where T : class, IIdentifier
        {
            InternalGenerateType<T>(fileName.SafeToString(typeof(T).Name), scriptOutputFilter, specificRules);
        }

        /// <summary>
        /// Generates the type of the by core.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="needOperatorColumns">if set to <c>true</c> [need operator columns].</param>
        /// <param name="scriptOutputFilter">The script output filter.</param>
        /// <param name="specificRules">The specific rules.</param>
        public void GenerateByCoreType<T>(string fileName, bool needOperatorColumns, SqlScriptType scriptOutputFilter = SqlScriptType.Table | SqlScriptType.StoredProcedure, Dictionary<string, SqlTableScriptGenerationRule> specificRules = null)
            where T : class, IIdentifier
        {
            var tableName = fileName.SafeToString(typeof(T).Name);

            if (typeof(ISimpleBaseObject).IsAssignableFrom(typeof(T)))
            {
                InternalGenerateType<T>(tableName, scriptOutputFilter, specificRules);
            }

            if (needOperatorColumns)
            {
                GenerateByExactType<BaseObject<T>>(fileName, scriptOutputFilter, specificRules);
            }
            else
            {
                GenerateByExactType<SimpleBaseObject<T>>(fileName, scriptOutputFilter, specificRules);
            }
        }

        /// <summary>
        /// Internals the type of the generate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="scriptOutputFilter">The script output filter.</param>
        /// <param name="specificRules">The specific rules.</param>
        /// <returns></returns>
        protected string InternalGenerateType<T>(string tableName, SqlScriptType scriptOutputFilter, Dictionary<string, SqlTableScriptGenerationRule> specificRules)
                where T : class, IIdentifier
        {
            var type = typeof(T);

            try
            {
                return InternalGenerateType(type, tableName, type.IsSimpleBaseObject(), type.IsBaseObject(), scriptOutputFilter, specificRules);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { T = type.GetFullName(), tableName, scriptOutputFilter, specificRules });
            }
        }

        /// <summary>
        /// Internals the type of the generate.
        /// </summary>
        /// <param name="coreType">Type of the core.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="appendAuditStamp">if set to <c>true</c> [append audit stamp].</param>
        /// <param name="appendAuditOperator">if set to <c>true</c> [append audit operator].</param>
        /// <param name="scriptOutputFilter">The script output filter.</param>
        /// <param name="specificRules">The specific rules.</param>
        /// <returns></returns>
        protected string InternalGenerateType(Type coreType, string tableName, bool appendAuditStamp, bool appendAuditOperator, SqlScriptType scriptOutputFilter, Dictionary<string, SqlTableScriptGenerationRule> specificRules)
        {
            var parameters = coreType.GetProperties(System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            try
            {
                StringBuilder builder = new StringBuilder(512 + parameters.Length * 128);

                builder.AppendFormat(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[{1}]') AND type in (N'U'))
    DROP TABLE[{0}].[{1}]
GO
", this.DbSchemaName, tableName);

                builder.AppendLineWithFormat(@"CREATE TABLE [{0}].[{1}](", this.DbSchemaName, tableName);

                // Columns starts here
                builder.AppendLine("[RowId] INT NOT NULL IDENTITY(1,1), ");
                // Considering Generic constraint of IIdentifier, generate Key column here directly.
                builder.AppendLine("[Key] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), ");

                // Columns ends here
                builder.AppendLineWithFormat(@"CONSTRAINT [PK_{0}_Key] PRIMARY KEY NONCLUSTERED
(
    [Key] ASC
),
CONSTRAINT [CIX_{0}] UNIQUE CLUSTERED
(
    [RowId] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
);

GO
", tableName);

                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { coreType = coreType.GetFullName(), parameterCount = parameters.Length, tableName, appendAuditStamp, appendAuditOperator, scriptOutputFilter, specificRules });
            }
        }

        /// <summary>
        /// Gets the default SQL table script generation rule.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected static string GetDefaultSqlTableScriptGenerationRule(Type type)
        {
            type.CheckNullObject(nameof(type));

            var underlyingType = (type.IsNullable() ? Nullable.GetUnderlyingType(type) : type);
            var underlyingTypeName = underlyingType.GetFullName();

            if (underlyingType.IsEnum || underlyingTypeName.StartsWith("System.Int") || underlyingTypeName.StartsWith("System.UInt"))
            {
                return "[INT]";
            }
            else if (underlyingType == typeof(float) || underlyingType == typeof(double))
            {
                return "[Float]";
            }
            else if (underlyingType == typeof(decimal))
            {
                return "[DECIMAL](16,2)";
            }
            else if (underlyingType == typeof(string))
            {
                return "[NVARCHAR](256)";
            }
            else if (underlyingType == typeof(Guid))
            {
                return "[UNIQUEIDENTIFIER]";
            }
            else if (underlyingType == typeof(DateTime))
            {
                return "[DATETIME]";
            }
            else
            {
                return "[NVARCHAR](MAX)";
            }
        }

        #region GetCoreProperties

        /// <summary>
        /// Gets the core properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="isSimpleBaseObject">if set to <c>true</c> [is simple base object].</param>
        /// <param name="isBaseObject">if set to <c>true</c> [is base object].</param>
        /// <returns></returns>
        protected static List<PropertyInfo> GetCoreProperties(Type type, out bool isSimpleBaseObject, out bool isBaseObject)
        {
            type.CheckNullObject(nameof(type));

            isSimpleBaseObject = typeof(ISimpleBaseObject).IsAssignableFrom(type);
            isBaseObject = typeof(IBaseObject).IsAssignableFrom(type);
            Func<PropertyInfo, bool> filter = null;

            if (isBaseObject)
            {
                filter = BaseObjectPropertyFilter;
            }
            else if (isSimpleBaseObject)
            {
                filter = SimpleBaseObjectPropertyFilter;
            }
            else
            {
                filter = IdentifierObjectPropertyFilter;
            }

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, true, filter);
        }

        /// <summary>
        /// The simple base object properties
        /// </summary>
        protected static string[] simpleBaseObjectProperties = typeof(ISimpleBaseObject).GetProperties().Select(x => x.Name).ToArray();

        /// <summary>
        /// The base object properties
        /// </summary>
        protected static string[] baseObjectProperties = typeof(IBaseObject).GetProperties().Select(x => x.Name).ToArray();

        /// <summary>
        /// The identifier object properties
        /// </summary>
        protected static string[] identifierObjectProperties = typeof(IIdentifier).GetProperties().Select(x => x.Name).ToArray();

        /// <summary>
        /// Simples the base object property filter.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        protected static bool SimpleBaseObjectPropertyFilter(PropertyInfo propertyInfo)
        {
            return propertyInfo != null && !simpleBaseObjectProperties.Contains(propertyInfo.Name);
        }

        /// <summary>
        /// Bases the object property filter.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        protected static bool BaseObjectPropertyFilter(PropertyInfo propertyInfo)
        {
            return propertyInfo != null && !baseObjectProperties.Contains(propertyInfo.Name);
        }

        /// <summary>
        /// Identifiers the object property filter.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        protected static bool IdentifierObjectPropertyFilter(PropertyInfo propertyInfo)
        {
            return propertyInfo != null && !identifierObjectProperties.Contains(propertyInfo.Name);
        }

        #endregion GetCoreProperties

        #region Default SQL contents

        /// <summary>
        /// Generates the default stored procedure.
        /// </summary>
        /// <param name="dbSchema">The database schema.</param>
        /// <returns></returns>
        public static string GenerateDefaultStoredProcedure(string dbSchema)
        {
            return string.Format(@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[sp_ThrowException]') AND type in (N'P', N'PC'))
DROP PROCEDURE [{0}].[sp_ThrowException];

GO
CREATE PROCEDURE [dbo].[sp_ThrowException](
    @Name [NVARCHAR](256),
    @Code INT,
    @Reason [NVARCHAR](256),
    @Message [NVARCHAR](512)
)
AS
BEGIN
    SELECT
        @Name AS [SqlStoredProcedureName],
        ISNULL(@Code, 500) AS [SqlErrorCode],
        @Reason AS [SqlErrorReason],
        @Message AS [SqlErrorMessage];
END

GO
", dbSchema.SafeToString(defaultDbSchema));
        }

        /// <summary>
        /// Generates the default function.
        /// </summary>
        /// <param name="dbSchema">The database schema.</param>
        /// <returns></returns>
        public static string GenerateDefaultFunction(string dbSchema)
        {
            return string.Format(@"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_ObjectCanUpdateOrDelete]'))
DROP FUNCTION [{0}].[fn_ObjectCanUpdateOrDelete]
GO

CREATE FUNCTION [{0}].[fn_ObjectCanUpdateOrDelete](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x1: Deleted, 0x4: readonly
    IF @State IS NOT NULL AND NOT ((@State & 0x1 = 0x1) OR (@State & 0x4 = 0x4))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_ObjectIsVisible]'))
DROP FUNCTION [{0}].[fn_ObjectIsVisible]
GO

CREATE FUNCTION [{0}].[fn_ObjectIsVisible](
	@State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x1: Deleted, 0x2: Invisible
    IF @State IS NOT NULL AND NOT ((@State & 0x1  = 0x1) OR (@State & 0x2 = 0x2))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;
    RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_ObjectIsWorkable]'))
DROP FUNCTION [{0}].[fn_ObjectIsWorkable]
GO

CREATE FUNCTION [{0}].[fn_ObjectIsWorkable](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    -- State is neither Deleted (0x1) or Disabled (0x8)
    IF @State IS NOT NULL AND NOT ((@State & 0x1 = 0x1) OR (@State & 0x1 = 0x8))
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_SetObjectDeleted]'))
DROP FUNCTION [{0}].[fn_SetObjectDeleted]
GO

CREATE FUNCTION [{0}].[fn_SetObjectDeleted](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    -- 0x1: Deleted
    IF @State IS NOT NULL
        SET @ReturnValue = (@State | 0x1);
    ELSE
        SET @ReturnValue = 0x1;

        RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_ObjectIsApproved]'))
DROP FUNCTION [{0}].[fn_ObjectIsApproved]
GO

CREATE FUNCTION [{0}].[fn_ObjectIsApproved](
    @State INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;
    -- 0x110: Approved
    IF @State IS NOT NULL AND @State & 0xFF0 = 0x110
        SET @ReturnValue = 1;
    ELSE
        SET @ReturnValue = 0;

    RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_ContainsValue]'))
DROP FUNCTION [{0}].[fn_ContainsValue]
GO

CREATE FUNCTION [{0}].[fn_ContainsValue](
    @Value INT,
    @BitValue INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @ReturnValue AS BIT;

    IF @Value IS NOT NULL AND @BitValue IS NOT NULL
        SELECT @ReturnValue = 1 WHERE (@Value & @BitValue) = @BitValue;
    ELSE
        SET @ReturnValue =  0;

    RETURN @ReturnValue;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_GetWorkflowValue]'))
DROP FUNCTION [{0}].[fn_GetWorkflowValue]
GO

CREATE FUNCTION [{0}].[fn_GetWorkflowValue](
    @State INT
)
RETURNS INT
AS
BEGIN
        RETURN @State & 0x1F0;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_GenerateSqlExpression]'))
DROP FUNCTION [{0}].[fn_GenerateSqlExpression]
GO
/*
Sample:
	{0}.fn_GenerateSqlExpression('Name','Like','%W', 1)
	RETURNS: [Name] Like '%W'
Sample:
	{0}.fn_GenerateSqlExpression('CreatedStamp','>','2010-01-02', 1)
	RETURNS: [CreatedStamp] > '2010-01-02'
Sample:
	{0}.fn_GenerateSqlExpression('FileSize','<=','3600', 0)
	RETURNS: [FileSize] <= 3600
*/
CREATE FUNCTION [{0}].[fn_GenerateSqlExpression](
    @ColumnName NVARCHAR(MAX),
    @Operator NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX);
    SET @Result = '';

    IF ISNULL(@ColumnName,'') <> '' AND ISNULL(@Operator, '') <> ''
    BEGIN
        IF(@Value IS NULL)
            SET @Result = '['+ @ColumnName + '] IS NULL';
        ELSE
            SET @Result = '['+ @ColumnName + '] ' + @Operator + ' '
                + (CASE WHEN @IsStringType = 1 THEN 'N''' ELSE '' END)
                + @Value
                + (CASE WHEN @IsStringType = 1 THEN '''' ELSE '' END);

    END

    RETURN @Result;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_GenerateWherePattern]'))
DROP FUNCTION [{0}].[fn_GenerateWherePattern]
GO

CREATE FUNCTION [fn_GenerateWherePattern](
    @ColumnName NVARCHAR(MAX),
    @Operator NVARCHAR(MAX),
    @Value NVARCHAR(MAX),
    @IsStringType BIT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Result AS NVARCHAR(MAX);
    SET @Result = '';

    IF @Value IS NOT NULL AND @ColumnName IS NOT NULL AND @Operator IS NOT NULL
    BEGIN
        SET @Value = REPLACE(@Value, '''', '''''');
        SET @Value = REPLACE(@Value, '--', '');
        SET @Value = REPLACE(@Value, '/*', '');
        SET @Value = REPLACE(@Value, '*/', '');

        IF LOWER(@Operator) = 'like'
        BEGIN
            SET @Value = '%' + @Value +'%';
            SET @IsStringType = 1;
        END
        SET @Result =  [{0}].[fn_GenerateSqlExpression](@ColumnName,@Operator,@Value, @IsStringType) + ' AND ';
    END

    RETURN @Result;
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}].[fn_SplitStringToList]'))
DROP FUNCTION [{0}].[fn_SplitStringToList]
GO

CREATE FUNCTION [{0}].[fn_SplitStringToList](
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

", dbSchema.SafeToString(defaultDbSchema));
        }

        #endregion Default SQL contents
    }
}