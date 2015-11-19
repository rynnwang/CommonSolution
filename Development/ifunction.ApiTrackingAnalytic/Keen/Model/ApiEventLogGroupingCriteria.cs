using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ifunction.KeenSDK.Model;
using ifunction.Model;
using ifunction.RestfulApi;

namespace ifunction.ApiTrackingAnalytic.KeenAnalytics
{
    /// <summary>
    /// Class EventLogGroupingCriteria.
    /// </summary>
    public class EventLogGroupingCriteria : ApiEventLogGroupingCriteria
    {
        /// <summary>
        /// Gets or sets the time frame interval.
        /// </summary>
        /// <value>The time frame interval.</value>
        public QueryInterval TimeFrameInterval { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogGroupingCriteria" /> class.
        /// </summary>
        public EventLogGroupingCriteria()
            : base()
        {
        }
    }
}
