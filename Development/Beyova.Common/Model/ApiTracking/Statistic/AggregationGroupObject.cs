namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class AggregationGroupObject.
    /// </summary>
    public class AggregationGroupObject<T>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public T Identifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier string.
        /// </summary>
        /// <value>The identifier string.</value>
        public string IdentifierString { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the sub group objects.
        /// </summary>
        /// <value>The sub group objects.</value>
        public MatrixList<AggregationGroupObject<T>> SubGroupObjects { get; set; }
    }
}