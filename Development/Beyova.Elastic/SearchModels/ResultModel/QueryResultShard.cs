using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Newtonsoft.Json;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class QueryResultShard.
    /// </summary>
    public class QueryResultShard
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        [JsonProperty(PropertyName = "total")]
        public int? Total { get; set; }

        /// <summary>
        /// Gets or sets the suncessful.
        /// </summary>
        /// <value>The suncessful.</value>
        [JsonProperty(PropertyName = "successful")]
        public int? Successful { get; set; }

        /// <summary>
        /// Gets or sets the failed.
        /// </summary>
        /// <value>The failed.</value>
        [JsonProperty(PropertyName = "failed")]
        public int? Failed { get; set; }
    }
}
