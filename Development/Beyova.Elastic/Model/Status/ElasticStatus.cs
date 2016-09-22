using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticStatus.
    /// </summary>
    public class ElasticStatus<T>
    {
        /// <summary>
        /// Gets or sets the total shards.
        /// </summary>
        /// <value>
        /// The total shards.
        /// </value>
        [JsonProperty(PropertyName = "total")]
        public long TotalShards { get; set; }

        /// <summary>
        /// Gets or sets the successful shards.
        /// </summary>
        /// <value>
        /// The successful shards.
        /// </value>
        [JsonProperty(PropertyName = "successful")]
        public long SuccessfulShards { get; set; }

        /// <summary>
        /// Gets or sets the failed shards.
        /// </summary>
        /// <value>
        /// The failed shards.
        /// </value>
        [JsonProperty(PropertyName = "failed")]
        public long FailedShards { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<T> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticStatus{T}"/> class.
        /// </summary>
        public ElasticStatus()
        {
            this.Items = new List<T>();
        }
    }
}
