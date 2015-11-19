namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class GroupStatistic.
    /// </summary>
    public class GroupStatistic : AnalyticStatistic, IGroupByResult
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }
    }
}
