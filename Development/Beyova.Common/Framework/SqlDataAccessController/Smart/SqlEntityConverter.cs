using System;
using System.Collections.Generic;
using System.Reflection;

namespace Beyova
{
    /// <summary>
    /// Class SqlEntityConverter.
    /// </summary>
    public sealed class SqlEntityConverter
    {
        /// <summary>
        /// Gets a value indicating whether this instance is simple base object.
        /// </summary>
        /// <value><c>true</c> if this instance is simple base object; otherwise, <c>false</c>.</value>
        public bool IsSimpleBaseObject { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is base object.
        /// </summary>
        /// <value><c>true</c> if this instance is base object; otherwise, <c>false</c>.</value>
        public bool IsBaseObject { get; private set; }

        /// <summary>
        /// Gets or sets the converter.
        /// </summary>
        /// <value>The converter.</value>
        public List<SqlFieldConverter> FieldConverters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlFieldConverter" /> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="fieldConverters">The field converters.</param>
        public SqlEntityConverter(Type entityType, List<SqlFieldConverter> fieldConverters)
        {
            IsSimpleBaseObject = entityType.InheritsFrom(typeof(ISimpleBaseObject));
            IsBaseObject = entityType.InheritsFrom(typeof(IBaseObject));
            this.FieldConverters = fieldConverters;
        }
    }
}
