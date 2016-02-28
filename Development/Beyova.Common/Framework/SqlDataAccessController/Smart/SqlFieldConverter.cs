using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class SqlFieldConverter.
    /// </summary>
    public sealed class SqlFieldConverter
    {
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <value>The property information.</value>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ignored.
        /// </summary>
        /// <value><c>true</c> if this instance is ignored; otherwise, <c>false</c>.</value>
        public bool IsIgnored { get; private set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public SqlDataConverter Converter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlFieldConverter" /> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="isIgnored">The is ignored.</param>
        /// <param name="columnName">Name of the column.</param>
        public SqlFieldConverter(PropertyInfo propertyInfo, SqlDataConverter converter = null, bool isIgnored = false, string columnName = null)
        {
            this.PropertyInfo = propertyInfo;
            this.PropertyName = propertyInfo?.Name;
            this.ColumnName = columnName.SafeToString(this.PropertyName);
            this.IsIgnored = isIgnored;
            this.Converter = converter ?? SqlDataConverter.GetDefaultSqlDataConverterByType(propertyInfo?.PropertyType);
        }

        /// <summary>
        /// To the database parameter object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Object.</returns>
        public object ToDbParameterObject(object instance)
        {
            return this.Converter?.ToDbParameterObject(instance, PropertyInfo);
        }

        /// <summary>
        /// Sets the property value object.
        /// </summary>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="databaseObject">The database object.</param>
        public void SetPropertyValueObject(object objectInstance, object databaseObject)
        {
            this.Converter?.SetPropertyValueObject(objectInstance, PropertyInfo, databaseObject);
        }
    }
}
