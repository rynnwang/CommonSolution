using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class AggregationsCriteria.
    /// </summary>
    public class AggregationsCriteria
    {
        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        /// <value>The terms.</value>
        [JsonProperty(PropertyName = "terms", NullValueHandling = NullValueHandling.Ignore)]
        public object Terms { get; set; }

        /// <summary>
        /// Gets or sets the date histogram.
        /// </summary>
        /// <value>The date histogram.</value>
        [JsonProperty(PropertyName = "date_histogram", NullValueHandling = NullValueHandling.Ignore)]
        public DateHistogramCriteria DateHistogram { get; set; }

        /// <summary>
        /// Gets or sets the histogram.
        /// </summary>
        /// <value>The histogram.</value>
        [JsonProperty(PropertyName = "histogram", NullValueHandling = NullValueHandling.Ignore)]
        public object Histogram { get; set; }

        /// <summary>
        /// Gets or sets the sub aggregations.
        /// </summary>
        /// <value>The range.</value>
        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, AggregationsCriteria> SubAggregations { get; set; }
    }
}
