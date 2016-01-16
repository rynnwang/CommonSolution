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
    /// Class RawDataItem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RawDataItem<T>
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        [JsonProperty(PropertyName = "_index")]
        public string Index
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty(PropertyName = "_type")]
        public string Type
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [JsonProperty(PropertyName = "_source")]
        public T Source { get; set; }
    }
}
