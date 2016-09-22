using Beyova.ApiTracking;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class AggregationsQueryResult.
    /// </summary>
    public class AggregationsQueryResult : AggregationsQueryResult<object>
    {
    }

    /// <summary>
    /// Class AggregationsQueryResult.
    /// </summary>
    public class AggregationsQueryResult<T>
    {
        /// <summary>
        /// Gets or sets the shards.
        /// </summary>
        /// <value>The shards.</value>
        public QueryResultShard Shards { get; set; }

        /// <summary>
        /// Gets or sets the aggregations.
        /// </summary>
        /// <value>The aggregations.</value>
        public MatrixList<AggregationGroupObject<T>> Aggregations { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total { get; set; }
    }
}
