namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ExceptionGroupingCriteria.
    /// </summary>
    public class ExceptionGroupingCriteria : ExceptionStatisticCriteria
    {
        /// <summary>
        /// Gets or sets a value indicating whether [group by service identifier].
        /// </summary>
        /// <value><c>true</c> if [group by service identifier]; otherwise, <c>false</c>.</value>
        public bool GroupByServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// </summary>
        /// <value>The server identifier.</value>
        public bool GroupByServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [group by exception code].
        /// </summary>
        /// <value><c>true</c> if [group by exception code]; otherwise, <c>false</c>.</value>
        public bool GroupByExceptionCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupingCriteria" /> class.
        /// </summary>
        public ExceptionGroupingCriteria()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupingCriteria"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public ExceptionGroupingCriteria(ExceptionStatisticCriteria criteria)
            : base(criteria)
        {
        }
    }
}