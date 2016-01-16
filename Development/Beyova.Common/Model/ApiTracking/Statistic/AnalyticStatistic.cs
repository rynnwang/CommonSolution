
namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class AnalyticStatistic.
    /// </summary>
    public class AnalyticStatistic : IAnalyticStatistic
    {
        /// <summary>
        /// Gets or sets the stamp identifier.
        /// </summary>
        /// <value>The stamp identifier.</value>
        public string StampIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }
}
