using ifunction.KeenSDK.Model;
using ifunction.Model;

namespace ifunction.ApiTrackingAnalytic.KeenAnalytics
{
    /// <summary>
    /// Class ExceptionGroupingCriteria.
    /// </summary>
    public class ExceptionGroupingCriteria : ExceptionInfoGroupingCriteria
    {
        /// <summary>
        /// Gets or sets the time frame days.
        /// </summary>
        /// <value>The time frame days.</value>
        public int TimeFrameDays { get; set; }

        /// <summary>
        /// Gets or sets the time frame interval.
        /// </summary>
        /// <value>The time frame interval.</value>
        public QueryInterval TimeFrameInterval { get; set; }

        /// <summary>
        /// Gets or sets the timezone offset.
        /// <remarks>Unit: second. </remarks>
        /// </summary>
        /// <value>The timezone offset.</value>
        public int TimezoneOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionGroupingCriteria" /> class.
        /// </summary>
        public ExceptionGroupingCriteria()
            : base()
        {
        }
    }
}
