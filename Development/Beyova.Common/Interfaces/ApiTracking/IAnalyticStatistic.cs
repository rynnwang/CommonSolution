
namespace Beyova.ApiTracking
{
    /// <summary>
    /// Interface IAnalyticStatistic
    /// </summary>
    public interface IAnalyticStatistic
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; set; }

        /// <summary>
        /// Gets or sets the stamp identifier.
        /// </summary>
        /// <value>The stamp identifier.</value>
        string StampIdentifier { get; set; }
    }
}
