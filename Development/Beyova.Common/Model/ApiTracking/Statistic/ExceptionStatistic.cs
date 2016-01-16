using Beyova.ExceptionSystem;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ExceptionStatistic.
    /// </summary>
    public class ExceptionStatistic : ExceptionBase, IAnalyticStatistic
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionStatistic"/> class.
        /// </summary>
        public ExceptionStatistic()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionStatistic"/> class.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        public ExceptionStatistic(ExceptionStatistic statistic)
            : base(statistic)
        {
            if (statistic != null)
            {
                this.StampIdentifier = statistic.StampIdentifier;
                this.Count = statistic.Count;
            }
        }
    }
}
