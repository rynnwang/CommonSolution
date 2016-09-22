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
    /// Class ElasticIndicesStatus.
    /// </summary>
    public class ElasticIndicesStatus
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the document count.
        /// </summary>
        /// <value>
        /// The document count.
        /// </value>
        public long? DocumentCount { get; set; }

        /// <summary>
        /// Gets or sets the index count.
        /// </summary>
        /// <value>
        /// The index count.
        /// </value>
        public long? IndexCount { get; set; }

        /// <summary>
        /// Gets or sets the size of the storage.
        /// </summary>
        /// <value>
        /// The size of the storage.
        /// </value>
        public long? StorageSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the cache.
        /// </summary>
        /// <value>
        /// The size of the cache.
        /// </value>
        public long? CacheSize { get; set; }
    }
}
