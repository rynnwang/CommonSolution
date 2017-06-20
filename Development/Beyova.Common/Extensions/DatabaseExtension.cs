using System;
using System.Data;

namespace Beyova
{
    /// <summary>
    /// Extensions for common type and common actions
    /// </summary>
    public static class DatabaseExtension
    {
        /// <summary>
        /// Prevents the SQL injection.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string PreventSqlInjection(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? value : (value.Replace("--", string.Empty).Replace("'", "''").Replace("/*", string.Empty).Replace("*/", string.Empty));
        }

        /// <summary>
        /// Determines whether the specified data reader has column.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns><c>true</c> if the specified dr has column; otherwise, <c>false</c>.</returns>
        public static bool HasColumn(this IDataRecord dataReader, string columnName)
        {
            if (dataReader != null)
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var result = dataReader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}