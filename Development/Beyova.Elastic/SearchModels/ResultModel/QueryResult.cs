using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class QueryResult.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T>
    {
        /// <summary>
        /// Gets or sets the shards.
        /// </summary>
        /// <value>The shards.</value>
        public QueryResultShard Shards { get; set; }

        /// <summary>
        /// Gets or sets the hits.
        /// </summary>
        /// <value>The hits.</value>
        public List<RawDataItem<T>> Hits { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; set; }
    }
}
