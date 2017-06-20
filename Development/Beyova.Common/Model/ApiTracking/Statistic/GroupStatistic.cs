namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class GroupStatistic.
    /// </summary>
    public class GroupStatistic : IGroupByResult
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public long Count { get; set; }
    }
}