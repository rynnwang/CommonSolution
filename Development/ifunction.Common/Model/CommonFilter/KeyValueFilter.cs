namespace ifunction.Common.Model
{
    /// <summary>
    /// Class KeyValueFilter
    /// </summary>
    public class KeyValueFilter
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is string based.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is string based; otherwise, <c>false</c>.
        /// </value>
        public bool IsStringBased { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>The operator.</value>
        public FilterOperator Operator { get; set; }
    }
}
