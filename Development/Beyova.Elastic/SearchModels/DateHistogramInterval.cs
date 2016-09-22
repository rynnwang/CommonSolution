using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Enum DateHistogramInterval
    /// </summary>
    public enum DateHistogramInterval
    {
        /// <summary>
        /// The year
        /// </summary>
        year = 0,
        /// <summary>
        /// The quarter
        /// </summary>
        quarter,
        /// <summary>
        /// The month
        /// </summary>
        month,
        /// <summary>
        /// The week
        /// </summary>
        week,
        /// <summary>
        /// The day
        /// </summary>
        day,
        /// <summary>
        /// The hour
        /// </summary>
        hour,
        /// <summary>
        /// The minute
        /// </summary>
        minute,
        /// <summary>
        /// The second
        /// </summary>
        second
    }
}
