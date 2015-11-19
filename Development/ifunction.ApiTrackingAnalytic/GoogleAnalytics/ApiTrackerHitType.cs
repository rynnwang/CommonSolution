using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.ApiTrackingAnalytic.GoogleAnalytics
{
    /// <summary>
    /// Enum ApiTrackerHitType
    /// <remarks>
    /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters?hl=en-us#t
    /// </remarks>
    /// </summary>
    public enum ApiTrackerHitType
    {
        /// <summary>
        /// Value indicating it is undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Value indicating it is page view
        /// </summary>
        PageView = 0x1,
        /// <summary>
        /// Value indicating it is application view
        /// </summary>
        AppView = 0x2,
        /// <summary>
        /// Value indicating it is event
        /// </summary>
        Event = 0x4,
        /// <summary>
        /// Value indicating it is transaction
        /// </summary>
        Transaction = 0x8,
        /// <summary>
        /// Value indicating it is item
        /// </summary>
        Item = 0x10,
        /// <summary>
        /// Value indicating it is social
        /// </summary>
        Social = 0x20,
        /// <summary>
        /// Value indicating it is exception
        /// </summary>
        Exception = 0x40,
        /// <summary>
        /// Value indicating it is timing
        /// </summary>
        Timing = 0x80
    }
}
