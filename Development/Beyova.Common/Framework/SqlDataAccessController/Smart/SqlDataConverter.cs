using System;
using System.Reflection;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class SqlDataConverter.
    /// </summary>
    public class SqlDataConverter
    {
        /// <summary>
        /// The _convert database parameter object delegate
        /// </summary>
        protected Func<object, PropertyInfo, object> _convertDbParameterObjectDelegate;

        /// <summary>
        /// The _set property value delegate
        /// </summary>
        protected Action<object, PropertyInfo, object> _setPropertyValueDelegate;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value><c>true</c> if this instance is nullable; otherwise, <c>false</c>.</value>
        public bool IsNullable { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataConverter"/> class.
        /// </summary>
        protected SqlDataConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataConverter"/> class.
        /// </summary>
        /// <param name="convertDbParameterObjectDelegate">The convert database parameter object delegate.</param>
        /// <param name="setPropertyValueDelegate">The set property value delegate.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        public SqlDataConverter(Func<object, PropertyInfo, object> convertDbParameterObjectDelegate, Action<object, PropertyInfo, object> setPropertyValueDelegate, bool isNullable = false)
        {
            _convertDbParameterObjectDelegate = convertDbParameterObjectDelegate;
            _setPropertyValueDelegate = setPropertyValueDelegate;
        }

        /// <summary>
        /// To the database parameter object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>System.Object.</returns>
        public virtual object ToDbParameterObject(object instance, PropertyInfo propertyInfo)
        {
            return _convertDbParameterObjectDelegate != null ? _convertDbParameterObjectDelegate(instance, propertyInfo) : null;
        }

        /// <summary>
        /// Sets the property value object.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="property">The property.</param>
        /// <param name="databaseObject">The database object.</param>
        public virtual void SetPropertyValueObject(object objectInstance, PropertyInfo property, object databaseObject)
        {
            _setPropertyValueDelegate?.Invoke(objectInstance, property, databaseObject);
        }

        #region Default

        /// <summary>
        /// The default convert database parameter object delegate
        /// </summary>
        private static Func<object, PropertyInfo, object> defaultConvertDbParameterObjectDelegate = ((i, p) =>
        {
            var o = p.GetValue(i);
            return o ?? DBNull.Value;
        });

        /// <summary>
        /// Gets the boolean data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetBooleanDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                (i, p) =>
                {
                    var o = p.GetValue(i);
                    return bool.Equals(o, true) ? "1" : "0";
                },
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToBoolean() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the int32 data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetInt32DataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToInt32() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the int64 data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetInt64DataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToInt64() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the x element data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetXElementDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                (i, p) =>
                {
                    var o = p.GetValue(i);
                    return o == null ? DBNull.Value : o.ToString() as object;
                },
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToXml() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the j token data converter.
        /// </summary>
        /// <param name="nullable">The nullable.</param>
        /// <returns>Beyova.SqlDataConverter.</returns>
        internal static SqlDataConverter GetJTokenDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                (i, p) =>
                {
                    var o = p.GetValue(i);
                    return o == null ? DBNull.Value : o.ToString() as object;
                },
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToString().TryParseToJToken() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the string data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetStringDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToString() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the date time data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetDateTimeDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToDateTime() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the float data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetFloatDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToFloat() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the double data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetDoubleDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToDouble() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the decimal data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetDecimalDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToDecimal() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the unique identifier data converter.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <returns>SqlDataConverter.</returns>
        internal static SqlDataConverter GetGuidDataConverter(bool nullable)
        {
            return new SqlDataConverter(
                defaultConvertDbParameterObjectDelegate,
                (i, p, d) => { p.SetValue(i, (Convert.IsDBNull(d) || d == null) ? null : d.ObjectToGuid() as object); },
                nullable
                );
        }

        /// <summary>
        /// Gets the default type of the SQL data converter by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>SqlDataConverter.</returns>
        /// <exception cref="Beyova.ExceptionSystem.UnimplementedException"></exception>
        public static SqlDataConverter GetDefaultSqlDataConverterByType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var isNullable = type.IsNullable();
            var fullName = (isNullable ? Nullable.GetUnderlyingType(type) : type).GetFullName();

            switch (fullName)
            {
                case "System.String":
                    return GetStringDataConverter(isNullable);
                case "System.Int16":
                case "System.UInt16":
                case "System.Int32":
                case "System.UInt32":
                    return GetInt32DataConverter(isNullable);
                case "System.Int64":
                case "System.UInt64":
                    return GetInt64DataConverter(isNullable);
                case "System.DateTime":
                    return GetDateTimeDataConverter(isNullable);
                case "System.Double":
                    return GetDoubleDataConverter(isNullable);
                case "System.Single":
                    return GetFloatDataConverter(isNullable);
                case "System.Guid":
                    return GetGuidDataConverter(isNullable);
                case "System.Boolean":
                    return GetBooleanDataConverter(isNullable);
                case "System.Xml.Linq.XElement":
                    return GetXElementDataConverter(isNullable);
                case "Newtonsoft.Json.Linq.JToken":
                    return GetJTokenDataConverter(isNullable);
                default:
                    throw new UnimplementedException(string.Format("GetDefaultSqlDataConverterByType.{0}", fullName));
            }
        }

        #endregion
    }
}
