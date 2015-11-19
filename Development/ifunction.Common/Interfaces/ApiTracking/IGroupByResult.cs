
namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Interface IGroupByResult
    /// </summary>
    public interface IGroupByResult : IAnalyticStatistic
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; set; }
    }
}
