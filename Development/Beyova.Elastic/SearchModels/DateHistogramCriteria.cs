using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class DateHistogramCriteria.
    /// </summary>
    public class DateHistogramCriteria
    {
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        [JsonProperty(PropertyName = "interval")]
        public string Interval { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [JsonProperty(PropertyName = "format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }


        /// <summary>
        /// Gets or sets the time zone. Example: "-01:00"
        /// </summary>
        /// <value>
        /// The time zone.
        /// </value>
        [JsonProperty(PropertyName = "time_zone", NullValueHandling = NullValueHandling.Ignore)]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the offset. Example: +6h
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public string Offset { get; set; }

        /// <summary>
        /// Gets or sets the missing default value. if field is missing, use specific as default value.
        /// </summary>
        /// <value>
        /// The missing default value.
        /// </value>
        [JsonProperty(PropertyName = "missing", NullValueHandling = NullValueHandling.Ignore)]
        public string MissingDefaultValue { get; set; }
    }
}
