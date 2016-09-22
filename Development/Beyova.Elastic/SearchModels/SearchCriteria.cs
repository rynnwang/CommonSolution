using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class SearchCriteria.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the query criteria.
        /// </summary>
        /// <value>The query criteria.</value>
        [JsonProperty(PropertyName = "query", NullValueHandling = NullValueHandling.Ignore)]
        public QueryCriteria QueryCriteria { get; set; }

        /// <summary>
        /// Gets or sets the aggregations criteria.
        /// </summary>
        /// <value>The aggregations criteria.</value>
        [JsonProperty(PropertyName = "aggregations", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, AggregationsCriteria> AggregationsCriteria { get; set; }

        /// <summary>
        /// Gets or sets the order by desc.
        /// </summary>
        /// <value>The order by desc.</value>
        [JsonProperty(PropertyName = "sort", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> OrderBy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteria"/> class.
        /// </summary>
        public SearchCriteria()
        {
            this.OrderBy = new Dictionary<string, string>();
        }
    }
}
